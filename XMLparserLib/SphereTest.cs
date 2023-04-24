using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SphServiceLib;

namespace XMLparserLib
{
	public class Trajectory
	{
		public int Id;
		public bool Selected;
		public Color TrajectoryColor;
		public short LineType;
		public short StartArea;
		public int StartMarker;
		public float StartX;
		public float StartY;
		public short EndArea;
		public int EndMarker;
		public float EndX;
		public float EndY;
		public float MidX;
		public float MidY;
	}

	// действие, которое совершается например, 
	// когда близко время, отведенное спортсмену на круг
	public class LapToEndActionClass
	{
		public string AsString;				// строковое описание действия
		public bool ChangeColor;			// изменение цвета	
		public Color NewColor;				// новый цвет
		public bool Twinkling;				// мигание
		public short TwinkleIntensity;		// интенсивность мигания, в мс
	}

	// действия теста (общие для всех маркеров)
	public class TestActionClass
	{
		public string AsString;				// строковое описание действия
		public bool ChangeColor;			// изменение цвета	
		public Color NewColor;				// новый цвет
		public bool Twinkling;				// мигание
		public short TwinkleIntensity;		// интенсивность мигания, в мс
		// пауза до следующей попытки, в случае, если спортсмен не справился
		public bool EnablePause;
		public short PauseInSec;			// размер паузы, в секундах
		// конец теста, полный конец, это была последняя попытка
		public bool EndOfTest;
	}

	// описание маркера
	public class XMLMarkerClass
	{
		public int Id;
		public MarkerType Type;
		public bool IsGate;					// ворота или нет
		public bool Selected;				// выделен ли маркер в данный момент (имеет фокус)
		public Color ColorDB;				// цвет маркера «в базе»
		public int Diameter;				// диаметр, если круг
		public int Side;					// сторона, если квадрат
		public int GSize;					// диаметр/сторона наконечников ворот
		public int GLineLen;				// длина у ворот между центрами наконечников
		public int GateAngle;				// угол поворота ворот

		//координаты центра маркера или центра линии ворот (между наконечниками)
		public int PosX;
		public int PosY;
		// координаты центров наконечников ворот
		public int Gate1X;
		public int Gate1Y;
		public int Gate2X;
		public int Gate2Y;

		/// <summary>
		/// кол-во траекторий около маркера. 1 траектория – это один приход и один уход. 
		/// Максимум 3 траектории ( как правило больше 2 не будет использоваться, 3- наверное, 
		/// редчайший случай)
		/// </summary>
		public short CntGoToMarker;
		/// <summary>
		/// Занятость стартовой точки 1ой траектории. Стартовая - это с которой траектория уходит от маркера.
		/// </summary>
		public bool StartTrajectoryBusy1;
		public bool StartTrajectoryBusy2;	// то же для 2ой
		public bool StartTrajectoryBusy3;	// и 3-ей
		// зона стартовой точки первой траектории (см. рисунок в описании xml)
		// зоны бывают 0, 1, 2, 3 и 4. 100 – это значит, что тут траектории нет
		public short StartTrajectoryZone1;
		public short StartTrajectoryZone2;	// то же для 2ой
		public short StartTrajectoryZone3;	// и 3-ей
		// Занятость конечной точки 1ой траектории
		public bool EndTrajectoryBusy1;
		public bool EndTrajectoryBusy2;	// то же для 2ой
		public bool EndTrajectoryBusy3;	// и 3-ей
		// зона конечной точки первой траектории (см. рисунок в описании xml)
		// зоны бывают 0, 1, 2, 3 и 4. 100 – это значит, что тут траектории нет
		public short EndTrajectoryZone1;
		public short EndTrajectoryZone2;	// то же для 2ой
		public short EndTrajectoryZone3;	// и 3-ей
		// описывает ли траектория полный круг вокруг маркера. 
		// (Если траектория приходит в одну зону и уходит из нее же, то редактор задает вопрос: 
		// описать ли круг вокруг маркера. Вот это оно)
		public bool GoAroundMarker1;
		public bool GoAroundMarker2;		// то же для 2ой
		public bool GoAroundMarker3;		// и 3-ей

		public List<MarkerActionClass> MarkerActions;		// действия маркеров
	}

	// действие маркера
	public class MarkerActionClass
	{
		public string AsString;				// краткое описание действия
		public MarkerActivity Activity;
		public MarkerActivateTime ActivateTime;		// используется если Activity = TIME
		public bool ChangeColor;			// изменение цвета	
		public Color NewColor;				// новый цвет
		public bool ChangePos;				// изменение позиции
		public int NewX;					// новые координаты
		public int NewY;
		//public bool ChangeSize;			// изменение размера (задается радиус либо сторона). 
											// Работает только для одиночных маркеров.
		//public int NewSize;				// новый размер

		// чекпоинт. Является достаточным действием, как и стартфинишмаркер, то есть все другие свойства 
		// действия учтены не будут (либо чекпоинт, либо стартфинишмаркер). Чекпоинты нужны для задания порядка
		// прохождения маршрута. Один маркер может иметь 2 чекпоинта (есть упражнения, где нужно бегать 
		// туда-сюда). Стартфинишмаркер это как-бы нулевой чекпоинт, он обнуляет круг
		public bool CheckPoint;
		public short CheckPointNum;			// номер чекпоинта

		public short CommonTime;			// общее время, при activatetime = COMMON
        public short LapTime;				// время круга, при activatetime = LAP
		public bool Twinkling;				// моргание (было где-то выше. Только теперь моргать или прекращать 
											// моргать будет один маркер, к которому применимо действие.
		public short TwinkleIntensity;		// интенсивность мигания, в мс 

		public short MarkerZone;			// зона маркера,  означает что срабатывание действия, например моргания,
					//происходит при забегании в конкретную зону маркера. Соответсвенно работает  при activity=ZONE
		//public bool Show;					// showhide, показать или скрыть маркер

		public bool StartPoint;				// это как раз стартфинишмаркер

		// эти два поля только для теста point-to-point!
		public bool Stopwatch;				// засекание времени между двумя маркерами
		public bool StopwatchStart;			// если true то начало отсчета времени, если false то окончание
	}

	public class MarkerTest
	{
		// тип теста (point-точка-точка, cycle – циклический с выполнением пока спортсмен 
		// не перестанет справляться. Всего 2 типа)
		private MarkerTestType type_;
		// тип площадки
		private string background_;
		// цвет контура площадки
		private Color forecolor_;
		// будет ли показываться траектория на проекции
		private bool showTrajectory_;

		// какая величина задана юзером - время или скорость
		private UserSetValue userSetValue_;
		// стартовая скорость
		private float startLapSpeed_;		//km/hour
		// увеличение скорости с каждым циклом
		private float increaseLapSpeed_;
		// начальное время
		private float startLapTime_;
		// уменьшение времени с каждым циклом
		private float lapTimeDecrease_;

		// длина дистанции (без учета оббеганий, суммарное расстояние между маркерами)
		private double lapDistance_;
		// время восстановления между циклами (кругами)
		private float timeToRecovery_;

		// если true, то есть предупреждение в виде мигания маркеров, например, 
		// когда близко время, отведенное спортсмену на круг
		private bool notifyTwinkling_;
		// время за которое наступает это самое мигание
		private float timeTwinkling_;

		// увеличение интенсивности в зависимости от (от времени, от круга, ...)
		private IncreaseType curIncreaseType_;
		private short increaseLapValue_;	// если от круга, то кол-во кругов
		private float increaseTimeValue_;	// если от времени, то секунды

		//действие, которое происходит по notifyTwinkling_
		private LapToEndActionClass laptoEndAction_;

		// Количество айтемов в этой коллекции равно кол-ву попыток «неудачи» спортсмена
		private List<TestActionClass> testActions_;

		private List<XMLMarkerClass> markers_;

		private List<Trajectory> trajectories_;

		public bool GetCoordStartFinishMarker(out int x, out int y)
		{
			try
			{
				x = -1;
				y = -1;
				for (int iMarker = 0; iMarker < Markers.Count; ++iMarker)
				{
					for (int iAct = 0; iAct < Markers[iMarker].MarkerActions.Count; ++iAct)
					{
						if (Markers[iMarker].MarkerActions[iAct].Activity == MarkerActivity.STARTFINISHMARKER)
						{
							x = Markers[iMarker].PosX;
							y = Markers[iMarker].PosY;
							return true;
						}
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in GetCoordStartFinishMarker():");
				throw;
			}
		}

		public int GetAllMarkersCount()
		{
			int res = 0;
			for (int iMarker = 0; iMarker < Markers.Count; ++iMarker)
			{
				if (!Markers[iMarker].IsGate)
					res += 1;
				else
					res += 2;
			}
			return res;
		}

		#region Properties

		public MarkerTestType Type
		{
			get { return type_; }
			set { type_ = value; }
		}

		public string Background
		{
			get { return background_; }
			set { background_ = value; }
		}

		public Color Forecolor
		{
			get { return forecolor_; }
			set { forecolor_ = value; }
		}

		public bool ShowTrajectory
		{
			get { return showTrajectory_; }
			set { showTrajectory_ = value; }
		}

		//public bool UserSetLapSpeed
		//{
		//    get { return userSetLapSpeed_; }
		//    set { userSetLapSpeed_ = value; }
		//}

		/// <summary>starting speed (m/sec)</summary>
		public float StartLapSpeed
		{
			get { return startLapSpeed_; }
			set { startLapSpeed_ = value; }
		}

		/// <summary>increase lap speed (m/sec)</summary>
		public float IncreaseLapSpeed
		{
			get { return increaseLapSpeed_; }
			set { increaseLapSpeed_ = value; }
		}

		//public bool UserSetLapTime
		//{
		//    get { return userSetLapTime_; }
		//    set { userSetLapTime_ = value; }
		//}

		/// <summary>in seconds</summary>
		public float StartLapTime
		{
			get { return startLapTime_; }
			set { startLapTime_ = value; }
		}

		/// <summary>in seconds</summary>
		public float LapTimeDecrease
		{
			get { return lapTimeDecrease_; }
			set { lapTimeDecrease_ = value; }
		}

		/// <summary>length of the whole distance (in meters)</summary> 
		public double LapDistance
		{
			get { return lapDistance_; }
			set { lapDistance_ = value; }
		}

		/// <summary> user can determine either speed or time</summary>
		public UserSetValue UserSetValue
		{
			get { return userSetValue_; }
			set { userSetValue_ = value; }
		}

		/// <summary>time to recovery between laps (in seconds)</summary>
		public float TimeToRecovery
		{
			get { return timeToRecovery_; }
			set { timeToRecovery_ = value; }
		}

		/// <summary>
		/// предупреждение в виде мигания маркеров, например, когда близко время, 
		/// отведенное спортсмену на круг
		/// </summary>
		public bool NotifyTwinkling
		{
			get { return notifyTwinkling_; }
			set { notifyTwinkling_ = value; }
		}

		public float TimeTwinkling
		{
			get { return timeTwinkling_; }
			set { timeTwinkling_ = value; }
		}

		public LapToEndActionClass LaptoEndAction
		{
			get { return laptoEndAction_; }
			set { laptoEndAction_ = value; }
		}

		public List<TestActionClass> TestActions
		{
			get { return testActions_; }
			set { testActions_ = value; }
		}

		public List<XMLMarkerClass> Markers
		{
			get { return markers_; }
			set { markers_ = value; }
		}

		public List<Trajectory> Trajectories
		{
			get { return trajectories_; }
			set { trajectories_ = value; }
		}

		/// <summary>
		/// увеличение интенсивности в зависимости от (от времени, от круга, ...)
		/// </summary>
		public IncreaseType CurIncreaseType
		{
			get { return curIncreaseType_; }
			set { curIncreaseType_ = value; }
		}

		/// <summary>
		/// если увеличение интенсивности - от круга, то кол-во кругов
		/// </summary>
		public short IncreaseLapValue
		{
			get { return increaseLapValue_; }
			set { increaseLapValue_ = value; }
		}

		/// <summary>
		/// если увеличение интенсивности - от времени, то кол-во секунд
		/// </summary>
		public float IncreaseTimeValue
		{
			get { return increaseTimeValue_; }
			set { increaseTimeValue_ = value; }
		}

		#endregion
	}
}
