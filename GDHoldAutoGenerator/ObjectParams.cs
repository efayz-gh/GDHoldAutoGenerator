namespace GDHoldAutoGenerator;

public static class Param
{
    public const string ObjectId = "1";
    public const string X = "2";
    public const string Y = "3";
    public const string MirrorVertical = "4";
    public const string MirrorHorizontal = "5";
    public const string Rotation = "6"; // clockwise (+90 = quarter turn clockwise)
    
    // todo: more properties
    // ...
    public const string MoveTime = "10";
    public const string TouchTriggered = "11";
    public const string EditorLayer = "20";
    public const string IsTrigger = "36";
    public const string Groups = "57";
    public const string GroupParent = "58";
    public const string EditorLayer2 = "61";
    public const string SpawnTriggered = "62";
    public const string DontFade = "64";
    public const string DontEnter = "67";
    public const string LinkId = "108"; // id of object link
    public const string IdkButImportant = "155"; // every object has this
    public const string OptionsTriggerStreakAdditive = "159";
    public const string OptionsTriggerUnlinkDualGravity = "160";
    public const string OptionsTriggerHideGround = "161";
    public const string OptionsTriggerHideP1 = "162";
    public const string OptionsTriggerHideP2 = "163";
    public const string OptionsTriggerDisableP1Controls = "165";
    public const string OptionsTriggerHideMG = "195";
    public const string OptionsTriggerDisableP2Controls = "199";
    public const string SfxTriggerSoundId = "392";
    public const string SfxTriggerVolume = "406";
    public const string SfxTriggerStart = "408";
    public const string SfxTriggerEnd = "410";
    public const string OptionsTriggerHideAttempts = "532";
    public const string OptionsTriggerEditRespawnTime = "573";
    public const string OptionsTriggerRespawnTime = "574";
    public const string OptionsTriggerAudioOnDeath = "575";
    public const string OptionsTriggerNoDeathSFX = "576";
    public const string OptionsTriggerBoostSlide = "593";
    public const string StartPosGameMode = "kA2"; // 0 = cube, 1 = ship, 2 = ball, 3 = ufo, 4 = wave, 5 = robot, 6 = spider, 7 = swing
    public const string StartPosDisabled = "kA28"; // 0 = true (disabled), 1 = false (enabled)
    // unfortunately, the parameters of the start position object use strings instead of numbers
}

public static class ObjectIds
{
    public const string Block = "1";
    public const string Spike = "8";
    public const string StartPos = "31";
    public const string MoveTrigger = "901";
    public const string OptionsTrigger = "2899";
    public const string SfxTrigger = "3602";
    // add more...
}