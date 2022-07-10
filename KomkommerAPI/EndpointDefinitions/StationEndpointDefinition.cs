using AngleSharp.Html.Parser;
using KomkommerAPI.EndpointConfigurations;
using KomkommerAPI.Models;
using KomkommerAPI.Repositories;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace KomkommerAPI.EndpointDefinitions
{
    public class StationEndpointDefinition : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            app.MapGet("/station/now-playing", GetNowPlaying);
        }

        internal async Task GetNowPlaying(IStationRepository repo)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var resp = client.GetStringAsync("https://rarock.com/wp-admin/admin-ajax.php?action=rdj_now_playing");
            var msg = await resp;

            // parse response
            var parser = new HtmlParser();
            var document = parser.ParseDocument(msg);

            // now playing
            // todo: safty checking
            var nowPlayingRaw = document.All.Where(m => m.LocalName == "td" && m.ClassList.Contains("playing_track")).FirstOrDefault();
            var nowPlaying = new Track
            {
                Id = Guid.NewGuid(),
                Artist = nowPlayingRaw?.Children?.Where(m => m.ClassName.Contains("artist"))?.FirstOrDefault()?.TextContent,
                Title = nowPlayingRaw?.Children?.Where(m => m.ClassName.Contains("title"))?.FirstOrDefault()?.TextContent
            };

            // queue
            // todo: safty checking
            List<string> trackQueue = new List<string>();
            var queueRaw = document.All.Where(m => m.LocalName == "td" && m.ClassList.Contains("comming-soon")).FirstOrDefault();
            var queueTracks = queueRaw?.InnerHtml.Split('\\');
            // remove scrap
            var match = new Regex(@"[\n\t]");
            foreach (var track in queueTracks)
            {
                trackQueue.Add(match.Replace(track, string.Empty));
            }

            // history
            // todo: safty checking
            var historyRaw = document.All.Where(m => m.LocalName == "tr" && m.ClassName == "recent-tracks");

            var history = new List<Track>();

            foreach (var track in historyRaw)
            {
                history.Add(new Track
                {
                    Id = Guid.NewGuid(),
                    Artist = track.Children?.FirstOrDefault()?.Children?.Where(w => w.ClassName.Contains("artist"))?.FirstOrDefault()?.TextContent,
                    Title = track.Children?.FirstOrDefault()?.Children?.Where(w => w.ClassName.Contains("title"))?.FirstOrDefault()?.TextContent
                });
            }
        }

        public void DefineServices(IServiceCollection services)
        {
            services.AddSingleton<IStationRepository, StationRepository>();
        }
    }
}
