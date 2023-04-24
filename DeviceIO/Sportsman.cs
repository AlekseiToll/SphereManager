using System.Collections.Generic;

using SphServiceLib;

namespace DeviceIO
{
	internal class SportsmanClass
	{
		public int Id;
		public int X;
		public int Y;
		//public SportsmanInfo Info;

		public SportsmanClass(int id, int x, int y)
		{
			Id = id;
			X = x;
			Y = y;
		}
	}

	internal class SportsmanListClass
	{
		private List<SportsmanClass> list_ = new List<SportsmanClass>();

		public void Add(int id, int x, int y)
		{
			list_.Add(new SportsmanClass(id, x, y));
		}

		public bool ChangePos(int id, int coordinateValue, string coordinateType)
		{
			SportsmanClass curMan = GetSportsmanById(id);
			if (curMan == null)
			{
				SphService.WriteToLogFailed("Unknown sportsman id = " + id);
				return false;
			}
			switch (coordinateType)
			{
				case "LPM_MON_POSITION_X":
					curMan.X = coordinateValue;
					break;
				case "LPM_MON_POSITION_Y":
					curMan.Y = coordinateValue;
					break;
				case "LPM_MON_POSITION_Z":	// not used
					break;
				default:
					SphService.WriteToLogFailed("Unknown type of AMPQ packet: " + coordinateType);
					return false;
			}
			return true;
		}

		public int GetX(int id)
		{
			SportsmanClass curMan = GetSportsmanById(id);
			if (curMan == null)
			{
				SphService.WriteToLogFailed("x: Unknown sportsman id = " + id);
				return -1;
			}
			return curMan.X;
		}

		public int GetY(int id)
		{
			SportsmanClass curMan = GetSportsmanById(id);
			if (curMan == null)
			{
				SphService.WriteToLogFailed("y: Unknown sportsman id = " + id);
				return -1;
			}
			return curMan.Y;
		}

		public SportsmanClass GetSportsmanById(int id)
		{
			foreach (var sportsman in list_)
			{
				if (sportsman.Id == id) return sportsman;
			}
			return null;
		}
	}
}