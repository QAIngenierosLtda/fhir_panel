using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataConduitManager.Repositories.DTO;

namespace LenelServices.Repositories.DTO
{
    public class ResLastLocation_DTO
    {
        public List<LastLocation_DTO> locations { get; set; }
        public string result { get; set; }
        public bool success { get; set; }
    }
}
