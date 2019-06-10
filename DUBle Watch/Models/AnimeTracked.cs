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
        public int UserId { get; set; }

        ApplicationUser CurrentUser { get; set; }

        public int CompletedCount { get; set; }

        public bool CurrentlyCompleted { get; set; }

        public int CurrentEpisode { get; set; }

       



    }
}
