namespace GDReplayToHold;

public static class Param
{
    public const int ObjectId = 1;
    public const int X = 2;
    public const int Y = 3;
    public const int MirrorVertical = 4;
    public const int MirrorHorizontal = 5;
    public const int Rotation = 6; // clockwise (+90 = quarter turn clockwise)
    
    // todo: more properties
    // ...
    public const int MoveTime = 10;
    public const int TouchTriggered = 11;
    public const int EditorLayer = 20;
    public const int IsTrigger = 36;
    public const int Groups = 57;
    public const int GroupParent = 58;
    public const int EditorLayer2 = 61;
    public const int SpawnTriggered = 62;
    public const int DontFade = 64;
    public const int DontEnter = 67;
    public const int IdkButImportant = 155; // every object has this
    public const int OptionsTriggerStreakAdditive = 159;
    public const int OptionsTriggerUnlinkDualGravity = 160;
    public const int OptionsTriggerHideGround = 161;
    public const int OptionsTriggerHideP1 = 162;
    public const int OptionsTriggerHideP2 = 163;
    public const int OptionsTriggerDisableP1Controls = 165;
    public const int OptionsTriggerHideMG = 195;
    public const int OptionsTriggerDisableP2Controls = 199;
    public const int SfxTriggerSoundId = 392;
    public const int SfxTriggerVolume = 406;
    public const int SfxTriggerStart = 408;
    public const int SfxTriggerEnd = 410;
    public const int OptionsTriggerHideAttempts = 532;
    public const int OptionsTriggerEditRespawnTime = 573;
    public const int OptionsTriggerRespawnTime = 574;
    public const int OptionsTriggerAudioOnDeath = 575;
    public const int OptionsTriggerNoDeathSFX = 576;
    public const int OptionsTriggerBoostSlide = 593;
}

public static class ObjectIds
{
    public const string Block = "1";
    public const string Spike = "8";
    public const string MoveTrigger = "901";
    public const string OptionsTrigger = "2899";
    public const string SfxTrigger = "3602";
    // add more...
}