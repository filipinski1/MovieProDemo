using MovieProDemo.Models.TMDB;
using System.Collections.Generic;
using MovieProDemo.Models.Database;
using System;
using System.Threading.Tasks;
using System.Linq;


namespace MovieProDemo.Models.ViewModels
{
    public class LandingPageVM
    {
        public List<Collection> CustomCollections { get; set; }
        public MovieSearch NowPlaying { get; set; }
        public MovieSearch Popular { get; set; }
        public MovieSearch TopRated { get; set; }
        public MovieSearch Upcoming { get; set; }
    }
}
