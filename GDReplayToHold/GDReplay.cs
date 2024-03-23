using System.Globalization;
using System.Text.Json.Serialization;
using static GDReplayToHold.GDObject;

namespace GDReplayToHold;

public class GDReplay
{
    [JsonPropertyName("author")] public string Author { get; set; }
    [JsonPropertyName("bot")] public Bot Bot { get; set; }
    [JsonPropertyName("coins")] public long Coins { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; }
    [JsonPropertyName("duration")] public long Duration { get; set; }
    [JsonPropertyName("gameVersion")] public double GameVersion { get; set; }
    [JsonPropertyName("inputs")] public List<Input> Inputs { get; set; }
    [JsonPropertyName("ldm")] public bool Ldm { get; set; }
    [JsonPropertyName("level")] public Level Level { get; set; }
    [JsonPropertyName("seed")] public long Seed { get; set; }
    [JsonPropertyName("version")] public double Version { get; set; }
    
    public bool TwoPlayer => Inputs.Any(input => input.TwoPlayer);

    public IEnumerable<GDObject> ToObjects()
    {
        var objects = new List<GDObject>
        {
            new() // set initial options trigger to disable all controls
            {
                [Param.ObjectId] = ObjectIds.OptionsTrigger,
                [Param.X] = "-15",
                [Param.Y] = "-15",
                [Param.IdkButImportant] = "1",
                [Param.IsTrigger] = "1",
                [Param.OptionsTriggerDisableP1Controls] = "1",
                [Param.OptionsTriggerDisableP2Controls] = "1"
            }
        };

        // enable/disable controls according to replay inputs
        objects.AddRange(Inputs.Select(input =>
        {
            var obj = new GDObject
            {
                [Param.ObjectId] = ObjectIds.OptionsTrigger,
                [Param.X] = input.MhrX.ToString(CultureInfo.InvariantCulture),
                [Param.Y] = (input.MhrY - 90.0).ToString("F3", CultureInfo.InvariantCulture), // y position seems to be 4 blocks off
                [Param.IdkButImportant] = "1",
                [Param.IsTrigger] = "1"
            };
            
            // Unfortunately, the options trigger is bugged in dual mode
            // but I'll keep this code in case it gets fixed
            
            if (TwoPlayer) // two player level - separate controls
            {
                if (input.TwoPlayer)
                    obj[Param.OptionsTriggerDisableP2Controls] = input.Down ? "-1" : "1";
                else
                    obj[Param.OptionsTriggerDisableP1Controls] = input.Down ? "-1" : "1";
            }
            else // not two player - operate both controls
            {
                obj[Param.OptionsTriggerDisableP1Controls] = input.Down ? "-1" : "1";
                obj[Param.OptionsTriggerDisableP2Controls] = input.Down ? "-1" : "1";
            }
            
            // mirror trigger on release for visual clarity
            if (input.Down)
                obj[Param.MirrorVertical] = "1";
            
            return obj;
        }));
        
        return objects;
    }
}

public class Bot
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("version")] public string Version { get; set; }
}

public class Input
{
    [JsonPropertyName("2p")] public bool TwoPlayer { get; set; }
    [JsonPropertyName("btn")] public long Btn { get; set; }
    [JsonPropertyName("down")] public bool Down { get; set; }
    [JsonPropertyName("frame")] public long Frame { get; set; }
    [JsonPropertyName("mhr_meta")] public bool MhrMeta { get; set; }
    [JsonPropertyName("mhr_x")] public double MhrX { get; set; }
    [JsonPropertyName("mhr_y")] public double MhrY { get; set; }
    [JsonPropertyName("mhr_yvel")] public double MhrYvel { get; set; }
}

public class Level
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
}