using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{
    
    [Table("user")]
    public class Usuario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("Rol")]
        public byte Rol { get; set; }

        [Column("FechaInicial")]
        public DateTime FechaInicial { get; set; }

        [Column("FechaFinal")]
        public DateTime FechaFinal { get; set; }
    }
}
