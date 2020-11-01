using Labyrinths.Core;

namespace Labyrinths.Heroes
{
    public class Gabriel : Hero
    {
        public Gabriel(string playerName)
            : base()
        {
            CharacterName = "Gabriel Mercedes";
            Name = playerName;
            HealthMeter = new HealthMeter(100, 100);
            Stats = new Stats(5, 4, 3, 3);
            Type = EntityType.Hero;
        }
    }
}
