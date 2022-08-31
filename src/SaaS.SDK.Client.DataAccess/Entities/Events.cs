﻿using System;

namespace Microsoft.Marketplace.SaasKit.Client.DataAccess.Entities
{
    public partial class Events
    {
        public int EventsId { get; set; }
        public string EventsName { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
