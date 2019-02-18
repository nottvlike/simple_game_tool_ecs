using System;
using FlatBuffers;

namespace Data
{
    public class GameSystemData : Data
    {
        public int unscaleDeltaTime;
        public int unscaleTime;
        public int clientFrame;

        public override void Clear()
        {
            unscaleDeltaTime = 0;
            unscaleTime = 0;
            clientFrame = 0;
        }
    }

    public class GameNetworkData : Data
    {
        public FlatBufferBuilder builder = new FlatBufferBuilder(Constant.NETWORK_CACHE_SIZE);
        public GameCoeNotification notification = new GameCoeNotification();
        public int lastHeartBeatTime;
        public int serverTime;

        public override void Clear()
        {
            builder = null;
            notification = null;
            lastHeartBeatTime = 0;
            serverTime = 0;
        }
    }

    public class GameLocalizationData : Data
    {
        public string zone;
        public LocalizationConfig currentConfig;

        public override void Clear()
        {
            zone = string.Empty;
            currentConfig = null;
        }
    }
}