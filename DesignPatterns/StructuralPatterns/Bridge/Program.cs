using Bridge;

Console.Title = "Bridge Pattern";

// 1. Controlling a TV with a basic remote
Console.WriteLine("--- Testing TV with Basic Remote ---");
var tv = new Tv();
var basicRemote = new RemoteControl(tv);

basicRemote.TogglePower();
basicRemote.ChannelUp();
basicRemote.VolumeUp();
tv.PrintStatus();

// 2. Controlling a Radio with an advanced remote
Console.WriteLine("\n--- Testing Radio with Advanced Remote ---");
var radio = new Radio();
var advancedRemote = new AdvancedRemoteControl(radio);

advancedRemote.TogglePower();
advancedRemote.VolumeUp();
radio.PrintStatus();

advancedRemote.Mute();
radio.PrintStatus();

advancedRemote.Mute();
radio.PrintStatus();
