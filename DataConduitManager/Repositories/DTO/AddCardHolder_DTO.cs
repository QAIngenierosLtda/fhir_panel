using System;
using System.Collections.Generic;
using System.Text;

namespace DataConduitManager.Repositories.DTO
{
    public class AddCardHolder_DTO
    {
        public string nroDocumento { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string ssno { get; set; }
        public string city { get; set; }
        public string empresa { get; set; }
        public string regional { get; set; }
        public string instalacion { get; set; }
        public string origen { get; set; }
        public string email { get; set; }
    }
}
