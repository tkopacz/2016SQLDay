namespace _2016ODataInEF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TblSample
    {
        public int Id { get; set; }

        [StringLength(3)]
        public string DeviceID { get; set; }

        public DateTime? Dt { get; set; }

        public long? Tick { get; set; }

        [StringLength(50)]
        public string SampleType { get; set; }

        [StringLength(50)]
        public string SampleValue { get; set; }
    }
}
