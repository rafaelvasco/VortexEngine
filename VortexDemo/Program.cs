using VortexCore;

namespace VortexDemo
{
    class Program
    {
        static void Main()
        {
            using var game = new DemoGame();
            game.Start();
        }
    }
}
