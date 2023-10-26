using System;
using System.Collections;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

namespace BattleStar
{
    public static class DetectMine 
    {
        public static bool MineDetected(GameData gameData, int spaceShipOwner, int spaceShipOwnerEnemy)
        {
            Vector2 _cirlcePos = gameData.SpaceShips[spaceShipOwner].Position + new Vector2(1f, 0);
            var hitColliders = Physics2D.OverlapCircle(_cirlcePos, 1f,  13);
            return hitColliders;
        }

    }
    
}

