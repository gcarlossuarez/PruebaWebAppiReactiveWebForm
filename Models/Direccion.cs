using System;
using System.Collections.Generic;

namespace PruebaWebAppiReactiveWebForm.Models
{
    public partial class Direccion
    {
        public int Id { get; set; }
        public string Calle { get; set; }
        public string Provincia { get; set; }
        public int PersonaId { get; set; }

        public virtual Persona Persona { get; set; }
    }
}
