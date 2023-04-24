using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using SphServiceLib;

namespace XMLparserLib
{
	public class XMLparser
	{
		public static bool Parse(string fileName, out MarkerTest test)
		{
			test = new MarkerTest();
			try
			{
				XDocument xDocument = XDocument.Load(fileName);
				// тип теста
				string stype = xDocument.Element("test").Attribute("type").Value;
				switch (stype.ToLower())
				{
					case "cycle": test.Type = MarkerTestType.CYCLE; break;
					case "point": test.Type = MarkerTestType.POINT; break;
					default:
						throw new SphException("Unknown test type: " + stype);
				}
				// тип площадки
				test.Background = xDocument.Element("test").Element("background").Value;
				// цвет контура площадки
				test.Forecolor = ColorTranslator.FromHtml(	//Color.FromName(
					xDocument.Element("test").Element("forecolor").Value);
				// будет ли показываться траектория на проекции
				test.ShowTrajectory = 
					Convert.ToBoolean(xDocument.Element("test").Element("showtraectory").Value);

				// какая величина задана юзером - время или скорость
				if (Convert.ToBoolean(xDocument.Element("test").Element("setspeed").Value))
				{
					test.UserSetValue = UserSetValue.SPEED;
					// стартовая скорость
					test.StartLapSpeed =
						Convert.ToSingle(xDocument.Element("test").Element("startlapspeed").Value);
					test.StartLapSpeed /= 3.6f;  // convert to m/sec
					// ускорение с каждым циклом
					test.IncreaseLapSpeed =
						Convert.ToSingle(xDocument.Element("test").Element("increaselapspeed").Value);
					test.IncreaseLapSpeed /= 3.6f;  // convert to m/sec
				}
				else
				{
					test.UserSetValue = UserSetValue.TIME;
					// начальное время
					test.StartLapTime =
						Convert.ToSingle(xDocument.Element("test").Element("startlaptime").Value);
					// уменьшение времени с каждым циклом
					test.LapTimeDecrease =
						Convert.ToSingle(xDocument.Element("test").Element("increaselaptime").Value);
				}
				// пользователь задал скорость (начальную и ускорение)
				//test.UserSetLapSpeed =
				//    Convert.ToBoolean(xDocument.Element("test").Element("setspeed").Value);
				// пользователь задал время
				//test.UserSetLapTime =
				//    Convert.ToBoolean(xDocument.Element("test").Element("settime").Value);
				
				// длина дистанции (без учета оббеганий, суммарное расстояние между маркерами)
				test.LapDistance =
					Convert.ToDouble(xDocument.Element("test").Element("lapdistance").Value);
				// время восстановления между циклами (кругами)
				test.TimeToRecovery =
						Convert.ToSingle(xDocument.Element("test").Element("recoverytime").Value);

				// если true, то есть предупреждение в виде мигания маркеров, например, 
				// когда близко время, отведенное спортсмену на круг
				test.NotifyTwinkling =
					Convert.ToBoolean(xDocument.Element("test").Element("laptoendnotify").Value);
				if (test.NotifyTwinkling)
				{
					// время за которое наступает это самое мигание
					test.TimeTwinkling =
						Convert.ToSingle(xDocument.Element("test").Element("laptoendtime").Value);
				}

				////////действие, которое происходит по laptoendnotify//////////
				// строковое описание действия
				test.LaptoEndAction.AsString =
					xDocument.Element("test").Element("laptoendaction").Element("actionstring").Value;
				// изменение цвета
				test.LaptoEndAction.ChangeColor = Convert.ToBoolean(
					xDocument.Element("test").Element("laptoendaction").Element("changecolor").Value);
				// новый цвет
				test.LaptoEndAction.NewColor = ColorTranslator.FromHtml(
					xDocument.Element("test").Element("laptoendaction").Element("newcolor").Value);
				// мигание
				test.LaptoEndAction.Twinkling =
					Convert.ToBoolean(xDocument.Element("test").Element("laptoendaction").Element("twinkle").Value);
				// интенсивность мигания, в мс
				test.LaptoEndAction.TwinkleIntensity = Convert.ToInt16(
					xDocument.Element("test").Element("laptoendaction").Element("twinkleintensity").Value);
				////////// end of LaptoEndAction /////////////////

				///// Количество айтемов в этой коллекции равно кол-ву попыток «неудачи» спортсмена
				test.TestActions.Clear();
				IEnumerable<XElement> testActions = xDocument.Descendants("testaction");
				foreach (XElement testAction in testActions)
				{
					TestActionClass curTestAction = new TestActionClass();
					curTestAction.AsString = testAction.Element("actionstring").Value;
					curTestAction.ChangeColor = Convert.ToBoolean(testAction.Element("changecolor").Value);
					curTestAction.NewColor = ColorTranslator.FromHtml(testAction.Element("newcolor").Value);
					curTestAction.Twinkling = Convert.ToBoolean(testAction.Element("twinkle").Value);
					curTestAction.TwinkleIntensity = Convert.ToInt16(testAction.Element("twinkleintensity").Value);
					curTestAction.EnablePause = Convert.ToBoolean(testAction.Element("enablepause").Value);
					curTestAction.PauseInSec = Convert.ToInt16(testAction.Element("pause").Value);
					curTestAction.EndOfTest = Convert.ToBoolean(testAction.Element("endoftest").Value);
					test.TestActions.Add(curTestAction);
				}
				/////////// end of testActions //////////////////

				// переходим к маркерам /////////////////////////
				test.Markers.Clear();
				IEnumerable<XElement> markers = xDocument.Descendants("marker");
				foreach (XElement marker in markers)
				{
					XMLMarkerClass curMarker = new XMLMarkerClass();
					curMarker.Id = Convert.ToInt32(marker.Attribute("id").Value);
					switch (marker.Element("type").Value.ToUpper())
					{
						case "SQUARE":
							curMarker.Type = MarkerType.SQUARE;
							break;
						case "CIRCLE":
							curMarker.Type = MarkerType.CIRCLE;
							break;
						case "STAR":
							curMarker.Type = MarkerType.STAR;
							break;
						case "SQUAREGATE":
							curMarker.Type = MarkerType.SQUAREGATE;
							break;
						case "CIRCLEGATE":
							curMarker.Type = MarkerType.CIRCLEGATE;
							break;
						case "STARGATE":
							curMarker.Type = MarkerType.STARGATE;
							break;
						default:
							throw new SphException("Unknown marker type: " + marker.Element("type").Value);
					}
					curMarker.IsGate = Convert.ToBoolean(marker.Element("isgate").Value);
					curMarker.Selected = Convert.ToBoolean(marker.Element("selected").Value);
					curMarker.ColorDB = ColorTranslator.FromHtml(marker.Element("color").Value);
					curMarker.Diameter = Convert.ToInt32(marker.Element("diameter").Value);
					curMarker.Side = Convert.ToInt32(marker.Element("side").Value);
					curMarker.GSize = Convert.ToInt32(marker.Element("gsize").Value);
					curMarker.GLineLen = Convert.ToInt32(marker.Element("linelen").Value);
					curMarker.GateAngle = Convert.ToInt32(marker.Element("gateangle").Value);

					curMarker.PosX = (int)Convert.ToSingle(marker.Element("posx").Value);
					curMarker.PosY = (int)Convert.ToSingle(marker.Element("posy").Value);
					if (curMarker.Type == MarkerType.CIRCLEGATE || curMarker.Type == MarkerType.SQUAREGATE ||
					    curMarker.Type == MarkerType.STARGATE)
					{
						curMarker.Gate1X = (int)Convert.ToSingle(marker.Element("gatex1").Value);
						curMarker.Gate1Y = (int)Convert.ToSingle(marker.Element("gatey1").Value);
						curMarker.Gate2X = (int)Convert.ToSingle(marker.Element("gatex2").Value);
						curMarker.Gate2Y = (int)Convert.ToSingle(marker.Element("gatey2").Value);
					}
					// кол-во траекторий около маркера (от 1 до 3)
					curMarker.CntGoToMarker = Convert.ToInt16(marker.Element("currtraectorypoints").Value);
					// Занятость стартовой точки 1ой траектории
					if (curMarker.CntGoToMarker > 0)
						curMarker.StartTrajectoryBusy1 = Convert.ToBoolean(marker.Element("starttraectory1").Value);
					if (curMarker.CntGoToMarker > 1)
						curMarker.StartTrajectoryBusy2 = Convert.ToBoolean(marker.Element("starttraectory2").Value);
					if (curMarker.CntGoToMarker > 2)
						curMarker.StartTrajectoryBusy3 = Convert.ToBoolean(marker.Element("starttraectory3").Value);
					// зона стартовой точки первой траектории
					if (curMarker.CntGoToMarker > 0)
						curMarker.StartTrajectoryZone1 = Convert.ToInt16(marker.Element("starttrvalue1").Value);
					if (curMarker.CntGoToMarker > 1)
						curMarker.StartTrajectoryZone2 = Convert.ToInt16(marker.Element("starttrvalue2").Value);
					if (curMarker.CntGoToMarker > 2)
						curMarker.StartTrajectoryZone3 = Convert.ToInt16(marker.Element("starttrvalue3").Value);
					if (curMarker.CntGoToMarker > 0)
						curMarker.EndTrajectoryBusy1 = Convert.ToBoolean(marker.Element("endtraectory1").Value);
					if (curMarker.CntGoToMarker > 1)
						curMarker.EndTrajectoryBusy2 = Convert.ToBoolean(marker.Element("endtraectory2").Value);
					if (curMarker.CntGoToMarker > 2)
						curMarker.EndTrajectoryBusy3 = Convert.ToBoolean(marker.Element("endtraectory3").Value);
					if (curMarker.CntGoToMarker > 0)
						curMarker.EndTrajectoryZone1 = Convert.ToInt16(marker.Element("endtrvalue1").Value);
					if (curMarker.CntGoToMarker > 1)
						curMarker.EndTrajectoryZone2 = Convert.ToInt16(marker.Element("endtrvalue2").Value);
					if (curMarker.CntGoToMarker > 2)
						curMarker.EndTrajectoryZone3 = Convert.ToInt16(marker.Element("endtrvalue3").Value);
					// описывает ли траектория полный круг вокруг маркера
					if (curMarker.CntGoToMarker > 0)
						curMarker.GoAroundMarker1 = Convert.ToBoolean(marker.Element("rounding1").Value);
					if (curMarker.CntGoToMarker > 1)
						curMarker.GoAroundMarker2 = Convert.ToBoolean(marker.Element("rounding2").Value);
					if (curMarker.CntGoToMarker > 2)
						curMarker.GoAroundMarker3 = Convert.ToBoolean(marker.Element("rounding3").Value);

					IEnumerable<XElement> markerActions = marker.Descendants("action");
					foreach (XElement markerAction in markerActions)
					{
						MarkerActionClass curMarkerAction = new MarkerActionClass();
						curMarkerAction.AsString = markerAction.Element("actionstring").Value;
						/////////////////////
						if (Convert.ToBoolean(markerAction.Element("startpoint")))
							curMarkerAction.Activity = MarkerActivity.STARTFINISHMARKER;
						else
						{
							if (Convert.ToBoolean(markerAction.Element("checkpoint")))
							{
								curMarkerAction.Activity = MarkerActivity.CHECKPOINT;
								// номер чекпоинта
								curMarkerAction.CheckPointNum =
									Convert.ToInt16(markerAction.Element("checkpointnum").Value);
							}
							else
							{
								switch (markerAction.Element("activity").Value.ToUpper())
								{
									case "TIME":
										curMarkerAction.Activity = MarkerActivity.TIME;
										break;
									case "ZONE":
										curMarkerAction.Activity = MarkerActivity.ZONE;
										curMarkerAction.MarkerZone = Convert.ToInt16(
											markerAction.Element("markerzone").Value);
										break;
									default:
										throw new SphException("Unknown activity type: " +
										                       markerAction.Element("activity").Value);
								}
							}
						}
						////////////////////
						if (curMarkerAction.Activity == MarkerActivity.TIME)
						{
							switch (markerAction.Element("activatetime").Value.ToUpper())
							{
								case "COMMON":
									curMarkerAction.ActivateTime = MarkerActivateTime.COMMON;
									curMarkerAction.CommonTime =
										Convert.ToInt16(markerAction.Element("commontime").Value);
									break;
								case "LAP":
									curMarkerAction.ActivateTime = MarkerActivateTime.LAP;
									curMarkerAction.LapTime =
										Convert.ToInt16(markerAction.Element("laptime").Value);
									break;
								default:
									throw new SphException("Unknown activatetime type: " +
									                       markerAction.Element("activatetime").Value);
							}
						}
						curMarkerAction.ChangeColor = Convert.ToBoolean(markerAction.Element("changecolor").Value);
						if (curMarkerAction.ChangeColor)
						{
							curMarkerAction.NewColor =
								ColorTranslator.FromHtml(markerAction.Element("newcolor").Value);
						}
						curMarkerAction.ChangePos = Convert.ToBoolean(markerAction.Element("changepos").Value);
						if (curMarkerAction.ChangePos)
						{
							curMarkerAction.NewX = (int)Convert.ToSingle(markerAction.Element("newx").Value);
							curMarkerAction.NewY = (int)Convert.ToSingle(markerAction.Element("newy").Value);
						}
						//curMarkerAction.ChangeSize = Convert.ToBoolean(markerAction.Element("changesize").Value);
						//if (curMarkerAction.ChangeSize)
						//{
						//    curMarkerAction.NewSize = Convert.ToInt32(markerAction.Element("newsize").Value);
						//}

						curMarkerAction.Twinkling = Convert.ToBoolean(markerAction.Element("twinkle").Value);
						curMarkerAction.TwinkleIntensity =
							Convert.ToInt16(markerAction.Element("twinkleintensity").Value);

						if (test.Type == MarkerTestType.POINT)
						{
							curMarkerAction.Stopwatch = Convert.ToBoolean(markerAction.Element("stopwatch").Value);
							curMarkerAction.StopwatchStart = Convert.ToBoolean(
															markerAction.Element("enablestopwatch").Value);
						}

						//curMarkerAction.Show = Convert.ToBoolean(markerAction.Element("showhide").Value);

						curMarker.MarkerActions.Add(curMarkerAction);
					}

					test.Markers.Add(curMarker);
				}
				/////////// end of markers
				
				test.Trajectories.Clear();
				IEnumerable<XElement> trajectories = xDocument.Descendants("traectory");
				foreach (XElement trajectory in trajectories)
				{
					Trajectory curTrajectory = new Trajectory();
					curTrajectory.Id = Convert.ToInt32(trajectory.Attribute("id").Value);
					curTrajectory.Selected = Convert.ToBoolean(trajectory.Element("selected").Value);
					curTrajectory.TrajectoryColor = ColorTranslator.FromHtml(trajectory.Element("color").Value);
					curTrajectory.LineType = Convert.ToInt16(trajectory.Element("linetype").Value);
					curTrajectory.StartArea = Convert.ToInt16(trajectory.Element("startarea").Value);
					curTrajectory.StartMarker = Convert.ToInt16(trajectory.Element("startmarker").Value);
					curTrajectory.StartX = Convert.ToSingle(trajectory.Element("startx").Value);
					curTrajectory.StartY = Convert.ToSingle(trajectory.Element("starty").Value);
					curTrajectory.EndArea = Convert.ToInt16(trajectory.Element("endarea").Value);
					curTrajectory.EndMarker = Convert.ToInt16(trajectory.Element("endmarker").Value);
					curTrajectory.EndX = Convert.ToSingle(trajectory.Element("endx").Value);
					curTrajectory.EndY = Convert.ToSingle(trajectory.Element("endy").Value);
					curTrajectory.MidX = Convert.ToSingle(trajectory.Element("midx").Value);
					curTrajectory.MidY = Convert.ToSingle(trajectory.Element("midy").Value);

					test.Trajectories.Add(curTrajectory);
				}
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error XMLparser::Parse():");
				return false;
			}
			return true;
		}
	}
}
