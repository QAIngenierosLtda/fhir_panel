using System;
using System.Collections.Generic;
using System.Text;

namespace DataConduitManager.Repositories.DTO
{
    public class AddBadge_DTO
    {
        public Int32 id { get; set; }
        public Int32 personId { get; set; }
        public Int32 status { get; set; }
        public Int32 type { get; set; }
        public Int32 dest_exemp { get; set; }
    }
}
