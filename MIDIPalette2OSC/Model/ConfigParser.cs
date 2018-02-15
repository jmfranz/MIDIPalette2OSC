using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Sanford.Multimedia;

namespace MIDIPalette2OSC.Model
{
    class ConfigParser
    {
        private JObject json;

        public ConfigParser()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Palette Files (*.plp)|*.plp";

            if (openFileDialog.ShowDialog() == true)
            {
                json = JObject.Parse(File.ReadAllText(openFileDialog.FileName));
            }
        }

        public Dictionary<Tuple<int, int>, MidiMapping> ParseMidiChannels()
        {
            var dict = new Dictionary<Tuple<int, int>, MidiMapping>();
            var mappings = json["module_mappings"]["midi_map"];

            foreach (var map in mappings)
            {
                var name = (string)map.GetType().GetProperty("Name")?.GetValue(map);
                if (name != "channel" && name != null)
                {
                    var cc = map.First["cc"].Value<int>();
                    var n = map.First["n"].Value<int>();

                    var input = new MidiMapping(name, cc, n);
                    var key = new Tuple<int, int>(1, cc);
                    dict.Add(key, input);
                }
            }

            return dict;
        }

    }
}
