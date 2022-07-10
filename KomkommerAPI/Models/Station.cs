namespace KomkommerAPI.Models
{
    record Station(Guid id, string name);
    record Song(Guid Id, string Title, string Artist, string Image, Station station);
}
