using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveChatAPp.Model
{
    public partial class Player
    {
        public int Playerid { get; set; }
        public int? VideoId { get; set; }
        public int Rank { get; set; }
        [Required]
        [StringLength(255)]
        public string PlayerName { get; set; }

        [ForeignKey("VideoId")]
        [InverseProperty("Player")]
        public virtual Video Video { get; set; }
    }
}
