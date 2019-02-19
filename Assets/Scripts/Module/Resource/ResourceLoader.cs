using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class ResourceLoader : Module
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ResourceData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return data.GetType() == typeof(ResourceStateData) || data.GetType() == typeof(ResourceData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            var resourceData = objData.GetData<ResourceData>();
            if (!string.IsNullOrEmpty(resourceData.resource) && resourceStateData.isGameObject)
            {
                if (!resourceStateData.isInstantiated)
                {
                    LoadResource(objData, resourceStateData, resourceData);
                }
            }
        }

        void LoadResource(ObjectData objData, ResourceStateData resourceStateData, ResourceData resourceData)
        {
            var worldMgr = WorldManager.Instance;
            worldMgr.ResourceMgr.LoadAsync(resourceData.resource, delegate (Object obj)
            {
                resourceStateData.isInstantiated = true;
                var resource = worldMgr.PoolMgr.GetGameObject(resourceData.resource, obj);
                var transform = resource.transform;
                resourceData.gameObject = resource;
                resource.name = resourceStateData.name;
                resource.transform.position = resourceData.initialPosition;

                if (resourceStateData.resourceType == ResourceType.Actor)
                {
                    var controller = objData.GetData<ActorController2DData>();
                    var rigidbody2D = resource.GetComponent<Rigidbody2D>();
                    controller.rigidbody2D = rigidbody2D;
                    controller.foot = transform.Find("Foot");
                    controller.positionY = Mathf.RoundToInt(controller.foot.position.y);
                }

                var battleData = worldMgr.GameCore.GetData<BattleResourceData>();
                var attackData = objData.GetData<ResourceAttackData>();
                if (attackData != null)
                {
                    attackData.attack = transform.Find("Attack").gameObject;
                    battleData.attackDictionary.Add(attackData.attack, objData.ObjectId);
                    attackData.attack.SetActive(attackData.attackInfo.initial);
                }

                var hurtData = objData.GetData<ResourceHurtData>();
                if (hurtData != null)
                {
                    hurtData.hurt = transform.Find("Hurt").gameObject;
                    battleData.hurtDictionary.Add(hurtData.hurt, objData.ObjectId);
                }
            });
        }

        public static ObjectData CreateActor(int actorId, ResourceCampType camp, Vector3 initialPosition)
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
            var attackInfo = worldMgr.BuffConfig.GetAttackInfo(actorInfo.defulttSkillAttackId);
            attackData.attackInfo = attackInfo;

            var resourceData = objData.AddData<ResourceData>();
            resourceData.resource = actorInfo.resourceName;
            resourceData.initialPosition = initialPosition;

            var resourceStateData = objData.AddData<ResourceStateData>();
            resourceStateData.isGameObject = true;
            resourceStateData.isInstantiated = false;
            resourceStateData.name = actorInfo.actorName;
            resourceStateData.resourceType = ResourceType.Actor;
            resourceStateData.campType = camp;

            if (camp == ResourceCampType.Player)
            {
                objData.AddData<ClientJoyStickData>();
                objData.AddData<FollowCameraData>();
            }

            objData.SetDirty();
            return objData;
        }

        public static ObjectData CreateBattleItem(int itemId, ResourceCampType camp, Vector3 initialPosition)
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
            resourceStateData.resourceType = ResourceType.BattleItem;
            resourceStateData.campType = camp;

            var attackData = objData.AddData<ResourceAttackData>();
            var attackInfo = worldMgr.BuffConfig.GetAttackInfo(itemInfo.attackId);
            attackData.attackInfo = attackInfo;

            objData.SetDirty();
            return objData;
        }

        public static void DestroyResource(ObjectData objData)
        {
            var worldMgr = WorldManager.Instance;

            var resourceData = objData.GetData<ResourceData>();
            if (resourceData.gameObject != null)
            {
                worldMgr.PoolMgr.ReleaseGameObject(resourceData.resource, resourceData.gameObject);
                resourceData.gameObject = null;
            }

            var controller = objData.GetData<ActorController2DData>();
            if (controller != null)
            {
                controller.rigidbody2D = null;
                controller.foot = null;
            }

            var battleData = worldMgr.GameCore.GetData<BattleResourceData>();
            var hurtData = objData.GetData<ResourceHurtData>();
            if (hurtData != null)
            {
                battleData.hurtDictionary.Remove(hurtData.hurt);
            }

            var attackData = objData.GetData<ResourceAttackData>();
            if (attackData != null)
            {
                battleData.attackDictionary.Remove(attackData.attack);
            }

            worldMgr.PoolMgr.ReleaseObjData(objData);
        }
    }
}
