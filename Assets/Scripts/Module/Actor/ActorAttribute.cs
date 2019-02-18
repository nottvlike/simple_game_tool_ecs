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
            var totalLeftHp = actorAttribute.baseAttribute.hp + actorAttribute.extraAttribute.hp;
            LogUtil.I("{0} hp : {1}!", resourceStateData.name, totalLeftHp);

            if (totalLeftHp <= 0)
            {
                ActorLoader.DestroyActor(objData);

                var battleData = WorldManager.Instance.GameCore.GetData<BattleData>();
                battleData.battleInitialize.OnActorDied(objData.ObjectId);
            }
        }
    }
}