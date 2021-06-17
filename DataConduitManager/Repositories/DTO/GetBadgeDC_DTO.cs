using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataConduitManager.Repositories.DTO
{
    public class GetBadgeDC_DTO
    {
        public string badgeID { get; set; } 
        public DateTime? activacion { get; set; }
        public DateTime? desactivacion { get; set; }
        public string estado { get; set; }
        public int? type { get; set; }
        public int badgekey { get; set; }
    }
}
