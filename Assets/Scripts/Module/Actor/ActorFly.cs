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
            _requiredDataTypeList.Add(typeof(PositionData));
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
            if (actorData.currentState != ActorStateType.SkillDefault)
            {
                actorData.currentState = ActorStateType.SkillDefault;
                objData.SetDirty(actorData);
            }

            var worldMgr = WorldManager.Instance;

            var physics2DData = objData.GetData<Physics2DData>();
            var positionData = objData.GetData<PositionData>();
            var actorGroundY = ActorPhysics2D.GetActorFootY(positionData, physics2DData);
            if (positionData.ground.y == actorGroundY && flyData.currentDuration == flyData.duration)
            {
                flyData.currentDuration = flyData.duration = 0;

                physics2DData.force.x = 0;
                physics2DData.force.y = 0;

                actorData.currentState = ActorStateType.Idle;
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
                        physics2DData.force.y = physics2DData.gravity + Constant.ACTOR_FLY_SHAKE_Y;
                    }
                    else
                    {
                        physics2DData.force.y = physics2DData.gravity - Constant.ACTOR_FLY_SHAKE_Y;
                    }
                }

                physics2DData.force.x = Constant.ACTOR_FLY_SPEED_X;
            }
        }
    }
}