using System.Collections.Generic;
using Newtonsoft.Json;

namespace TraktApp
{
    public class JsonParser
    {
        public static List<Trending> BoxOfficeParser(string content)
        {
            return JsonConvert.DeserializeObject<List<Trending>>(content);
        }

        public static MovieInfo MovieInfoParser(string content)
        {
            return JsonConvert.DeserializeObject<MovieInfo>(content);
        }
    }
}