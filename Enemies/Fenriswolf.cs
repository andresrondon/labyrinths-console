using Labyrinths.Core;
using Labyrinths.CLI;

namespace Labyrinths.Enemies
{
    public class Fenriswolf : Enemy
    {
        public Fenriswolf(int id)
                : base()
        {
            CharacterName = "Fenriswolf";
            Name = CharacterName;
            Rank = EnemyRank.Rank3;
            HealthMeter = new HealthMeter(280, 280);
            Stats = new Stats(1.2f, 4, 3, 5);
            Type = EntityType.Enemy;
            IQ = 5;
            //ItemBag.Add(new Potion());
        }

        override public void Spawn()
        {
            var message = "You have riched the final boss!\r\nBehold the great Fenriswolf, son of Loki.";
            ConsolePrinter.PrintMessage(message, true);
        }
    }
}