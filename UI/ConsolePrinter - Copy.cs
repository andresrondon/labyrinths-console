using Colorful;
using Labyrinths.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Labyrinths.UI
{
    public class ConsolePrinter
    {
        protected int origRow;
        protected int origCol;

        static private string consoleText;

        static private void _PrintMessage(string message, bool clear)
        {
                
            System.Console.WriteLine(message);

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
                //new ColorfulText("\r\n", Color.Black),
                new ColorfulText(figlet.ToAscii("Labyrinths:"), ColorTranslator.FromHtml("#aab64c")),
                new ColorfulText(figlet.ToAscii("Black Gate"), ColorTranslator.FromHtml("#3600ff"))
            };

            var height = 16;
            PrintFrame(height, false, false, text, height + 2);
        }

        public void PrintHUD(List<Entity> entities)
        {
            const int columns = 3;
            const int linesInColumn = 6;
            var lineLength = Colorful.Console.WindowWidth - 6;
            List<ColorfulText> text = new List<ColorfulText>();
            string[] textLines = new string[linesInColumn];

            var columnTitle = new string[columns] { " Heroes", "Enemies", "Avalaible Commands" };
            for (int i = 0; i < columnTitle.Count(); i++)
            {
                textLines[0] += columnTitle[i] + GetWhiteSpaces(lineLength / columns, columnTitle[i].Length);
            }
            text.Add(new ColorfulText(textLines[0], Color.BlueViolet));

            var heroes = entities.Where(e => e.Type == EntityType.Hero);
            var enemies = entities.Where(e => e.Type == EntityType.Enemy);
            var availableCommands = GetAvailableCommands();

            for (int i = 0; i < linesInColumn - 1; i++)
            {
                // Column 1: Heroes
                var lineText = " ";
                if (i + 1 <= heroes.Count())
                {
                    lineText += heroes.ElementAt(i).Name + ": " + heroes.ElementAt(i).HealthMeter.Health;
                }
                textLines[i + 1] += lineText + GetWhiteSpaces(lineLength / columns, lineText.Length);

                // Column 2: Enemies
                lineText = "";
                if (i + 1 <= enemies.Count())
                {
                    lineText += enemies.ElementAt(i).Name + ": " + enemies.ElementAt(i).HealthMeter.Health;
                }
                textLines[i + 1] += lineText + GetWhiteSpaces(lineLength / columns, lineText.Length);

                // Column 3: Available Commands
                lineText = "";
                if (i + 1 <= availableCommands.Count())
                {
                    lineText += availableCommands.ElementAt(i);
                }
                textLines[i + 1] += lineText + GetWhiteSpaces(lineLength / columns, lineText.Length);

                //text.Add(new ColorfulText(textLines[i + 1], Color.Aqua));
            }

            var height = 6;
            PrintFrame(height, true, true, text, -1);

            // Save the top and left coordinates.
            origRow = Colorful.Console.CursorTop;
            origCol = Colorful.Console.CursorLeft;
        }

        protected void PrintFrame(int height, bool clear, bool rewrite, List<ColorfulText> text, int returnCursorToRow)
        {
            string horizontalLine = String.Empty;

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

            var width = Colorful.Console.WindowWidth - 4;

            for (int i = 0; i <= width; i++)
            {
                horizontalLine += "-";
            }
            // Draw the bottom side.
            WriteAt("+" + horizontalLine + "+", 0, height);

            // Draw the top side.
            WriteAt("+" + horizontalLine + "+", 0, 0);
            
            for (int i = 1; i <= height - 1; i++)
            {
                WriteAt("|", 0, i);
                WriteAt("|", width + 2, i);
            }

            Colorful.Console.SetCursorPosition(origCol, origRow + returnCursorToRow);
        }

        protected void WriteAt(string s, int x, int y)
        {
            try
            {
                Colorful.Console.SetCursorPosition(origCol + x, origRow + y);
                System.Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Colorful.Console.Clear();
                Colorful.Console.WriteLine(e.Message);
            }
        }

        protected string PrintHealthBar(HealthMeter healthMeter)
        {
            var healthBar = string.Empty;


            return healthBar;
        }

        protected string GetWhiteSpaces(int spaceLength, int charactersLength)
        {
            var x = spaceLength - charactersLength;
            string whiteSpaces = "";

            for (int i = 1; i <= x; i++)
            {
                whiteSpaces += " ";
            }

            return whiteSpaces;
        }

        protected IEnumerable<string> GetAvailableCommands()
        {
            var availableCommands = new List<string>()
            {
                "Attack (atk:[ENEMY NAME])",
                "Defend (dfd)",
                "Status (sts:[HERO/ENEMY NAME])",
                "Show items in bag (bag)",
                "Use item (itm:[ITEM NAME])"
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
