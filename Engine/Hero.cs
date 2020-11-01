using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinths.Engine
{
    abstract public class Hero : Entity
    {
        public Hero ()
            : base()
        {
        }
        override public void Walk(WalkDirection direction)
        {
            // ..
        }

        override public void ReceiveDamage(Entity entity)
        {
            //_printer.PrintAction(entity, "attacked", this, false);
            var message = String.Format("{0} attacked {1}.", entity.Name, this.Name);

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

            _printer.PrintMessage(message + postMessage, false);

            if (CheckDeath())
            {
                Death(entity);
            }
        }
        public override void Death(Entity byEntity)
        {
            _printer.PrintAction(byEntity, "killed", this, true);
            EntityKilled(this);
        }
    }
}
