using System.Text;
using System.Text.Json;
using static GDReplayToHold.GDObject;

namespace GDReplayToHold;

public static class Program
{
    public static void Main()
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + "../../../../";

        Console.Write("Encode [e] or Decode [d] or Convert [c]?: ");
        var mode = Console.ReadLine();

        switch (mode)
        {
            case "e":
            {
                Console.Write("Enter .dec file name: ");
                var file = Console.ReadLine()!;

                File.ReadAllText(Path.Combine(path, file)).EncodeLvlStr()
                    .WriteEncodedString(Path.Combine(path, file[..^3]));

                break;
            }
            case "d":
            {
                Console.Write("Enter .gmd file name: ");
                var file = Console.ReadLine()!;
                
                WriteDecryptedFile(Path.Combine(path, file), Path.Combine(path, file).GetEncodedLevelString().DecodeLvlStr());

                break;
            }
            case "c":
            {
                Console.Write("Enter .gmd file name: ");
                var gmdFile = Path.Combine(path, Console.ReadLine()!);

                Console.Write("Enter .gdr.json file name: ");
                var replayFile = Path.Combine(path, Console.ReadLine()!);

                var levelString = gmdFile.GetEncodedLevelString().DecodeLvlStr();

                var replay = JsonSerializer.Deserialize<GDReplay>(
                    File.ReadAllText(replayFile)
                )!;

                StringBuilder sb = new();
                foreach (var obj in replay.ToObjects())
                    sb.Append(obj + ";");

                // add new objects to level string
                levelString += sb.ToString();

                levelString.EncodeLvlStr().WriteEncodedString(gmdFile);

                break;
            }
            default:
                Console.WriteLine("Invalid mode.");
                break;
        }
    }

    private static void WriteDecryptedFile(string file, string levelString) =>
        File.WriteAllText(file + ".dec", levelString);
}