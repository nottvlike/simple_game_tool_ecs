using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorAttribute : Module
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ActorAttributeData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
            _requiredDataTypeList.Add(typeof(ResourceData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return data.GetType() == typeof(ActorAttributeData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var actorAttribute = objData.GetData<ActorAttributeData>();
            if (actorAttribute.baseAttribute.hp <= 0)
            {
                LogUtil.I("actor {0} is dying!", resourceStateData.name);
            }
        }
    }
}