using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider2D : MonoBehaviour
{
    public int[] buffIdList;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Module.ActorBuff.Attack(collision.gameObject, buffIdList);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
