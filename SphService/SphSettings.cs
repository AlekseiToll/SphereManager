using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;

namespace SphServiceLib
{
	public class SphSettings
	{
		#region Fields

		//// <summary>Synchronization state</summary>
		//[NonSerialized]
		//private bool settingsChanged_ = false;

		/// <summary>Path to XML tests</summary>
		[NonSerialized]
		private string xmlPath_ = string.Empty;

		/// <summary>Settings file name</summary>
		[NonSerialized]
		private string settingsFileName_ = SphService.AppDirectory + "SphereManager.config";

		/// <summary>Host name for RabbitMQ</summary>
		[NonSerialized]
		private string ampqHost_;
		
		/// <summary>User name for RabbitMQ</summary>
		[NonSerialized]
		private string ampqUser_;

		/// <summary>Password for RabbitMQ</summary>
		[NonSerialized]
		private string ampqPswd_;

		/// <summary>Queue name for RabbitMQ</summary>
		[NonSerialized]
		private string ampqQueueName_;

		/// <summary>Web server IP</summary>
		[NonSerialized]
		//private IPAddress webServerIp_;
		private string webServerIp_;

		/// <summary>Web server port</summary>
		[NonSerialized] 
		private int webServerPort_ = 8082;

		/// <summary>Web server login</summary>
		[NonSerialized]
		private string webServerUser_;

		/// <summary>Web server password</summary>
		[NonSerialized]
		private string webServerPswd_;

		/// <summary>Path to sportsmen data at Web server</summary>
		[NonSerialized]
		private string webSportsmanFolder_ = @"/ptcp/sportsman/";

		/// <summary>Path to sensor data at Web server</summary>
		[NonSerialized]
		private string webSensorFolder_ = @"/ptcp/sensor/";

		/// <summary>
		/// Path to data of link between sportsmen and sensors at Web server
		/// </summary>
		[NonSerialized]
		private string webSportsmanSensorFolder_ = @"/ptcp/sensorsportsmanlink/";

		/// <summary>Popt to get UDP packets from markers</summary>
		[NonSerialized]
		private int portUdpFromMarkers_;

		/// <summary>Popt to get UDP packets from pos system</summary>
		[NonSerialized]
		private int portUdpFromPosSystem_;

		/// <summary>Popt to get UDP packets from pos system</summary>
		[NonSerialized] 
		private int timeoutForMoving_ = 600000;			// 10 min

		/// <summary>Distance between sportsman and marker when we consider
		/// that sportaman reached it</summary>
		[NonSerialized]
		private int innaccuracySportsmanPos_ = 700;		// 70 sm

		/// <summary>Distance between marker and specified position when we consider
		/// that marker reached it</summary>
		[NonSerialized]
		private int innaccuracyMarkerPos_ = 50;		// 5 sm

		#endregion

		#region Properties

		/// <summary>Host name for RabbitMQ</summary>
		public string AmpqHost
		{
			get { return ampqHost_; }
			set { ampqHost_ = value; }
		}

		/// <summary>User name for RabbitMQ</summary>
		public string AmpqUser
		{
			get { return ampqUser_; }
			set { ampqUser_ = value; }
		}

		/// <summary>Password for RabbitMQ</summary>
		public string AmpqPswd
		{
			get { return ampqPswd_; }
			set { ampqPswd_ = value; }
		}

		/// <summary>Queue name for RabbitMQ</summary>
		public string AmpqQueueName
		{
			get { return ampqQueueName_; }
			set { ampqQueueName_ = value; }
		}

		/// <summary>Web server IP</summary>
		//public IPAddress WebServerIp
		public string WebServerIp
		{
			get { return webServerIp_; }
			set { webServerIp_ = value; }
		}

		/// <summary>Path to sportsmen data at Web server</summary>
		public string WebSportsmanFolder
		{
			get { return webSportsmanFolder_; }
			set { webSportsmanFolder_ = value; }
		}

		/// <summary>Path to sensor data at Web server</summary>
		public string WebSensorFolder
		{
			get { return webSensorFolder_; }
			set { webSensorFolder_ = value; }
		}

		/// <summary>
		/// Path to data of link between sportsmen and sensors at Web server
		/// </summary>
		public string WebSportsmanSensorLinkFolder
		{
			get { return webSportsmanSensorFolder_; }
			set { webSportsmanSensorFolder_ = value; }
		}

		/// <summary>Web server port</summary>
		public int WebServerPort
		{
			get { return webServerPort_; }
			set { webServerPort_ = value; }
		}

		/// <summary>Web server login</summary>
		public string WebServerUser
		{
			get { return webServerUser_; }
			set { webServerUser_ = value; }
		}

		/// <summary>Web server password</summary>
		public string WebServerPswd
		{
			get { return webServerPswd_; }
			set { webServerPswd_ = value; }
		}

		/// <summary>Path to XML tests</summary>
		public string XmlPath
		{
			get { return xmlPath_; }
			set { xmlPath_ = value; }
		}

		/// <summary>Popt to get UDP packets from markers</summary>
		public int PortUdpFromMarkers
		{
			get { return portUdpFromMarkers_; }
			set { portUdpFromMarkers_ = value; }
		}

		/// <summary>Popt to get UDP packets from pos system</summary>
		public int PortUdpFromPosSystem
		{
			get { return portUdpFromPosSystem_; }
			set { portUdpFromPosSystem_ = value; }
		}

		/// <summary>Popt to get UDP packets from pos system</summary>
		public int TimeoutForMoving
		{
			get { return timeoutForMoving_; }
			set { timeoutForMoving_ = value; }
		}

		/// <summary>Distance between sportsman and marker when we consider
		/// that sportaman reached it</summary>
		public int InnaccuracySportsmanPos
		{
			get { return innaccuracySportsmanPos_; }
			set { innaccuracySportsmanPos_ = value; }
		}

		/// <summary>Distance between marker and specified position when we consider
		/// that marker reached it</summary>
		public int InnaccuracyMarkerPos
		{
			get { return innaccuracyMarkerPos_; }
			set { innaccuracyMarkerPos_ = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor with defaut settings
		/// </summary>
		public SphSettings()
		{
			ampqHost_ = "rabbitmq";
			ampqUser_ = "admin";
			ampqPswd_ = "admin";
			ampqQueueName_ = "default_queue";
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Serializes the class to the config file if any of the settings have changed.
		/// </summary>
		public void SaveSettings()
		{
			StreamWriter myWriter = null;
			XmlSerializer mySerializer = null;
			try
			{
				// Create an XmlSerializer for the ApplicationSettings type.
				mySerializer = new XmlSerializer(typeof(SphSettings));
				myWriter = new StreamWriter(settingsFileName_, false);
				// Serialize this instance of the ApplicationSettings 
				// class to the config file.
				mySerializer.Serialize(myWriter, this);
				//settingsChanged_ = false;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Exception in SaveSettings():");
				throw;
			}
			finally
			{
				if (myWriter != null) myWriter.Close();
			}
		}

		/// <summary>
		/// Deserializes the class from the config file.
		/// </summary>
		public void LoadSettings()
		{
			XmlSerializer mySerializer = null;
			FileStream myFileStream = null;
			try
			{
				// Create an XmlSerializer for the ApplicationSettings type.
				mySerializer = new XmlSerializer(typeof(SphSettings));
				FileInfo fi = new FileInfo(settingsFileName_);
				// If the config file exists, open it.
				if (fi.Exists)
				{
					myFileStream = fi.OpenRead();
					// Create a new instance of the ApplicationSettings by
					// deserializing the config file.
					SphSettings myAppSettings = (SphSettings)mySerializer.Deserialize(myFileStream);
					// Assign the property values to this instance of 
					// the ApplicationSettings class.
					ampqHost_ = myAppSettings.ampqHost_;
					ampqUser_ = myAppSettings.ampqUser_;
					ampqPswd_ = myAppSettings.ampqPswd_;
					ampqQueueName_ = myAppSettings.ampqQueueName_;
					settingsFileName_ = myAppSettings.settingsFileName_;
					webSensorFolder_ = myAppSettings.webSensorFolder_;
					webServerIp_ = myAppSettings.webServerIp_;
					webServerPort_ = myAppSettings.webServerPort_;
					webServerPswd_ = myAppSettings.webServerPswd_;
					webServerUser_ = myAppSettings.webServerUser_;
					webSportsmanFolder_ = myAppSettings.webSportsmanFolder_;
					webSportsmanSensorFolder_ = myAppSettings.webSportsmanSensorFolder_;
					xmlPath_ = myAppSettings.xmlPath_;
					timeoutForMoving_ = myAppSettings.timeoutForMoving_;
					innaccuracyMarkerPos_ = myAppSettings.innaccuracyMarkerPos_;
					innaccuracySportsmanPos_ = myAppSettings.innaccuracySportsmanPos_;
				}
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Exception in LoadSettings(): ");
				throw;
			}
			finally
			{
				if (myFileStream != null) myFileStream.Close();
			}
		}

		/// <summary>
		/// Clone method
		/// </summary>
		/// <returns>Copy of this object</returns>
		public SphSettings Clone()
		{
			SphSettings obj = new SphSettings();
			obj.ampqHost_ = ampqHost_;
			obj.ampqUser_ = ampqUser_;
			obj.ampqPswd_ = ampqPswd_;
			obj.ampqQueueName_ = ampqQueueName_;
			obj.settingsFileName_ = settingsFileName_;
			obj.webSensorFolder_ = webSensorFolder_;
			obj.webServerIp_ = webServerIp_;
			obj.webServerPort_ = webServerPort_;
			obj.webServerPswd_ = webServerPswd_;
			obj.webServerUser_ = webServerUser_;
			obj.webSportsmanFolder_ = webSportsmanFolder_;
			obj.webSportsmanSensorFolder_ = webSportsmanSensorFolder_;
			obj.xmlPath_ = xmlPath_;
			obj.timeoutForMoving_ = timeoutForMoving_;
			obj.innaccuracyMarkerPos_ = innaccuracyMarkerPos_;
			obj.innaccuracySportsmanPos_ = innaccuracySportsmanPos_;
			return obj;
		}

		#endregion
	}
}
