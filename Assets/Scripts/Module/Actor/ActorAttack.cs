using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class ActorAttack : Module
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

        public static void Attack(GameObject attack, GameObject hurt, int[] attackBuffList)
        {
            var worldMgr = WorldManager.Instance;

            var battleData = worldMgr.GameCore.GetData<BattleResourceData>();
            var objId = 0;

            var attackObjId = 0;
            battleData.attackDictionary.TryGetValue(attack, out attackObjId);

            if (battleData.hurtDictionary.TryGetValue(hurt, out objId))
            {
                var objData = worldMgr.GetObjectData(objId);

                var hurtData = objData.GetData<ResourceHurtData>();
                hurtData.attackObjDataId = attackObjId;

                var buffData = objData.GetData<ActorBuffData>();

                for (var i = 0; i < attackBuffList.Length; i++)
                {
                    var buffId = attackBuffList[i];
                    var buff = worldMgr.BuffConfig.Get(buffId);

                    buffData.buffList.Add(buff);
                }

                objData.SetDirty(buffData);
            }
        }

        public static void Clear(GameObject hurt, int[] removeBuffList)
        {
            var worldMgr = WorldManager.Instance;

            var battleData = worldMgr.GameCore.GetData<BattleResourceData>();
            var objId = 0;
            if (battleData.hurtDictionary.TryGetValue(hurt, out objId))
            {
                var objData = worldMgr.GetObjectData(objId);
                var buffData = objData.GetData<ActorBuffData>();
                var buffList = buffData.buffList;
                for (var i = 0; i < removeBuffList.Length; i++)
                {
                    var buffId = removeBuffList[i];
                    for (var j = 0; j < buffList.Count; j++)
                    {
                        var buff = buffList[j];
                        if (buff.id == buffId)
                        {
                            buff.buffState = BuffState.Finished;
                            buffList[j] = buff;
                            break;
                        }
                    }
                }

                objData.SetDirty(buffData);
            }
        }
    }
}
