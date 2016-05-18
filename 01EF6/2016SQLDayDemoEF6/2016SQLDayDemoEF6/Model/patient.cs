namespace _2016SQLDayDemoEF6.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class patient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int patientId { get; set; }

        [StringLength(256)]
        public string name { get; set; }

        public int? room { get; set; }

        public int? wing { get; set; }

        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }
    }
}
