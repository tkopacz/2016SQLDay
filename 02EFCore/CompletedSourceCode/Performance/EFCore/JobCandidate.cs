﻿using System;
using System.Collections.Generic;

namespace Performance.EFCore
{
    public partial class JobCandidate
    {
        public int JobCandidateID { get; set; }
        public int? BusinessEntityID { get; set; }
        public string Resume { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Employee BusinessEntity { get; set; }
    }
}
