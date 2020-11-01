using Labyrinths.UI;
using System.Collections.Generic;

namespace Labyrinths.Engine
{
    abstract public class Entity
    {
        public string CharacterName { get; set; }
        public string Name { get; set; }
        public HealthMeter HealthMeter { get; set; }
        public Stats Stats { get; set; }
        public EntityType Type { get; set; }
        public EntityKilledDelegate EntityKilled;
        public bool IsDefending { get; set; }
        public List<IItem> ItemBag { get; set; }

        public ConsolePrinter _printer;

        public Entity()
        {
            _printer = new ConsolePrinter();
            IsDefending = false;
            ItemBag = new List<IItem>();
        }

        abstract public void ReceiveDamage(Entity entity);

        abstract public void Walk(WalkDirection direction);

        virtual public float Damage()
        {
            float damage = (this.Stats.Power / this.Stats._maxPower) * 100;
            return damage;
        }
        virtual public void Attack(Entity entity)
        {
            entity.ReceiveDamage(this);
        }
        virtual public void ShowStats()
        {
            _printer.PrintStats(CharacterName, Stats);
        }

        virtual public void ShowHealth()
        {
            _printer.PrintHealth(CharacterName, HealthMeter);
        }

        virtual public bool CheckDeath()
        {
            bool value;
            if (this.HealthMeter.Health < 3)
            {
                value = true;
            }
            else
            {
                value = false;
            }
            return value;
        }

        virtual public void UseItem(IItem item)
        {
            item.Use(this);
        }

        abstract public void Death(Entity byEntity);

        virtual public void SpecialAttack(Entity entity)
        {
            if (this.Stats.Stamina >= 1)
            {
                var originalPower = this.Stats.Power;
                this.Stats.Power *= 1.75f;

                _printer.PrintMessage(this.Name + " performed a special attack!", false);
                entity.ReceiveDamage(this);

                this.Stats.Stamina -= 1;
                this.Stats.Power = originalPower;
            }
            else
            {
                _printer.PrintMessage(this.Name + " tried to perform a special attack, but he/she is tired. No stamina left to perform a special attack.", false);
            }
        }

        virtual public void Defend()
        {
            IsDefending = true;
            //_printer.PrintMessage(Name + " defended!", false);
        }
    }
}
