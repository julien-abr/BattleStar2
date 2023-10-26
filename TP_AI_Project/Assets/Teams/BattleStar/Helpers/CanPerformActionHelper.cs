using System.Collections;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

namespace BattleStar
{
    public static class CanPerformActionHelper 
    {
        public static bool CanShockWave(GameData gameData, int spaceShipOwner, int spaceShipOwnerEnemy, float ShockWaveRadius)
        {

            return Vector2.Distance(gameData.SpaceShips[spaceShipOwner].Position,
                gameData.SpaceShips[spaceShipOwnerEnemy].Position) <= ShockWaveRadius;
        }

        public static void UpdateMineCounter(GameData gameData, int spaceShipOwner, int currentWaypoint, BattleStarController controller)
        {
            if (currentWaypoint == gameData.SpaceShips[spaceShipOwner].WaypointScore) return;
            controller.OnValueChangedWaypoints();
        }
    }
    
}

