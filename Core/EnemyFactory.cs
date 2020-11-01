using System;
using System.Collections.Generic;
using Labyrinths.Enemies;

namespace Labyrinths.Core
{
    static public class EnemyFactory
    {
        // The higher, the softer. The lower, the harder.
        static readonly float difficultyAdjuster = 1.03f;

        // The higher, the harder. The lower, the softer.
        static readonly int baseDifficulty = 170;

        const int maxEnemiesInChamber = 5;

        // Enemy counters
        static Dictionary<Type, int> enemyCount;

        public static List<Enemy> GenerateEnemies(float difficulty)
        {
            enemyCount = new Dictionary<Type, int>();

            var maxHpCombined = ((1 / difficulty) + difficulty - 1) * baseDifficulty / difficultyAdjuster;

            var enemies = new List<Enemy>();

            var enemyTypes = GetEnemyTypes();
            
            if (difficulty > 3.2)
            {
                var fenriswolf = GetEnemyByType(typeof(Fenriswolf));
                enemies.Add(fenriswolf);
                fenriswolf.Spawn();
                return enemies;
            }

            Random rand = new Random();
            var hPSum = 0f;
            for (int i = 0; i < maxEnemiesInChamber; i++)
            {
                Enemy enemy = GetEnemyByType(enemyTypes[rand.Next(0, enemyTypes.Count - 1)]);
                hPSum += enemy.HealthMeter.Health;
                if (hPSum >= maxHpCombined)
                {
                    hPSum -= enemy.HealthMeter.Health;
                    continue;
                }
                else
                {
                    enemies.Add(enemy);
                    enemy.Spawn();
                }

                if (i == maxEnemiesInChamber - 1 && enemies.Count < 1)
                    i = -1;
            }
            
            return enemies;
        }

        static private Enemy GetEnemyByType(Type type)
        {
            enemyCount[type] += 1;
            var enemy = (Enemy)Activator.CreateInstance(type, enemyCount[type]);

            return enemy;
        }

        static private List<Type> GetEnemyTypes()
        {
            var enemyTypes = new List<Type>
            {
                typeof(Slime),
                typeof(Bat),
                typeof(Clown),
                typeof(Tarantula),
                typeof(Fenriswolf)
            };

            foreach (var type in enemyTypes)
            {
                enemyCount.Add(type, 0);
            }

            return enemyTypes;
        }
    }
}
