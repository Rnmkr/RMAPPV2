namespace RMAPPV2
{ 
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Personal")]
    public partial class Personal
    {
        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string NumeroLegajo { get; set; }

        [Required]
        [StringLength(10)]
        public string NumeroAcceso { get; set; }

        [Required]
        [StringLength(30)]
        public string Apellido { get; set; }

        [Required]
        [StringLength(30)]
        public string Nombre { get; set; }

        [StringLength(30)]
        public string SegundoNombre { get; set; }
    }
}
