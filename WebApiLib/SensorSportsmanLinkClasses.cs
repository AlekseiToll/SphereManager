
namespace WebApiLib
{
	public class SensorSportsmanLink
	{
		public int id { get; set; }
		public Sensor sensor { get; set; }
		public int sportsman { get; set; }
		public string startTime { get; set; }
		public string finishTime { get; set; }
	}
}
