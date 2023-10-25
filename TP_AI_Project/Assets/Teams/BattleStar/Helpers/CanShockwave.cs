using System.Collections;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

namespace BattleStar
{
    public static class CanShockwave 
    {
        public static bool LaunchShockWave(GameData gameData, int spaceShipOwner, int spaceShipOwnerEnemy, float ShockWaveRadius)
        {

            return Vector2.Distance(gameData.SpaceShips[spaceShipOwner].Position,
                gameData.SpaceShips[spaceShipOwnerEnemy].Position) <= ShockWaveRadius;

        }
    }
    
}

