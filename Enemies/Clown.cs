using Labyrinths.Core;
using System;

namespace Labyrinths.Enemies
{
    public class Clown : Enemy
    {
        public Clown(int id)
                : base()
        {
            CharacterName = "Clown";
            Name = CharacterName + " " + id;
            Rank = EnemyRank.Rank2;
            HealthMeter = new HealthMeter(166, 166);
            Stats = new Stats(1, 2, 2, 2);
            Type = EntityType.Enemy;
            IQ = 3;
        }
    }
}
