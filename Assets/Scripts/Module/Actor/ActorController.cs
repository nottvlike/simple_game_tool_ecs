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
                if (serverAction.frame == gameSystemData.clientFrame && serverAction.actorId == actorData.actorId)
                {
                    switch (serverAction.actionType)
                    {
                        case JoyStickActionType.Move:
                            physics2DData.friction = 0;
                            speedData.speed = actorInfo.speed;
                            directionData.direction.x = serverAction.actionParam == JoyStickActionFaceType.Right ? 1 : -1;

                            actorData.currentState |= (int)ActorStateType.Move;

                            objData.SetDirty(speedData, directionData, physics2DData, actorData);
                            break;
                        case JoyStickActionType.CancelMove:
                            physics2DData.friction = actorInfo.friction;

                            objData.SetDirty(speedData, physics2DData);
                            break;
                        case JoyStickActionType.Jump:
                            jumpData.currentJump = actorInfo.jump;

                            actorData.currentState |= (int)ActorStateType.Jump;

                            objData.SetDirty(jumpData, physics2DData, actorData);
                            break;
                        case JoyStickActionType.SkillDefault:
                            actorData.currentState |= (int)ActorStateType.SkillDefault;

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

        public static bool CanMove(int currentState)
        {
            if ((currentState & (int)ActorStateType.Jump) != 0 && (currentState & (int)ActorStateType.SkillDefault) != 0 
                && (currentState & (int)ActorStateType.SkillCustom) != 0)
                return false;

            return true;
        }

        public static bool CanJump(int currentState)
        {
            if ((currentState & (int)ActorStateType.SkillDefault) != 0 && (currentState & (int)ActorStateType.SkillCustom) != 0)
                return false;

            return true;
        }

        public static bool CanSkillDefault(int currentState)
        {
            if ((currentState & (int)ActorStateType.SkillCustom) != 0)
                return false;

            return true;
        }

        public static bool CanSkillCustom(int currentState)
        {
            if ((currentState & (int)ActorStateType.SkillDefault) != 0)
                return false;

            return true;
        }
    }
}
