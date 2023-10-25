using System.Collections;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

public class FindNearestWayPointHelper : MonoBehaviour
{
    public Vector2 actualNearestWaypoint(GameData gameData, SpaceShipView spaceShipView, int spaceShipOwner)
    {
        float distance = float.MaxValue;
        Vector2 nearestPos = new Vector2();
        
        
        for (int i = 0; i < gameData.WayPoints.Count; i++)
        {
            if (gameData.WayPoints[i].Owner != spaceShipOwner)
            {
                var newrestPosition = Vector2.Distance(gameData.WayPoints[i].Position, spaceShipView.Position);
                if (newrestPosition < distance)
                {
                    distance = newrestPosition;
                    nearestPos = gameData.WayPoints[i].Position;
                } 
            }
            else
            {
                continue;
            }
            
        }

        return nearestPos;

    }
}
