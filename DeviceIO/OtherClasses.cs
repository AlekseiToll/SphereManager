using System;

namespace DeviceIO
{
	public class StopWatchClass
	{
		public DateTime StartTime = DateTime.MinValue;
		public DateTime EndTime = DateTime.MinValue;
		public int StartMarkerId = -1;
		public int EndMarkerId = -1;
		public bool Enabled = false;

		public StopWatchClass()
		{
		}

		public StopWatchClass(DateTime start, int id)
		{
			StartTime = start;
			StartMarkerId = id;
		}

		public void Reset()
		{
			StartTime = DateTime.MinValue;
			EndTime = DateTime.MinValue;
			StartMarkerId = -1;
			EndMarkerId = -1;
			Enabled = false;
		}
	}

	public class ResultPointToPointTest
	{
		public StopWatchClass StopWatch;

		public TimeSpan DistanceTime;
	}
}
