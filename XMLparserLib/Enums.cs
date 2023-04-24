namespace XMLparserLib
{
	// тип теста
	public enum MarkerTestType
	{
		CYCLE = 0,		// циклический с выполнением пока спортсмен не перестанет справляться
		POINT = 1,		// точка-точка
		NONE = 2
	}

	public enum UserSetValue
	{
		TIME = 0,
		SPEED = 1
	}

	public enum IncreaseType
	{
		ON_SECOND = 0,	// увеличение интенсивности происходит каждое x количество кругов
		ON_TIME = 1		// увеличение интенсивности происходит каждый промежуток времени
	}

	public enum MarkerType
	{
		SQUARE = 0, 
		CIRCLE = 1, 
		STAR = 2, 
		SQUAREGATE = 3, 
		CIRCLEGATE = 4, 
		STARGATE = 5
	}

	public enum MarkerActivity
	{
		/// <summary>срабатывание при забегании в зону</summary>
		ZONE = 0,
		/// <summary>срабатывание по времени</summary>
		TIME = 1,
		/// <summary>стартфиниш маркер</summary>
		STARTFINISHMARKER = 2,
		/// <summary>чекпоинт</summary>
		CHECKPOINT = 3
	}

	public enum MarkerActivateTime
	{
		/// <summary>общее время с начала всего цикла</summary>
		COMMON = 0,
		/// <summary>время от начала круга</summary>
		LAP = 1
	}
}
