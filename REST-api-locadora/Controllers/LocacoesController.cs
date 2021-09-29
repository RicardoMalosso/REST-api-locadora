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
        private readonly LocadoraContext _context;

        public LocacoesController(LocadoraContext context)
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
            Filme filmeAAlugar = _context.Filmes.Find(locacao.MovieId);
            Cliente locatario = _context.Clientes.Find(locacao.RenterId);

            if (!ClienteExists(locatario))
            {
                return BadRequest("O id do Cliente deve ser válido.");
            }
            if (!FilmeExists(filmeAAlugar))
                {
                return BadRequest("O id do Filme deve ser válido.");
            }
            if (isMovieAlreadyRented(locacao.MovieId))
            {
                return BadRequest("O filme já está alugado.");
            }

            _context.Locacoes.Add(locacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocacao", new { id = locacao.Id }, locacao);
        }
        //POST: api/Locacoes/retornar/5
        [HttpPost("retornar/{id}")]
        public async Task<IActionResult> RetornarLocacao(long id)
        {
            var locacao = await _context.Locacoes.FindAsync(id);
            if (locacao == null)
            {
                return NotFound();
            }
            if (locacao.ReturnDate.HasValue)
            {
                return BadRequest("A locação já foi entregue.");
            }

            locacao.IsReturned = true;
            locacao.RentalDate = null;
            locacao.ReturnDate = DateTime.Now;
            await _context.SaveChangesAsync();
            if (locacao.RentalDate.HasValue)
            {
                if (DateTime.Today.Subtract(locacao.RentalDate.Value) > new TimeSpan(locacao.RentalPeriod, 0, 0, 0))
                {
                    return Ok("Entrega atrasada.");
                }
            }
            return Ok();
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

        private bool ClienteExists(Cliente? cliente)
        {
            return !(cliente == null || cliente.IsDeleted);
        }
        private bool FilmeExists(Filme? filme)
        {
            return !(filme == null || filme.IsDeleted);
        }
    }
}
