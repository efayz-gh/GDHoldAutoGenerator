namespace GDReplayToHold;

public class GDObject
{
    public Dictionary<int, string?> Params { get; } = new();

    public GDObject(string objString)
    {
        var objParams = objString.Split(',');

        for (var i = 0; i < objParams.Length; i += 2)
            Params[int.Parse(objParams[i])] = objParams[i + 1];
    }

    public GDObject()
    {
    }

    public static IEnumerable<GDObject> GetObjects(string levelString) => levelString
        .Split(';')
        .Skip(1).SkipLast(1) // skip first element (level info) and last element (empty)
        .Select(objString => new GDObject(objString));

    public override string ToString() => Params
        .Select(kv => kv.Key + "," + kv.Value)
        .Aggregate((kv1, kv2) => kv1 + "," + kv2);

    public string? this[Param key]
    {
        get => Params.GetValueOrDefault((int)key);
        set => Params[(int)key] = value;
    }

    public string? this[int key]
    {
        get => Params.GetValueOrDefault(key);
        set => Params[key] = value;
    }

    // param properties (reverse engineer more properties...)
    public enum Param
    {
        ObjectId = 1,
        X = 2,
        Y = 3,
        MirrorVertical = 4,
        MirrorHorizontal = 5,
        Rotation = 6, // clockwise (+90 = quarter turn clockwise)

        // todo: more properties
        // ...
        MoveTime = 10,
        TouchTriggered = 11,
        EditorLayer = 20,
        IsTrigger = 36,
        Groups = 57,
        GroupParent = 58,
        EditorLayer2 = 61,
        SpawnTriggered = 62,
        DontFade = 64,
        DontEnter = 67,
        IdkButImportant = 155, // every object has this
        OptionsTriggerStreakAdditive = 159,
        OptionsTriggerUnlinkDualGravity = 160,
        OptionsTriggerHideGround = 161,
        OptionsTriggerHideP1 = 162,
        OptionsTriggerHideP2 = 163,
        OptionsTriggerDisableP1Controls = 165,
        OptionsTriggerHideMG = 195,
        OptionsTriggerDisableP2Controls = 199,
        OptionsTriggerHideAttempts = 532,
        OptionsTriggerEditRespawnTime = 573,
        OptionsTriggerRespawnTime = 574,
        OptionsTriggerAudioOnDeath = 575,
        OptionsTriggerNoDeathSFX = 576,
        OptionsTriggerBoostSlide = 593,
    }

    /*public enum ObjectIds
    {
        Block = 1,
        Spike = 8,
        MoveTrigger = 901,
        OptionsTrigger = 2899,
        // add more...
    }*/
    public static class ObjectIds
    {
        public const string Block = "1";
        public const string Spike = "8";
        public const string MoveTrigger = "901";

        public const string OptionsTrigger = "2899";
        // add more...
    }

    public int ObjectId => int.Parse(this[Param.ObjectId]!);
    public double X => double.Parse(this[Param.X]!);
    public double Y => double.Parse(this[Param.Y]!);

    public int[] Groups => this[Param.Groups]?.Split('.').Select(int.Parse).ToArray() ?? Array.Empty<int>();
    // possibly add more properties
}