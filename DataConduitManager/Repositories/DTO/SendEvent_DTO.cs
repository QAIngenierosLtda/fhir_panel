﻿using System;
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
        public int? badgeId { get; set; }
        public bool? tapabocas { get; set; }
        public float? temperatura { get; set; }
        public float? tempRef { get; set; }
        public string panelId { get; set;}
        public string readerId { get; set; }
        public bool? validaIdentidad { get; set; }
        public string documentoRfId { get; set; }
        public bool? esVisitante { get; set; }
        public bool? ecoPassVigente { get; set; }
        public bool? PersonaConocida { get; set; }
        public bool? BiomeConectado { get; set; }
    }
}
