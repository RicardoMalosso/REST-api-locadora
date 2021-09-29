using System;
using System.Collections.Generic;
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
    public class LocacoesController : ControllerBase
    {
        private readonly LocacaoContext _context;

        public LocacoesController(LocacaoContext context)
        {
            _context = context;
        }

        // GET: api/Locacaos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Locacao>>> GetLocacoes()
        {
            return await _context.Locacoes.Where(e => !e.IsDeleted).ToListAsync();
        }

        // GET: api/Locacaos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Locacao>> GetLocacao(long id)
        {
            var locacao = await _context.Locacoes.Where(e => !e.IsDeleted && e.Id == id).FirstOrDefaultAsync();

            if (locacao == null)
            {
                return NotFound();
            }

            return locacao;
        }

        // PUT: api/Locacaos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocacao(long id, Locacao locacao)
        {
            if (id != locacao.Id)
            {
                return BadRequest();
            }

            _context.Entry(locacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocacaoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        

        // POST: api/Locacoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Locacao>> PostLocacao(Locacao locacao)
        {
            if (locacao.MovieId == 0)
            {
                return BadRequest("A locação deve ter um id de filme.");
            }
            if (locacao.RenterId == 0)
            {
                return BadRequest("A locação deve ter um id de cliente.");
            }
            if (isMovieAlreadyRented(locacao.MovieId))
            {
                return BadRequest("O filme já está alugado.");
            }
            //todo: validar que o id do cliente e do filme existem

            _context.Locacoes.Add(locacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocacao", new { id = locacao.Id }, locacao);
        }

        // DELETE: api/Locacaos/5
        //DELETE apenas muda a prop isDeleted pra True, já que um
        //dos requisitos de negócio é que Não é permitido excluir fisicamente um registro
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocacao(long id)
        {
            var locacao = await _context.Locacoes.FindAsync(id);
            if (locacao == null)
            {
                return NotFound();
            }

            locacao.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocacaoExists(long id)
        {
            return _context.Locacoes.Any(e => e.Id == id);
        }
        private bool isMovieAlreadyRented(long movieId)
        {
            return _context.Locacoes.Any(e => e.MovieId == movieId && !e.IsDeleted && !e.IsReturned);
        }
    }
}
