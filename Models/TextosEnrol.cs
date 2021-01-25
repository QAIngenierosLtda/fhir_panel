using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace AspStudio.Models
{

    // Definicion del Objeto Empresas (tabla para almacenar la informacion de Empresas)
    [Table("textEnrol")]
    public class TextosEnrol
    {

            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Column("id")]
            [Key]
            public int id { get; set; }

            [Column("Texto")]
            public string Texto { get; set; }

            [Column("Fecha")]
            public DateTime Fecha { get; set; }

            [Column("Version")]
            public Double Version { get; set; }

            [Column("Tipo")]
            public Int16 Tipo { get; set; }

            [Column("Pregunta")]
            public string Pregunta { get; set; }

    }
}
