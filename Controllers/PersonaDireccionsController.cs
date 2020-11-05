using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaWebAppiReactiveWebForm.Models;

namespace reactiveFormWeb2.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[ApiController]
    public class PersonaDireccionsController : ControllerBase
    {
        private readonly Prueba1Context _context;

        public PersonaDireccionsController() //ApplicationDbContext context)
        {
            //_context = context;
            _context = new Prueba1Context();
        }

        // GET: api/PersonaDireccions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonaDireccion>>> GetPersonaDireccion()
        {
            //return await _context.PersonaDireccion.ToListAsync();
            List<PersonaDireccion> l_ListPersonaDireccion = new List<PersonaDireccion>();
            var l_ListPersona = await _context.Persona.ToListAsync();

            foreach (var itemPersona in l_ListPersona)
            {
                foreach (var itemDireccion in itemPersona.Direccion)
                {
                    PersonaDireccion l_PersonaDireccion = InstanciarPersonaDireccion(itemPersona, itemDireccion);
                    l_ListPersonaDireccion.Add(l_PersonaDireccion);
                }
            }

            return l_ListPersonaDireccion;
        }

        // GET: api/PersonaDireccions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonaDireccion>> GetPersonaDireccion(int id)
        {
            //var personaDireccion = await _context.PersonaDireccion.FindAsync(id);

            //if (personaDireccion == null)
            //{
            //    return NotFound();
            //}

            //return personaDireccion;
            return Ok();
        }


        private PersonaDireccion InstanciarPersonaDireccion(Persona persona, Direccion direccion)
        {
            PersonaDireccion personaDireccion = new PersonaDireccion();

            personaDireccion.Id = persona.Id;
            personaDireccion.Nombre = persona.Nombre;
            personaDireccion.FechaNacimiento = persona.FechaNacimiento;
            personaDireccion.DireccionId = direccion.Id;
            personaDireccion.Calle = direccion.Calle;
            personaDireccion.Provincia = direccion.Provincia;

            return personaDireccion;
        }

        // PUT: api/PersonaDireccions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonaDireccion([FromRoute] int id, [FromBody] PersonaDireccion personaDireccion)
        {
            //if (id != personaDireccion.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(personaDireccion).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!PersonaDireccionExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return NoContent();
        }

        // POST: api/PersonaDireccions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PersonaDireccion>> PostPersonaDireccion([FromBody] PersonaDireccion personaDireccion)
        {
            //_context.PersonaDireccion.Add(personaDireccion);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetPersonaDireccion", new { id = personaDireccion.Id }, personaDireccion);

            Persona persona = await _context.Persona.Where(x => x.Id == personaDireccion.PersonaId).FirstOrDefaultAsync();
            if (persona == null)
            {
                int l_IdUltimo = 0;
                Persona l_UltimaPersonaPorId = _context.Persona.OrderByDescending(x => x.Id).FirstOrDefault();
                if (l_UltimaPersonaPorId != null)
                {
                    l_IdUltimo = l_UltimaPersonaPorId.Id;
                }
                personaDireccion.PersonaId = l_IdUltimo + 1;
                persona = this.GetPersonaDePersonaDireccion(personaDireccion);
                _context.Persona.Add(persona);
            }
            Direccion direccion = this.GetDireccionDePersonaDireccion(personaDireccion);
            direccion.PersonaId = personaDireccion.PersonaId;

            persona.Direccion.Add(direccion);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonaDireccion", new { id = personaDireccion.Id }, personaDireccion);
        }


        private Persona GetPersonaDePersonaDireccion(PersonaDireccion p_PersonaDireccion)
        {
            Persona l_Persona = new Persona()
            {
                Id = p_PersonaDireccion.PersonaId,
                Nombre = p_PersonaDireccion.Nombre,
                FechaNacimiento = p_PersonaDireccion.FechaNacimiento,
                Direccion = new List<Direccion>()
            };

            return l_Persona;
        }

        private Direccion GetDireccionDePersonaDireccion(PersonaDireccion p_PersonaDireccion)
        {
            Direccion l_Direccion = new Direccion()
            {
                Id = p_PersonaDireccion.DireccionId,
                Calle = p_PersonaDireccion.Calle,
                Provincia = p_PersonaDireccion.Provincia
            };

            return l_Direccion;
        }

        // DELETE: api/PersonaDireccions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PersonaDireccion>> DeletePersonaDireccion(int id)
        {
            //var personaDireccion = await _context.PersonaDireccion.FindAsync(id);
            //if (personaDireccion == null)
            //{
            //    return NotFound();
            //}

            //_context.PersonaDireccion.Remove(personaDireccion);
            await _context.SaveChangesAsync();

            return Ok(); // personaDireccion;
        }

        private bool PersonaDireccionExists(int id)
        {
            return true; // _context.PersonaDireccion.Any(e => e.Id == id);
        }
    }
}
