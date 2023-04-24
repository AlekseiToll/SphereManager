using System;
using System.Collections.Generic;
using System.Net;

using SphServiceLib;
using XMLparserLib;

namespace DeviceIO
{
	internal class MarkerListClass
	{
		private List<MarkerClass> list_ = new List<MarkerClass>();

		public MarkerClass this[int i]
		{
			get
			{
				return list_[i];
			}
			//set
			//{
			//    list_[i] = value;
			//}
		}

		public MarkerClass GetMarkerById(int id)
		{
			foreach (var curMarker in list_)
			{
				if (curMarker.Id == id)
				{
					return curMarker;
				}
			}
			return null;
		}

		public MarkerClass FindSecondGoalpost(int id)
		{
			MarkerClass marker = GetMarkerById(id);
			if (!marker.XmlMarker.IsGate) return null;
			foreach (var curMarker in list_)
			{
				if (curMarker.XmlMarker.Id == marker.XmlMarker.Id)
					return curMarker;
			}
			return null;
		}

		public bool AddMarker(int id, IPAddress ip)
		{
			try
			{
				if (GetMarkerById(id) == null)
				{
					list_.Add(new MarkerClass(id, ip));
				}
				return true;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in MarkerList::AddMarker():");
				return false;
			}
		}

		public bool DeleteMarker(int id)
		{
			try
			{
				foreach (var curMarker in list_)
				{
					if (curMarker.Id == id)
					{
						return list_.Remove(curMarker);
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				SphService.DumpException(ex, "Error in MarkerList::DeleteMarker():");
				return false;
			}
		}

		public int Count
		{
			get { return list_.Count; }
		}
	}

	public class MarkerClass
	{
		/// <summary>real Id of the marker</summary>
		private int id_;
		/// <summary>The descpiption of the marker in xml test</summary>
		private XMLMarkerClass xmlMarker_;

		private IPAddress ipAddress_;
		private int startX_ = -1;		// position at the start
		private int startY_ = -1;
		private int currentX_ = -1;	// current position
		private int currentY_ = -1;
		private int requiredX_ = -1;	// the target
		private int requiredY_ = -1;
		private bool isMoving_ = false;	// the marker is moving now

		/// <summary>For gates only!</summary>
		private int coordRevativelyCenterOgGateX_;
		/// <summary>For gates only!</summary>
		private int coordRevativelyCenterOgGateY_;

		private bool isSecondGoalpost_ = false;

		public MarkerClass(int id)
		{
			id_ = id;
		}

		public MarkerClass(int id, IPAddress ip)
		{
			id_ = id;
			ipAddress_ = ip;
		}

		#region Properties

		public IPAddress IpAddress
		{
			get { return ipAddress_; }
			set { ipAddress_ = value; }
		}

		public int Id
		{
			get { return id_; }
			set { id_ = value; }
		}

		public int CurrentX
		{
			get { return currentX_; }
			set { currentX_ = value; }
		}

		public int CurrentY
		{
			get { return currentY_; }
			set { currentY_ = value; }
		}

		public int RequiredX
		{
			get { return requiredX_; }
			set { requiredX_ = value; }
		}

		public int RequiredY
		{
			get { return requiredY_; }
			set { requiredY_ = value; }
		}

		public int StartX
		{
			get { return startX_; }
			set { startX_ = value; }
		}

		public int StartY
		{
			get { return startY_; }
			set { startY_ = value; }
		}

		public bool IsMoving
		{
			get { return isMoving_; }
			set { isMoving_ = value; }
		}

		/// <summary>The descpiption of the marker in xml test</summary>
		public XMLMarkerClass XmlMarker
		{
			get { return xmlMarker_; }
			set { xmlMarker_ = value; }
		}

		/// <summary>The marker is the second goalpost of the gate</summary>
		public bool IsSecondGoalpost
		{
			get { return isSecondGoalpost_; }
			set { isSecondGoalpost_ = value; }
		}

		/// <summary>For gates only!</summary>
		public int CoordRevativelyCenterOgGateX
		{
			get { return coordRevativelyCenterOgGateX_; }
			set { coordRevativelyCenterOgGateX_ = value; }
		}

		/// <summary>For gates only!</summary>
		public int CoordRevativelyCenterOgGateY
		{
			get { return coordRevativelyCenterOgGateY_; }
			set { coordRevativelyCenterOgGateY_ = value; }
		}

		#endregion
	}
}