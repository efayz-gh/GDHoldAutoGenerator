using System.IO.Compression;
using System.Text;
using System.Text.Unicode;
using System.Xml;

namespace GDReplayToHold;

public class LevelString
{
    public string LvlString { get; set; }

    public string EncodedLvlString
    {
        get
        {
            using MemoryStream ms = new();
            using GZipStream gzip = new(ms, CompressionMode.Compress, true);

            var bytes = Encoding.UTF8.GetBytes(LvlString);

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
        XmlDocument gmd = new();
        gmd.LoadXml(File.ReadAllText(gmdFile));

        // get level string from plist (key: k4)
        EncodedLvlString = gmd.SelectSingleNode("/plist/dict/k[text()='k4']/following-sibling::*[1]")?.InnerText.Trim()
            ?? throw new Exception("Level string not found.");
    }

    public void WriteToGmdFile(string gmdFile)
    {
        XmlDocument gmd = new();
        gmd.LoadXml(File.ReadAllText(gmdFile));

        // create k4 key if it doesn't exist
        if (gmd.SelectSingleNode("/plist/dict/k[text()='k4']") == null)
        {
            var k4 = gmd.CreateElement("k");
            k4.InnerText = "k4";

            var v4 = gmd.CreateElement("s");
            v4.InnerText = "";

            gmd.SelectSingleNode("/plist/dict")!.AppendChild(k4);
            gmd.SelectSingleNode("/plist/dict")!.AppendChild(v4);
        }

        gmd.SelectSingleNode("/plist/dict/k[text()='k4']/following-sibling::*[1]")!.InnerText = EncodedLvlString;

        gmd.Save(gmdFile);
    }
    
    public IEnumerable<GDObject> GetObjects() => LvlString
        .Split(';')
        .Skip(1).SkipLast(1) // skip first element (level info) and last element (empty)
        .Select(objString => new GDObject(objString));
    
    public void AppendObjects(IEnumerable<GDObject> objects)
    {
        var sb = new StringBuilder();

        foreach (var obj in objects)
            sb.Append(obj + ";");

        LvlString += sb.ToString();
    }
}