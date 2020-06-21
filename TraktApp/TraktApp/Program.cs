using System;
using System.Collections.Generic;
using Npgsql;

namespace TraktApp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Trending> trending = JsonParser.BoxOfficeParser(Downloader.TrendingDownload());
            List<MovieInfo> moviesInfo = new List<MovieInfo>();
            List<int> ids = new List<int>();

            foreach (var trend in trending)
            {
                moviesInfo.Add(JsonParser.MovieInfoParser(Downloader.MovieInfoDownload(trend.Movie.Ids.Trakt.ToString())));
                ids.Add(trend.Movie.Ids.Trakt);
            }

            int i = 0;
            
            foreach (var info in moviesInfo)
            {
                Database.InsertMoviesData(ids[i], info.Title, info.Year, info.Overview, info.Rating, info.Runtime, info.Genres);

                Database.InsertTrendingData(ids[i], i);
                
                Console.WriteLine(info.Title);
                Console.WriteLine($"-> {info.Overview}");
                Console.WriteLine($"-> {info.Released}");
                Console.WriteLine($"-> {info.Rating}");

                i++;
            }
        }
    }
}