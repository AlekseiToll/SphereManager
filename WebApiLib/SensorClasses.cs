
namespace WebApiLib
{
	public class SensorType
	{
		public string code { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public int id { get; set; }
		public string created { get; set; }
	}

	public class Sensor
	{
		public string externalId { get; set; }
		public SensorType sensorType { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public int id { get; set; }
		public string created { get; set; }
	}
}
