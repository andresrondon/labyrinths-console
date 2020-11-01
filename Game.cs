using Labyrinths.Engine;

namespace Labyrinths
{
    class Game
    {
        static void Main(string[] args)
        {
            string playerName = args[0];

            GameEngine Engine = new GameEngine(playerName: playerName);
        }
    }
}
