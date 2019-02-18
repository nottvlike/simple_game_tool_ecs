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
            _requiredDataTypeList.Add(typeof(ActorHurtData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return data.GetType() == typeof(ActorHurtData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var hurtData = objData.GetData<ActorHurtData>();

            var force = hurtData.hurt.force;
            if (force.y == 0 && force.x == 0 && hurtData.hurt.duration == 0)
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
                hurtData.hurt.force = Vector3Int.zero;
                hurtData.hurt.duration = 0;
                return;
            }

            var physics2DData = objData.GetData<Physics2DData>();
            if (hurtData.hurt.force.y == 0 && hurtData.hurt.duration == 0)
            {
                hurtData.hurt.force.x = 0;
                hurtData.hurt.duration = 0;

                physics2DData.force.x = 0;
                physics2DData.force.y = 0;

                actorData.currentState &= ~(int)ActorStateType.Hurt;
                objData.SetDirty(actorData);
            }
            else
            {
                hurtData.hurt.force.y = force.y - physics2DData.airFriction;
                if (hurtData.hurt.force.y < 0)
                {
                    hurtData.hurt.force.y = 0;
                    physics2DData.force.y += -physics2DData.airFriction;
                }
                else
                {
                    physics2DData.force.y = force.y;
                }

                if (hurtData.hurt.duration > 0)
                {
                    var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>();
                    hurtData.hurt.duration = Mathf.Max(0, hurtData.hurt.duration - gameSystemData.unscaleDeltaTime);
                }

                physics2DData.force.x = hurtData.hurt.force.x;

                objData.SetDirty(physics2DData);
            }
        }
    }
}