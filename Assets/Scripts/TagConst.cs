using System;

public static class TagConst{
    public struct NameEnemy
    {
        public const string BOAR = "boar", SNAIL = "snail",BEE = "bee";
    }
    
    public struct NameInfo
    {
        public const string SCORE = "score";
    }

    public const string GROUND = "Ground";
    public const string PLAYER = "Player";
    public const string CAM = "MainCamera";
    public const string ENEMY = "Enemy";
    public const string ParamDeath = "isDeath";
    public const string DEATHZONE = "DeathZone";
    public const string FINISH = "Finish";
    public const string KEY = "Key";

    public const string A_JUMP = "Jump",
        A_Run = "Run",
        A_Fall = "Fall",
        A_Landing = "Landing",
        A_IDLE = "Idle",
        A_WALK = "Walk",
        A_SPRINGY = "Springy";

    public const string AUDIO_JUMP1 = "Jump1",
        AUDIO_JUMP2 = "Jump2",
        AUDIO_KILL = "Kill",
        MUSIC = "Music",
        AUDIO_PICKUP = "PickUp",
        AUDIO_WIN = "Win",AUDIO_HIT = "Hit";
}