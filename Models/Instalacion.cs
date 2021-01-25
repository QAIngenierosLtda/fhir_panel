using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{

    // Definicion del Objeto Empresas (tabla para almacenar la informacion de Instalaciones)
    [Table("Instalaciones")]
    public class Instalacion
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Codigo")]
        [Key]
        public byte Codigo { get; set; }

        [Column("Descripcion")]
        public string Descripcion { get; set; }

        [Column("Regional")]
        public Int16 Regional { get; set; }


    }
}
