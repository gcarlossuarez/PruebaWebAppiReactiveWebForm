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
                foreach (var itemDireccion in itemPersona.Direcciones)
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
            var persona = await _context.Persona.FindAsync(id);

            if (persona == null)
            {
                return NotFound();
            }

            return Ok(persona);
            //return Ok();
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

            persona.Direcciones.Add(direccion);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonaDireccion", new { id = personaDireccion.Id }, personaDireccion);
        }

        [HttpPost("post/list")]
        public IActionResult PostList([FromBody] List<PersonaDireccion> listPersonaDireccion)
        {
            //_context.PersonaDireccion.Add(personaDireccion);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetPersonaDireccion", new { id = personaDireccion.Id }, personaDireccion);
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    PersonaDireccion personaDireccion = listPersonaDireccion.FirstOrDefault();
                    Persona persona = _context.Persona.Where(x => x.Id == personaDireccion.PersonaId).FirstOrDefault();
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
                    foreach (var l_PersonaDireccion in listPersonaDireccion)
                    {
                        Direccion direccion = this.GetDireccionDePersonaDireccion(l_PersonaDireccion);
                        //direccion.PersonaId = personaDireccion.PersonaId;

                        persona.Direcciones.Add(direccion);
                    }

                    _context.SaveChanges();

                    transaccion.Commit();

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    transaccion.Rollback();
                    string aux = ex.Message;
                    throw;
                }
                
            }

            return Ok(); // CreatedAtAction("GetPersonaDireccion", new { id = personaDireccion.Id }, personaDireccion);
        }


        [HttpPut("put/list/{id}")]
        public async Task<IActionResult> PutList([FromRoute] int id, [FromBody] List<PersonaDireccion> listPersonaDireccion)
        {
            PersonaDireccion personaDireccion = listPersonaDireccion.FirstOrDefault();
            if (personaDireccion == null)
            {
                return BadRequest();
            }

            Persona persona = this.GetPersonaDePersonaDireccion(personaDireccion);

            using(var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    //_context.Entry(persona).State = EntityState.Modified;
                    _context.Update(persona);

                    await _context.SaveChangesAsync();

                    List<Direccion> listDireccion = new List<Direccion>();
                    foreach (var l_PersonaDireccion in listPersonaDireccion)
                    {
                        Direccion direccion = this.GetDireccionDePersonaDireccion(l_PersonaDireccion);
                        listDireccion.Add(direccion);
                    }

                    CrearOEditarDirecciones(persona, listDireccion);

                    //await _context.SaveChangesAsync();
                    transaccion.Commit();

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    transaccion.Rollback();
                    string aux = ex.Message;
                    throw;
                }

            }

            //return NoContent();

            // NOTA.- El '08/11/2020', ya funciona; pero, buscar la forma de optimizar, para que devuelva el listado, "Angular" lo reciba 
            // bien y velva a la patnalla principa de "Personas", en "Angular"
            return Ok(); // CreatedAtAction("GetPersonaDireccion", new { id = personaDireccion.Id }, personaDireccion);
            //return CreatedAtAction("GetPersona", "PersonasController", new { id = personaDireccion.Id }, personaDireccion);
            //return CreatedAtAction("GetPersonaDireccion", new { id = personaDireccion.Id }, personaDireccion);
            //return (IActionResult)GetPersonaDireccion(id);
            //return (IActionResult)_context.Persona.ToList();
        }

        private void CrearOEditarDirecciones(Persona persona, List<Direccion> direcciones)
        {
            List<Direccion> direccionesACrear = direcciones.Where(x => x.Id == 0).ToList();
            List<Direccion> direccionesAEditar = direcciones.Where(x => x.Id != 0).ToList();
            List<Direccion> direccionesAEliminar = new List<Direccion>();
            
            foreach(var l_Direccion in _context.Direccion.Where(x=> x.PersonaId == persona.Id))
            {
                if(direcciones.Where(x => x.PersonaId == persona.Id && x.Id == l_Direccion.Id).Count() == 0)
                {
                    direccionesAEliminar.Add(l_Direccion);
                }
            }

            if (direccionesAEliminar.Any())
            {
                _context.RemoveRange(direccionesAEliminar);
            }
            _context.SaveChanges();

            if (direccionesACrear.Any())
            {
                foreach(var d in direccionesACrear)
                {
                    d.Persona = persona;
                }
                _context.AddRange(direccionesACrear);
            }
            _context.SaveChanges();

            // Por el momento '08/11/2020', no sé por qué cae; por eso, lo comento. Si el usuario quiere modificar, que elimine y vuelva
            // a insertar. No es lo mejor, pero sirve, por el momento. Parece que algo tiene que ver lo de las claves foráneas y lo de
            // las referencias circulares
            //if (direccionesAEditar.Any())
            //{
            //    foreach (var d in direccionesAEditar)
            //    {
            //        d.Persona = persona;
            //    }
            //    _context.UpdateRange(direccionesAEditar);
            //}
            foreach (var direccionAEditar in direccionesAEditar)
            {
                Direccion direccionOriginal = _context.Direccion.Where(x => x.Id == direccionAEditar.Id).FirstOrDefault();
                if (direccionOriginal != null)
                {
                    direccionOriginal.Provincia = direccionAEditar.Provincia;
                    direccionOriginal.Calle = direccionAEditar.Calle;
                }
            }
            _context.SaveChanges();


            //foreach (var l_Direccion in direccionesACrear)
            //{
            //    _context.Direccion.Add(l_Direccion);
            //}
            //_context.SaveChanges();

            //foreach (var l_Direccion in direccionesAEditar)
            //{
            //    _context.Direccion.Update(l_Direccion);
            //}
            //_context.SaveChanges();

            //foreach (var l_Direccion in direccionesAEliminar)
            //{
            //    _context.Direccion.Remove(l_Direccion);
            //}
            //_context.SaveChanges();
        }
        private bool PersonaExists(int id)
        {
            return _context.Persona.Any(e => e.Id == id);
        }
        private Persona GetPersonaDePersonaDireccion(PersonaDireccion p_PersonaDireccion)
        {
            Persona l_Persona = new Persona()
            {
                Id = p_PersonaDireccion.PersonaId,
                Nombre = p_PersonaDireccion.Nombre,
                FechaNacimiento = p_PersonaDireccion.FechaNacimiento,
                Direcciones = new List<Direccion>()
            };

            return l_Persona;
        }


        private Direccion GetDireccionDePersonaDireccion(PersonaDireccion p_PersonaDireccion)
        {
            Direccion l_Direccion = new Direccion()
            {
                Id = p_PersonaDireccion.DireccionId,
                Calle = p_PersonaDireccion.Calle,
                Provincia = p_PersonaDireccion.Provincia,
                PersonaId = p_PersonaDireccion.PersonaId
            };

            return l_Direccion;
        }

        // DELETE: api/PersonaDireccions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PersonaDireccion>> DeletePersonaDireccion(int id)
        {
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    Persona persona = _context.Persona.Where(x => x.Id == id).FirstOrDefault();
                    if (persona != null)
                    {
                        List<Direccion> direccionesAEliminar = _context.Direccion.Where(x => x.PersonaId == id).ToList();
                        if (direccionesAEliminar != null)
                        {
                            _context.RemoveRange(direccionesAEliminar);
                        }

                        _context.Persona.Remove(persona);

                        await _context.SaveChangesAsync();

                    }
                    transaccion.Commit();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    transaccion.Rollback();
                    string aux = ex.Message;
                    throw;
                }
            }

            return Ok(); // personaDireccion;
        }

        //private bool PersonaDireccionExists(int id)
        //{
        //    return true; // _context.PersonaDireccion.Any(e => e.Id == id);
        //}
    }
}
