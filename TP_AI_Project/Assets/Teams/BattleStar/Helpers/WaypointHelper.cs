using System;
using System.Collections;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

namespace BattleStar
{
    public static class WaypointHelper
    {
        public static bool HavingAllWaypoint(GameData gameData, int spaceShipOwner)
        {
            return gameData.SpaceShips[spaceShipOwner].WaypointScore == gameData.WayPoints.Count;
        }
        
        public static bool EnemyHasWaypoint(GameData gameData, int spaceShipOwnerEnemy)
        {
            return gameData.SpaceShips[spaceShipOwnerEnemy].WaypointScore > 0;
        }
        
        public static bool NeutralWaypointExist(GameData gameData, int spaceShipOwner, int spaceShipOwnerEnemy)
        {
            return gameData.SpaceShips[spaceShipOwner].WaypointScore + gameData.SpaceShips[spaceShipOwnerEnemy].WaypointScore < gameData.SpaceShips.Count;
        }
        
    }
}

