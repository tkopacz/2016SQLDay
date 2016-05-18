namespace _2016ODataInEF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MarketingCampaignEffectiveness")]
    public partial class MarketingCampaignEffectiveness
    {
        [Key]
        public int McEffId { get; set; }

        [Required]
        [StringLength(256)]
        public string ProfileID { get; set; }

        [Required]
        [StringLength(256)]
        public string SessionStart { get; set; }

        [Required]
        [StringLength(256)]
        public string Duration { get; set; }

        [Required]
        [StringLength(256)]
        public string State { get; set; }

        [Required]
        [StringLength(256)]
        public string SrcIPAddress { get; set; }

        [Required]
        [StringLength(256)]
        public string GameType { get; set; }

        [Required]
        [StringLength(256)]
        public string Multiplayer { get; set; }

        [Required]
        [StringLength(256)]
        public string EndRank { get; set; }

        [Required]
        [StringLength(256)]
        public string WeaponsUsed { get; set; }

        [Required]
        [StringLength(256)]
        public string UsersInteractedWith { get; set; }

        public int Impressions { get; set; }
    }
}
