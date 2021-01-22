using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{

    // Definicion del Objeto TipoDoc (tabla para almacenar la informacion de Tipos de documento)
    [Table("docType")]
    public class Tipodoc
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("codigo")]
        [Key]
        public string codigo { get; set; }

        [Column("descripcion")]
        public string descripcion { get; set; }
    }
}
