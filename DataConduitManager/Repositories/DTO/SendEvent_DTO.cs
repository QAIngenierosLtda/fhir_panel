using System;
using System.Collections.Generic;
using System.Text;

namespace DataConduitManager.Repositories.DTO
{
    public class SendEvent_DTO
    {
        public string source { get; set; }
        public string device { get; set; }
        public string subdevice { get; set; }
        public string description { get; set; }
        public bool? isAccessGranted { get; set; }
        public bool? isAccessDeny { get; set; }
        public string documento { get; set; }
        public bool? tapabocas { get; set; }
        public float? temperatura { get; set; }
        public float? tempRef { get; set; }
    }
}
