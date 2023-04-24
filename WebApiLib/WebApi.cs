using System;
using System.Net;
using System.Text;

using Newtonsoft.Json;
using SphServiceLib;

namespace WebApiLib
{
	public class WebApi
	{
		private SphSettings settings_;

		public WebApi(ref SphSettings settings)
		{
			settings_ = settings;
		}

		private bool ReadData(string folder, ref string res)
		{
			try
			{
				string url = @"http://" + settings_.WebServerIp + ":" + 
					settings_.WebServerPort.ToString() + folder;
				WebClient wc = new WebClient();
				wc.Credentials = new NetworkCredential(settings_.WebServerUser, settings_.WebServerPswd);
				byte[] response = wc.DownloadData(url);

				UTF8Encoding encoding = new UTF8Encoding();
				res = encoding.GetString(response);
				return true;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Exception in WebApi::ReadData(): ");
				return false;
			}
		}

		public bool GetSportsmanData(ref SportsmanInfo[] listSportsman)
		{
			try
			{
				string res = string.Empty;
				if (!ReadData(settings_.WebSportsmanFolder, ref res))
					return false;

				listSportsman = JsonConvert.DeserializeObject<SportsmanInfo[]>(res);
				return true;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Exception in GetSportsmanData(): ");
				return false;
			}
		}

		public bool GetSensorData(ref Sensor[] listSensor)
		{
			try
			{
				string res = string.Empty;
				if (!ReadData(settings_.WebSensorFolder, ref res))
					return false;

				listSensor = JsonConvert.DeserializeObject<Sensor[]>(res);
				return true;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Exception in GetSensorData(): ");
				return false;
			}
		}

		public bool GetSensorSportsmanLinkData(ref SensorSportsmanLink[] listSensorManLink)
		{
			try
			{
				string res = string.Empty;
				if (!ReadData(settings_.WebSportsmanSensorLinkFolder, ref res))
					return false;

				listSensorManLink = JsonConvert.DeserializeObject<SensorSportsmanLink[]>(res);
				return true;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Exception in GetSensorSportsmanLinkData(): ");
				return false;
			}
		}
	}
}
