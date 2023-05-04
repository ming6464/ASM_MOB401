using System;

public static class TagConst{
    
    public enum Skill
    {
        F,G
    }
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
    public const string ParamImmortal = "isImmortal";
    public const string ParamHit = "isHit";
    public const string DEATHZONE = "DeathZone";
    public const string FINISH = "Finish";
    public const string KEY = "Key";
    public const string AUDIOMANAGER = "AudioManager";
    public const string AFTERIMAGEPOOL = "AfterImagePool";
    public const string HEALTHBAR = "HealthBarPlayer";
    public const string MENUDIALOG = "MenuDialog";
    public const string MAINCAMERA = "MainCamera";

    public const string A_JUMP = "Jump",
        A_Run = "Run",
        A_Fall = "Fall",
        A_IDLE = "Idle",
        A_WALK = "Walk",
        A_SPRINGY = "Springy",
        A_ATTACK_1 = "Attack_1",
        A_ATTACK_2 = "Attack_2",
        A_ATTACK_3 = "Attack_3",
        A_SKILLF = "SkillF",
        A_SKILLG = "SkillG";

    public const string AUDIO_JUMP1 = "Jump1",
        AUDIO_KILL = "Kill",
        MUSIC = "Music",
        AUDIO_PICKUP = "PickUp",
        AUDIO_WIN = "Win",
        AUDIO_HIT = "Hit",
        AUDIO_DEATH = "Death",
        AUDIO_ATTACK = "Attack",
        AUDIO_SKILLG = "SkillG",
        AUDIO_SKILLF = "SkillF";

    public const string URL_MATERIALS = "Materials/",URL_PREFABS = "Prefabs/";
}