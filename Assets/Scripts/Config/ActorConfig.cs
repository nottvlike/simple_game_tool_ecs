using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ActorInfo
{
    public int actorId;
    public string actorName;
    public string resourceName;
    public int speed;
    public int friction;
    public Vector3 jump;
    public int airFriction;
}

public class ActorConfig : ScriptableObject
{
    public ActorInfo[] actorInfoList;

    ActorInfo _defaultActorInfo;

    public ActorInfo Get(int actorId)
    {
        for (var i = 0; i < actorInfoList.Length; i++)
        {
            var actorInfo = actorInfoList[i];
            if (actorInfo.actorId == actorId)
            {
                return actorInfo;
            }
        }

        LogUtil.E("Failed to find ActorInfo " + actorId);
        return _defaultActorInfo;
    }
}
