namespace _2016ODataInEF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("abc")]
    public partial class abc
    {
        public int id { get; set; }

        public int a { get; set; }

        public double? b { get; set; }

        public string c { get; set; }

        public DateTime? d { get; set; }

        [StringLength(100)]
        public string e { get; set; }
    }
}
