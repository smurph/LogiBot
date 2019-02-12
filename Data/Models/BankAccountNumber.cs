using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class BankAccountNumber
    {
        public int Id { get; set; }

        public long PlayerId { get; set; }

        public string PlayerName { get; set; }

        public int AccountNumber { get; set; }
    }
}
