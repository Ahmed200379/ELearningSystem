using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Payment:Base
    {
        public double Amount { get; set; } = 0.0;
        public DateTime PaidAt { get; set; }=DateTime.Now;
        public bool Status { get; set; }= false;
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.VesaPay;
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
