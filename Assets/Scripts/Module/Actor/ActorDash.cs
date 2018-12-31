using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorDash : UpdateModule
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ActorData));
            _requiredDataTypeList.Add(typeof(Physics2DData));
            _requiredDataTypeList.Add(typeof(ActorDashData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return data.GetType() == typeof(ActorDashData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var dashData = objData.GetData<ActorDashData>();
            if (dashData.duration == 0)
            {
                Stop(objData.ObjectId);
                return;
            }

            var actorData = objData.GetData<ActorData>();
            if (actorData.currentState != ActorStateType.SkillDefault)
            {
                dashData.currentDuration = dashData.duration = 0;
                return;
            }

            var physics2DData = objData.GetData<Physics2DData>();
            if (dashData.currentDuration == dashData.duration)
            {
                dashData.currentDuration = dashData.duration = 0;

                physics2DData.force.x = 0;

                actorData.currentState = ActorStateType.Idle;
                objData.SetDirty(actorData);
            }
            else
            {
                var worldMgr = WorldManager.Instance;
                var gameSystemData = worldMgr.GameCore.GetData<GameSystemData>();
                dashData.currentDuration += gameSystemData.unscaleDeltaTime;
                if (dashData.currentDuration >= dashData.duration)
                {
                    dashData.currentDuration = dashData.duration;
                    physics2DData.force.x = 0;
                }
                else
                {
                    physics2DData.force.x = Constant.ACTOR_DASH_SPEED_X;
                }
            }
        }
    }

}