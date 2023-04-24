using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.MessagePatterns;

using SphServiceLib;

namespace AMQPlib
{
	public class AMQPpacket
	{
		public int IdSportsman;
		public int CoordinateValue;
		public string CoordinateType;

		public AMQPpacket(int id, int val, string type)
		{
			IdSportsman = id;
			CoordinateValue = val;
			CoordinateType = type;
		}
	}

	public class AMQPlistenThread
	{
		private bool bStop_ = false;
		private SphSettings settings_;

		//public PacketReceiver packetReceiver;

		public delegate void AMQPpacketReceivedHandler(AMQPpacket packet);
		public event AMQPpacketReceivedHandler OnAMQPpacketReceived;

		// ссылка на главное окно
		//FormMain frmMain_;

		public AMQPlistenThread(ref SphSettings settings) //(Settings s, frmMain frm)
		{
			settings_ = settings;
			//frmMain_ = frm;
		}

		public void Run()
		{
			//try
			//{
				#region Old Code
				//AutoResetEvent autoEvent = new AutoResetEvent(false);
				//packetReceiver = new PacketReceiver(this);

				//TimerCallback timerDelegate =
				//    new TimerCallback(packetReceiver.TryToReceive);

				//Timer stateTimer =
				//        new Timer(timerDelegate, autoEvent, 1000, 
				//            1000 * timeInterval_);

				//autoEvent.WaitOne(Timeout.Infinite, false);

				// When autoEvent signals, dispose of the timer.
				//stateTimer.Dispose();
				//SphService.WriteToLogGeneral("timer destroyed");
				#endregion

				#region Old code2

				//ConnectionFactory factory = new ConnectionFactory();

				//using (IConnection connection = factory.CreateConnection())
				//{
				//    using (IModel channel = connection.CreateModel())
				//    {
				//        channel.ExchangeDeclare(exchangeName_, "fanout");

				//        string queueName = channel.QueueDeclare();//or introduced by the user?

				//        channel.QueueBind(queueName, exchangeName_, "");

				//        //Console.WriteLine("Waiting for messages");

				//        QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
				//        channel.BasicConsume(queueName, true, consumer);

				//        while (true)
				//        {
				//            AMQPpacket packet = null;
				//            BasicDeliverEventArgs e = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
				//            //Console.WriteLine(Encoding.ASCII.GetString(e.Body));

				//            // dummy: parse message and create packet

				//            // notify the main window
				//            if (OnAMQPpacketReceived != null) OnAMQPpacketReceived(packet);

				//            SphService.WriteToLogGeneral("AMPQ packet was received");
				//        }
				//    }
				//}

				#endregion

			while (!bStop_)
			{
				try
				{
					settings_.LoadSettings();

					//string exchangeName = "amq.fanout";
					//string queueName = "measValuePhis";
					var factory = new ConnectionFactory();
					factory.HostName = settings_.AmpqHost; //"rabbitmq"; //"95.161.7.119"; //"localhost";
					factory.UserName = settings_.AmpqUser; //"admin"; //"alex";
					factory.Password = settings_.AmpqPswd; //"admin"; //"12345";

					using (IConnection connection = factory.CreateConnection())
					{
						using (IModel channel = connection.CreateModel())
						{
							channel.QueueDeclare(settings_.AmpqQueueName, true, false, false, null);

							QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
							String consumerTag = channel.BasicConsume(settings_.AmpqQueueName, false, consumer);
							while (!bStop_)
							{
								try
								{
									BasicDeliverEventArgs ea;
									bool res = consumer.Queue.Dequeue(5000, out ea);
									if (ea != null && res)
									{
										//IBasicProperties props = ea.BasicProperties;
										//byte[] body = ea.Body;
										string rkey = ea.RoutingKey;
										string messageContent = Encoding.UTF8.GetString(ea.Body);

										XDocument xDocument = XDocument.Parse(messageContent);
										int value;
										string valueString = xDocument.
											Element("com.rsisportmon.storage.measure.model.SingleMeasureValue").
											Element("value").Value;
										bool valueValid = Int32.TryParse(valueString, out value);
										//string rkey =
										// xDocument.Element("com.rsisportmon.storage.measure.model.SingleMeasureValue")
										//             .Element("measureSpecCode")
										//             .Value;
										if (String.IsNullOrEmpty(rkey) || !valueValid)
										{
											SphService.WriteToLogFailed(
												string.Format("Invalid AMPQ packet: {0}, {1}", rkey, valueString));
											continue;
										}

										int sportsmanId = 1;  // dummy (на случай появления групповых тестов)
										AMQPpacket packet = new AMQPpacket(sportsmanId, value, rkey);

										// notify the main window
										if (OnAMQPpacketReceived != null) OnAMQPpacketReceived(packet);

										SphService.WriteToLogGeneral("AMPQ packet was received");

										channel.BasicAck(ea.DeliveryTag, false);
									}
									else
									{
										//SphService.WriteToLogFailed("Rabbit msg = null");
									}
								}
								catch (OperationInterruptedException)
								{
									SphService.WriteToLogFailed("OperationInterruptedException AMQPlisten::Run():");
									// The consumer was removed, either through
									// channel or connection closure, or through the
									// action of IModel.BasicCancel().
									break;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					SphService.DumpException(ex, "Error AMQPlisten::Run():");
				}
			}
			//}
			//catch (Exception ex)
			//{
			//    SphService.DumpException(ex, "Error AMQPlisten::Run():");
			//}
		}

		public void StopThread()
		{
			bStop_ = true;
		}

		//public void AMQPpacketReceivedEvent(AMQPpacket packet)
		//{
		//    try
		//    {
		//        if (OnAMQPpacketReceived != null) OnAMQPpacketReceived(packet);
		//    }
		//    catch (Exception ex)
		//    {
		//        SphService.DumpException(ex, "Error AMQPpacketReceivedEvent():");
		//    }
		//}
		
	
	}

	//public class PacketReceiver
	//{
	//    //public bool bStop = false;
	//    //Settings settings_;
	//    AMQPlistenThread owner_;
	//    //frmMain frmMain_;

	//    public PacketReceiver(AMQPlistenThread th)//, Settings s, frmMain frm)
	//    {
	//        owner_ = th;
	//        //settings_ = s;
	//        //frmMain_ = frm;
	//    }

	//    // This method is called by the timer delegate.
	//    public void TryToReceive(object stateInfo)
	//    {
	//        try
	//        {
	//            AutoResetEvent autoEvent = (AutoResetEvent) stateInfo;

	//            AMQPpacket packet = null;
	//            bool bReceived; // dummy

	//            string messageContent;
	//            var connectionFactory = new ConnectionFactory();
	//            using (IConnection connection = connectionFactory.CreateConnection())
	//            {
	//                using (IModel model = connection.CreateModel())
	//                {
	//                    var subscription = new Subscription(model, "MyQueue", false);
	//                    //while (true)
	//                    //{
	//                    BasicDeliverEventArgs basicDeliveryEventArgs =
	//                        subscription.Next();
	//                    messageContent = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body);
	//                    bReceived = messageContent.Length > 0;

	//                    subscription.Ack(basicDeliveryEventArgs);
	//                    //}
	//                }
	//            }


	//            if (!bReceived)
	//            {
	//                return;
	//            }
	//            //else
	//            //{
	//            //SphService.WriteToLogGeneral("AMPQ packet was received");
	//            //owner_.AMQPpacketReceivedEvent(packet);
	//            //}

	//        }
	//        catch (Exception ex)
	//        {
	//            SphService.DumpException(ex, "Error PacketReceiver::TryToReceive():");
	//        }
	//    }
	//}

	//Here is the full subscribe code:
	//from: http://www.jarloo.com/rabbitmq-c-tutorial/

	//using System;
	//using System.Text;
	//using RabbitMQ.Client;
	//using RabbitMQ.Client.Events;
 
	//namespace Consumer
	//{
	//    public class Program
	//    {
	//        private const string EXCHANGE_NAME = "helloworld";
 
	//        private static void Main(string[] args)
	//        {
	//            ConnectionFactory factory = new ConnectionFactory {HostName = "localhost"};
 
	//            using(IConnection connection = factory.CreateConnection())
	//            {
	//                using(IModel channel = connection.CreateModel())
	//                {
	//                    channel.ExchangeDeclare(EXCHANGE_NAME, "fanout");
 
	//                    string queueName = channel.QueueDeclare();
 
	//                    channel.QueueBind(queueName, EXCHANGE_NAME, "");
 
	//                    Console.WriteLine("Waiting for messages");
 
	//                    QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
	//                    channel.BasicConsume(queueName, true, consumer);
 
	//                    while (true)
	//                    {
	//                        BasicDeliverEventArgs e = (BasicDeliverEventArgs) consumer.Queue.Dequeue();
	//                        Console.WriteLine(Encoding.ASCII.GetString(e.Body));
	//                    }
	//                }
	//            }
	//        }
	//    }
	//}


	// one else https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html
}
