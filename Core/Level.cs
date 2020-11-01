using System;
using System.Collections.Generic;
using System.Linq;
using Labyrinths.Enemies;
using Labyrinths.UI;

namespace Labyrinths.Core
{
    public class Level
    {
        public int Id { get; set; }
        public float Difficulty { get; set; }
        public Chamber CurrentChamber { get; set; }
        
        public Level(int id, float difficulty, WalkDirection cameFrom, List<Entity> entities)
        {
            Id = id;
            Difficulty = difficulty;
            CurrentChamber = new Chamber(1, cameFrom, this, entities);
        }
    }
}