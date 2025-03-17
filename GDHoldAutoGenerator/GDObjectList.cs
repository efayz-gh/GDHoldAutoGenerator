namespace GDHoldAutoGenerator;

public class GDObjectList : List<GDObject>
{
    public GDObjectList()
    {
    }

    public GDObjectList(IEnumerable<GDObject> collection) : base(collection)
    {
    }

    public string GetNextFreeEditorLayer()
    {
        var layers = this
            .Where(o => o[Param.EditorLayer] != null)
            .Select(o => int.Parse(o[Param.EditorLayer]!))
            .Distinct()
            .Order()
            .ToList();

        for (var i = 1; i < layers.Count; i++)
        {
            if (layers[i] != layers[i - 1] + 1)
                return layers[i - 1] + 1 + "";
        }

        return layers[^1] + 1 + "";
    }

    public int GetNextFreeLinkId() => this
        .Where(o => o[Param.LinkId] != null)
        .Select(o => int.Parse(o[Param.LinkId]!))
        .Max() + 1;
}