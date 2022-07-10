using KomkommerAPI.Models;

namespace KomkommerAPI.Repositories
{
    interface IStationRepository
    {
        List<Song> NowPlaying(Guid StationId);
    }
    class StationRepository : IStationRepository
    {
        private readonly Dictionary<Guid, Song> _songs = new();

        public List<Song> NowPlaying(Guid id)
        {
            return _songs.Values.ToList();
        }
    }
}
