namespace _2016ODataInEF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sample_table
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int c1 { get; set; }

        [StringLength(10)]
        public string c2 { get; set; }

        public DateTime? c3 { get; set; }
    }
}
