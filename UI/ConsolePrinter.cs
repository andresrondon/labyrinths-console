using Colorful;
using Labyrinths.Engine;
using Labyrinths.Utils;
using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Labyrinths.UI
{
    public class ConsolePrinter
    {
        protected int origRow;
        protected int origCol;

        static private string consoleText;

        static public void _PrintMessage(string message, bool clear)
        {    
            Colorful.Console.WriteLine(message);

            if (!clear)
            {
                consoleText += message + "\r\n";
            }
            else
            {
                consoleText = message + "\r\n";
            }
        }

        public void PrintMessage(string message, bool clear)
        {
            _PrintMessage(message, clear);
        }

        public void PrintAction(Entity performerEntity, string action, Entity performedOnEntity, bool exclamation)
        {
            var exc = exclamation ? "!" : ".";
            var message = String.Format("{0} {1} {2}{3}", performerEntity.Name, action, performedOnEntity.Name, exc);
            _PrintMessage(message, false);
        }

        public void PrintStats(string characterName, Stats stats)
        {
            var message = String.Format("{0} (Power: {1} | Speed: {2} | Stamina: {3} | Defense: {4})", characterName, stats.Power, stats.Speed, stats.Stamina, stats.Defense);
            _PrintMessage(message, false);
        }

        public void PrintHealth(string name, HealthMeter healthMeter)
        {
            var message = String.Format("{0} (Health: {1})", name, healthMeter.Health);
            _PrintMessage(message, false);
        }

        public void PrintMenu()
        {

        }

        public void PrintStartScreen()
        {
            FigletFont font = FigletFont.Load("big.flf");
            Figlet figlet = new Figlet(font);

            // Set the Title Screen text
            List<ColorfulText> text = new List<ColorfulText>
            {
                new ColorfulText(figlet.ToAscii("Labyrinths:"), ColorTranslator.FromHtml("#aab64c")),
                new ColorfulText(figlet.ToAscii("Black Gate"), ColorTranslator.FromHtml("#4640ff"))
            };

            var height = 16;
            PrintFrame(height, false, false, text, height + 2);
        }

        public void PrintHUD(List<Entity> entities, string playerName)
        {
            const int columns = 4;
            const int linesInColumn = 6;
            var lineLength = Colorful.Console.WindowWidth - 6;
            List<ColorfulText> text = new List<ColorfulText>();
            string[] textLines = new string[linesInColumn];

            var columnTitle = new string[columns] { " Heroes", " Enemies", " Item Bag", " Avalaible Commands"};
            for (int i = 0; i < columnTitle.Count(); i++)
            {
                textLines[0] += columnTitle[i] + ' '.Repeat(lineLength / columns - columnTitle[i].Length);
            }
            text.Add(new ColorfulText(textLines[0], Color.BlueViolet));

            var heroes = entities.Where(e => e.Type == EntityType.Hero);
            var enemies = entities.Where(e => e.Type == EntityType.Enemy);
            var items = heroes.Where(h => h.Name == playerName).FirstOrDefault().ItemBag
                        .GroupBy(item => item.Name)
                        .Select(item => new { Name = item.Key, Count = item.Count() });
            var availableCommands = GetAvailableCommands();

            var height = 6;
            PrintFrame(height, true, true, text, -1);

            // Save the top and left coordinates.
            origRow = Colorful.Console.CursorTop;
            origCol = Colorful.Console.CursorLeft;

            var count = 0;
            foreach (var hero in heroes)
            {
                Colorful.Console.SetCursorPosition(origCol + 1, origRow + 3 + count);
                Colorful.Console.WriteLine(hero.Name + ": " + PrintHealthBar(hero.HealthMeter), Color.Aqua);
                count++;
            }
            count = 0;
            foreach (var enemy in enemies)
            {
                Colorful.Console.SetCursorPosition((lineLength / columns) + 1, origRow + 3 + count);
                Colorful.Console.WriteLine(enemy.Name + ": " + PrintHealthBar(enemy.HealthMeter), Color.Aqua);
                count++;
            }
            count = 0;
            foreach (var item in items)
            {
                Colorful.Console.SetCursorPosition(2 * (lineLength / columns) + 1, origRow + 3 + count);
                Colorful.Console.WriteLine(item.Count + ": " + item.Name, Color.Yellow);
                count++;
            }
            count = 0;
            foreach (var command in availableCommands)
            {
                Colorful.Console.SetCursorPosition(3 * (lineLength / columns) + 1, origRow + 3 + count);
                Colorful.Console.WriteLine(command.ToString(), Color.Green);
                count++;
            }
            Colorful.Console.SetCursorPosition(origCol, origRow);
        }

        protected void PrintFrame(int height, bool clear, bool rewrite, List<ColorfulText> text, int returnCursorToRow)
        {
            height += 1;

            // Clear the screen, then print previous text.
            if (clear)
            {
                Colorful.Console.Clear();
            }
            if (rewrite)
            {
                Colorful.Console.WriteLine(consoleText);
            }

            // Save the top and left coordinates.
            origRow = Colorful.Console.CursorTop;
            origCol = Colorful.Console.CursorLeft;

            // Write the text inside the frame
            Colorful.Console.SetCursorPosition(origCol, origRow + 1);
            foreach (var line in text)
            {
                Colorful.Console.WriteLine(line.Text, line.Color);
            }
            Colorful.Console.SetCursorPosition(origCol, origRow);

            var width = Colorful.Console.WindowWidth - 3;

            var horizontalLine = '-'.Repeat(width);
            
            // Draw the bottom side.
            WriteAt("+" + horizontalLine + "+", 0, height);

            // Draw the top side.
            WriteAt("+" + horizontalLine + "+", 0, 0);
            
            for (int i = 1; i <= height - 1; i++)
            {
                WriteAt("|", 0, i);
                WriteAt("|", width + 1, i);
            }

            Colorful.Console.SetCursorPosition(origCol, origRow + returnCursorToRow);
        }

        protected void WriteAt(string s, int x, int y)
        {
            try
            {
                Colorful.Console.SetCursorPosition(origCol + x, origRow + y);
                Colorful.Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Colorful.Console.Clear();
                Colorful.Console.WriteLine(e.Message);
            }
        }

        protected string PrintHealthBar(HealthMeter healthMeter)
        {
            var healthBar = "[";
            int emChars = Convert.ToInt32((healthMeter.Health / healthMeter.MaxHealth) * 10);
            healthBar += '\u25A1'.Repeat(emChars);
            healthBar += '-'.Repeat(10 - emChars);
            healthBar += "]";
            healthBar += healthMeter.Health;

            return healthBar;
        }

        protected IEnumerable<string> GetAvailableCommands()
        {
            var availableCommands = new List<string>()
            {
                "Attack (atk:ENEMY NAME)",
                "Special Attack (spc:ENEMY NAME)",
                "Defend (dfd)",
                "Use item (itm:ITEM NAME)",
                //"Show health (hlt:HERO/ENEMY NAME)",
                "Show stats (sts:HERO/ENEMY NAME)"
            };

            return availableCommands;
        }
    }
    public class ColorfulText
    {
        public ColorfulText(object text, Color color)
        {
            Text = text;
            Color = color;
        }

        public object Text { get; set; }
        public Color Color { get; set; }
    }
}
