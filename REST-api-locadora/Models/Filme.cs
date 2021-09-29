using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST_api_locadora.Models
{
    public class Filme
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
