namespace _2016SQLDayDemoEF6.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class wing
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int wingId { get; set; }

        [StringLength(128)]
        public string name { get; set; }
    }
}
