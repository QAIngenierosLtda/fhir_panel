using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{

    // Definicion del Objeto Empresas (tabla para almacenar la informacion de Regionales)
    [Table("Regionales")]
    public class Regional
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Codigo")]
        [Key]
        public Int16 Codigo { get; set; }

        [Column("Descripcion")]
        public string Descripcion { get; set; }

    }
}
