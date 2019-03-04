using Data;
using UnityEngine;

namespace Module
{
    public class ActorFly : UpdateModule
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ActorData));
            _requiredDataTypeList.Add(typeof(Physics2DData));
            _requiredDataTypeList.Add(typeof(ActorFlyData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return data.GetType() == typeof(ActorFlyData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var flyData = objData.GetData<ActorFlyData>();
            if (flyData.duration == 0)
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
                flyData.currentDuration = flyData.duration = 0;
                return;
            }

            var worldMgr = WorldManager.Instance;

            var physics2DData = objData.GetData<Physics2DData>();
            if (flyData.currentDuration == flyData.duration)
            {
                flyData.currentDuration = flyData.duration = 0;

                physics2DData.force.x = 0;
                physics2DData.force.y = 0;

                actorData.currentState &= ~(int)ActorStateType.SkillDefault;
                actorData.currentState &= ~(int)ActorStateType.SkillCustom;
                objData.SetDirty(actorData);
            }
            else
            {
                var gameSystemData = worldMgr.GameCore.GetData<GameSystemData>();
                flyData.currentDuration += gameSystemData.unscaleDeltaTime;
                if (flyData.currentDuration >= flyData.duration)
                {
                    flyData.currentDuration = flyData.duration;
                    physics2DData.force.y = 0;
                }
                else
                {
                    var seconds = flyData.currentDuration * Constant.ACTOR_FLY_SHAKE_DURATION / Constant.SECOND_TO_MILLISECOND;
                    if (seconds % 2 == 0)
                    {
                        physics2DData.force.y = Constant.ACTOR_FLY_SHAKE_Y;
                    }
                    else
                    {
                        physics2DData.force.y = -Constant.ACTOR_FLY_SHAKE_Y;
                    }
                }

                physics2DData.force.x = Constant.ACTOR_FLY_SPEED_X;

                objData.SetDirty(physics2DData);
            }
        }
    }
}