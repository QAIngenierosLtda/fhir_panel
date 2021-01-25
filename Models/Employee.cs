using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{
    
    [Table("employees")]
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("id_lenel")]
        public string IdLenel { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("ssno")]
        public string SSNO { get; set; }

        [Column("id_status")]
        public string IdStatus { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("documento")]
        public string Documento { get; set; }

        [Column("empresa")]
        public string Empresa { get; set; }

        [Column("nit")]
        public string Nit { get; set; }

        [Column("imageUrl")]
        public string imageUrl { get; set; }

        [Column("start_time")]
        public DateTime? StartTime { get; set; }

        [Column("hasPhoto")]
        public bool hasPhoto { get; set; }

        [Column("end_time")]
        public DateTime? EndTime { get; set; }

        [Column("created")]
        public DateTime? Created { get; set; }

        [Column("updated")]
        public DateTime? Updated { get; set; }

        [Column("ciudadEnroll")]
        public string Ciudad { get; set; }


    }
}
