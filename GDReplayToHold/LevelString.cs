using System.IO.Compression;
using System.Text;
using System.Text.Unicode;
using System.Xml;

namespace GDReplayToHold;

public static class LevelString
{
    public static string DecodeLvlStr(this string encodedLevelString)
    {
        var lvlStringB64 = Convert.FromBase64String(
            encodedLevelString
                .Replace('-', '+')
                .Replace('_', '/')
        );

        using var gzip = new GZipStream(new MemoryStream(lvlStringB64), CompressionMode.Decompress);

        var buf = new byte[4096];
        int len;

        var sb = new StringBuilder();

        while ((len = gzip.Read(buf, 0, buf.Length)) > 0)
            sb.Append(Encoding.UTF8.GetString(buf, 0, len));

        return sb.ToString();
    }
    
    public static string EncodeLvlStr(this string levelString)
    {
        using MemoryStream ms = new();
        using GZipStream gzip = new(ms, CompressionMode.Compress, true);
        
        var bytes = Encoding.UTF8.GetBytes(levelString);
        
        gzip.Write(bytes, 0, bytes.Length);
        gzip.Close();
        
        var lvlStringB64 = Convert.ToBase64String(ms.ToArray());

        return lvlStringB64
            .Replace('+', '-')
            .Replace('/', '_');
    }

    public static string GetEncodedLevelString(this string gmdFile)
    {
        if (!File.Exists(gmdFile))
            throw new FileNotFoundException("File not found.", gmdFile);

        // load .gmd file
        XmlDocument gmd = new();
        gmd.LoadXml(File.ReadAllText(gmdFile));

        // get level string from plist (key: k4)
        return gmd.SelectSingleNode("/plist/dict/k[text()='k4']/following-sibling::*[1]")?.InnerText.Trim()
                          ?? throw new Exception("Level string not found.");
    }
    
    public static void WriteEncodedString(this string encodedLevelString, string gmdFile)
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
        
        gmd.SelectSingleNode("/plist/dict/k[text()='k4']/following-sibling::*[1]")!.InnerText = encodedLevelString;
        
        gmd.Save(gmdFile);
    }
}