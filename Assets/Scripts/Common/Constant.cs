using System;

public partial class Constant
{
    public const int NETWORK_CACHE_SIZE = 65535;

    public const int NOTIFICATION_TYPE_NETWORK = 1;
    public const int NOTIFICATION_TYPE_RESOURCE_PRELOAD = 2;
    public const int NOTIFICATION_TYPE_UI = 3;

    public const int NOTIFICATION_TYPE_ITEM     = 1000;
    public const int NOTIFICATION_TYPE_PLAYER   = 1001;
    public const int NOTIFICATION_TYPE_GAME_CORE = 1002;
    public const int NOTIFICATION_TYPE_RESOURCE_LOADER = 1003;

    public const int SECOND_TO_MILLISECOND = 1000;
    public const int JOYSTICK_DELAY_FRAME_COUNT = 5;

    public const int CAMERA_FOLLOW_INTERVAL = 800;

    public const int UNITY_UNIT_TO_GAME_UNIT = 1000;

    public const int SCREEN_WIDTH = 960;
    public const int SCREEN_HEIGHT = 640;

    public const float MAX_FOLLOW_REGION = 0.6f;

    public const int ACTOR_FLY_SHAKE_DURATION = 5; // 1 / 5 = 0.2s
    public const int ACTOR_FLY_SHAKE_Y = 2;
    public const int ACTOR_FLY_SPEED_X = 3;

    public const int ACTOR_DASH_SPEED_X = 40;
    public const int ACTOR_STRESS_RATE_Y = 3;

    public const int ACTOR_COLLIDER_MAGIC_Y = 20;

    public const string LAYER_GROUND_NAME = "Ground";

    public const float PERCENT = 0.0001f;
}
