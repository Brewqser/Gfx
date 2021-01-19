using System;

namespace EMBC
{
    internal class EntryPoint
    {
        [STAThread]
        private static void Main() => new Client.Program().Run();
    }
}