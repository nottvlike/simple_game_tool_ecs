using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorStress : UpdateModule
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ActorData));
            _requiredDataTypeList.Add(typeof(Physics2DData));
            _requiredDataTypeList.Add(typeof(ActorStressData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return data.GetType() == typeof(ActorStressData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var stressData = objData.GetData<ActorStressData>();
            if (stressData.duration == 0)
            {
                Stop(objData.ObjectId);
                return;
            }

            var actorData = objData.GetData<ActorData>();
            if (!ActorController.CanSkillDefault(actorData.currentState))
            {
                return;
            }

            if ((actorData.currentState & (int)ActorStateType.SkillDefault) == 0
                && (actorData.currentState & (int)ActorStateType.SkillCustom) == 0)
            {
                stressData.currentDuration = stressData.duration = 0;
                return;
            }

            var worldMgr = WorldManager.Instance;

            var physics2DData = objData.GetData<Physics2DData>();
            if (stressData.currentDuration == stressData.duration)
            {
                stressData.currentDuration = stressData.duration = 0;
                physics2DData.force.y = 0;

                actorData.currentState &= ~(int)ActorStateType.SkillDefault;
                actorData.currentState &= ~(int)ActorStateType.SkillCustom;
                objData.SetDirty(actorData);
            }
            else
            {
                var gameSystemData = worldMgr.GameCore.GetData<GameSystemData>();
                stressData.currentDuration += gameSystemData.unscaleDeltaTime;
                if (stressData.currentDuration >= stressData.duration)
                {
                    stressData.currentDuration = stressData.duration;
                    physics2DData.force.y = 0;
                }
                else
                {
                    physics2DData.force.y = -physics2DData.mass * Constant.ACTOR_STRESS_RATE_Y;

                    objData.SetDirty(physics2DData);
                }
            }
        }
    }
}
