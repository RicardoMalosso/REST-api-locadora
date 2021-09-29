using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST_api_locadora.Models
{
    public class Locacao
    {
        public long Id { get; set; }

        public long RenterId { get; set; }

        public long MovieId { get; set; }

        public DateTime? RentalDate { get; set; } = DateTime.Today;

        public int RentalPeriod { get; set; } = 7;

        public DateTime? ReturnDate { get; set; }
        public bool IsDeleted { get; set; } = false;

        public bool IsReturned { get; set; } = false;
    }
}
