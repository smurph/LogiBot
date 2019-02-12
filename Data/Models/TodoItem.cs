using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        public int Rank { get; set; }

        public long? ChannelId { get; set; }

        public string Description { get; set; }

        public string RequestingUser { get; set; }

        public string HandlingUser { get; set; }

        public bool Active { get; set; }

        public override string ToString()
        {
            var s = $"• #{Id}: {Description}";

            if (!string.IsNullOrEmpty(HandlingUser))
            {
                s += $" - Being handled by {HandlingUser}.";
            }
            return s;
        }
    }
}
