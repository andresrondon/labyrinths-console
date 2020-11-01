using System;
using Labyrinths.Engine;

namespace Labyrinths.Enemies
{
    public class Slime : Enemy
    {
        public Slime(int id)
                :base()
        {
            CharacterName = "Slime";
            Name = CharacterName + " " + id;
            Rank = EnemyRank.Rank1;
            HealthMeter = new HealthMeter(70, 70);
            Stats = new Stats(0.6f, 2, 2, 2);
            Type = EntityType.Enemy;
            IQ = 1;
        }

        override public void Spawn()
        {
            var message = String.Format("'{0}' spawned!", Name);
            _printer.PrintMessage(message, false);
        }
    }
}
