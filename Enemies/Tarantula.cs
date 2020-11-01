using Labyrinths.Engine;
using System;

namespace Labyrinths.Enemies
{
    public class Tarantula : Enemy
    {
        public Tarantula(int id)
                : base()
        {
            CharacterName = "Tarantula";
            Name = CharacterName + " " + id;
            Rank = EnemyRank.Rank3;
            HealthMeter = new HealthMeter(170, 170);
            Stats = new Stats(1.1f, 2, 2, 2);
            Type = EntityType.Enemy;
            IQ = 4;
        }

        override public void Spawn()
        {
            var message = String.Format("'{0}' spawned!", Name);
            _printer.PrintMessage(message, false);
        }
    }
}