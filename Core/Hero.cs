using Labyrinths.CLI;

namespace Labyrinths.Core
{
    abstract public class Hero : Entity
    {
        override public void ReceiveDamage(Entity entity)
        {
            var message = string.Format("{0} attacked {1}.", entity.Name, this.Name);

            string postMessage;
            if (!this.IsDefending)
            {
                var value = entity.Damage() - (this.Stats.Defense * 2);
                postMessage = " -" + value;
                postMessage += value >= (this.HealthMeter.MaxHealth / 2) ? " CRITICAL" : "";
                this.HealthMeter.ReceiveDamage(value);
            }
            else
            {
                postMessage = "..but " + this.Name + " defended.";

                this.HealthMeter.ReceiveDamage(0);
            }

            ConsolePrinter.PrintMessage(message + postMessage, false);

            if (CheckDeath())
            {
                Die(entity);
            }
        }
        public override void Die(Entity byEntity)
        {
            ConsolePrinter.PrintAction(byEntity, "killed", this, true);
            OnEntityKilled.Invoke(this);
        }
    }
}
