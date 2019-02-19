using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorHurt : UpdateModule
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ActorData));
            _requiredDataTypeList.Add(typeof(ActorController2DData));
            _requiredDataTypeList.Add(typeof(Physics2DData));
            _requiredDataTypeList.Add(typeof(ResourceHurtData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return data.GetType() == typeof(ResourceHurtData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var hurtData = objData.GetData<ResourceHurtData>();

            var force = hurtData.hurtInfo.force;
            if (force.y == 0 && force.x == 0 && hurtData.hurtInfo.duration == 0)
            {
                Stop(objData.ObjectId);
                return;
            }

            var actorData = objData.GetData<ActorData>();
            if (!ActorController.CanHurt(actorData.currentState))
            {
                return;
            }

            if ((actorData.currentState & (int)ActorStateType.Hurt) == 0)
            {
                hurtData.hurtInfo.force = Vector3Int.zero;
                hurtData.hurtInfo.duration = 0;
                return;
            }

            var physics2DData = objData.GetData<Physics2DData>();
            if (hurtData.hurtInfo.force.y == 0 && hurtData.hurtInfo.duration == 0)
            {
                hurtData.hurtInfo.force.x = 0;
                hurtData.hurtInfo.duration = 0;

                physics2DData.force.x = 0;
                physics2DData.force.y = 0;

                actorData.currentState &= ~(int)ActorStateType.Hurt;
                objData.SetDirty(actorData);
            }
            else
            {
                hurtData.hurtInfo.force.y = force.y - physics2DData.airFriction;
                if (hurtData.hurtInfo.force.y < 0)
                {
                    hurtData.hurtInfo.force.y = 0;
                    physics2DData.force.y += -physics2DData.airFriction;
                }
                else
                {
                    physics2DData.force.y = force.y;
                }

                if (hurtData.hurtInfo.duration > 0)
                {
                    var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>();
                    hurtData.hurtInfo.duration = Mathf.Max(0, hurtData.hurtInfo.duration - gameSystemData.unscaleDeltaTime);
                }

                physics2DData.force.x = hurtData.hurtInfo.force.x;

                objData.SetDirty(physics2DData);
            }
        }
    }
}