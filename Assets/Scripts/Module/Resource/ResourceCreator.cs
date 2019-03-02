using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ResourceCreator : Module
    {
        protected override void InitRequiredDataType()
        {
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return false;
        }

        public override void Refresh(ObjectData objData)
        {
        }

        public static ObjectData CreateActor(int actorId, CreatureCampType camp, Vector3 initialPosition)
        {
            var worldMgr = WorldManager.Instance;

            var objData = worldMgr.PoolMgr.GetObjData();

            var actorInfo = worldMgr.ActorConfig.Get(actorId);
            var actorData = objData.AddData<ActorData>();
            actorData.actorId = actorId;
            actorData.defaultSkill = actorInfo.defaultSkill;
            if (actorData.defaultSkill == SkillDefaultType.Fly)
            {
                objData.AddData<ActorFlyData>();
            }
            else if (actorData.defaultSkill == SkillDefaultType.Dash)
            {
                objData.AddData<ActorDashData>();
            }
            else if (actorData.defaultSkill == SkillDefaultType.Stress)
            {
                objData.AddData<ActorStressData>();
            }

            var actorAttributeData = objData.AddData<ActorAttributeData>();
            actorAttributeData.baseAttribute = actorInfo.attributeInfo;

            var physics2DData = objData.AddData<Physics2DData>();
            physics2DData.gravity = 10;
            physics2DData.airFriction = actorInfo.airFriction;
            physics2DData.mass = actorInfo.mass;

            var directionData = objData.AddData<DirectionData>();
            directionData.direction.x = 1;

            objData.AddData<ActorController2DData>();
            objData.AddData<ActorJumpData>();
            objData.AddData<ServerData>();
            objData.AddData<SpeedData>();
            objData.AddData<ServerJoyStickData>();
            objData.AddData<ActorSyncData>();
            objData.AddData<ActorBuffData>();
            objData.AddData<ResourceHurtData>();

            var attackData = objData.AddData<ResourceAttackData>();
            var effect = worldMgr.BuffConfig.GetEffect(actorInfo.defulttSkillEffectId);
            attackData.attackEffect = effect;

            var resourceData = objData.AddData<ResourceData>();
            resourceData.resource = actorInfo.resourceName;
            resourceData.initialPosition = initialPosition;

            var resourceStateData = objData.AddData<ResourceStateData>();
            resourceStateData.isGameObject = true;
            resourceStateData.isInstantiated = false;
            resourceStateData.name = actorInfo.actorName;

            var creatureStateData = objData.AddData<CreatureStateData>();
            creatureStateData.type = CreatureType.Actor;
            creatureStateData.campType = camp;
            creatureStateData.stateType = CreatureStateType.Load;

            if (camp == CreatureCampType.Player)
            {
                objData.AddData<ClientJoyStickData>();
                objData.AddData<FollowCameraData>();
            }

            objData.SetDirty();
            return objData;
        }

        public static ObjectData CreateBattleItem(int itemId, CreatureCampType camp, Vector3 initialPosition)
        {
            var worldMgr = WorldManager.Instance;

            var objData = worldMgr.PoolMgr.GetObjData();

            var itemInfo = worldMgr.BattleItemConfig.Get(itemId);
            var resourceData = objData.AddData<ResourceData>();
            resourceData.resource = itemInfo.resource;
            resourceData.initialPosition = initialPosition;

            var resourceStateData = objData.AddData<ResourceStateData>();
            resourceStateData.isGameObject = true;
            resourceStateData.isInstantiated = false;
            resourceStateData.name = itemInfo.itemName;

            var creatureStateData = objData.AddData<CreatureStateData>();
            creatureStateData.type = CreatureType.BattleItem;
            creatureStateData.campType = camp;
            creatureStateData.stateType = CreatureStateType.Load;

            var attackData = objData.AddData<ResourceAttackData>();
            var effect = worldMgr.BuffConfig.GetEffect(itemInfo.attackId);
            attackData.attackEffect = effect;

            objData.SetDirty();
            return objData;
        }

        public static void ReleaseResource(ObjectData objData)
        {
            var creatureStateData = objData.GetData<CreatureStateData>();
            creatureStateData.stateType = CreatureStateType.Release;

            objData.SetDirty(creatureStateData);

            WorldManager.Instance.PoolMgr.ReleaseObjData(objData);
        }
    }
}
