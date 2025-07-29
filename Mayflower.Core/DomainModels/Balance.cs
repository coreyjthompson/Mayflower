using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayflower.Core.DomainModels
{
    public class Balance
    {
        public decimal Amount { get; set; }

        public DateTime AsOf { get; set; }
    }
}
