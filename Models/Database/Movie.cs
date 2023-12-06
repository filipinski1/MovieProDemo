
using System;
using System.ComponentModel.DataAnnotations;

namespace MovieProDemo.Models.Database
{
    public class Movie
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string Title { get; set; }   
        public string TagLine { get; set; }
        public string Overview { get; set; }
        public int RunTime { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }
        public MovieRating Rating { get; set; }

            
           
    }
}
