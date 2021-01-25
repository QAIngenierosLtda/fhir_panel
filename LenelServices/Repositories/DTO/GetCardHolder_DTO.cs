using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LenelServices.Repositories.DTO
{
    public class GetCardHolder_DTO
    {
        public string apellidos { get; set; }
        public string nombres { get; set; }
        public string ssno { get; set; }
        public string status { get; set; }
        public string documento { get; set; }
        public string empresa { get; set; }
        public string ciudad { get; set; }
        public string email { get; set; }
    }
}
