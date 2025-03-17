namespace GDHoldAutoGenerator;

public class GDObject
{
    private Dictionary<string, string?> Params { get; } = new();

    public GDObject(string objString)
    {
        var objParams = objString.Split(',');

        for (var i = 0; i < objParams.Length; i += 2)
            Params[objParams[i]] = objParams[i + 1];
    }

    public GDObject()
    {
    }

    public override string ToString() => Params
        .Select(kv => kv.Key + "," + kv.Value)
        .Aggregate((kv1, kv2) => kv1 + "," + kv2);

    public string? this[string key]
    {
        get => Params.GetValueOrDefault(key);
        set => Params[key] = value;
    }
}