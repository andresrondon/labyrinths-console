namespace Labyrinths.Core
{
    public class HealthMeter
    {
        public float Health { get; set; }
        public float MaxHealth { get; set; }

        public HealthMeter (float health, float maxHealth)
        {
            this.Health = health;
            this.MaxHealth = maxHealth;
        }

        public void ReceiveDamage (float value)
        {
            this.Health -= value;
        }

        public void RestoreHealth (float value)
        {
            this.Health += value;
            this.Health = Health > MaxHealth ? MaxHealth : Health;
        }
    }
}
