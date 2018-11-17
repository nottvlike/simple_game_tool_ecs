using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class GameSystem : Module, IUpdateEvent
    {
        public GameSystem()
        {
            WorldManager.Instance.UnityEventMgr.Add(this);
        }

        public override bool IsBelong(List<Data.Data> dataList)
        {
            for (var i = 0; i < dataList.Count; ++i)
            {
                var dataType = dataList[i].GetType();
                if (dataType == typeof(GameSystemData))
                {
                    return true;
                }
            }

            return false;
        }

        public void Update()
        {
            var objData = WorldManager.Instance.GameCore;
            var gameSystemData = objData.GetData<GameSystemData>() as GameSystemData;

            gameSystemData.clientFrame++;
        }
    }
}
