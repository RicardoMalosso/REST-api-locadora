using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using REST_api_locadora.Models;

namespace REST_api_locadora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmesController : ControllerBase
    {
        private readonly LocadoraContext _context;

        public FilmesController(LocadoraContext context)
        {
            _context = context;
        }

        // GET: api/Filmes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filme>>> GetFilmes()
        {
            return await _context.Filmes.Where(e => !e.IsDeleted).ToListAsync();
        }

        // GET: api/Filmes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Filme>> GetFilme(long id)
        {
            var filme = await _context.Filmes.Where(e => !e.IsDeleted && e.Id == id).FirstOrDefaultAsync();

            if (filme == null)
            {
                return NotFound();
            }

            return filme;
        }

        
        // POST: api/Filmes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Filme>> PostFilme(Filme filme)
        {
            if (!(filme.Name is string) || filme.Name.Length == 0){
                return BadRequest("O filme deve ter um nome.");
            }
            _context.Filmes.Add(filme);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFilme), new { id = filme.Id }, filme);
        }

        // DELETE: api/Filmes/5
        //DELETE apenas muda a prop isDeleted pra True, já que um
        //dos requisitos de negócio é que Não é permitido excluir fisicamente um registro
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilme(long id)
        {
            var filme = await _context.Filmes.FindAsync(id);
            if (filme == null)
            {
                return NotFound();
            }
            filme.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FilmeExists(long id)
        {
            return _context.Filmes.Any(e => e.Id == id);
        }
    }
}
