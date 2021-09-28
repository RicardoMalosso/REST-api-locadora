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
        public bool Available { get; set; } = true;

        public DateTime? rentalDate { get; set; } = null;
        //limite de dias para o retorno. Default 7 dias
        public int limitInDays { get; set; } = 7;

        public bool isDeleted { get; set; } = false;
    }
}
