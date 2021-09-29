using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST_api_locadora.Models
{
    public class LocacaoContext : DbContext
    {
        public LocacaoContext(DbContextOptions<LocacaoContext> options) : base(options)
        {

        }
        public DbSet<Locacao> Locacoes { get; set; }
    }
}
