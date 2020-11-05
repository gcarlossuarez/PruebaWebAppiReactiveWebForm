using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaWebAppiReactiveWebForm.Models
{
    public class PersonaDireccion
    {
        [Key]
        public int Id { get; set; }
        public int PersonaId { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int DireccionId { get; set; }
        public string Calle { get; set; }
        public string Provincia { get; set; }

    }
}