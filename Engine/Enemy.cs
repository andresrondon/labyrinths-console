using Labyrinths.Engine.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Colorful;
using System.Drawing;
using Labyrinths.Items;

namespace Labyrinths.Engine
{
    abstract public class Enemy : Entity, IEnemyAI
    {
        public EnemyRank Rank { get; set; }
        public int IQ { get; set; }

        public event BeingAttackedEvent BeingAttacked;

        public Enemy ()
            :base()
        {
            BeingAttacked += DecideAction;
        }

        override public void Walk (WalkDirection direction)
        {

        }

        abstract public void Spawn();

        public void DecideAction(Hero heroToAttack)
        {
            int intelligenceFactor = 15 + IQ;
            int chanceFactor = 0;

            Random rand = new Random();
            chanceFactor = rand.Next(1, intelligenceFactor + 1);

            // Common attack
            if (chanceFactor >= 1 && chanceFactor <= 11)
            {
                Attack(heroToAttack);
            }
            // Defend
            else if (chanceFactor >= 12 && chanceFactor <= 15)
            {
                Defend();
                // Will defend on next turn
                Attack(heroToAttack);
            }
            // Special Attack
            else if (chanceFactor >= 16 && chanceFactor <= 18)
            {
                SpecialAttack(heroToAttack);
            }
            // Use Potion
            else if (chanceFactor >= 19)
            {
                UseItem(new Potion(65));
            }
        }

        public override void ReceiveDamage(Entity entity)
        {
            //_printer.PrintAction(entity, "attacked", this, false);
            var message = String.Format("{0} attacked {1}.", entity.Name, this.Name);

            string postMessage = "";
            if (!this.IsDefending)
            {
                var value = entity.Damage() - (this.Stats.Defense * 2);
                postMessage = " -" + value;
                postMessage += value >= (this.HealthMeter.MaxHealth / 2) ? " CRITICAL" : "";
                _printer.PrintMessage(message + postMessage, false);
                this.HealthMeter.ReceiveDamage(value);

                if (CheckDeath())
                {
                    Death(entity);
                }
                else
                {
                    BeingAttacked(entity as Hero);
                }
            }
            else
            {
                postMessage = "..but " + this.Name + " defended.";
                _printer.PrintMessage(message + postMessage, false);

                this.HealthMeter.ReceiveDamage(0);
                IsDefending = false;
            }
        }

        public override void Death(Entity byEntity)
        {
            _printer.PrintAction(byEntity, "killed", this, true);
            EntityKilled(this);

            // Drop items  (give the player Potions and stuff.)
            Random rand = new Random();

            var luckFactor = rand.Next(1, 4);
            if (luckFactor == 1)
            {
                var item = GenerateItem();
                byEntity.ItemBag.Add(item);
                _printer.PrintMessage(byEntity.Name + " gadered an item '" + item.Name + "' from " + this.Name + "!", false);
            }
        }

        private IItem GenerateItem()
        {
            Random rand = new Random();
            var itemTypes = new List<Type>
            {
                typeof(Potion),
                typeof(Potion),
                typeof(Potion),
                typeof(Elixir),
                typeof(Elixir)
            };

            var item = (IItem)Activator.CreateInstance(itemTypes.ElementAt(rand.Next(0, itemTypes.Count)));
            return item;
        }
    }
}
