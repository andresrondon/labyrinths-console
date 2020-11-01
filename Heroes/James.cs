using Labyrinths.Core;

namespace Labyrinths.Heroes
{
    public class James : Hero
    {
        public James(string playerName)
            :base()
        {
            CharacterName = "James Arias";
            Name = playerName;
            HealthMeter = new HealthMeter(100, 100);
            Stats = new Stats(4, 3, 5, 5);
            Type = EntityType.Hero;
        }
    }
}
