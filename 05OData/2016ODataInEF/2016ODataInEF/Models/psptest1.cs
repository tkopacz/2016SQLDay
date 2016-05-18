namespace _2016ODataInEF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class psptest1
    {
        [Key]
        public int col1 { get; set; }

        public int? col2 { get; set; }

        [MaxLength(200)]
        public byte[] col3 { get; set; }
    }
}
