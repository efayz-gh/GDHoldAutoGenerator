namespace GDReplayToHold;

public class GDObject
{
    private Dictionary<int, string?> Params { get; } = new();

    public GDObject(string objString)
    {
        var objParams = objString.Split(',');

        for (var i = 0; i < objParams.Length; i += 2)
            Params[int.Parse(objParams[i])] = objParams[i + 1];
    }

    public GDObject()
    {
    }

    public override string ToString() => Params
        .Select(kv => kv.Key + "," + kv.Value)
        .Aggregate((kv1, kv2) => kv1 + "," + kv2);

    public string? this[int key]
    {
        get => Params.GetValueOrDefault(key);
        set => Params[key] = value;
    }
}