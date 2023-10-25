using System.Collections;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

namespace BattleStar
{
    public static class CheckScore
    {
        public static bool IsWining(GameData gameData, int spaceShipOwner, int spaceShipOwnerEnemy)
        {
            return gameData.SpaceShips[spaceShipOwner].Score > gameData.SpaceShips[spaceShipOwnerEnemy].Score;
        }
    }
}

