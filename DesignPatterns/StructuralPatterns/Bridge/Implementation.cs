namespace Bridge
{
    /// <summary>
    /// Implementor interface
    /// </summary>
    public interface IDevice
    {
        bool IsEnabled { get; }
        void Enable();
        void Disable();
        int Volume { get; set; }
        int Channel { get; set; }
        void PrintStatus();
    }

    /// <summary>
    /// Concrete Implementor A
    /// </summary>
    public class Tv : IDevice
    {
        public bool IsEnabled { get; private set; }
        public int Volume { get; set; } = 30;
        public int Channel { get; set; } = 1;

        public void Enable() => IsEnabled = true;
        public void Disable() => IsEnabled = false;

        public void PrintStatus()
        {
            Console.WriteLine($"TV is {(IsEnabled ? "ON" : "OFF")} | Channel: {Channel} | Volume: {Volume}");
        }
    }

    /// <summary>
    /// Concrete Implementor B
    /// </summary>
    public class Radio : IDevice
    {
        public bool IsEnabled { get; private set; }
        public int Volume { get; set; } = 20;
        public int Channel { get; set; } = 101;

        public void Enable() => IsEnabled = true;
        public void Disable() => IsEnabled = false;

        public void PrintStatus()
        {
            Console.WriteLine($"Radio is {(IsEnabled ? "ON" : "OFF")} | Station: {Channel} FM | Volume: {Volume}");
        }
    }

    /// <summary>
    /// Abstraction
    /// </summary>
    public class RemoteControl
    {
        protected readonly IDevice _device;

        public RemoteControl(IDevice device)
        {
            _device = device;
        }

        public void TogglePower()
        {
            if (_device.IsEnabled)
            {
                _device.Disable();
            }
            else
            {
                _device.Enable();
            }
        }

        public void VolumeUp()
        {
            _device.Volume += 10;
        }

        public void VolumeDown()
        {
            _device.Volume = Math.Max(0, _device.Volume - 10);
        }

        public void ChannelUp()
        {
            _device.Channel++;
        }

        public void ChannelDown()
        {
            _device.Channel = Math.Max(1, _device.Channel - 1);
        }
    }

    /// <summary>
    /// Refined Abstraction
    /// </summary>
    public class AdvancedRemoteControl : RemoteControl
    {
        private int _previousVolume;

        public AdvancedRemoteControl(IDevice device) : base(device) { }

        public void Mute()
        {
            if (_device.Volume > 0)
            {
                _previousVolume = _device.Volume;
                _device.Volume = 0;
                Console.WriteLine("Device muted.");
            }
            else
            {
                _device.Volume = _previousVolume;
                Console.WriteLine($"Device unmuted to volume {_device.Volume}.");
            }
        }
    }
}
