using Labyrinths.UI;

namespace Labyrinths
{
    class Game
    {
        static void Main(string[] args)
        {
            string playerName = args[0];

            TextBasedEngine Engine = new TextBasedEngine(playerName);
        }
    }
}
