namespace Proxy
{
    public record Video(string Id, string Title, byte[] Data);

    /// <summary>
    /// Subject Interface
    /// </summary>
    public interface IVideoService
    {
        Video GetVideo(string videoId);
    }

    /// <summary>
    /// Real Subject
    /// </summary>
    public class YouTubeService : IVideoService
    {
        public Video GetVideo(string videoId)
        {
            // Simulate slow network roundtrip / high latency downloading
            Console.WriteLine($"[YouTubeService] Connecting to YouTube API & downloading video '{videoId}'...");
            Thread.Sleep(300); // simulated latency
            return new Video(videoId, $"Video Title for {videoId}", [0x01, 0x02, 0x03]);
        }
    }

    /// <summary>
    /// Proxy (Caching / Virtual Proxy)
    /// </summary>
    public class CachedYouTubeServiceProxy : IVideoService
    {
        private readonly IVideoService _realService;
        private readonly Dictionary<string, Video> _cache = [];

        public CachedYouTubeServiceProxy(IVideoService realService)
        {
            _realService = realService;
        }

        public Video GetVideo(string videoId)
        {
            if (_cache.TryGetValue(videoId, out var cachedVideo))
            {
                Console.WriteLine($"[Proxy] Returning video '{videoId}' from memory cache (No remote call).");
                return cachedVideo;
            }

            Console.WriteLine($"[Proxy] Video '{videoId}' not in cache. Forwarding request to real service...");
            var video = _realService.GetVideo(videoId);
            _cache[videoId] = video;
            return video;
        }

        public void ResetCache()
        {
            _cache.Clear();
            Console.WriteLine("[Proxy] Cache cleared.");
        }
    }
}
