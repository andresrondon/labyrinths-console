using Labyrinths.Engine;

namespace Labyrinths.Heroes
{
    class Lea : Hero
    {
        public Lea(string playerName)
            :base()
        {
            CharacterName = "Lea Westbrook";
            Name = playerName;
            HealthMeter = new HealthMeter(100, 100);
            Stats = new Stats(3, 5, 4, 4);
            Type = EntityType.Hero;
        }
    }
}
