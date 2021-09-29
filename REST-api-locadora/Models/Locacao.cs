using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST_api_locadora.Models
{
    public class Locacao
    {
        public long Id { get; set; }

        public long RenterId { get; set; }

        public long MovieId { get; set; }
        
        public DateTime? RentalDate { get; set; } = null;

        public bool IsDeleted { get; set; } = false;

        public bool IsReturned { get; set; } = false;
    }
}
