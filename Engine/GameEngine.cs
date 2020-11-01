using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Labyrinths.UI;
using Labyrinths.Utils;

namespace Labyrinths.Engine
{
    public class GameEngine : CommandEngine
    {
        private string GameName = "Labyrinths: Black Gate";
        LevelSystem levelSystem;
        
        override public List<Entity> Entities { get; set; }

        override public string PlayerName { get; set; }

        public GameEngine (string playerName)
        {
            // Game Configuration.
            Console.Clear();
            PlayerName = playerName;
            Console.WindowHeight = 32;
            Console.Title = GameName;
            Console.OutputEncoding = Encoding.UTF8;

            // Start screen.
            _printer.PrintStartScreen();
            _printer.PrintMessage("Hello " + PlayerName, false);
            _printer.PrintMessage("Welcome to " + GameName + ".", false);

            Entities = new List<Entity>
            {
                this.SelectCharacterCommand(PlayerName)
            };
            Console.Clear();
            this.StartGame(1);
        }

        private void StartGame(float difficultyFactor)
        {
            levelSystem = new LevelSystem(difficultyFactor, Entities);

            this.ReadCommand();
        }

        override public void AttackEnemy(Enemy enemy)
        {
            var hero = Entities.Where(e => e.Name == PlayerName).ElementAt(0);
            hero.Attack(enemy);
            AttackHeroes(enemy);
        }

        override public void AttackEnemySP(Enemy enemy)
        {
            var hero = Entities.Where(e => e.Name == PlayerName).ElementAt(0);
            hero.SpecialAttack(enemy);
            AttackHeroes(enemy);
        }

        override public void UseItem(IItem item, Hero hero)
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
                enemies = Entities.Where(e => e.Type == EntityType.Enemy);
            }
            else
            {
                enemies = Entities.Where(e => e.Type == EntityType.Enemy && e != enemyBeingAttacked);
            }
            var heroes = Entities.Where(e => e.Type == EntityType.Hero);
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



        override public void DefendCommand()
        {
            var hero = Entities.Where(e => e.Name == PlayerName).ElementAt(0);
            hero.Defend();
            AttackHeroes(null);
            hero.IsDefending = false;
        }

        override public void CheckGameStatus()
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

        private void Loose()
        {
            Console.Clear();
            _printer.PrintMessage("Thou art dead!", true);
            _printer.PrintMessage("Thyself have been lost wandering the labyrinth.", false);
            Console.ReadLine();
            Environment.Exit(0);
        }

        public void ChamberCleared()
        {
            _printer.PrintMessage("Thou eliminated the monsters in this chamber!", false);
            _printer.PrintMessage("Thou shall now proceed to the next chamber in the direction you choose.", false);
            _printer.PrintMessage(levelSystem.CurrentLevel.CurrentChamber.GetPossibleDirections(), false);
            _printer.PrintHUD(Entities, PlayerName);

            var speed = Entities.Where(e => e.Name == PlayerName).FirstOrDefault().Stats.Speed;

            levelSystem.MoveToChamber(MoveToChamberCommand(Console.ReadLine(), levelSystem.CurrentLevel.CurrentChamber.CameFrom), speed);
        }
        private void WinGame()
        {
            Console.Clear();
            _printer.PrintMessage("Congratulations, thou have defeated Fenriswolf!", true);
            _printer.PrintMessage("Thou have conquered the Labyrinth.", false);
            _printer.PrintMessage("Press enter to exit.", false);
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
