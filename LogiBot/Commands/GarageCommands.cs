using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Contexts;
using Data.Models;

namespace LogiBot.Commands
{
    public class GarageCommands : ModuleBase<SocketCommandContext>
    {
        [Command("stock")]
        public async Task GetStock()
        {
            var ships = new List<Ship>();

            using (var db = new LogiContext())
            {
                ships = db.Ships.ToList();
            }

            var message = new StringBuilder();

            if (!ships.Any())
            {
                message.Append("No ships found");
            }

            foreach(var ship in ships)
            {
                message.AppendLine($"{ship.Id} - {ship.Name} ({ship.Owner})");
            }

            await Context.Channel.SendMessageAsync(message.ToString());
        }

        [Command("add-ship")]
        public async Task AddShip(string name)
        {
            var ship = new Ship()
            {
                Name = name,
                Owner = Context.User.Username
            };

            using (var db = new LogiContext())
            {
                db.Ships.Add(ship);
                db.SaveChanges();
            }

            await Context.Channel.SendMessageAsync($"@{Context.User.Username}, your ship \"{name}\" has been saved!");
        }
    }
}
