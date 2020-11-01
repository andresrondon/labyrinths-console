using System;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinths.Core
{
    public abstract class GameEngine
    {
        protected string gameName = "Labyrinths: Black Gate";
        protected LevelManager levelSystem;

        protected Hero Player { get; set; }
        protected List<Entity> Entities { get; set; }

        abstract protected void StartGame(float difficultyFactor);

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
                enemies = GetEnemies();
            }
            else
            {
                enemies = GetEnemies().Where(e => e != enemyBeingAttacked);
            }
            var heroes = GetHeroes();
            Random rand = new Random();

            foreach (var enemy in enemies)
            {
                var count = heroes.Count();
                if (count < 1)
                    break;
                enemy.Attack(heroes.ElementAt(rand.Next(0, count - 1)));
                if (count != heroes.Count())
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
            CheckLoose();
            CheckChamberCleared();
        }

        private void CheckLoose()
        {
            if (!HasEntity(EntityType.Hero))
            {
                Loose();
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

        private bool HasEntity(EntityType entityType)
        {
            return Entities.Any(e => e.Type == entityType);
        }

        protected IEnumerable<Hero> GetHeroes()
        {
            return Entities.Where(e => e.Type == EntityType.Hero).Select(e => e as Hero);
        }

        protected IEnumerable<Enemy> GetEnemies()
        {
            return Entities.Where(e => e.Type == EntityType.Enemy).Select(e => e as Enemy);
        }

        protected Enemy GetEnemy(string name)
        {
            return Entities.Find(e => e.Name.ToLower() == name) as Enemy;
        }

        abstract protected void WinGame();
        abstract protected void Loose();
        abstract protected void ChamberCleared();
    }
}
