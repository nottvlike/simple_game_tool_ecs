using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class ActorSyncClient : UpdateModule
    {
        public override void Refresh(ObjectData objData)
        {
            var joyStickData = objData.GetData<JoyStickData>();

            joyStickData.serverActionList.AddRange(joyStickData.clientActionList);
            joyStickData.clientActionList.Clear();
        }

        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(JoyStickData));
            _requiredDataTypeList.Add(typeof(ActorSyncData));
        }
    }
}