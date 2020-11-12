using Labyrinths.CLI;

namespace Labyrinths
{
    class Game
    {
        static void Main(string[] args)
        {
            new TextBasedEngine(playerName: args[0]);
        }
    }
}
