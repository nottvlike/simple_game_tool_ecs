using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInitialize : MonoBehaviour
{
    public int battleId;
    public ResourceCreator[] resourceCreatorList;

    void Awake()
    {
        for (var i = 0; i < resourceCreatorList.Length; i++)
        {
            resourceCreatorList[i].Init(battleId);
        }
    }
}
