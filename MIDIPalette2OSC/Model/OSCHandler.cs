using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Rug.Osc;

namespace MIDIPalette2OSC.Model
{
    class OscHandler
    {
        private readonly OscSender sender;
        

        public OscHandler(string OscAddr)
        {
            //TODO: Use TryParse instead of parse
            var addr = IPAddress.Parse(OscAddr.Split(':')[0]);
            var port = int.Parse(OscAddr.Split(':')[1]);
            sender = new OscSender(addr, port);

            //TODO: Try catch for network exception
            sender.Connect();
         
        }

        public void Send(string path, List<MidiMapping> data)
        {
            var msg = "";
            foreach (var midiMapping in data)
            {
                msg += midiMapping.ToString() + ";";
            }
            sender.Send(new OscMessage(path, msg.Substring(0,msg.Length-1)));
        }

    }
}
