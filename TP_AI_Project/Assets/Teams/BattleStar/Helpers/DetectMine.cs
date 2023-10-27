using System;
using System.Collections;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

namespace BattleStar
{
    public static class DetectMine 
    {
        // public static bool MineDetectedSphere(GameData gameData, int spaceShipOwner)
        // {
        //     Vector2 _cirlcePos = gameData.SpaceShips[spaceShipOwner].Position +
        //                          gameData.SpaceShips[spaceShipOwner].Velocity.normalized * 1;
        //     var hitColliders = Physics2D.OverlapCircle(_cirlcePos, 3f,  LayerMask.GetMask("Mine"));
        //     if (hitColliders != null)
        //     {
        //         Debug.DrawLine(gameData.SpaceShips[spaceShipOwner].Position, hitColliders.transform.position, Color.red);
        //     }
        //     return hitColliders;
        // }
        
        public static bool MineDetectedBox(GameData gameData, int spaceShipOwner, BattleStarController controller, float distance)
        { 
            RaycastHit2D hit = Physics2D.Raycast(gameData.SpaceShips[spaceShipOwner].Position,  gameData.SpaceShips[spaceShipOwner].LookAt, distance, LayerMask.GetMask("Mine"));
            if (hit.collider != null)
            {
                Debug.DrawLine(gameData.SpaceShips[spaceShipOwner].Position, hit.collider.transform.position, Color.green);
                controller.SetNearestMine(hit.collider.transform.position);
                return true;
            }
            return false;
        }

    }
    
}

