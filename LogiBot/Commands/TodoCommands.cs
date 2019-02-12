using Data.Contexts;
using Data.Models;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogiBot.Commands
{
    public class TodoCommands : ModuleBase<SocketCommandContext>
    {
        [Command("todo")]
        public async Task ListTodoItemsInChannel()
        {
            var todoList = new List<TodoItem>();

            using (var db = new LogiContext())
            {
                todoList = db.TodoItems.Where(item => item.Active == true).ToList();
            }

            var message = new StringBuilder();
            
            if (!todoList.Any())
            {
                message.Append("Looks like there's nothing to do!");
            }
            else
            {
                message.Append("Todo Items from this channel:\n");
            }

            foreach (var item in todoList)
            {
                message.AppendLine(item.ToString());
            }

            await Context.Channel.SendMessageAsync(message.ToString());
        }

        [Command("todo")]
        public async Task AddTodoItem(params string[] input)
        {
            using (var db = new LogiContext())
            {
                db.TodoItems.Add(new TodoItem()
                {
                    Active = true,
                    Description = string.Join(" ", input),
                    RequestingUser = Context.User.Username,
                    ChannelId = (long)Context.Channel.Id
                });

                db.SaveChanges();
            }

            var message = "Added a new todo item!";

            await Context.Channel.SendMessageAsync(message.ToString());
        }

        [Command("handle-todo")]
        public async Task HandleTodoItem(int id)
        {
            TodoItem todoItem = null;
            using (var db = new LogiContext())
            {
                todoItem = db.TodoItems.Find(id);
                if (todoItem == null)
                {
                    await Context.Channel.SendMessageAsync("Todo Item Not Found");
                }
                else
                {
                    todoItem.HandlingUser = Context.User.Username;
                    db.SaveChanges();

                    await Context.Channel.SendMessageAsync("Thanks for handling that!");
                }
            }
        }

        [Command("unhandle-todo")]
        public async Task UnhandleTodoItem(int id)
        {
            TodoItem todoItem = null;
            using (var db = new LogiContext())
            {
                todoItem = db.TodoItems.Find(id);
                if (todoItem == null)
                {
                    await Context.Channel.SendMessageAsync("Todo Item Not Found");
                }
                else if (todoItem.HandlingUser == Context.User.Username)
                {
                    todoItem.HandlingUser = null;
                    db.SaveChanges();

                    await Context.Channel.SendMessageAsync("You're free to go.");
                }
                else
                {
                    await Context.Channel.SendMessageAsync("You don't seem to be handling this");
                }
            }
        }

        [Command("remove-todo")]
        public async Task RemoveTodoItem(int id)
        {
            TodoItem todoItem = null;
            using (var db = new LogiContext())
            {
                todoItem = db.TodoItems.Find(id);

                if (todoItem == null)
                {
                    await Context.Channel.SendMessageAsync("Todo Item Not Found");
                }
                else 
                {
                    todoItem.Active = false;
                    db.SaveChanges();
                    await Context.Channel.SendMessageAsync($"Todo #{id} removed.");
                }
                
            }
        }

        [Command("todo-help")]
        public async Task TodoHelp()
        {
            var message = new StringBuilder();
            message.AppendLine("Hello, I'm LogiBot!\n");
            message.AppendLine("`~todo-help` - This menu");
            message.AppendLine("`~todo` - List all active todo items");
            message.AppendLine("`~todo {description}` - Adds a todo item with the provided description");
            message.AppendLine("`~handle-todo {Id#}` - Let others know you're handling an item");
            message.AppendLine("`~unhandle-todo {Id#}` - Let others know you're no longer handling an item");
            message.AppendLine("`~remove-todo` - Sets a todo item as inactive, and it will no longer show up in the list");

            await Context.Channel.SendMessageAsync(message.ToString());
        }
    }
}
