namespace _2016SQLDayDemoEF6.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TK.tkViewAll")]
    public partial class tkViewAll
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdArticle { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdOrder { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal Count { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "datetime2")]
        public DateTime tkOrderToArticle_DtAdd { get; set; }

        [Key]
        [Column(Order = 5)]
        public double Height { get; set; }

        [StringLength(50)]
        public string Notes { get; set; }

        [Key]
        [Column(Order = 6)]
        public decimal TotalPrice { get; set; }

        public string BlobImageUrl { get; set; }

        [Key]
        [Column(Order = 7, TypeName = "datetime2")]
        public DateTime tkArticles_DtAdd { get; set; }

        [Column(TypeName = "ntext")]
        public string LongDescription { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public string Address { get; set; }

        [Key]
        [Column(Order = 8, TypeName = "datetime2")]
        public DateTime tkOrders_DtAdd { get; set; }

        [StringLength(50)]
        public string OrderNo { get; set; }
    }
}
