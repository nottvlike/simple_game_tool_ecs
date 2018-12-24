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
            _requiredDataTypeList.Add(typeof(ActorJumpData));
            _requiredDataTypeList.Add(typeof(DirectionData));
            _requiredDataTypeList.Add(typeof(SpeedData));
            _requiredDataTypeList.Add(typeof(JoyStickData));
            _requiredDataTypeList.Add(typeof(ResourceData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var worldMgr = WorldManager.Instance;

            var gameSystemData = worldMgr.GameCore.GetData<GameSystemData>();

            var actorData = objData.GetData<ActorData>();
            var actorInfo = worldMgr.ActorConfig.Get(actorData.actorId);

            var speedData = objData.GetData<SpeedData>();
            var directionData = objData.GetData<DirectionData>();

            var jumpData = objData.GetData<ActorJumpData>();
            var resourceData = objData.GetData<ResourceData>();

            var joyStickData = objData.GetData<JoyStickData>();
            var serverActionList = joyStickData.serverActionList;
            for (var i = 0; i < serverActionList.Count;)
            {
                var serverAction = serverActionList[i];
                if (serverAction.frame == gameSystemData.clientFrame)
                {
                    switch (serverAction.actionType)
                    {
                        case JoyStickActionType.Run:
                            speedData.speed = actorInfo.speed;
                            speedData.friction = 0;
                            directionData.x = serverAction.actionParam == JoyStickActionFaceType.Right ? 1 : -1;
                            break;
                        case JoyStickActionType.CancelRun:
                            speedData.friction = actorInfo.friction;
                            break;
                        case JoyStickActionType.Jump:
                            jumpData.currentJump = actorInfo.jump;
                            jumpData.fall = 0;
                            jumpData.groundPosition = resourceData.gameObject.transform.position;
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
    }
}
