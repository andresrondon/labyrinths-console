using Labyrinths.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using Labyrinths.Engine;

namespace Labyrinths.UI
{
    abstract public class CommandEngine
    {
        abstract public List<Entity> Entities { get; set; }

        public ConsolePrinter _printer;

        abstract public string PlayerName { get; set; }
        public CommandEngine()
        {
            _printer = new ConsolePrinter();
        }
        virtual public void ReadCommand()
        {
            CheckGameStatus();

            _printer.PrintMessage("Please enter a command.", false);

            _printer.PrintHUD(Entities, PlayerName);

            string command = Console.ReadLine();
            command = command.ToLower();
            
            var commandArray = command.Split(':');

            var primaryCommand = commandArray[0].Trim();
            var parameters = commandArray.Skip(1).ToArray();

            this.DoCommand(primaryCommand, parameters);
        }

        private void DoCommand(string command, params string[] parameters)
        {
            if (parameters.Length > 0 &&  parameters[0] != null)
                parameters[0] = parameters[0].Trim().ToLower();

            if (command == "atk")
            {
                this.AttackCommand(parameters[0]);
            }
            else if (command == "dfd")
            {
                this.DefendCommand();
            }
            else if (command == "spc")
            {
                this.SpecialAttackCommand(parameters[0]);
            }
            else if (command == "itm")
            {
                this.UseItemCommand(parameters[0]);
            }
            else if (command == "hlt")
            {
                this.ShowHealthCommand(parameters[0]);
            }
            else if (command == "sts")
            {
                this.ShowStatsCommand(parameters[0]);
            }
            else if (command == "exit")
            {
                Console.Clear();
                Environment.Exit(0);
            }
            else
            {
                _printer.PrintMessage("'" + command + "' is not a valid command.", false);
            }
            this.ReadCommand();
        }

        virtual public Hero SelectCharacterCommand(string playerName)
        {
            _printer.PrintMessage("Please select a character with whom to explore the labyrinth:\r\nJames | Lea | Gabriel", false);
            new James("").ShowStats();
            new Lea("").ShowStats();
            new Gabriel("").ShowStats();

            Hero hero = SetCharacter(Console.ReadLine(), playerName);

            _printer.PrintAction(hero, "selected", new DefaultHero() { Name = hero.CharacterName }, false);
            _printer.PrintMessage("Press enter to start the game.", false);
            Console.ReadLine();

            return hero;
        }

        private bool CheckEntityByName(string entityName)
        {
            bool result;
            if(Entities.Where(e => e.Name.ToLower() == entityName).Any())
            {
                result = true;
            }
            else
            {
                _printer.PrintMessage("Invalid entity name '" + entityName + "'!", false);
                _printer.PrintHUD(Entities, PlayerName);
                result = false;
            }
            return result;
        }

        private bool CheckItemByName(string itemName)
        {
            bool result;
            if (Entities.Where(e => e.Name.ToLower() == PlayerName.ToLower()).FirstOrDefault().ItemBag.Where(i => i.Name.ToLower() == itemName).Any())
            {
                result = true;
            }
            else
            {
                _printer.PrintMessage("You don't have any '" + itemName + "'.", false);
                _printer.PrintHUD(Entities, PlayerName);
                result = false;
            }
            return result;
        }

        private void ShowHealthCommand(string entityName)
        {
            if (CheckEntityByName(entityName))
            {
                var entity = Entities.Where(e => e.Name.ToLower() == entityName).FirstOrDefault();
                entity.ShowHealth();
            }
            else
            {
                return;
            }
        }


        private void ShowStatsCommand(string entityName)
        {
            if (CheckEntityByName(entityName))
            {
                var entity = Entities.Where(e => e.Name.ToLower() == entityName).FirstOrDefault();
                entity.ShowStats();
            }
            else
            {
                return;
            }
        }

        private void AttackCommand(string enemyName)
        {
            if (CheckEntityByName(enemyName))
            {
                var enemy = Entities.Where(e => e.Name.ToLower() == enemyName).FirstOrDefault();
                AttackEnemy(enemy as Enemy);
            }
            else
            {
                return;
            }
        }

        private void SpecialAttackCommand(string enemyName)
        {
            if (CheckEntityByName(enemyName))
            {
                var enemy = Entities.Where(e => e.Name.ToLower() == enemyName).FirstOrDefault();
                AttackEnemySP(enemy as Enemy);
            }
            else
            {
                return;
            }
        }
        abstract public void DefendCommand();

        private void UseItemCommand(string itemName)
        {
            if (CheckItemByName(itemName))
            {
                var hero = Entities.Where(e => e.Name.ToLower() == PlayerName.ToLower()).FirstOrDefault();
                var item = hero.ItemBag.Where(i => i.Name.ToLower() == itemName).FirstOrDefault();

                UseItem(item, hero as Hero);
            }
        }

        public WalkDirection MoveToChamberCommand(string directionString, WalkDirection cameFrom)
        {
            var dirSring = directionString.ToLower();
            WalkDirection direction;
            if (dirSring == "north")
            {
                direction = WalkDirection.North;
            }
            else if (dirSring == "east")
            {
                direction = WalkDirection.East;
            }
            else if (dirSring == "south")
            {
                direction = WalkDirection.South;
            }
            else if (dirSring == "west")
            {
                direction = WalkDirection.West;
            }
            else
            {
                _printer.PrintMessage(directionString + " is not a valid direction.", false);
                _printer.PrintHUD(Entities, PlayerName);
                direction = MoveToChamberCommand(Console.ReadLine(), cameFrom);
            }

            if (direction == cameFrom)
            {
                _printer.PrintMessage("You can't go back in there...", false);
                _printer.PrintHUD(Entities, PlayerName);
                direction = MoveToChamberCommand(Console.ReadLine(), cameFrom);
            }
            return direction;
        }

        virtual public Hero SetCharacter(string characterName, string playerName)
        {
            Hero hero;
            var charName = characterName.ToLower();

            if (charName == "james")
            {
                hero = new James(playerName);
            }
            else if (charName == "lea")
            {
                hero = new Lea(playerName);
            }
            else if (charName == "gabriel")
            {
                hero = new Gabriel(playerName);
            }
            else
            {
                _printer.PrintMessage(characterName + " is not a valid character.", false);
                hero = SetCharacter(Console.ReadLine(), playerName);
            }

            return hero;
        }

        abstract public void AttackEnemy(Enemy enemy);
        abstract public void AttackEnemySP(Enemy enemy);
        abstract public void UseItem(IItem item, Hero hero);
        abstract public void CheckGameStatus();
    }
}
