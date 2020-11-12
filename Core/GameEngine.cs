using System;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinths.Core
{
    public abstract class GameEngine
    {
        protected readonly string gameName = "Labyrinths: Black Gate";
        protected LevelManager levelSystem;

        protected Hero Player { get; set; }
        protected List<Entity> Entities { get; set; }

        protected IEnumerable<Hero> Heroes => Entities.Where(e => e.Type == EntityType.Hero).Select(e => e as Hero);
        protected IEnumerable<Enemy> Enemies => Entities.Where(e => e.Type == EntityType.Enemy).Select(e => e as Enemy);
        protected Enemy GetEnemy(string name) => Entities.Find(e => e.Name.ToLower() == name) as Enemy;
        private bool HasEntity(EntityType entityType) => Entities.Any(e => e.Type == entityType);

        protected void AttackEnemy(Enemy enemy)
        {
            Player.Attack(enemy);
            AttackHeroes(enemy);
        }

        protected void AttackEnemySP(Enemy enemy)
        {
            Player.SpecialAttack(enemy);
            AttackHeroes(enemy);
        }

        protected void UseItem(IItem item, Hero hero)
        {
            hero.UseItem(item);
            hero.ItemBag.Remove(item);
            AttackHeroes(null);
        }

        private void AttackHeroes(Enemy enemyBeingAttacked)
        {
            IEnumerable<Entity> enemies;
            if (enemyBeingAttacked == null)
            {
                enemies = Enemies;
            }
            else
            {
                enemies = Enemies.Where(e => e != enemyBeingAttacked);
            }

            Random rand = new Random();
            foreach (var enemy in enemies)
            {
                var count = Heroes.Count();
                if (count < 1)
                    break;
                enemy.Attack(Heroes.ElementAt(rand.Next(0, count - 1)));
                if (count != Heroes.Count())
                    break;
            }
        }

        protected void Defend()
        {
            Player.Defend();
            AttackHeroes(null);
            Player.IsDefending = false;
        }

        protected void CheckGameStatus()
        {
            CheckLose();
            CheckChamberCleared();
        }

        private void CheckLose()
        {
            if (!HasEntity(EntityType.Hero))
            {
                Lose();
            }
        }

        private void CheckChamberCleared()
        {
            if (!HasEntity(EntityType.Enemy))
            {
                if (levelSystem.CurrentLevel.Id < 4)
                {
                    ChamberCleared();
                }
                else
                {
                    WinGame();
                }
            }
        }

        abstract protected void StartGame(float difficultyFactor);
        abstract protected void WinGame();
        abstract protected void Lose();
        abstract protected void ChamberCleared();
    }
}
