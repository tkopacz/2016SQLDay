namespace _2016ODataInEF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TK.tkLog")]
    public partial class tkLog
    {
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DtAdd { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Ts { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        [Required]
        [StringLength(50)]
        public string Notes { get; set; }

        public decimal TelemetryMs { get; set; }
    }
}
