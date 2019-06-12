using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DUBle_Watch.Models
{
    public class AnimeTracked
    {

        [Key]
        public int AnimeTrackedId { get; set; }

        public int AnimeId { get; set; }

        public Anime Anime { get; set; }
        public string UserId { get; set; }

        public ApplicationUser CurrentUser { get; set; }

        public int TimesCompleted { get; set; }

        public bool IsInCurrentlyCompletedSection { get; set; }

        public int CurrentEpisode { get; set; }

        public DateTime LastTimeEpisodeUpdated{ get; set; }






    }
}
