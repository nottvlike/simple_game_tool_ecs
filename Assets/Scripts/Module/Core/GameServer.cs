using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class GameServer : Module
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(GameNetworkData));
        }

        public override void Refresh(ObjectData obj)
        {
        }
    }
}
