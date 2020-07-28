using System;
using System.Threading;
using OpenWiz;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WizLightUniversal.Core.Models
{
    public class WizLightModel : INotifyPropertyChanged
    {
        public string IP { get; set; }
        public string MAC { get; set; }
        public int MinimumTemperature { get; set; }
        public int MaximumTemperature { get; set; }

        private int _red;
        private int _green;
        private int _blue;
        private int _warm;
        private int _cool;
        private int _temp;
        private int _dimming;
        private bool _power;

        public int Red
        {
            get { return _red; }
            set { if (_red != value) { _red = value; SendUpdate(false); } else _red = value; }
        }
        public int Green
        {
            get { return _green; }
            set { if (_green != value) { _green = value; SendUpdate(false); } else _green = value; }
        }
        public int Blue
        {
            get { return _blue; }
            set { if (_blue != value) { _blue = value; SendUpdate(false); } else _blue = value; }
        }
        public int WarmWhite
        {
            get { return _warm; }
            set { if (_warm != value) { _warm = value; SendUpdate(false); } else _warm = value; }
        }
        public int CoolWhite
        {
            get { return _cool; }
            set { if (_cool != value) { _cool = value; SendUpdate(false); } else _cool = value; }
        }

        public int Temperature
        {
            get { return _temp; }
            set { if (_temp != value) { _temp = value; SendUpdate(true); } else _temp = value; }
        }

        public int Brightness
        {
            get { return _dimming; }
            set { if (_dimming != value) { _dimming = value; SendUpdate(false); } else _dimming = value; }
        }
        public bool Power
        {
            get { return _power; }
            set { if (_power != value) { _power = value; SendUpdate(false); } else _power = value; }
        }

        private readonly WizHandle Handle;
        private readonly WizSocket Socket;
        public event PropertyChangedEventHandler PropertyChanged;

        public volatile bool ShouldPoll;
        private volatile bool UpdateInProgress;
        private readonly Timer PollTimer;

        public WizLightModel(WizHandle handle)
        {
            // Initial values for these fields
            IP = handle.Ip.ToString();
            MAC = handle.Mac;

            // set defaults
            MaximumTemperature = 1;
            MinimumTemperature = 0;

            _temp = 0;
            _red = 0;
            _green = 0;
            _blue = 0;
            _warm = 0;
            _cool = 0;

            _dimming = 1;
            _power = false;

            // enables us to receive updates
            Handle = handle;
            Socket = new WizSocket();
            Socket.Bind();
            Socket.BeginRecieveFrom(Handle, WhenGetState, null);

            // Send config request
            Socket.SendTo(WizState.MakeGetUserConfig(), Handle);

            // Start the timer for polling
            UpdateInProgress = false;
            PollTimer = new Timer(PollCallback, this, 1000, 2000);
        }

        // Called to ping the light
        private void PollCallback(object state)
        {
            try
            {
                if (ShouldPoll && !UpdateInProgress) Socket.SendTo(WizState.MakeGetPilot(), Handle);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Model for {Handle.Ip}: Failed to issue poll -- {e.Message}");
            }
        }

        // Called when a light needs to be updated
        private void SendUpdate(bool usingTemp)
        {
            UpdateInProgress = true;
            WizState wizState = new WizState()
            { Method = WizMethod.setPilot, Params = new WizParams() { State = _power } };

            if (_power)
            {
                if (usingTemp)
                {
                    wizState.Params.Temp = _temp;
                }
                else
                {
                    wizState.Params.R = _red;
                    wizState.Params.G = _green;
                    wizState.Params.B = _blue;
                    wizState.Params.W = _warm;
                    wizState.Params.C = _cool;
                }
                wizState.Params.Dimming = _dimming;
            }

            Console.WriteLine($"Model for {Handle.Ip}: Sending update -- {wizState}");
            Socket.SendTo(wizState, Handle);
            UpdateInProgress = false;
        }

        // Called when data is recieved
        private void WhenGetState(IAsyncResult asyncResult)
        {
            WizState state = Socket.EndReceiveFrom(Handle, asyncResult);
            if (state == null) return;
            Console.WriteLine($"Model for {Handle.Ip}: Got state -- {state}");

            HandleGetError(state.Error);
            switch (state.Method)
            {
                case WizMethod.getPilot:
                case WizMethod.setPilot:
                    HandleGetPilot(state.Result);
                    break;
                case WizMethod.syncPilot:
                    HandleGetPilot(state.Params);
                    break;
                case WizMethod.getUserConfig:
                    HandleGetUserConfig(state.Result);
                    break;
            }
            Socket.BeginRecieveFrom(Handle, WhenGetState, null);
        }

        // Handles the user configuration
        private void HandleGetUserConfig(WizResult config)
        {
            if (config == null) return;
            MaximumTemperature = config.ExtRange[1];
            MinimumTemperature = config.ExtRange[0];
        }

        private void HandleGetPilot(WizParams pilot)
        {
            if (pilot == null) return;

            // update power and brightness
            _power = pilot.State ?? Power;
            PropertyChanged(this, new PropertyChangedEventArgs("Power"));
            _dimming = pilot.Dimming ?? Brightness;
            PropertyChanged(this, new PropertyChangedEventArgs("Brightness"));

            // update temp
            if (pilot.Temp != null)
            {
                _temp = pilot.Temp.Value;
                PropertyChanged(this, new PropertyChangedEventArgs("Temperature"));
            }

            // update colors
            else
            {
                _red = pilot.R ?? _red;
                PropertyChanged(this, new PropertyChangedEventArgs("Red"));
                _green = pilot.G ?? _green;
                PropertyChanged(this, new PropertyChangedEventArgs("Green"));
                _blue = pilot.B ?? _blue;
                PropertyChanged(this, new PropertyChangedEventArgs("Blue"));

                _warm = pilot.W ?? _warm;
                PropertyChanged(this, new PropertyChangedEventArgs("WarmWhite"));
                _cool = pilot.C ?? _cool;
                PropertyChanged(this, new PropertyChangedEventArgs("CoolWhite"));
            }
        }

        private void HandleGetError(WizError error)
        {
            if (error == null) return;
            Console.WriteLine($"Model for {Handle.Ip}: Got error: {error.Message}");
        }
    }
}
