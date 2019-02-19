using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class AttackCollider2D : MonoBehaviour
{
    ResourceAttackInfo _attackInfo;

    bool GetAttackInfo()
    {
        var worldMgr = WorldManager.Instance;
        var battleData = worldMgr.GameCore.GetData<BattleResourceData>();
        var objId = 0;
        if (battleData.attackDictionary.TryGetValue(gameObject, out objId))
        {
            var objData = worldMgr.GetObjectData(objId);

            var attackData = objData.GetData<ResourceAttackData>();
            _attackInfo = attackData.attackInfo;

            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetAttackInfo())
        {
            Module.ActorAttack.Attack(gameObject, collision.gameObject, _attackInfo.buffList);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    [HideInInspector]
    public List<int> removedBuffList = new List<int>();
    private void OnTriggerExit2D(Collider2D collision)
    {
        removedBuffList.Clear();

        if (GetAttackInfo())
        {
            var buffList = _attackInfo.buffList;
            for (var i = 0; i < buffList.Length; i++)
            {
                var buffId = buffList[i];
                var buff = WorldManager.Instance.BuffConfig.Get(buffId);
                if ((buff.buffAttribute & (int)BuffAttribute.NeedRemove) != 0)
                {
                    removedBuffList.Add(buffId);
                }
            }

            if (removedBuffList.Count > 0)
            {
                Module.ActorAttack.Clear(collision.gameObject, removedBuffList.ToArray());
            }
        }
    }
}
