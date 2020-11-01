using Labyrinths.Core.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using Labyrinths.Items;
using Labyrinths.UI;

namespace Labyrinths.Core
{
    abstract public class Enemy : Entity, IEnemyAI
    {
        public EnemyRank Rank { get; set; }
        public int IQ { get; set; }

        public event BeingAttackedEvent OnBeingAttacked;

        public Enemy ()
            :base()
        {
            OnBeingAttacked += DecideAction;
        }

        public void DecideAction(Hero heroToAttack)
        {
            int intelligenceFactor = 15 + IQ;
            Random rand = new Random();
            int chanceFactor = rand.Next(1, intelligenceFactor + 1);

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
            var message = string.Format("{0} attacked {1}.", entity.Name, Name);

            string postMessage;
            if (!IsDefending)
            {
                var value = (int)entity.Damage() - (Stats.Defense * 2);
                postMessage = " -" + value;
                postMessage += value >= (HealthMeter.MaxHealth / 2) ? " CRITICAL" : "";
                ConsolePrinter.PrintMessage(message + postMessage, false);
                HealthMeter.ReceiveDamage(value);

                if (CheckDeath())
                {
                    Die(entity);
                }
                else
                {
                    OnBeingAttacked.Invoke(entity as Hero);
                }
            }
            else
            {
                postMessage = "..but " + Name + " defended.";
                ConsolePrinter.PrintMessage(message + postMessage, false);

                HealthMeter.ReceiveDamage(0);
                IsDefending = false;
            }
        }

        public override void Die(Entity byEntity)
        {
            ConsolePrinter.PrintAction(byEntity, "killed", this, true);
            OnEntityKilled(this);

            // Drop items  (give the player Potions and stuff.)
            Random rand = new Random();

            var luckFactor = rand.Next(1, 4);
            if (luckFactor == 1)
            {
                var item = GenerateItem();
                byEntity.ItemBag.Add(item);
                ConsolePrinter.PrintMessage(byEntity.Name + " gadered an item '" + item.Name + "' from " + Name + "!", false);
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

        virtual public void Spawn()
        {
            var message = string.Format("'{0}' spawned!", Name);
            ConsolePrinter.PrintMessage(message, false);
        }
    }
}
