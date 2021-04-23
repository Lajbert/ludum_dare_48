using System;

namespace lurum_dare_48
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new LD48Game())
                game.Run();
        }
    }
}
