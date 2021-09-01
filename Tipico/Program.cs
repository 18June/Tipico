using System;
using System.Threading.Tasks;

namespace TipicoSite
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            TipicoSite test = new TipicoSite();
            await test.Start();
        }
    }
}
