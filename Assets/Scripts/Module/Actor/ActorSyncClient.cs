using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class ActorSyncClient : UpdateModule
    {
        public override bool IsBelong(List<Data.Data> dataList)
        {
            var index = 0;
            for (var i = 0; i < dataList.Count; ++i)
            {
                var dataType = dataList[i].GetType();
                if (dataType == typeof(JoyStickData) || dataType == typeof(ActorSyncData))
                {
                    index++;
                }
            }

            return index == 2;
        }

        protected override void UpdateObject(int objId, ObjectData objData)
        {
            var joyStickData = objData.GetData<JoyStickData>() as JoyStickData;

            joyStickData.serverActionList.AddRange(joyStickData.clientActionList);
            joyStickData.clientActionList.Clear();
        }
    }
}