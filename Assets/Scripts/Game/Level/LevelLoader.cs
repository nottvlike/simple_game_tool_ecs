using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader
{
    int currentLevelId;
    int currentLevelIndex;

    public void DoDrama()
    {
        var worldMgr = WorldManager.Instance;
        var levelConfig = worldMgr.GameConfig.GetLevelConfig(currentLevelId);
#if UNITY_EDITOR
        if (levelConfig == null || currentLevelIndex >= levelConfig.levelData.dramaItemList.Count)
        {
            LogUtil.E("Failed do drama, invalid level id or invalid level index!");
            return;
        }
#endif

        var dramaItem = levelConfig.levelData.dramaItemList[currentLevelIndex];
        switch(dramaItem.dramaItemType)
        {
            case DramaItemType.CreatePlayer:
                {
                    var playerData = levelConfig.playerData;
                    var objData = worldMgr.Player;

                    objData.AddData(playerData.positionData.Clone());
                    objData.AddData(playerData.directionData.Clone());
                    objData.AddData(playerData.speedData.Clone());

                    objData.AddData(playerData.resourceData.Clone());
                    objData.AddData(playerData.resourceStateData.Clone());

                    objData.AddData(new Data.JoyStickData());
                    objData.AddData(new Data.ActorSyncData());
                    objData.SetDirty();
                }
                break;
            case DramaItemType.CreateEnemy:
                break;
            case DramaItemType.Talking:
                break;
            case DramaItemType.Video:
                break;
        }
    }

    public void Save()
    {

    }

    public void Load()
    {
        currentLevelId = 0;
        currentLevelIndex = 0;
    }
}
