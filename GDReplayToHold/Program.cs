namespace GDReplayToHold;

public static class Program
{
    public static void Main()
    {
        string path = AppDomain.CurrentDomain.BaseDirectory;

        Console.Write("Encode [e] or Decode [d]?: ");
        var mode = Console.ReadLine();

        switch (mode)
        {
            case "e":
            {
                Console.Write("Enter .dec file name: ");
                var file = Console.ReadLine()!;

                File.ReadAllText(Path.Combine(path, file)).EncodeLvlStr().WriteEncodedString(Path.Combine(path, file[..^3]));
                
                break;
            }
            case "d":
            {
                Console.Write("Enter .gmd file name: ");
                var file = Console.ReadLine()!;
                
                File.WriteAllText(Path.Combine(path, file + ".dec"), 
                    Path.Combine(path, file).GetEncodedLevelString().DecodeLvlStr());
                
                break;
            }
            default:
                Console.WriteLine("Invalid mode.");
                break;
        }
    }
}