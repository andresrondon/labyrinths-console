using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinths.Core
{
    public class Stats
    {
        public Stats (float power, float speed, float stamina, float defense)
        {
            this.Power = power;
            this.Speed = speed;
            this.Stamina = stamina;
            this.Defense = defense;
        }

        public float _maxPower = 5;
        public float _maxSpeed = 5;
        public float _maxDefense = 5;

        public float Power { get; set; }
        public float Speed { get; set; }
        public float Defense { get; set; }
        public float Stamina { get; set; }

    }
}
