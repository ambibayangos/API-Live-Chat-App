using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveChatAPp.Model
{
    public partial class Video
    {
        public Video()
        {
            Player = new HashSet<Player>();
        }

        public int VideoId { get; set; }
        [Required]
        [StringLength(255)]
        public string VideoTitle { get; set; }
        public int VideoLength { get; set; }
        [Required]
        [Column("WebURL")]
        [StringLength(255)]
        public string WebUrl { get; set; }
        [Required]
        [Column("ThumbnailURL")]
        [StringLength(255)]
        public string ThumbnailUrl { get; set; }
        [Column("isFavourite")]
        public bool IsFavourite { get; set; }

        [InverseProperty("Video")]
        public virtual ICollection<Player> Player { get; set; }
    }
}
