using Labyrinths.Engine;
using System;

namespace Labyrinths.Enemies
{
    public class Bat : Enemy
    {
        public Bat(int id)
                : base()
        {
            CharacterName = "Bat";
            Name = CharacterName + " " + id;
            Rank = EnemyRank.Rank1;
            HealthMeter = new HealthMeter(95, 95);
            Stats = new Stats(0.8f, 2, 2, 4);
            Type = EntityType.Enemy;
            IQ = 2;
        }

        override public void Spawn()
        {
            var message = String.Format("'{0}' spawned!", Name);
            _printer.PrintMessage(message, false);
        }
    }
}
