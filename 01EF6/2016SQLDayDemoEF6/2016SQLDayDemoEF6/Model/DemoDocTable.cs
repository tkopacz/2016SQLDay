namespace _2016SQLDayDemoEF6.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DemoDocTable
    {
        public int Id { get; set; }

        public string DocOwner { get; set; }

        public string Title { get; set; }

        public string Keywords { get; set; }

        public int? Importance { get; set; }

        public string DocPath { get; set; }
    }
}
