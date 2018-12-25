using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class ActorSyncClient : Module
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ClientJoyStickData));
            _requiredDataTypeList.Add(typeof(ServerJoyStickData));
            _requiredDataTypeList.Add(typeof(ActorSyncData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return data.GetType() == typeof(ClientJoyStickData);
        }

        public override void Refresh(ObjectData objData)
        {
            var clientJoyStickData = objData.GetData<ClientJoyStickData>();
            if (clientJoyStickData.actionList.Count == 0)
            {
                return;
            }

            var serverJoyStickData = objData.GetData<ServerJoyStickData>();
            serverJoyStickData.actionList.AddRange(clientJoyStickData.actionList);
            clientJoyStickData.actionList.Clear();

            objData.SetDirty(serverJoyStickData);
        }
    }
}