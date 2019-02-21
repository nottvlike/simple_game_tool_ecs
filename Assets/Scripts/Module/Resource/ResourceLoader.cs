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

                    var attackCollider2DList = transform.GetComponentsInChildren<AttackCollider2D>();
                    for (var i = 0; i < attackCollider2DList.Length; i++)
                    {
                        attackCollider2DList[i].Init(objData);
                    }

                    attackData.attack.SetActive(attackData.attackEffect.initial);
                }

                var hurtData = objData.GetData<ResourceHurtData>();
                if (hurtData != null)
                {
                    hurtData.hurt = transform.Find("Hurt").gameObject;
                    battleData.hurtDictionary.Add(hurtData.hurt, objData.ObjectId);
                }
            });
        }
    }
}
