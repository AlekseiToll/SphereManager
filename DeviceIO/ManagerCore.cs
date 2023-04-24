using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using AMQPlib;
using SphServiceLib;
using XMLparserLib;

namespace DeviceIO
{
	public class ManagerCore
	{
		public delegate void CloseWaitFormHandler();
		public event CloseWaitFormHandler OnCloseWaitForm;

		private SphSettings settings_;
		private MarkerTest test_ = null;
		private SportsmanListClass sportsmanList_ = new SportsmanListClass();
		private MarkerListClass markerList_ = new MarkerListClass();
		private List<MarkerTimer> markerTimerList_ = new List<MarkerTimer>();

		// for "cycle" test
		private System.Timers.Timer timerTimeForCurrentLap_ = new System.Timers.Timer();
		// recovery between attempts
		//private System.Timers.Timer timerSportsmanRecovery_ = new System.Timers.Timer();
		// pause between laps
		//private System.Timers.Timer timerSportsmanPause_ = new System.Timers.Timer();

		// for "point" test
		//private List<StopWatch> listStopWatch_ = new List<StopWatch>();
		private StopWatchClass stopwatch_;

		//////////////events///////////////////
		//private AutoResetEvent evSportsmanRecovery_ = new AutoResetEvent(false);
		//private AutoResetEvent evSportsmanPause_ = new AutoResetEvent(false);
		private WaitHandle[] waitHandlesTestEvents_ = new WaitHandle[]
		    {
		        new AutoResetEvent(false),	// SPORTSMAN_REACH_LAP_FINISH
				new AutoResetEvent(false)	// LAP_TIME_IS_UP
		    };

		private WaitHandle[] waitHandlesAnswer_ = new WaitHandle[]
		    {
		        new AutoResetEvent(false),	// COMMAND_OK
				new AutoResetEvent(false)	// COMMAND_FAILED
		    };

		private AutoResetEvent evMarkerMovingFinished_ = new AutoResetEvent(false);

		private AutoResetEvent evSportsmanFinished_ = new AutoResetEvent(false);
		///////////////////////////////////////

		public delegate void AddMarkerHandler(int markerId, IPAddress ip);
		public event AddMarkerHandler OnAddMarker;

		private MarkerConnector connector_;

		// for receiving packets from markers
		//private static int portIn_ = 51722;

		// for receiving packets from positioning system
		//private static int portInXY_ = 51722;

		private bool bStopListening_ = false;
		private bool bStopMovingAndTest_ = false;

		// listening thread
		private BackgroundWorker bwListen_;
		private BackgroundWorker bwListenXY_;

		//private List<MarkerClass> listMarkers_ = new List<MarkerClass>();
		//private object lockListMarkerId_ = new object();

		#region Constructor

		public ManagerCore(ref SphSettings settings)
		{
			settings_ = settings;
			connector_ = new MarkerConnector(ref settings_);
		}

		public void SetSettings(ref SphSettings settings)
		{
			settings_ = settings;
		}

		#endregion

		// start receiving packets from markers
		public void StartListenMarkers()
		{
			try
			{
				// start the listening thread
				bwListen_ = new BackgroundWorker();
				//bwListen_.WorkerReportsProgress = true;
				bwListen_.WorkerSupportsCancellation = true;
				bwListen_.DoWork += bwListenMarkers_DoWork;
				//bwListen_.ProgressChanged += bwListen_ProgressChanged;
				//bwListen_.RunWorkerCompleted += bwListen_RunWorkerCompleted;
				bwListen_.RunWorkerAsync();
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in StartListenMarkers():");
				throw;
			}
		}

		// start receiving packets from positioning system
		public void StartListenXY()
		{
			try
			{
				// Create a UDP socket
				//socketXY_ = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

				// start the listening thread
				bwListenXY_ = new BackgroundWorker();
				//bwListenXY_.WorkerReportsProgress = true;
				bwListenXY_.WorkerSupportsCancellation = true;
				bwListenXY_.DoWork += bwListenXY_DoWork;
				//bwListenXY_.ProgressChanged += bwListen_ProgressChanged;
				//bwListenXY_.RunWorkerCompleted += bwListen_RunWorkerCompleted;
				bwListenXY_.RunWorkerAsync();
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in ManagerCore::StartListenXY():");
				throw;
			}
		}

		public void StopListening()
		{
			bStopListening_ = true;
		}

		public bool MoveMarkersToStartPositions(ref DoWorkEventArgs e)
		{
			bStopMovingAndTest_ = false;
			try
			{
				e.Result = true;
				// when we opened the test we checked that the count of real markers is
				// not less than the count of markers in the test. so now we can accept 
				// it as a fact that realMarkersCount = test_.Markers.Count
				int realMarkersCount = 0;

				// send commands to markers to move them to start positions
				for (int iMarker = 0; iMarker < test_.Markers.Count; ++iMarker)
				{
					if (realMarkersCount >= markerList_.Count)
					{
						SphService.WriteToLogFailed(
							"MoveMarkersToStartPositions: realMarkersCount >= markerList_.Count");
						return false;
					}

					if (!test_.Markers[iMarker].IsGate)
					{
						MarkerClass curMarker = markerList_[realMarkersCount];
						curMarker.XmlMarker = test_.Markers[iMarker];
						curMarker.RequiredX = test_.Markers[iMarker].PosX;
						curMarker.RequiredY = test_.Markers[iMarker].PosY;

						if (Math.Abs(curMarker.RequiredX - curMarker.CurrentX) > 100 ||
						    Math.Abs(curMarker.RequiredY - curMarker.CurrentY) > 100) // 10 sm
						{
							if (!Move(curMarker.Id, curMarker.RequiredX, curMarker.RequiredY))
								if (!Move(curMarker.Id, curMarker.RequiredX, curMarker.RequiredY))
								{
									SphService.WriteToLogFailed("Error while moving marker " + curMarker.Id);
									e.Result = false;
									continue;
								}
						}

						realMarkersCount++;
					}
					else // is gate
					{
						MarkerClass curMarker1 = markerList_[realMarkersCount];
						MarkerClass curMarker2 = markerList_[realMarkersCount + 1];
						curMarker1.XmlMarker = test_.Markers[iMarker];
						curMarker1.RequiredX = test_.Markers[iMarker].Gate1X;
						curMarker1.RequiredY = test_.Markers[iMarker].Gate1Y;
						curMarker1.CoordRevativelyCenterOgGateX =
							test_.Markers[iMarker].Gate1X - test_.Markers[iMarker].PosX;
						curMarker1.CoordRevativelyCenterOgGateY =
							test_.Markers[iMarker].Gate1Y - test_.Markers[iMarker].PosY;

						curMarker2.XmlMarker = test_.Markers[iMarker];
						curMarker2.RequiredX = test_.Markers[iMarker].Gate2X;
						curMarker2.RequiredY = test_.Markers[iMarker].Gate2Y;
						curMarker2.CoordRevativelyCenterOgGateX =
							test_.Markers[iMarker].Gate2X - test_.Markers[iMarker].PosX;
						curMarker2.CoordRevativelyCenterOgGateY =
							test_.Markers[iMarker].Gate2Y - test_.Markers[iMarker].PosY;
						curMarker2.IsSecondGoalpost = true;

						if (Math.Abs(curMarker1.RequiredX - curMarker1.CurrentX) > 100 ||
							Math.Abs(curMarker1.RequiredY - curMarker1.CurrentY) > 100) // 10 sm
						{
							if (!Move(curMarker1.Id, curMarker1.RequiredX, curMarker1.RequiredY))
								if (!Move(curMarker1.Id, curMarker1.RequiredX, curMarker1.RequiredY))
								{
									SphService.WriteToLogFailed("Error while moving marker " + curMarker1.Id);
									e.Result = false;
									continue;
								}
						}
						if (Math.Abs(curMarker2.RequiredX - curMarker2.CurrentX) > 100 ||
							Math.Abs(curMarker2.RequiredY - curMarker2.CurrentY) > 100) // 10 sm
						{
							if (!Move(curMarker2.Id, curMarker2.RequiredX, curMarker2.RequiredY))
								if (!Move(curMarker2.Id, curMarker2.RequiredX, curMarker2.RequiredY))
								{
									SphService.WriteToLogFailed("Error while moving marker " + curMarker2.Id);
									e.Result = false;
									continue;
								}
						}

						realMarkersCount += 2;
					}

					if (bStopMovingAndTest_)
					{
						e.Cancel = true;
						if (OnCloseWaitForm != null)
							OnCloseWaitForm();
						return true;
					}
				}
				if (OnCloseWaitForm != null)
					OnCloseWaitForm();
				return true;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in MoveMarkersToStartPositions():");
				throw;
			}
		}

		public bool Move(int markerId, int x, int y)
		{
			try
			{
				MarkerClass marker = markerList_.GetMarkerById(markerId);
				if (marker == null) return false;

				// at first we must store coordinates at start to be able to
				// check direction later
				marker.StartX = marker.CurrentX;
				marker.StartY = marker.CurrentY;
				// then set required coordinates
				marker.RequiredX = x;
				marker.RequiredY = y;
				connector_.StartMarker(ref marker);
				// wait for finish
				return evMarkerMovingFinished_.WaitOne(settings_.TimeoutForMoving);
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in ManagerCore::Move():");
				return false;
			}
		}

		/// <summary>
		/// Listen packet from markers
		/// </summary>
		private void bwListenMarkers_DoWork(object sender, DoWorkEventArgs e)
		{
			uint numBytes, cmd;
			int markerId;

			UdpClient listener = new UdpClient(settings_.PortUdpFromMarkers);
			IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, settings_.PortUdpFromMarkers);

			SphService.WriteToLogGeneral("Begin to listen to markers");
			while (!bStopListening_)
			{
				try
				{
					byte[] bytes = listener.Receive(ref groupEP); // Ожидание входящего пакета

					numBytes = (uint) bytes[0] + (((uint) bytes[1]) << 8);	//Число полученных байт
					cmd = (uint) bytes[2] + (((uint) bytes[3]) << 8);		//Команда

					switch (cmd)
					{
						case (uint) COMMANDS_MARKER.NODE_INFO:
							uint ip = (uint) bytes[4] + (((uint) bytes[5]) << 8) +
							          (((uint) bytes[6]) << 16) +
							          (((uint) bytes[7]) << 24);			//IP адрес

							IPAddress receivedIP = new IPAddress(ip);		//Полученный IP

							//Уникальный номер шара
							markerId = bytes[8] + (bytes[9] << 8);
							
							// if marker doesn't exist
							if(markerList_.GetMarkerById(markerId) == null)
							{
								SphService.WriteToLogGeneral(
									string.Format("Packet from new marker was received: {0}, {1}", 
									markerId, receivedIP));
								//lock (lockListMarkerId_)
								//{
								markerList_.AddMarker(markerId, receivedIP);
								//}
								// inform main window
								if (OnAddMarker != null) OnAddMarker(markerId, receivedIP);
							}

							#region Other fields
							//if (ReceivedIP.Equals(SelectedIP))
							//{

							// прием текущих данных ///////////////////////////////////////
							// Номер выборки. Используется для оси Х при отображении графиков.
							//uint sampleNumber = (uint) bytes[10] + (((uint) bytes[11]) << 8) + 
							//                        (((uint) bytes[12]) << 16) +
							//                        (((uint) bytes[13]) << 24);
							//"Сырые" данные от акселерометра и гироскопа:
							//short accX = (short) ((int) bytes[14] + (((int) bytes[15]) << 8));
							//short accY = (short)((int)bytes[16] + (((int)bytes[17]) << 8));
							//short SetAngleRotccZ = (short)((int)bytes[18] + (((int)bytes[19]) << 8));
							//short gyroX = (short)((int)bytes[20] + (((int)bytes[21]) << 8));
							//short gyroY = (short)((int)bytes[22] + (((int)bytes[23]) << 8));
							//short gyroZ = (short)((int)bytes[24] + (((int)bytes[25]) << 8));
							//short alfaXRes = (short)((int)bytes[26] + (((int)bytes[27]) << 8));
							//short alfaYRes = (short)((int)bytes[28] + (((int)bytes[29]) << 8));
							//uint flags = (uint) bytes[30] + (((uint) bytes[31]) << 8) + (((uint) bytes[32]) << 16) +
							//       (((uint) bytes[33]) << 24); //Текущие флаги
							//short curr1 = (short) ((int) bytes[34] + (((int) bytes[35]) << 8)); //Ток1
							//short curr2 = (short) ((int) bytes[36] + (((int) bytes[37]) << 8)); //Ток2
							//Заданная скорость движения
							//short setSpeed = (short) ((int) bytes[38] + (((int) bytes[39]) << 8));
							// Заданная угловая скорость поворота
							//short setSpeedRot = (short)((int)bytes[40] + (((int)bytes[41]) << 8));
							//Текущая скорость движения
							//short realSpeed = (short) ((int) bytes[42] + (((int) bytes[43]) << 8));
							//Текущая скорость поворота
							//short realSpeedRot = (short)((int)bytes[44] + (((int)bytes[45]) << 8));
							//Заданный угол поворота
							//short setAngleRot = (short) ((int) bytes[46] + (((int) bytes[47]) << 8));
							//Оставшийся угол поворота
							//short residueAngleRot = (short)((int)bytes[48] + (((int)bytes[49]) << 8));					
							//}
							#endregion
							break;

						case (uint) COMMANDS_MARKER.CONFIRM: //Подтвержение исолнения команды
							((AutoResetEvent)waitHandlesAnswer_[(int)MARKER_ANSWER.COMMAND_OK]).Set();
							//markerId = bytes[bytesCnt++] + (bytes[bytesCnt++] << 8); //Номер шара
							SphService.WriteToLogGeneral("Received COMMAND_OK from marker " + DateTime.Now);
							break;

						case (uint) COMMANDS_MARKER.BAD_COMMAND: //Не распознанная команда
							((AutoResetEvent)waitHandlesAnswer_[(int)MARKER_ANSWER.COMMAND_FAILED]).Set();
							//markerId = bytes[bytesCnt++] + (bytes[bytesCnt++] << 8); //Номер шара
							SphService.WriteToLogFailed("Received BAD_COMMAND from marker " + DateTime.Now);
							break;

						#region COMMANDS_MARKER.OPTIONS
						//case (uint) COMMANDS_MARKER.OPTIONS: //Массив с настройками
						//    markerId = bytes[0] + (bytes[1] << 8); //Номер шара
						//    BallIdx = FindBallIndex(markerId);
						//    if (BallIdx != -1)
						//    {
						//        //Распаковка полученного массива:
						//        //bytesCnt++;
						//        Kp[BallIdx] = (int) bytes[2] + (((int) bytes[3]) << 8);
						//        Ki[BallIdx] = (int) bytes[4] + (((int) bytes[5]) << 8);

						//        //Дальше настройки заносятся НЕ в массивы, а просто в переменные. Если понадобится хранить настройки ВСЕХ шаров, то эти переменные всегда можно заменить на массивы
						//        //Калибровочные значения:
						//        CalibrationAccX = (short) ((int) bytes[6] + (((int) bytes[7]) << 8));
						//        CalibrationAccY = (short) ((int) bytes[8] + (((int) bytes[9]) << 8));
						//        CalibrationAccZ = (short) ((int) bytes[10] + (((int) bytes[11]) << 8));
						//        CalibrationGyroX = (short) ((int) bytes[12] + (((int) bytes[13]) << 8));
						//        CalibrationGyroY = (short) ((int) bytes[14] + (((int) bytes[15]) << 8));
						//        CalibrationGyroZ = (short) ((int) bytes[16] + (((int) bytes[17]) << 8));
						//        StatusFlags = ((int) bytes[18] + (((int) bytes[19]) << 8));
						//        //Флаг состояния (который запоминается в EEPROM)

						//        KpRot = (short) ((int) bytes[20] + (((int) bytes[21]) << 8));
						//        KiRot = (short) ((int) bytes[22] + (((int) bytes[23]) << 8));

						//        KGYRO = (short) ((int) bytes[24] + (((int) bytes[25]) << 8));
						//        BallAddr = (short) ((int) bytes[26] + (((int) bytes[27]) << 8));
						//        //Сюда будут добавляться настройки по мере их появления
						//    }
						//    break;
						#endregion

						default:
							SphService.WriteToLogFailed("Unknown marker command: " + cmd);
							break;
					}
				}
				catch (Exception ex)
				{
					SphService.DumpException(ex, "Error in bwListenMarkers_DoWork():");
				}
			}
		}

		private void bwListenXY_DoWork(object sender, DoWorkEventArgs e)
		{
			//uint numBytes;
			//int markerId;

			UdpClient listener = new UdpClient(settings_.PortUdpFromPosSystem);
			IPEndPoint ep = new IPEndPoint(IPAddress.Any, settings_.PortUdpFromPosSystem);

			SphService.WriteToLogGeneral("Begin to listen to the pos system");
			while (!bStopListening_)
			{
				try
				{
					byte[] bytes = listener.Receive(ref ep); // waiting for incoming packet

					if(!BitConverter.IsLittleEndian)
						typeof(BitConverter).GetField("IsLittleEndian").SetValue(null, true);
					//ByteBuffer buf = ByteBuffer.wrap(bytes).order(ByteOrder.LITTLE_ENDIAN);

					PosDataMessage msg = new PosDataMessage();
					try
					{
						msg.Timestamp = BitConverter.ToInt32(bytes, 0);
						msg.TpId = BitConverter.ToInt16(bytes, 4);
						msg.Id = (short)(msg.TpId & 0x3F);
						// to do: transponder id and correctness flag
						msg.X = BitConverter.ToInt32(bytes, 12);
						msg.Y = BitConverter.ToInt32(bytes, 16);
						#region unused fields
						//msg.Z = BitConverter.ToInt32(bytes, 20);
						//if (bytes.Length > 24)
						//{
						//    msg.Speed3dX = BitConverter.ToInt16(bytes, 24);
						//    msg.Speed3dY = BitConverter.ToInt16(bytes, 26);
						//    msg.Speed3dZ = BitConverter.ToInt16(bytes, 28);
						//    msg.Accel3dX = BitConverter.ToInt16(bytes, 30);
						//    msg.Accel3dY = BitConverter.ToInt16(bytes, 32);
						//    msg.Accel3dZ = BitConverter.ToInt16(bytes, 34);
						//    msg.RawX = BitConverter.ToInt32(bytes, 36);
						//    msg.RawY = BitConverter.ToInt32(bytes, 40);
						//    msg.RawZ = BitConverter.ToInt32(bytes, 44);
						//}
						#endregion

						SetMarkerCoordinates(ref msg);
					}
					catch (Exception exx)
					{
						SphService.DumpException(exx, "bwListenXY_DoWork() failed conversion:");
					}
				}
				catch (Exception ex)
				{
					SphService.DumpException(ex, "Error in bwListenXY_DoWork():");
				}
			}
		}

		private bool SetMarkerCoordinates(ref PosDataMessage msg)
		{
			try
			{
				MarkerClass curMarker = markerList_.GetMarkerById(msg.Id);
				if (curMarker != null)
				{
					curMarker.CurrentX = msg.X;
					curMarker.CurrentY = msg.Y;

					if (CheckIfMarkerInFinishPosition(ref curMarker))
					{
						if (curMarker.IsMoving)
						{
							connector_.StopMarker(ref curMarker);
							evMarkerMovingFinished_.Set();
						}
					}
					else
						CheckAndCorrectDirection(ref curMarker);
				}
				else
				{
					SphService.WriteToLogFailed("SetMarkerCoordinates: Unable to find marker!" +
						msg.Id.ToString());
					return false;
				}
				return true;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in SetMarkerCoordinates():");
				return false;
			}
		}

		private bool CheckIfMarkerInFinishPosition(ref MarkerClass marker)
		{
			return Math.Abs(marker.CurrentX - marker.RequiredX) < settings_.InnaccuracyMarkerPos &&
					Math.Abs(marker.CurrentY - marker.RequiredY) < settings_.InnaccuracyMarkerPos;
		}

		/// <summary>
		/// Check direction and correct it if it's necessary
		/// </summary>
		private bool CheckAndCorrectDirection(ref MarkerClass marker)
		{
			try
			{
				// my formula:
				// yc = (yf(xc - x1) - y1(xc - xf)) / (xf - x1)
				// 1 - at the start, c - current, f - finish

				int yc = (marker.RequiredY * (marker.CurrentX - marker.StartX) -
						  marker.StartY * (marker.CurrentX - marker.RequiredX)) /
						 (marker.RequiredX - marker.StartX);

				if (Math.Abs(yc - marker.CurrentY) < 200) // direction is right (diff 20 sm)
					return true;

				// the angle is acute if yc is between start and finish
				// the angle is obtuse if marker is moving to the opposite side
				bool acuteAngle = (yc > marker.StartY && yc < marker.RequiredY) ||
				                  (yc < marker.StartY && yc > marker.RequiredY);


				// if direction is false, correct it
				double angle;
				bool res = false;
				connector_.StopMarker(ref marker);  // we must stop before calculation!
				if (CalculateAngle(ref marker, out angle, acuteAngle))
				{
					res = connector_.TurnMarker(marker.IpAddress, marker.Id, angle);
				}
				connector_.StartMarker(ref marker);

				return res;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in CheckAndCorrectDirection():");
				return false;
			}
		}

		private bool CalculateAngle(ref MarkerClass marker, out double angle, bool acuteAngle)
		{
			angle = 0;
			try
			{
				if (acuteAngle)
				{
					double angle_b = Math.Atan(
						(marker.CurrentY - marker.StartY) / 
						(marker.CurrentX - marker.StartX));
					double angle_g = Math.Atan(
						(marker.RequiredY - marker.CurrentY) / 
						(marker.RequiredX - marker.CurrentX));
					angle = angle_g - angle_b;
				}
				else
				{
					double angle_b = Math.Atan(
						(marker.RequiredX - marker.CurrentX) /
						(marker.RequiredY - marker.CurrentY));
					double angle_g = Math.Atan(
						(marker.CurrentX - marker.StartX) /
						(marker.CurrentY - marker.StartY));
					angle = 180 - angle_g - angle_b;
				}

				// from the forum:
				// Все тригонометрические методы в C# считают углы в радианах. 
				// Поэтому если хотите вводить величину угла в градусах, то затем делите 
				// его на 57.3 перед вычислениями.
				angle /= 57.3;
				SphService.WriteToLogGeneral("CalculateAngle: res = " + angle);
				return true;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in CalculateAngle():");
				return false;
			}
		}

		public void ProcessNewAMQPpacket(ref AMQPpacket packet)
		{
			try
			{
				sportsmanList_.ChangePos(packet.IdSportsman, packet.CoordinateValue,
				                         packet.CoordinateType);

				SportsmanClass curMan = sportsmanList_.GetSportsmanById(packet.IdSportsman);
				if (curMan == null)
				{
					SphService.WriteToLogFailed("ampq: Unknown sportsman id = " +
						packet.IdSportsman);
					return;
				}

				for (int iMarker = 0; iMarker < markerList_.Count; ++iMarker)
				{
					MarkerClass curMarker = markerList_[iMarker];
					for (int iAct = 0; iAct < curMarker.XmlMarker.MarkerActions.Count; ++iAct)
					{
						if (curMarker.XmlMarker.MarkerActions[iAct].Activity ==
						    MarkerActivity.ZONE)
						{
							if (Math.Abs(curMan.X - curMarker.CurrentX) < settings_.InnaccuracySportsmanPos ||
								Math.Abs(curMan.Y - curMarker.CurrentY) < settings_.InnaccuracySportsmanPos)
							{
								// turn the light on
								MARKER_LIGHT_COLOR color = ConvertColorToMarkerColor(
									curMarker.XmlMarker.MarkerActions[iAct].NewColor);
								connector_.TurnLightForTime(curMarker.IpAddress, 5, color);
							}
						}

						if (test_.Type == MarkerTestType.POINT)
						{
							if (curMarker.XmlMarker.MarkerActions[iAct].Stopwatch)
							{
								if (curMarker.XmlMarker.MarkerActions[iAct].StopwatchStart)
								{
									stopwatch_.StartTime = DateTime.Now;
									stopwatch_.StartMarkerId = curMarker.Id;
								}
								else
								{
									stopwatch_.EndTime = DateTime.Now;
									stopwatch_.EndMarkerId = curMarker.Id;
									if (stopwatch_.StartTime != DateTime.MinValue &&
									    stopwatch_.StartMarkerId != -1)
										stopwatch_.Enabled = true;
								}
							}
						}
					}
				}

				int x, y;
				test_.GetCoordStartFinishMarker(out x, out y);
				if (Math.Abs(curMan.X - x) < settings_.InnaccuracySportsmanPos ||
				    Math.Abs(curMan.Y - y) < settings_.InnaccuracySportsmanPos)
				{
					// sportsman has reached the finish
					if (test_.Type == MarkerTestType.CYCLE)
					{
						((AutoResetEvent) waitHandlesTestEvents_[
							(int) TEST_EVENT.SPORTSMAN_REACH_LAP_FINISH]).Set();
					}
					else if (test_.Type == MarkerTestType.POINT)
					{
						evSportsmanFinished_.Set();
					}
				}
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in ProcessNewAMQPpacket():");
				throw;
			}
		}

		//public bool AddMarker(int id)
		//{
		//    return markerList_.AddMarker(id);
		//}

		public bool DeleteMarker(int id)
		{
			return markerList_.DeleteMarker(id);
		}

		//public MarkerClass GetMarkerById(int id)
		//{
		//    return markerList_.GetMarkerById(id);
		//}

		public void SetTest(ref MarkerTest test)
		{
			test_ = test;
		}

		public MarkerTestType GetTestType()
		{
			if (test_ != null) return test_.Type;
			return MarkerTestType.NONE;
		}

		public bool ExecuteTest(ref DoWorkEventArgs e)
		{
			settings_.LoadSettings();
			bStopMovingAndTest_ = false;
			if (test_.Type == MarkerTestType.CYCLE)
				e.Result = (int)0;
			else if (test_.Type == MarkerTestType.POINT)
				e.Result = null;

			try
			{
				markerTimerList_.Clear();
				connector_.ClearResources();
				stopwatch_.Reset();

				for (int iMarker = 0; iMarker < markerList_.Count; ++iMarker)
				{
					if (!markerList_[iMarker].IsSecondGoalpost)
					{
						MarkerClass curMarker = markerList_[iMarker];
						for (int iAct = 0; iAct < curMarker.XmlMarker.MarkerActions.Count; ++iAct)
						{
							// start timer to measure off time from start of the test 
							// to switch the marker on
							if (curMarker.XmlMarker.MarkerActions[iAct].Activity ==
							    MarkerActivity.TIME &&
							    curMarker.XmlMarker.MarkerActions[iAct].ActivateTime ==
							    MarkerActivateTime.COMMON)
							{
								MarkerTimer timer = new MarkerTimer();
								timer.MarkerAction = curMarker.XmlMarker.MarkerActions[iAct];
								timer.Marker = curMarker;
								timer.Elapsed += MarkerTimerFunc;
								timer.AutoReset = false;
								timer.Interval = curMarker.XmlMarker.MarkerActions[iAct].CommonTime * 1000;
								timer.Start();
								markerTimerList_.Add(timer);
							}
						}
					}
				}

				if (test_.Type == MarkerTestType.CYCLE)
				{
					int lapsCount = 0;
					float curSpeed = test_.StartLapSpeed;
					float curTime = test_.StartLapTime;  // time in which sportsman has to pass this lap
					// if user set speed we must calculate time at first
					if (test_.UserSetValue == UserSetValue.SPEED)
					{
						curTime = (float) (test_.LapDistance / curSpeed);
					}
					bool ok;

					for (int iAttempt = 0; iAttempt < test_.TestActions.Count; ++iAttempt)
					{
						lapsCount = 0;
						SphService.WriteToLogGeneral("Cycle test, attempt " + iAttempt);
						ok = true;
						// turn on green light for 5 sec
						for (int iMarker = 0; iMarker < markerList_.Count; ++iMarker)
						{
							connector_.TurnLightForTime(markerList_[iMarker].IpAddress,
										5, MARKER_LIGHT_COLOR.GREEN);
						}

						while (ok)
						{
							SphService.WriteToLogGeneral("Cycle test, lap " + lapsCount);
							for (int iMarker = 0; iMarker < markerList_.Count; ++iMarker)
							{
								if (!markerList_[iMarker].IsSecondGoalpost)
								{
									MarkerClass curMarker = markerList_[iMarker];
									for (int iAct = 0; iAct < curMarker.XmlMarker.MarkerActions.Count; ++iAct)
									{
										// start timer to measure off time from start of the test 
										// to switch the marker on
										if (curMarker.XmlMarker.MarkerActions[iAct].Activity ==
										    MarkerActivity.TIME &&
										    curMarker.XmlMarker.MarkerActions[iAct].ActivateTime ==
										    MarkerActivateTime.LAP)
										{
											MarkerTimer timer = new MarkerTimer();
											timer.MarkerAction = curMarker.XmlMarker.MarkerActions[iAct];
											timer.Marker = curMarker;
											timer.Elapsed += MarkerTimerFunc;
											timer.AutoReset = false;
											timer.Interval = curMarker.XmlMarker.MarkerActions[iAct].LapTime * 1000;
											timer.Start();
											markerTimerList_.Add(timer);
										}
									}
								}
							}

							// start timer for curTime
							timerTimeForCurrentLap_.Elapsed += TimeForCurrentLapTimerFunc;
							timerTimeForCurrentLap_.AutoReset = false;
							timerTimeForCurrentLap_.Interval = curTime * 1000;
							timerTimeForCurrentLap_.Start();

							int testEvent = WaitHandle.WaitAny(waitHandlesTestEvents_);
							switch (testEvent)
							{
								case (int) TEST_EVENT.SPORTSMAN_REACH_LAP_FINISH:
									lapsCount++;
									timerTimeForCurrentLap_.Stop();
									break;
								case (int) TEST_EVENT.LAP_TIME_IS_UP:
									ok = false;
									break;
								default:
									SphService.WriteToLogFailed("StartTest: Unknown test event! " +
									                            testEvent.ToString());
									break;
							}

							if (ok)
							{
								// recalc curTime
								if (test_.UserSetValue == UserSetValue.SPEED)
								{
									curSpeed += test_.IncreaseLapSpeed;
									curTime = (float)(test_.LapDistance / curSpeed);
								}
								else
								{
									curTime -= test_.LapTimeDecrease;
								}
								SphService.WriteToLogGeneral("New value of curTime is " + curTime);
							}

							// start timer for sportsman pause
							SphService.WriteToLogGeneral("Start sportsman pause");
							Thread.Sleep(test_.TestActions[iAttempt].PauseInSec * 1000);
							//timerSportsmanPause_.Elapsed += PauseTimerFunc;
							//timerSportsmanPause_.AutoReset = false;
							//timerSportsmanPause_.Interval = test_.TestActions[iAttempt].PauseInSec * 1000;
							//timerSportsmanPause_.Start();
							//evSportsmanPause_.WaitOne();
							SphService.WriteToLogGeneral("End sportsman pause");
						}

						if (bStopMovingAndTest_)
						{
							e.Cancel = true;
							return false;
						}

						if (!ok)
						{
							SphService.WriteToLogGeneral("Start sportsman recovery");
							// turn on red light
							for (int iMarker = 0; iMarker < markerList_.Count; ++iMarker)
							{
								connector_.TurnLightForTime(markerList_[iMarker].IpAddress,
											test_.TestActions[iAttempt].PauseInSec - 5, 
											MARKER_LIGHT_COLOR.RED);
							}

							// start timer for sportsman recovery
							SphService.WriteToLogGeneral("Start sportsman recovery");
							Thread.Sleep((int)test_.TimeToRecovery * 1000);
							//timerSportsmanRecovery_.Elapsed += RecoveryTimerFunc;
							//timerSportsmanRecovery_.AutoReset = false;
							//timerSportsmanRecovery_.Interval = test_.TimeToRecovery * 1000;
							//timerSportsmanRecovery_.Start();
							//evSportsmanRecovery_.WaitOne();
							SphService.WriteToLogGeneral("End sportsman recovery");
						}

						if (bStopMovingAndTest_)
						{
							e.Cancel = true;
							return false;
						}
					}
					e.Result = lapsCount;
				}
				else if(test_.Type == MarkerTestType.POINT)
				{
					SphService.WriteToLogGeneral("Point test start");
					// turn on green light for 5 sec
					for (int iMarker = 0; iMarker < markerList_.Count; ++iMarker)
					{
						connector_.TurnLightForTime(markerList_[iMarker].IpAddress,
									5, MARKER_LIGHT_COLOR.GREEN);
					}

					// save start time
					DateTime startTime = DateTime.Now;

					evSportsmanFinished_.WaitOne();

					// save start time
					DateTime endTime = DateTime.Now;

					if (bStopMovingAndTest_)
					{
						e.Cancel = true;
						return false;
					}

					if (bStopMovingAndTest_)
					{
						e.Cancel = true;
						return false;
					}

					ResultPointToPointTest res = new ResultPointToPointTest();
					if (stopwatch_.Enabled)
						res.StopWatch = stopwatch_;
					res.DistanceTime = endTime - startTime;
					e.Result = res;
				}

				return true;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in ExecuteTest():");
				return false;
			}
		}

		public void BreakTest()
		{
			bStopMovingAndTest_ = true;
		}

		private void TimeForCurrentLapTimerFunc(object obj, EventArgs e)
		{
			try
			{
				((AutoResetEvent)waitHandlesTestEvents_[(int)TEST_EVENT.LAP_TIME_IS_UP]).Set();
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in RecoveryTimerFunc():");
				throw;
			}
		}

		//private void PauseTimerFunc(object obj, EventArgs e)
		//{
		//    try
		//    {
		//        evSportsmanPause_.Set();
		//    }
		//    catch (Exception ex)
		//    {
		//        SphService.DumpException(ex, "Error in PauseTimerFunc():");
		//        throw;
		//    }
		//}

		//private void RecoveryTimerFunc(object obj, EventArgs e)
		//{
		//    try
		//    {
		//        evSportsmanRecovery_.Set();
		//    }
		//    catch (Exception ex)
		//    {
		//        SphService.DumpException(ex, "Error in RecoveryTimerFunc():");
		//        throw;
		//    }
		//}

		private void MarkerTimerFunc(object obj, EventArgs e)
		{
			try
			{
				MarkerActionClass action = ((MarkerTimer) obj).MarkerAction;
				if (action.ChangeColor)
				{
					MARKER_LIGHT_COLOR color = ConvertColorToMarkerColor(action.NewColor);
					connector_.TurnLightOnOff(((MarkerTimer) obj).Marker.IpAddress, true, color);
				}
				if (action.ChangePos)
				{
					if (!((MarkerTimer) obj).Marker.XmlMarker.IsGate)
					{
						Move(((MarkerTimer) obj).Marker.Id, action.NewX, action.NewY);
					}
					else
					{
						Move(((MarkerTimer)obj).Marker.Id,
							action.NewX + ((MarkerTimer)obj).Marker.CoordRevativelyCenterOgGateX,
							action.NewY + ((MarkerTimer)obj).Marker.CoordRevativelyCenterOgGateY);
						MarkerClass secondGoalPost = markerList_.FindSecondGoalpost(((MarkerTimer) obj).Marker.Id);
						if (secondGoalPost != null)
						{
							Move(secondGoalPost.Id,
									action.NewX + secondGoalPost.CoordRevativelyCenterOgGateX,
									action.NewY + secondGoalPost.CoordRevativelyCenterOgGateY);
						}
						else SphService.WriteToLogFailed("MarkerTimerFunc: secondGoalPost = null");
					}
				}
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in MarkerTimerFunc():");
				throw;
			}
		}

		private MARKER_LIGHT_COLOR ConvertColorToMarkerColor(Color color)
		{
			MARKER_LIGHT_COLOR res = MARKER_LIGHT_COLOR.GREEN; // by default
			if (color == Color.Red) res = MARKER_LIGHT_COLOR.RED;
			else if (color == Color.Blue) res = MARKER_LIGHT_COLOR.BLUE;
			else if (color == Color.Yellow) res = MARKER_LIGHT_COLOR.YELLOW;
			return res;
		}

		public bool CalcNewCoordinates(int id, float distance, int angle, out int newX, out int newY)
		{
			newX = newY = -1;
			try
			{
				MarkerClass curMarker = markerList_.GetMarkerById(id);
				if (curMarker == null)
				{
					SphService.WriteToLogFailed("CalcNewCoordinates(): curMarker = null");
					return false;
				}

				newX = (int)(distance * Math.Sin(angle)) + curMarker.CurrentX;
				newY = (int)(distance * Math.Cos(angle)) + curMarker.CurrentY;
				SphService.WriteToLogGeneral(
					string.Format("CalcNewCoordinates: old {0}, {1}; new {2}, {3}",
						curMarker.CurrentX, curMarker.CurrentY, newX, newY));
				return true;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in MarkerList::CalcNewCoordinates():");
				return false;
			}
		}

		//private bool SetCoordinates(int id, int newX, int newY)
		//{
		//    return markerList_.SetCoordinates(id, newX, newY);
		//}

		public bool GetMarkerCoordinates(int id, out int x, out int y)
		{
			x = y = -1;
			MarkerClass marker = markerList_.GetMarkerById(id);
			if (marker != null)
			{
				x = marker.CurrentX;
				y = marker.CurrentY;
				return true;
			}
			return false;
		}
	}

	public struct PosDataMessage
	{
		public int Timestamp;
		public short TpId;
		public short Id;

		public int X;
		public int Y;
		public int Z;

		public short Speed3dX;
		public short Speed3dY;
		public short Speed3dZ;
		public short Accel3dX;
		public short Accel3dY;
		public short Accel3dZ;
		public int RawX;
		public int RawY;
		public int RawZ;
	}
}
