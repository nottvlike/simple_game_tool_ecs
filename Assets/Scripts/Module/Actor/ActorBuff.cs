using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorBuff : UpdateModule
    {
        public BaseAttributeInfo baseAttribute;
        public ExtraAttributeInfo extraAttribute;

        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ActorData));
            _requiredDataTypeList.Add(typeof(ActorAttributeData));
            _requiredDataTypeList.Add(typeof(ActorBuffData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return data.GetType() == typeof(ActorBuffData);
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var buffData = objData.GetData<ActorBuffData>();
            var buffList = buffData.buffList;
            if (buffList.Count == 0 || !CheckIfHasNeedRefreshBuff(buffList))
            {
                Stop(objData.ObjectId);
                return;
            }

            ResetAttributeInfo(objData);

            var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>();

            var isDirty = false;
            for (var i = 0; i < buffList.Count;)
            {
                var buff = buffList[i];
                if (buff.buffState != BuffState.Finished)
                {
                    if (buff.buffState == BuffState.Init)
                    {
                        if (buff.lastUpdateTime == 0)
                        {
                            buff.lastUpdateTime = gameSystemData.unscaleTime;
                        }

                        if (buff.lastUpdateTime + buff.delay <= gameSystemData.unscaleTime)
                        {
                            buff.buffState = BuffState.Start;
                        }
                    }
                    else if (buff.buffState == BuffState.Stop)
                    {
                        if (buff.lastUpdateTime + buff.interval <= gameSystemData.unscaleTime)
                        {
                            buff.buffState = BuffState.Start;
                        }
                    }
                    else if (buff.buffState == BuffState.Start)
                    {
                        if (buff.count > 0)
                        {
                            isDirty = true;
                            AddBuffAttribute(objData, buff);

                            buff.count--;
                            buff.currentCount++;
                        }

                        if (buff.time > 0)
                        {
                            buff.time = Mathf.Max(0, buff.time - gameSystemData.unscaleDeltaTime);
                        }

                        if (buff.time == 0 && buff.count == 0)
                        {
                            buff.buffState = BuffState.Finished;
                        }
                        else if (buff.time == 0)
                        {
                            buff.buffState = BuffState.Stop;
                        }

                        buff.lastUpdateTime = gameSystemData.unscaleTime;
                    }

                    buffList[i] = buff;
                    i++;
                }
                else
                {
                    if ((buff.buffAttribute & (int)BuffAttribute.NeedRemove) != 0)
                    {
                        isDirty = true;
                        RemoveBuffAttribute(objData, buff);
                    }

                    buffList.RemoveAt(i);
                }
            }

            if (isDirty)
            {
                var attributeData = objData.GetData<ActorAttributeData>();
                UpdateAttributeInfo(attributeData);

                objData.SetDirty(attributeData);
            }
        }

        public bool CheckIfHasNeedRefreshBuff(List<Buff> buffList)
        {
            for (var i = 0; i < buffList.Count; i++)
            {
                var buff = buffList[i];
                if (buff.count > 0 || buff.time > 0 || buff.buffState == BuffState.Finished)
                {
                    return true;
                }
            }

            return false;
        }

        public void ResetAttributeInfo(ObjectData objData)
        {
            baseAttribute.hp = 0;
            baseAttribute.mp = 0;
            baseAttribute.atk = 0;
            baseAttribute.def = 0;

            var attributeData = objData.GetData<ActorAttributeData>();
            extraAttribute.hp = attributeData.extraAttribute.hp;
            extraAttribute.hpMax = attributeData.extraAttribute.hpMax;
            extraAttribute.mp = attributeData.extraAttribute.mp;
            extraAttribute.atk = attributeData.extraAttribute.atk;
            extraAttribute.def = attributeData.extraAttribute.def;
        }

        public void UpdateAttributeInfo(ActorAttributeData attributeData)
        {
            attributeData.baseAttribute.hp += baseAttribute.hp;
            attributeData.baseAttribute.mp += baseAttribute.mp;
            attributeData.baseAttribute.atk += baseAttribute.atk;
            attributeData.baseAttribute.def += baseAttribute.def;

            attributeData.extraAttribute.hp = extraAttribute.hp;
            attributeData.extraAttribute.hpMax = extraAttribute.hpMax;
            attributeData.extraAttribute.mp = extraAttribute.mp;
            attributeData.extraAttribute.atk = extraAttribute.atk;
            attributeData.extraAttribute.def = extraAttribute.def;
        }

        public void AddBuffAttribute(ObjectData objData, Buff buff)
        {
            var attributeData = objData.GetData<ActorAttributeData>();
            var actorData = objData.GetData<ActorData>();

            var value = 0;
            if (buff.currentCount < buff.value.Length)
            {
                value = buff.value[buff.currentCount];
            }
            else
            {
                value = buff.value[0];
            }

            switch (buff.buffType)
            {
                case BuffType.NormalChangeHp:
                    {
                        var hurt = buff.valueType == BuffValueType.Normal ? value : Mathf.FloorToInt(attributeData.baseAttribute.hp * value * Constant.PERCENT);
                        if (hurt < 0)
                        {
                            hurt += attributeData.baseAttribute.def;
                        }

                        baseAttribute.hp += hurt;
                    }
                    break;
                case BuffType.ChangeHpMax:
                    {
                        var actorInfo = WorldManager.Instance.ActorConfig.Get(actorData.actorId);
                        extraAttribute.hpMax += value;

                        var extraHp = (float)value / (actorInfo.attributeInfo.hp + attributeData.extraAttribute.hpMax) * attributeData.baseAttribute.hp;
                        baseAttribute.hp += Mathf.FloorToInt(extraHp);
                    }
                    break;
            }
        }

        public void RemoveBuffAttribute(ObjectData objData, Buff buff)
        {
            var attributeData = objData.GetData<ActorAttributeData>();
            var actorData = objData.GetData<ActorData>();
            var actorInfo = WorldManager.Instance.ActorConfig.Get(actorData.actorId);

            var value = 0;
            if (buff.currentCount < buff.value.Length)
            {
                value = buff.value[buff.currentCount];
            }
            else
            {
                value = buff.value[0];
            }

            switch (buff.buffType)
            {
                case BuffType.ChangeHpMax:
                    {
                        var extraHp = (float)value / (actorInfo.attributeInfo.hp + attributeData.extraAttribute.hpMax) * attributeData.baseAttribute.hp;
                        baseAttribute.hp -= Mathf.FloorToInt(extraHp);
                        extraAttribute.hpMax -= value;
                    }
                    break;
            }
        }

        public static void Attack(GameObject hurt, int[] attackBuffList)
        {
            var worldMgr = WorldManager.Instance;

            var battleData = worldMgr.GameCore.GetData<BattleData>();
            var objId = 0;

            if (battleData.hurtDictionary.TryGetValue(hurt, out objId))
            {
                var objData = worldMgr.GetObjectData(objId);
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

            var battleData = worldMgr.GameCore.GetData<BattleData>();
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
