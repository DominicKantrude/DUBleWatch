using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DUBle_Watch.Models
{
    public class Anime
    {
                [Key]
    public int AnimeId { get; set; }

        [Required]
        public string Name { get; set; }

        public string ImagePath { get; set; }

        public int CurrentLastEpisode { get; set; }
        public int? GenreId { get; set; }
        public Genre Genre { get; set; }
        public string AnimeLink { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        [DataType(DataType.Date)]
        public DateTime? AnimeReleaseDate { get; set; }
        public bool hasAnimeEnded { get; set;}
        public virtual ICollection<AnimeTracked> trackedInstancesOfAnime { get; set;}
    }
}
