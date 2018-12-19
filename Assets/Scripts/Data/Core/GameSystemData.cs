using System;
using FlatBuffers;

namespace Data
{
    public class GameSystemData : Data
    {
        public int unscaleDeltaTime;
        public int unscaleTime;
        public int clientFrame;
    }

    public class GameNetworkData : Data
    {
        public FlatBufferBuilder builder = new FlatBufferBuilder(Constant.NETWORK_CACHE_SIZE);
        public GameCoeNotification notification = new GameCoeNotification();
        public int lastHeartBeatTime;
        public int serverTime;
    }

    public class GameLocalizationData : Data
    {
        public string zone;
        public LocalizationConfig currentConfig;
    }
}