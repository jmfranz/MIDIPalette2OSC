using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDIPalette2OSC.Model
{
    class MidiMapping
    {
        public string DeviceName { get; }
        public int ContinuousControlNumber { get; }
        public int Note { get; }
        
        public  int Amout { get; set; }

        public MidiMapping(string name, int ccNumber, int note)
        {
            DeviceName = name;
            ContinuousControlNumber = ccNumber;
            Note = note;
            Amout = 40;
        }

        public override string ToString()
        {
            return $"[{ContinuousControlNumber},{Amout}]";
        }
    }
}
