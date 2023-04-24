using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SphServiceLib;

namespace DeviceIO
{
	class LightTimer : System.Timers.Timer
	{
		public IPAddress Ip;
		public MARKER_LIGHT_COLOR Color;
	}

	class MarkerConnector
	{
		private IPEndPoint endPoint_;
		private static int portOut_ = 51731;
		private Socket socket_;
		private SphSettings settings_;

		List<LightTimer> listLightTimers_ = new List<LightTimer>(); 

		private const short ANGLE_TO_MOVE = 30;

		public MarkerConnector(ref SphSettings settings)
		{
			// Create a UDP socket
			socket_ = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			settings_ = settings;
		}

		/// <summary>Turn the light for some period of time</summary>
		public bool TurnLightForTime(IPAddress ip, int seconds, MARKER_LIGHT_COLOR color)
		{
			try
			{
				TurnLightOnOff(ip, true, color);

				LightTimer timer = new LightTimer();
				timer.Ip = ip;
				timer.Color = color;
				timer.Elapsed += LightTimerFunc;
				timer.AutoReset = false;
				timer.Interval = seconds * 1000;
				timer.Start();
				listLightTimers_.Add(timer);
				return true;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in TurnLightForTime():");
				return false;
			}
		}

		private void LightTimerFunc(object obj, EventArgs e)
		{
			try
			{
				TurnLightOnOff(((LightTimer)obj).Ip, false, ((LightTimer)obj).Color);
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in LightTimerFunc():");
				throw;
			}
		}

		public bool TurnLightOnOff(IPAddress ip, bool on, MARKER_LIGHT_COLOR color)
		{
			try
			{
				COMMANDS_PC command;
				switch (color)
				{
					case MARKER_LIGHT_COLOR.BLUE:
						if (on) command = COMMANDS_PC.LedBlue_On;
						else command = COMMANDS_PC.LedBlue_Off;
						break;
					case MARKER_LIGHT_COLOR.GREEN:
						if (on) command = COMMANDS_PC.LedGreen_On;
						else command = COMMANDS_PC.LedGreen_Off;
						break;
					case MARKER_LIGHT_COLOR.RED:
						if (on) command = COMMANDS_PC.LedRed_On;
						else command = COMMANDS_PC.LedRed_Off;
						break;
					case MARKER_LIGHT_COLOR.YELLOW:
						if (on) command = COMMANDS_PC.LedYellow_On;
						else command = COMMANDS_PC.LedYellow_Off;
						break;
					default:
						SphService.WriteToLogFailed("TurnLightOnOff: invalid data!");
						return false;
				}

				SendPacket(ip, (short)command, null);
				return true;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in ManagerCore::TurnLightOnOff():");
				return false;
			}
		}

		public bool TurnMarker(IPAddress ip, int markerId, double angle)
		{
			try
			{
				//Для поворота на заданный угол вам нужно задать угловую скорость на заданное время. 
				//Допустим, его требуется повернуть на 90град. Для этого можно задать угловую скорость 
				//90град./сек на время 1 сек. Чтобы пересчитать угловую скорость в условные единицы для 
				//команды, эту угловую скорость нужно разделить на 0.00875. 
				//Итого имеем 90 / 0.00875 = 10285. Таким образом, команда поворота будет состоять 
				//из следующих полей:
				//1. PC_SPHERE__KeyLeftDown   или   PC_SPHERE__KeyRightDown
				//2. Угол наклона. В данном случае этот параметр приемным контроллером игнорируется.
				//3. Рассчитанная выше угловая скорость: 10285
				bool res;
				if (angle > 0)
					res = SendPacket(ip, (short)COMMANDS_PC.KeyRightDown, angle / 0.00875);
				else res = SendPacket(ip, (short)COMMANDS_PC.KeyLeftDown, angle / 0.00875);
				Thread.Sleep(1000);
				if (angle > 0)
					res = SendPacket(ip, (short)COMMANDS_PC.KeyRightUp, null);
				else res = SendPacket(ip, (short)COMMANDS_PC.KeyLeftUp, null);
				return res;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in ManagerCore::TurnMarker():");
				return false;
			}
		}

		public bool StopMarker(ref MarkerClass marker)
		{
			try
			{
				marker.IsMoving = !(SendPacket(marker.IpAddress, (short)COMMANDS_PC.KeyUpUp, null));
				return !marker.IsMoving;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in ManagerCore::StopMarker():");
				return false;
			}
		}

		// start the marker to the current direction
		public bool StartMarker(ref MarkerClass marker)
		{
			try
			{
				SendPacket(marker.IpAddress, (short)COMMANDS_PC.HORIZONT, 2);
				Thread.Sleep(500);
				marker.IsMoving = SendPacket(marker.IpAddress, (short)COMMANDS_PC.KeyUpDown, ANGLE_TO_MOVE);
				return marker.IsMoving;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in ManagerCore::StartMarker():");
				return false;
			}
		}

		private bool SendPacket(IPAddress ip, short command, object data)
		{
			try
			{
				byte[] sendbuf;
				endPoint_ = new IPEndPoint(ip, portOut_); //Создание конечной точки

				switch (command)
				{
					case (byte)COMMANDS_PC.KeyUpDown:
					case (byte)COMMANDS_PC.KeyDownDown:
						sendbuf = new byte[8];
						sendbuf[0] = 6;			//Число байт, младший байт
						sendbuf[1] = 0;			//Число байт, старший байт
						sendbuf[2] = (byte)command;			//Команда, младший байт
						sendbuf[3] = (byte)(command >> 8);		//Команда, старший байт
						sendbuf[4] = (byte)(short)data;				//Данные, младший байт
						sendbuf[5] = (byte)((short)data >> 8);		//Данные, старший байт
						sendbuf[6] = 0;		//Данные, младший байт (the 2nd parameter in not used)
						sendbuf[7] = 0;		//Данные, старший байт
						break;

					case (byte)COMMANDS_PC.KeyUpUp:
					case (byte)COMMANDS_PC.KeyDownUp:
					case (byte)COMMANDS_PC.KeyRightUp:
					case (byte)COMMANDS_PC.KeyLeftUp:
						sendbuf = new byte[6];
						sendbuf[0] = 2;			//Число байт, младший байт
						sendbuf[1] = 0;			//Число байт, старший байт
						sendbuf[2] = (byte)command;	//Команда, младший байт
						sendbuf[3] = (byte)(command >> 8); //Команда, старший байт
						sendbuf[4] = (byte)(short)data;				//Данные, младший байт
						sendbuf[5] = (byte)((short)data >> 8);		//Данные, старший байт
						break;

					case (byte)COMMANDS_PC.HORIZONT:
						sendbuf = new byte[4];
						sendbuf[0] = 2;			//Число байт, младший байт
						sendbuf[1] = 0;			//Число байт, старший байт
						sendbuf[2] = (byte)command;	//Команда, младший байт
						sendbuf[3] = (byte)(command >> 8); //Команда, старший байт
						break;

					case (byte)COMMANDS_PC.KeyRightDown:
					case (byte)COMMANDS_PC.KeyLeftDown:
						sendbuf = new byte[8];
						sendbuf[0] = 6;			//Число байт, младший байт
						sendbuf[1] = 0;			//Число байт, старший байт
						sendbuf[2] = (byte)command;			//Команда, младший байт
						sendbuf[3] = (byte)(command >> 8);		//Команда, старший байт
						sendbuf[4] = 0;		//Данные, младший байт (the 1st parameter in not used)
						sendbuf[5] = 0;		//Данные, старший байт
						sendbuf[6] = (byte)(short)data;				//Данные, младший байт
						sendbuf[7] = (byte)((short)data >> 8);		//Данные, старший байт
						break;

					//case (byte)COMMANDS_PC.KeyRightUp:
					//case (byte)COMMANDS_PC.KeyLeftUp:
					//    break;

					default:
						SphService.WriteToLogFailed("SendPacket: Unknown command " +
							command.ToString());
						return false;
				}

				int res = socket_.SendTo(sendbuf, endPoint_);
				SphService.WriteToLogGeneral(string.Format("Command {0} was sent {1}", command, DateTime.Now));
				if (res != sendbuf.Length)
				{
					SphService.WriteToLogFailed(string.Format("SendPacket: res {0} != numBytesToSend {1}",
						res, sendbuf.Length));
					return false;
				}

				// wait for answer
				//int answerType = WaitHandle.WaitAny(waitHandlesAnswer_);
				//if (answerType == (int)MARKER_ANSWER.COMMAND_OK)
				//    return true;
				//else
				//{
				//    SphService.WriteToLogFailed("SendPacket: MARKER_ANSWER.COMMAND_FAILED");
				//    return false;
				//}
				return true;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in SendPacket():");
				throw;
			}
		}

		public void ClearResources()
		{
			listLightTimers_.Clear();
		}
	}
}
