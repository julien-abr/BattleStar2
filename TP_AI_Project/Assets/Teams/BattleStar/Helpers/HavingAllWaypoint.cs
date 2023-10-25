using System;
using System.Collections;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

namespace BattleStar
{
    public class HavingAllWaypoint : MonoBehaviour
    {
        public static bool AllWaypoint(GameData gameData, int spaceShipOwner)
        {
            return gameData.SpaceShips[spaceShipOwner].WaypointScore == gameData.WayPoints.Count;
        }
    }
    
}

