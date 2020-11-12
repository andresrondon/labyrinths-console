using Labyrinths.CLI;
using System.Collections.Generic;

namespace Labyrinths.Core
{
    abstract public class Entity
    {
        public string CharacterName { get; set; }
        public string Name { get; set; }
        public HealthMeter HealthMeter { get; set; }
        public Stats Stats { get; set; }
        public EntityType Type { get; set; }
        public bool IsDefending { get; set; }
        public List<IItem> ItemBag { get; set; }

        public EntityKilledDelegate OnEntityKilled;

        public Entity()
        {
            ItemBag = new List<IItem>();
        }

        abstract public void ReceiveDamage(Entity entity);

        virtual public float Damage()
        {
            float damage = (Stats.Power / Stats._maxPower) * 100;
            return damage;
        }
        virtual public void Attack(Entity entity)
        {
            entity.ReceiveDamage(this);
        }

        virtual public bool CheckDeath()
        {
            return HealthMeter.Health < 3;
        }

        virtual public void UseItem(IItem item)
        {
            item.Use(this);
        }

        abstract public void Die(Entity byEntity);

        virtual public void SpecialAttack(Entity entity)
        {
            if (Stats.Stamina >= 1)
            {
                var originalPower = Stats.Power;
                Stats.Power *= 1.75f;

                ConsolePrinter.PrintMessage(Name + " performed a special attack!", false);
                entity.ReceiveDamage(this);

                Stats.Stamina -= 1;
                Stats.Power = originalPower;
            }
            else
            {
                ConsolePrinter.PrintMessage(Name + " tried to perform a special attack, but he/she is tired. No stamina left to perform a special attack.", false);
            }
        }

        public void Defend()
        {
            IsDefending = true;
        }
    }
}
