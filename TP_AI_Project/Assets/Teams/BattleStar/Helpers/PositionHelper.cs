using System.Collections;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

namespace BattleStar
{
    public static class PositionHelper
    {
        public static Vector2 FindNearestWaypointEnemy(GameData gameData, SpaceShipView spaceShipView, int spaceShipOwnerEnemy)
        {
            float distance = float.MaxValue;
            Vector2 nearestPos = new Vector2();
        
        
            for (int i = 0; i < gameData.WayPoints.Count; i++)
            {
                if (gameData.WayPoints[i].Owner == spaceShipOwnerEnemy)
                {
                    var newrestPosition = Vector2.Distance(gameData.WayPoints[i].Position, spaceShipView.Position);
                    if (newrestPosition < distance)
                    {
                        distance = newrestPosition;
                        nearestPos = gameData.WayPoints[i].Position;
                    } 
                }
            }

            return nearestPos;
        }
        
        public static Vector2 FindNearestWaypointNeutral(GameData gameData, SpaceShipView spaceShipView)
        {
            float distance = float.MaxValue;
            Vector2 nearestPos = new Vector2();
        
        
            for (int i = 0; i < gameData.WayPoints.Count; i++)
            {
                if (gameData.WayPoints[i].Owner == -1)
                {
                    var newrestPosition = Vector2.Distance(gameData.WayPoints[i].Position, spaceShipView.Position);
                    if (newrestPosition < distance)
                    {
                        distance = newrestPosition;
                        nearestPos = gameData.WayPoints[i].Position;
                    } 
                }
            }

            return nearestPos;
        }

        public static float DistanceBtwPlayers(GameData gameData, int spaceShipOwner, int spaceShipOwnerEnemy)
        {
            return Vector2.Distance(gameData.SpaceShips[spaceShipOwner].Position, gameData.SpaceShips[spaceShipOwnerEnemy].Position);
        }
    }

}
