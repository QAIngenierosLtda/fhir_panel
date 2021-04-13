using System;
using System.Collections.Generic;
using System.Text;

namespace DataConduitManager.Repositories.DTO
{
    public class LastLocation_DTO
    {
        public string badgeId { get; set; }
        public string eventTime { get; set; }
        public int panelId { get; set; }
        public int readerId { get; set; }
    }
}
