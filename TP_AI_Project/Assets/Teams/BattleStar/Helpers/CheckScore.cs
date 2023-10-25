using System.Collections;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

public class CheckScore : MonoBehaviour
{
    public bool IsWining(GameData gameData, int spaceShipOwner, int spaceShipOwnerEnemy)
    {
        if (gameData.SpaceShips[spaceShipOwner].Score > gameData.SpaceShips[spaceShipOwnerEnemy].Score)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
