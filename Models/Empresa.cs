using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{

    // Definicion del Objeto Empresas (tabla para almacenar la informacion de Empresas)
    [Table("Empresas")]
    public class Empresa
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("codigo")]
        [Key]
        public string codigo { get; set; }

        [Column("descripcion")]
        public string descripcion { get; set; }

    }
}
