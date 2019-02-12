using Data.Contexts;
using Data.Models;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogiBot.Commands
{
    public class TradeCommands : ModuleBase<SocketCommandContext>
    {
        [Command("bank")]
        public async Task BankHelp()
        {
            var message = new StringBuilder();

            message.AppendLine("I can keep track of everyone's bank Ids and help look them up for you!");

            message.AppendLine("`~bank` if you don't include a number, you get this menu");
            message.AppendLine("`~bank {your bank number}` if you do include a number, I will remember your bank number");
            message.AppendLine("`~find-bank {username}` will attempt to find someone's bank Id. Can be a partial username, is not case sensitive");
            
            await Context.Channel.SendMessageAsync(message.ToString());
        }

        [Command("find-bank")]
        public async Task FindBank(string searchTerm)
        {
            BankAccountNumber acct = null;
            var message = "";

            using (var db = new LogiContext())
            {
                acct = db.BankAccounts.Where(a => a.PlayerName.ToLower().Contains(searchTerm.ToLower())).FirstOrDefault();

                if (acct == null && Context.Message.MentionedUsers.Any())
                {
                    searchTerm = Context.Message.MentionedUsers.ToList()[0].ToString();
                    acct = db.BankAccounts.Where(a => a.PlayerName.ToLower().Contains(searchTerm.ToLower())).FirstOrDefault();
                }
            }

            if (acct == null)
            {
                message = "Sorry, I couldn't find anyone by that name";
            }
            else
            {
                message = $"{acct.PlayerName}'s bank account number is **{acct.AccountNumber}**";
            }

            await Context.Channel.SendMessageAsync(message);
        }

        [Command("bank")]
        public async Task BankUpdate(int number)
        {
            BankAccountNumber acct = null;

            using (var db = new LogiContext())
            {
                acct = db.BankAccounts.Where(a => a.PlayerId == (long)Context.User.Id).FirstOrDefault();

                string message = "";
                if (acct == null)
                {
                    acct = new BankAccountNumber()
                    {
                        PlayerId = (long)Context.User.Id,
                        PlayerName = Context.User.ToString(),
                        AccountNumber = number
                    };

                    db.BankAccounts.Add(acct);
                    db.SaveChanges();
                    message = "Bank account info stored!";
                }
                else
                {
                    acct.AccountNumber = number;

                    db.SaveChanges();
                    message = "Bank account info updated!";
                }

                await Context.Channel.SendMessageAsync(message);
            }
        }
    }
}
