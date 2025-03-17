using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GDHoldAutoGenerator
{
    public abstract class GDReplay
    {
        public abstract GDObjectList ToObjects(bool clickSounds = false, string editorLayer = "", int linkId = 1);

        public static GDReplay? Deserialize(string json)
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            var botName = doc.RootElement.GetProperty("bot").GetProperty("name").GetString();

            return botName switch
            {
                "MH_REPLAY" => JsonSerializer.Deserialize<MHReplay>(json),
                _ => null
            };
        }
    }
}
