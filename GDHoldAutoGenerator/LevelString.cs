using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace GDHoldAutoGenerator;

public class LevelString
{
    public string? LvlString { get; set; }

    public string EncodedLvlString
    {
        get
        {
            using MemoryStream ms = new();
            using GZipStream gzip = new(ms, CompressionMode.Compress, true);

            var bytes = Encoding.UTF8.GetBytes(LvlString ?? throw new InvalidOperationException());

            gzip.Write(bytes, 0, bytes.Length);
            gzip.Close();

            var lvlStringB64 = Convert.ToBase64String(ms.ToArray());

            return lvlStringB64
                .Replace('+', '-')
                .Replace('/', '_');
        }
        set
        {
            var lvlStringB64 = Convert.FromBase64String(
                value.Replace('-', '+').Replace('_', '/')
            );

            using var gzip = new GZipStream(new MemoryStream(lvlStringB64), CompressionMode.Decompress);

            var buf = new byte[4096];
            int len;

            var sb = new StringBuilder();

            while ((len = gzip.Read(buf, 0, buf.Length)) > 0)
                sb.Append(Encoding.UTF8.GetString(buf, 0, len));

            LvlString = sb.ToString();
        }
    }

    public void LoadFromGmdFile(string gmdFile)
    {
        if (!File.Exists(gmdFile))
            throw new FileNotFoundException("File not found.", gmdFile);

        // load .gmd file
        XDocument gmd = XDocument.Load(gmdFile);

        // get level string from plist (key: k4)
        EncodedLvlString = gmd.Root?.Element("dict")?
                               .Elements("k")
                               .FirstOrDefault(k => k.Value == "k4")?
                               .ElementsAfterSelf()
                               .FirstOrDefault()?
                               .Value.Trim() ??
                           throw new XmlException("Invalid .gmd file.");
    }

    public void WriteToGmdFile(string gmdFile)
    {
        XDocument gmd = XDocument.Load(gmdFile);

        gmd.Descendants("k")
            .First(k => k.Value == "k4")
            .ElementsAfterSelf()
            .First()
            .Value = EncodedLvlString;

        gmd.Save(gmdFile);
    }

    public GDObjectList GetObjects() => new(LvlString!
        .Split(';')
        .Skip(1).SkipLast(1) // skip first element (level info) and last element (empty)
        .Select(objString => new GDObject(objString)));

    public void AppendObjects(IEnumerable<GDObject> objects)
    {
        var sb = new StringBuilder();

        foreach (var obj in objects)
            sb.Append(obj + ";");

        LvlString += sb.ToString();
    }
}