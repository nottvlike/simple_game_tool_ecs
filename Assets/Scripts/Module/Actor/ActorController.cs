using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class ActorController : UpdateModule
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ActorData));
            _requiredDataTypeList.Add(typeof(Physics2DData));
            _requiredDataTypeList.Add(typeof(ActorJumpData));
            _requiredDataTypeList.Add(typeof(DirectionData));
            _requiredDataTypeList.Add(typeof(SpeedData));
            _requiredDataTypeList.Add(typeof(ServerJoyStickData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return data.GetType() == typeof(ServerJoyStickData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var joyStickData = objData.GetData<ServerJoyStickData>();
            var serverActionList = joyStickData.actionList;
            if (serverActionList.Count == 0)
            {
                Stop(objData.ObjectId);
                return;
            }

            var worldMgr = WorldManager.Instance;

            var gameSystemData = worldMgr.GameCore.GetData<GameSystemData>();

            var actorData = objData.GetData<ActorData>();
            var actorInfo = worldMgr.ActorConfig.Get(actorData.actorId);

            var physics2DData = objData.GetData<Physics2DData>();
            var speedData = objData.GetData<SpeedData>();
            var directionData = objData.GetData<DirectionData>();
            var jumpData = objData.GetData<ActorJumpData>();

            for (var i = 0; i < serverActionList.Count;)
            {
                var serverAction = serverActionList[i];
                if (serverAction.frame == gameSystemData.clientFrame)
                {
                    ResetDefaultSkill(objData, serverAction);
                    switch (serverAction.actionType)
                    {
                        case JoyStickActionType.Move:
                            physics2DData.force = Vector3Int.zero;
                            physics2DData.friction = 0;
                            speedData.speed = actorInfo.speed;
                            directionData.direction.x = serverAction.actionParam == JoyStickActionFaceType.Right ? 1 : -1;

                            actorData.currentState = ActorStateType.Move;

                            objData.SetDirty(speedData, directionData, physics2DData, actorData);
                            break;
                        case JoyStickActionType.CancelMove:
                            physics2DData.force = Vector3Int.zero;
                            physics2DData.friction = actorInfo.friction;

                            objData.SetDirty(speedData, physics2DData);
                            break;
                        case JoyStickActionType.Jump:
                            speedData.speed = 0;
                            physics2DData.friction = 0;
                            physics2DData.force = Vector3Int.zero;

                            jumpData.currentJump = actorInfo.jump;

                            actorData.currentState = ActorStateType.Jump;

                            objData.SetDirty(jumpData, physics2DData, actorData);
                            break;
                        case JoyStickActionType.SkillDefault:
                            speedData.speed = 0;
                            physics2DData.friction = 0;
                            physics2DData.force = Vector3Int.zero;

                            actorData.currentState = ActorStateType.SkillDefault;

                            if (serverAction.skillDefaultType == SkillDefaultType.Fly)
                            {
                                var flyData = objData.GetData<ActorFlyData>();
                                flyData.duration = actorInfo.defaultSkillDuration;
                                flyData.currentDuration = 0;

                                objData.SetDirty(flyData, physics2DData, actorData);
                            }
                            else if (serverAction.skillDefaultType == SkillDefaultType.Dash)
                            {
                                var dashData = objData.GetData<ActorDashData>();
                                dashData.duration = actorInfo.defaultSkillDuration;
                                dashData.currentDuration = 0;

                                objData.SetDirty(dashData, physics2DData, actorData);
                            }
                            else if (serverAction.skillDefaultType == SkillDefaultType.Stress)
                            {
                                var stressData = objData.GetData<ActorStressData>();
                                stressData.duration = actorInfo.defaultSkillDuration;
                                stressData.currentDuration = 0;

                                objData.SetDirty(stressData, physics2DData, actorData);
                            }
                            break;
                    }

                    serverActionList.Remove(serverAction);
                    worldMgr.PoolMgr.Release(serverAction);
                }
                else
                {
                    i++;
                }
            }
        }

        void ResetDefaultSkill(ObjectData objData, JoyStickActionData serverAction)
        {
            if (serverAction.skillDefaultType == SkillDefaultType.Fly)
            {
                var flyData = objData.GetData<ActorFlyData>();
                flyData.duration = 0;
                flyData.currentDuration = 0;
            }
            else if (serverAction.skillDefaultType == SkillDefaultType.Dash)
            {
                var dashData = objData.GetData<ActorDashData>();
                dashData.duration = 0;
                dashData.currentDuration = 0;
            }
            else if (serverAction.skillDefaultType == SkillDefaultType.Stress)
            {
                var stressData = objData.GetData<ActorStressData>();
                stressData.duration = 0;
                stressData.currentDuration = 0;
            }
        }
    }
}
