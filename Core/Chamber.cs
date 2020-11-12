using Labyrinths.Enemies;
using Labyrinths.CLI;
using System;
using System.Collections.Generic;

namespace Labyrinths.Core
{
    public class Chamber
    {
        public int Id { get; set; }
        private Level Level { get; set; }
        public WalkDirection CameFrom { get; set; }

        public List<Entity> Entities { get; set; }

        public Chamber(int id, WalkDirection cameFrom, Level level, List<Entity> entities)
        {
            Id = id;
            CameFrom = cameFrom;
            Level = level;
            Entities = entities;
            Greet();
            foreach (var enemy in GenerateEnemies())
            {
                Entities.Add(enemy);
            }

            foreach (var entity in Entities)
            {
                entity.OnEntityKilled += OnEntityKilled;
            }
        }

        public string GetPossibleDirections()
        {
            string possibleDirections = string.Empty;
            var directionsArray = Enum.GetNames(typeof(WalkDirection));
            int index = (int)CameFrom - 1;
            index = index == 0 ? 2 : index;
            directionsArray.SetValue("", index);

            int count = 0;
            
            int length = directionsArray.Length;
            int i = index;
            do
            {
                if (i >= length - 1)
                {
                    i = -1;
                }
                i++;
                if (directionsArray[i] != "")
                {
                    possibleDirections += directionsArray[i] + " | ";
                }
                count++;
            }
            while (count < length);

            possibleDirections = possibleDirections.Remove(possibleDirections.Length - 3);
            possibleDirections = CameFrom == WalkDirection.North ? possibleDirections.Replace("North", "South") : possibleDirections;
            return possibleDirections;
        }
        private List<Enemy> GenerateEnemies()
        {
            return EnemyFactory.GenerateEnemies(Level.Difficulty);
        }

        public void Greet()
        {
            if (Level.Id < 4)
            {
                ConsolePrinter.PrintMessage("Storey " + Level.Id + ", Chamber " + Id + ".", true);
            }
        }

        private void OnEntityKilled(Entity entity)
        {
            Entities.Remove(entity);
        }
    }
}
