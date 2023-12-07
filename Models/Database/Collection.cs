using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieProDemo.Models.Database
{
    public class Collection
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<MovieCollection> MovieCollections { get; set; } = new HashSet<MovieCollection>();

    }
}
