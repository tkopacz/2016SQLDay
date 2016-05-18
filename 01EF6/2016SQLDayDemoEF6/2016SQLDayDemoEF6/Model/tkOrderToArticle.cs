namespace _2016SQLDayDemoEF6.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TK.tkOrderToArticle")]
    public partial class tkOrderToArticle
    {
        public int Id { get; set; }

        public int IdArticle { get; set; }

        public int IdOrder { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DtAdd { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Ts { get; set; }

        public decimal Count { get; set; }

        public decimal TotalPrice { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        [StringLength(50)]
        public string Notes { get; set; }

        public virtual tkArticle tkArticle { get; set; }

        public virtual tkOrder tkOrder { get; set; }
    }
}
