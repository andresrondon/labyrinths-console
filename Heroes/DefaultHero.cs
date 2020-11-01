using Labyrinths.Engine;

namespace Labyrinths.Heroes
{
    class DefaultHero : Hero
    {
        public DefaultHero()
            :base()
        {
            CharacterName = "";
            Name = "";
            HealthMeter = new HealthMeter(0, 0);
            Stats = new Stats(0, 0, 0, 0);
            Type = EntityType.Hero;
        }
    }
}
