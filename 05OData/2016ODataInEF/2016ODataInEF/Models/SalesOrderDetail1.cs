namespace _2016ODataInEF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("statistic.SalesOrderDetail")]
    public partial class SalesOrderDetail1
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SalesOrderID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SalesOrderDetailID { get; set; }

        [StringLength(25)]
        public string CarrierTrackingNumber { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short OrderQty { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductID { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SpecialOfferID { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "money")]
        public decimal UnitPrice { get; set; }

        [Key]
        [Column(Order = 6, TypeName = "money")]
        public decimal UnitPriceDiscount { get; set; }

        [Key]
        [Column(Order = 7, TypeName = "money")]
        public decimal LineTotal { get; set; }

        [Key]
        [Column(Order = 8)]
        public Guid rowguid { get; set; }

        [Key]
        [Column(Order = 9)]
        public DateTime ModifiedDate { get; set; }
    }
}
