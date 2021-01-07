using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{

    [Table("EnrolTemp")]
    public class EnrolData
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("Badge_id")]
        public string Badge_id { get; set; }

        [Column("FirstName")]
        public string firstName { get; set; }

        [Column("LastName")]
        public string lastName { get; set; }

        [Column("tipo_doc")]
        public string tipo_doc { get; set; }

        [Column("Documento")]
        public string documento { get; set; }

        [Column("acepta_terminos")]
        public bool acepta_terminos { get; set; }

        public string empresa { get; set; }

        public Int16 Regional { get; set; }
        public byte Instalacion { get; set; }

        [Column("CiudadOrigen")]
        public string Ciudad { get; set; }

        public string ciudadEnroll { get; set; }


        [Column("ssno")]
        public string SSNO { get; set; }


        [Column("id_status")]
        public string IdStatus { get; set; }

        [Column("status")]
        public string Status { get; set; }

        public string imageUrl { get; set; }

        [Column("Metadatos")]
        public string Metadatos { get; set; }

        [Column("created")]
        public DateTime created { get; set; }

        [Column("updated")]
        public DateTime updated { get; set; }

        [Column("Origen")]
        public string Origen { get; set; }

    }
}
