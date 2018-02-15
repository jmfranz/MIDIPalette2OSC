using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Input;
using MIDIPalette2OSC.Annotations;
using MIDIPalette2OSC.Model;
using MIDIPalette2OSC.Utilities;




namespace MIDIPalette2OSC.ViewModel
{
    class MainViewModel: INotifyPropertyChanged
    {
        //TODO: Change to C# 7.x and use named tuples
        //TODO: Implement multithread semaphores
        public Dictionary<Tuple<int , int>, MidiMapping> MidiMappings { get; private set; }
        public string OscAddr { get; set; } = "127.0.0.1:5005";
        public int OscInterval { get; set; } = 200;
        public string OscPath { get; set; } = "/MIDI";

        private ICommand loadCommand;
        public ICommand LoadCommand
        {
            get { return loadCommand ?? (loadCommand = new RelayCommand(call => LoadFile())); }
        }

        private ICommand startMidiCommand;
        public ICommand StartMidiCommand
        {
            get { return startMidiCommand ?? (startMidiCommand = new RelayCommand(call => StartMidi())); }
        }

        private ICommand startOsCommand;
        public ICommand StartOscCommand
        {
            get { return startOsCommand ?? (startOsCommand = new RelayCommand(call => StartOsc())); }
        }
        
        private MidiConnector midiConnector;
        private OscHandler oscHandler;
        private Timer oscTimer;

        public MainViewModel()
        {
            MidiMappings = new Dictionary<Tuple<int, int>, MidiMapping>();
        }

        private void LoadFile()
        {
            ConfigParser parser = new ConfigParser();
            MidiMappings = parser.ParseMidiChannels();
            OnPropertyChanged(nameof(MidiMappings));
        }

        private void StartMidi()
        {
            if (midiConnector != null) return;
            midiConnector = new MidiConnector();
            midiConnector.StartReceiving();
                
            midiConnector.MessageReadyEvent
                += (channel, cc, value) => { MidiMappings[new Tuple<int, int>(channel, cc)].Amout = value; };
            
            // OnPropertyChanged("MidiMappings[0]"); };
        }

        private void StartOsc()
        {
            if(oscHandler != null) return;
            oscHandler = new OscHandler(OscAddr);
            oscTimer = new Timer(OscInterval) {AutoReset = true};
            oscTimer.Elapsed +=
                (sender, args) => oscHandler.Send(OscPath, MidiMappings.Select(entries => entries.Value).ToList());
            oscTimer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
