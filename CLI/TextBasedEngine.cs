using Labyrinths.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using Labyrinths.Core;
using System.Text;

namespace Labyrinths.CLI
{
    public class TextBasedEngine : GameEngine
    {
        public TextBasedEngine(string playerName)
        {
            // Game Configuration.
            Console.Clear();
            Console.WindowHeight = 32;
            Console.Title = gameName;
            Console.OutputEncoding = Encoding.UTF8;

            // Start screen.
            ConsolePrinter.PrintStartScreen(playerName, gameName);
            Player = SelectCharacterCommand(playerName);
            Entities = new List<Entity> { Player };

            Console.Clear();
            StartGame(difficultyFactor: 1);
        }

        override protected void StartGame(float difficultyFactor)
        {
            levelSystem = new LevelManager(difficultyFactor, Entities);
            ReadCommand();
        }

        public void ReadCommand()
        {
            CheckGameStatus();

            ConsolePrinter.PrintMessage("Please enter a command.", false);
            PrintHUD();

            string command = Console.ReadLine();
            command = command.ToLower();

            var commandArray = command.Split(':');
            var primaryCommand = commandArray[0].Trim();
            var parameters = commandArray.Skip(1).ToArray();

            ExecuteCommand(primaryCommand, parameters);
        }

        private void ExecuteCommand(string command, params string[] parameters)
        {
            if (parameters.Length > 0 && parameters[0] != null)
                parameters[0] = parameters[0].Trim().ToLower();

            switch (command)
            {
                case "atk":
                    AttackCommand(parameters[0]);
                    break;
                case "dfd":
                    DefendCommand();
                    break;
                case "spc":
                    SpecialAttackCommand(parameters[0]);
                    break;
                case "itm":
                    UseItemCommand(parameters[0]);
                    break;
                case "hlt":
                    ShowHealthCommand(parameters[0]);
                    break;
                case "sts":
                    ShowStatsCommand(parameters[0]);
                    break;
                case "exit":
                    Console.Clear();
                    Environment.Exit(0);
                    break;
                default:
                    ConsolePrinter.PrintMessage("'" + command + "' is not a valid command.", false);
                    break;
            }

            ReadCommand();
        }

        private void DefendCommand()
        {
            Defend();
        }

        public Hero SelectCharacterCommand(string playerName)
        {
            ConsolePrinter.PrintMessage("Please select a character with whom to explore the labyrinth:\r\nJames | Lea | Gabriel", false);
            ConsolePrinter.PrintStats("James", new James("").Stats);
            ConsolePrinter.PrintStats("Lea", new Lea("").Stats);
            ConsolePrinter.PrintStats("Gabriel", new Gabriel("").Stats);

            Hero hero = SetCharacter(Console.ReadLine(), playerName);

            ConsolePrinter.PrintAction(hero, "selected", new DefaultHero() { Name = hero.CharacterName }, false);
            ConsolePrinter.PrintMessage("Press enter to start the game.", false);
            Console.ReadLine();

            return hero;
        }

        private bool CheckEntityByName(string entityName)
        {
            var result = Entities.Where(e => e.Name.ToLower() == entityName).Any();
            if (!result)
            {
                ConsolePrinter.PrintMessage("Invalid entity name '" + entityName + "'!", false);
                PrintHUD();
            }

            return result;
        }

        private bool CheckItemByName(string itemName)
        {
            var result = Player.ItemBag.Where(i => i.Name.ToLower() == itemName).Any();
            if (!result)
            {
                ConsolePrinter.PrintMessage("You don't have any '" + itemName + "'.", false);
                PrintHUD();
            }

            return result;
        }

        private void ShowHealthCommand(string entityName)
        {
            if (CheckEntityByName(entityName))
            {
                var entity = Entities.Where(e => e.Name.ToLower() == entityName).FirstOrDefault();
                ConsolePrinter.PrintHealth(entity.CharacterName, entity.HealthMeter);
            }
        }

        private void ShowStatsCommand(string entityName)
        {
            if (CheckEntityByName(entityName))
            {
                var entity = Entities.Where(e => e.Name.ToLower() == entityName).FirstOrDefault();
                ConsolePrinter.PrintStats(entity.CharacterName, entity.Stats);
            }
        }

        private void AttackCommand(string enemyName)
        {
            if (CheckEntityByName(enemyName))
            {
                var enemy = GetEnemy(enemyName);
                AttackEnemy(enemy);
            }
        }

        private void SpecialAttackCommand(string enemyName)
        {
            if (CheckEntityByName(enemyName))
            {
                var enemy = GetEnemy(enemyName);
                AttackEnemySP(enemy);
            }
        }

        private void UseItemCommand(string itemName)
        {
            if (CheckItemByName(itemName))
            {
                var item = Player.ItemBag.Where(i => i.Name.ToLower() == itemName).FirstOrDefault();

                UseItem(item, Player);
            }
        }

        public WalkDirection MoveToChamberCommand(string directionString, WalkDirection cameFrom)
        {
            var dirSring = directionString.ToLower();
            WalkDirection direction;

            switch (dirSring)
            {
                case "north":
                    direction = WalkDirection.North;
                    break;
                case "east":
                    direction = WalkDirection.East;
                    break;
                case "south":
                    direction = WalkDirection.South;
                    break;
                case "west":
                    direction = WalkDirection.West;
                    break;
                default:
                    ConsolePrinter.PrintMessage(directionString + " is not a valid direction.", false);
                    PrintHUD();
                    direction = MoveToChamberCommand(Console.ReadLine(), cameFrom);
                    break;
            }

            if (direction == cameFrom)
            {
                ConsolePrinter.PrintMessage("You can't go back in there...", false);
                PrintHUD();
                direction = MoveToChamberCommand(Console.ReadLine(), cameFrom);
            }

            return direction;
        }

        public Hero SetCharacter(string characterName, string playerName)
        {
            Hero hero;
            var charName = characterName.ToLower();

            switch (charName)
            {
                case "james":
                    hero = new James(playerName);
                    break;
                case "lea":
                    hero = new Lea(playerName);
                    break;
                case "gabriel":
                    hero = new Gabriel(playerName);
                    break;
                default:
                    ConsolePrinter.PrintMessage(characterName + " is not a valid character.", false);
                    hero = SetCharacter(Console.ReadLine(), playerName);
                    break;
            }

            return hero;
        }

        override protected void Lose()
        {
            Console.Clear();
            ConsolePrinter.PrintMessage("Thou art dead!", true);
            ConsolePrinter.PrintMessage("Thyself have been lost wandering the labyrinth.", false);
            Console.ReadLine();
            Environment.Exit(0);
        }

        override protected void ChamberCleared()
        {
            ConsolePrinter.PrintMessage("Thou eliminated the monsters in this chamber!", false);
            ConsolePrinter.PrintMessage("Thou shall now proceed to the next chamber in the direction you choose.", false);
            ConsolePrinter.PrintMessage(levelSystem.CurrentLevel.CurrentChamber.GetPossibleDirections(), false);
            PrintHUD();

            var speed = Player.Stats.Speed;

            levelSystem.MoveToChamber(MoveToChamberCommand(Console.ReadLine(), levelSystem.CurrentLevel.CurrentChamber.CameFrom), speed);
        }
        override protected void WinGame()
        {
            Console.Clear();
            ConsolePrinter.PrintMessage("Congratulations, thou have defeated Fenriswolf!", true);
            ConsolePrinter.PrintMessage("Thou have conquered the Labyrinth.", false);
            ConsolePrinter.PrintMessage("Press enter to exit.", false);
            Console.ReadLine();
            Environment.Exit(0);
        }

        private void PrintHUD()
        {
            ConsolePrinter.PrintHUD(Heroes, Enemies, Player.ItemBag);
        }
    }
}
