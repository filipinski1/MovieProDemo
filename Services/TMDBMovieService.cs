using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MovieProDemo.Enums;
using MovieProDemo.Models.Settings;
using MovieProDemo.Models.TMDB;
using MovieProDemo.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace MovieProDemo.Services
{
    public class TMDBMovieService : IRemoteMovieService
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _httpClient;

        public TMDBMovieService(IOptions<AppSettings> appSettings, IHttpClientFactory httpClient)
        {
            _appSettings = appSettings.Value;
            _httpClient = httpClient;
        }

        public async  Task<ActorDetail> ActorDetailAsync(int id)
        {
            {
                //step 1 setupp default return object
                ActorDetail actorDetail = new();
                //step 2 assemble the full request uri string
                var query = $"{_appSettings.TMDBSettings.BaseUrl}/movie/{id}";
                var queryParams = new Dictionary<string, string>()
            {
                {"api_key", _appSettings.MovieProSettings.TmDbApiKey },
                {"language", _appSettings.TMDBSettings.QueryOptions.Language },
            };
                var requestUri = QueryHelpers.AddQueryString(query, queryParams);

                //step 3 create a client and exucute the request
                var client = _httpClient.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
                var response = await client.SendAsync(request);

                //step 4 return the ActorDetail object

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    var dcjs = new DataContractJsonSerializer(typeof(ActorDetail));
                    actorDetail = (ActorDetail)dcjs.ReadObject(responseStream);

                }

                return actorDetail;
            }
        }

        public async Task<MovieDetail> MovieDetailAsync(int id)
        {
            {
                //step 1 setupp default return object
                MovieDetail movieDetail = new();
                //step 2 assemble the request
                var query = $"{_appSettings.TMDBSettings.BaseUrl}/movie/{id}";

                var queryParams = new Dictionary<string, string>()
            {
                {"api_key", _appSettings.MovieProSettings.TmDbApiKey },
                {"language", _appSettings.TMDBSettings.QueryOptions.Language },
                {"appent_to_response", _appSettings.TMDBSettings.QueryOptions.AppendToResponse }
            };
                var requestUri = QueryHelpers.AddQueryString(query, queryParams);

                //step 3 create a client and exucute the request
                var client = _httpClient.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
                var response = await client.SendAsync(request);

                //step 4 return the MovieSearch object

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    var dcjs = new DataContractJsonSerializer(typeof(MovieDetail));
                    movieDetail = dcjs.ReadObject(responseStream) as MovieDetail;

                }

                return movieDetail;
            }
        }

        public async Task<MovieSearch> SearchMoviesAsync(MovieCategory category, int count)
        {
            //step 1 setup a default instance of MovieSearch
            MovieSearch movieSearch = new();
            //step 2 assemble the full request uri string
            var query = $"{_appSettings.TMDBSettings.BaseUrl}/movie/{category}";

            var queryParams = new Dictionary<string, string>()
            {
                {"api_key", _appSettings.MovieProSettings.TmDbApiKey },
                {"language", _appSettings.TMDBSettings.QueryOptions.Language },
                {"page", _appSettings.TMDBSettings.QueryOptions.Page }
            };
            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            //step 3 create a client and exucute the request
            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await client.SendAsync(request);

            //step 4 return the MovieSearch object

            if (response.IsSuccessStatusCode)
            {
                var dcjs = new DataContractJsonSerializer(typeof(MovieSearch));
                using var responseStream = await response.Content.ReadAsStreamAsync();
                movieSearch = (MovieSearch)dcjs.ReadObject(responseStream);
                movieSearch.results = movieSearch.results.Take(count).ToArray();
                movieSearch.results.ToList().ForEach(r => r.poster_path = $"{_appSettings.TMDBSettings.BaseImagePath}/{_appSettings.MovieProSettings.DefaultPosterSize}/{r.poster_path}");

            }

            return movieSearch;
        }
    }
}
