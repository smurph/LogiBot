namespace LogiBot
{
    class Program
    {
        static void Main(string[] args)
        {
            LogiBot.GetInstance().StartAsync().GetAwaiter().GetResult();
        }
    }
}
