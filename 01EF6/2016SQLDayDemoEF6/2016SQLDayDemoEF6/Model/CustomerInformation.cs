namespace _2016SQLDayDemoEF6.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomerInformation")]
    public partial class CustomerInformation
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string CustomerName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string Company { get; set; }
    }
}
