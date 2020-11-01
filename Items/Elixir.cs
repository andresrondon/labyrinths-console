using Labyrinths.Core;
using Labyrinths.UI;

namespace Labyrinths.Items
{
    class Elixir : IItem
    {
        public Elixir()
        {
            Name = "Elixir";
            Value = 1;
        }
        public string Name { get; set; }
        public int Value { get; set; }
        public void Use(Entity entity)
        {
            entity.Stats.Stamina += Value;
            ConsolePrinter.PrintMessage(entity.Name + " used " + this.Name + ". Stamina +" + Value, false);
        }
    }
}