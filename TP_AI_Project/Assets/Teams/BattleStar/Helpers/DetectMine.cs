using System;
using System.Collections;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

namespace BattleStar
{
    public static class DetectMine 
    {
        public static bool MineDetected(GameData gameData, int spaceShipOwner)
        {
            Vector2 _cirlcePos = gameData.SpaceShips[spaceShipOwner].Position +
                                 gameData.SpaceShips[spaceShipOwner].Velocity.normalized * 1;
            var hitColliders = Physics2D.OverlapCircle(_cirlcePos, 3f,  LayerMask.GetMask("Mine"));
            if (hitColliders != null)
            {
                Debug.DrawLine(_cirlcePos, hitColliders.transform.position, Color.red);
            }
            return hitColliders;
        }

    }
    
}

