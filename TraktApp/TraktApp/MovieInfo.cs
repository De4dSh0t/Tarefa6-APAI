using System.Collections.Generic;

namespace TraktApp
{
    //Usado para guardar todas as informações necessárias sobre um filme (dependendo do Id)
    public class MovieInfo
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Overview { get; set; }
        public string Released { get; set; }
        public int Runtime { get; set; }
        public string Country { get; set; }
        public float Rating { get; set; }
        public string[] Genres { get; set; }
    }
}