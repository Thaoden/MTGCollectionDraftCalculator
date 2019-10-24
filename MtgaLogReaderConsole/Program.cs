using System;
using System.Threading.Tasks;

namespace MtgaLogReaderConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var inventory = await new MtgaLogParser.MtgaLogHelper().GetPlayerInventory();

            Console.WriteLine("Hello World!");
        }
    }
}
