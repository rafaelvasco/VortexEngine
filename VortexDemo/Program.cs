using VortexCore;

namespace VortexDemo
{
    class Program
    {
        static void Main()
        {
            using var game = new Game();
            game.Start(new DemoScene());
        }
    }
}
