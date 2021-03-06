using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tareas.Context;
using backend_tareas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_tareas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareaController : ControllerBase
    {
        private readonly AplicationDbContext _context;
        public TareaController(AplicationDbContext context)
        {
            _context = context;
        }

        //Leer datos
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listTareas = await _context.Tareas.ToListAsync();
                return Ok(listTareas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Insertar
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Tarea tarea)
        {
            try
            {
                _context.Tareas.Add(tarea);
                await _context.SaveChangesAsync();
                return Ok(new { message = "La tarea fue creada!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Editar
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Tarea tarea)
        {
            try
            {
                if(id != tarea.Id)
                {
                    return NotFound();
                }
                tarea.Estado = !tarea.Estado;
                _context.Entry(tarea).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { message = "Tarea actualizada" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Eliminar
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var tarea = await _context.Tareas.FindAsync(id);
                if(tarea == null)
                {
                    return NotFound();
                }
                _context.Tareas.Remove(tarea);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Tarea eliminada!" });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
