namespace _2016SQLDayDemoEF6.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int empId { get; set; }

        [StringLength(256)]
        public string name { get; set; }

        public int? databasePrincipalId { get; set; }
    }
}
