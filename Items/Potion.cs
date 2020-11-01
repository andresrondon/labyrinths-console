using Labyrinths.Core;
using Labyrinths.UI;

namespace Labyrinths.Items
{
    class Potion : IItem
    {
        public Potion()
        {
            Name = "Potion";
            Value = 50;
        }
        public Potion(int value)
        {
            Name = "Potion";
            Value = value;
        }
        public string Name { get; set; }
        public int Value { get; set; }

        public void Use(Entity entity)
        {
            entity.HealthMeter.RestoreHealth(Value);
            ConsolePrinter.PrintMessage(entity.Name + " used " + this.Name + ". Health +" + Value, false);
        }
    }
}
