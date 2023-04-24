using System;
using System.Collections.Generic;

using AMQPlib;
using DeviceIO;
using SphServiceLib;
using WebApiLib;
using XMLparserLib;

namespace CommandCoreLib
{
	

	//public class CommandCore
	//{
	//    //private MarkerTest test_;

	//    //private SportsmanListClass sportsmanList_;
	//    //private MarkerListClass markerList_;

	//    public bool ProcessNewAMQPpacket(ref AMQPpacket packet)
	//    {
	//        return sportsmanList_.ChangePos(packet.IdSportsman, packet.CoordinateValue, packet.CoordinateType);
	//    }

	//    public bool AddMarker(int id)
	//    {
	//        return markerList_.AddMarker(id);
	//    }

	//    public bool DeleteMarker(int id)
	//    {
	//        return markerList_.DeleteMarker(id);
	//    }

	//    public MarkerClass GetMarkerById(int id)
	//    {
	//        return markerList_.GetMarkerById(id);
	//    }

	//    public void SetTest(ref MarkerTest test)
	//    {
	//        test_ = test;
	//    }

	//    public bool CalcNewCoordinates(int id, float distance, int angle, out int newX, out int newY)
	//    {
	//        newX = newY = -1;
	//        try
	//        {
	//            MarkerClass curMarker = markerList_.GetMarkerById(id);
	//            if (curMarker == null)
	//            {
	//                SphService.WriteToLogFailed("CalcNewCoordinates(): curMarker = null");
	//                return false;
	//            }

	//            newX = (int) (distance * Math.Sin(angle)) + curMarker.CurrentX;
	//            newY = (int) (distance * Math.Cos(angle)) + curMarker.CurrentY;
	//            return true;
	//        }
	//        catch (Exception ex)
	//        {
	//            SphService.DumpException(ex, "Error in MarkerList::CalcNewCoordinates():");
	//            return false;
	//        }
	//    }

	//    public bool SetCoordinates(int id, int newX, int newY)
	//    {
	//        return markerList_.SetCoordinates(id, newX, newY);
	//    }
	//}
}
