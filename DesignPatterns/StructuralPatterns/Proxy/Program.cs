using Proxy;

Console.Title = "Proxy Pattern";

IVideoService realService = new YouTubeService();
IVideoService proxy = new CachedYouTubeServiceProxy(realService);

Console.WriteLine("--- First request for video 'csharp-course' ---");
var v1 = proxy.GetVideo("csharp-course");
Console.WriteLine($"Received: {v1.Title}\n");

Console.WriteLine("--- Second request for video 'csharp-course' (Cached) ---");
var v2 = proxy.GetVideo("csharp-course");
Console.WriteLine($"Received: {v2.Title}\n");

Console.WriteLine("--- Request for video 'design-patterns' ---");
var v3 = proxy.GetVideo("design-patterns");
Console.WriteLine($"Received: {v3.Title}\n");

Console.WriteLine("--- Request for video 'design-patterns' again (Cached) ---");
var v4 = proxy.GetVideo("design-patterns");
Console.WriteLine($"Received: {v4.Title}\n");
