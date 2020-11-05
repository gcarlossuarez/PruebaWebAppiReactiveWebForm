using System;
using System.Collections.Generic;

namespace PruebaWebAppiReactiveWebForm.Models
{
    public partial class Persona
    {
        public Persona()
        {
            Direccion = new HashSet<Direccion>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public virtual ICollection<Direccion> Direccion { get; set; }
    }
}
