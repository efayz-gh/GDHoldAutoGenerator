﻿using System.Text.Json;

namespace GDHoldAutoGenerator;

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
                var file = Path.Combine(path, Console.ReadLine()!);

                LevelString levelString = new()
                {
                    LvlString = File.ReadAllText(file)
                };

                levelString.WriteToGmdFile(file[..^3]);

                break;
            }
            case "d":
            {
                Console.Write("Enter .gmd file name: ");
                var file = Path.Combine(path, Console.ReadLine()!);
                
                LevelString levelString = new();
                levelString.LoadFromGmdFile(file);
                
                // add line breaks between objects
                levelString.LvlString = levelString.LvlString!.Replace(";1", ";\n1");
                
                WriteDecryptedFile(file, levelString.LvlString!);

                break;
            }
            case "c":
            {
                Console.Write("Enter .gmd file name: ");
                var gmdFile = Path.Combine(path, Console.ReadLine()!);

                Console.Write("Enter .gdr.json file name: ");
                var replayFile = Path.Combine(path, Console.ReadLine()!);

                LevelString levelString = new();
                levelString.LoadFromGmdFile(gmdFile);

                var replay = JsonSerializer.Deserialize<MHReplay>(
                    File.ReadAllText(replayFile)
                )!;

                levelString.AppendObjects(replay.ToObjects(true));

                levelString.WriteToGmdFile(gmdFile);

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