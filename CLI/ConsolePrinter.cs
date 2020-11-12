using Colorful;
using Labyrinths.Core;
using Labyrinths.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Labyrinths.CLI
{
    public static class ConsolePrinter
    {
        private static int origRow;
        private static int origCol;
        private static readonly int lineLength = Colorful.Console.WindowWidth - 6;

        private const int hudColumns = 4;
        private const int hudRows = 6;

        static private string consoleText;

        public static void PrintMessage(string message, bool clear)
        {
            Colorful.Console.WriteLine(message);

            message += "\r\n";
            if (!clear)
            {
                consoleText += message;
            }
            else
            {
                consoleText = message;
            }
        }

        public static void PrintAction(Entity performerEntity, string action, Entity performedOnEntity, bool exclamation)
        {
            var delimiter = exclamation ? "!" : ".";
            var message = string.Format("{0} {1} {2}{3}", performerEntity.Name, action, performedOnEntity.Name, delimiter);
            PrintMessage(message, false);
        }

        public static void PrintStats(string characterName, Stats stats)
        {
            var message = string.Format("{0} (Power: {1} | Speed: {2} | Stamina: {3} | Defense: {4})", characterName, stats.Power, stats.Speed, stats.Stamina, stats.Defense);
            PrintMessage(message, false);
        }

        public static void PrintHealth(string name, HealthMeter healthMeter)
        {
            var message = string.Format("{0} (Health: {1})", name, healthMeter.Health);
            PrintMessage(message, false);
        }

        public static void PrintStartScreen(string playerName, string gameName)
        {
#if DEBUG
            FigletFont font = FigletFont.Load("../../../big.flf");
#else
            FigletFont font = FigletFont.Load("big.flf");
#endif
            Figlet figlet = new Figlet(font);

            // Set the Title Screen text
            List<ColorfulText> text = new List<ColorfulText>
            {
                new ColorfulText(figlet.ToAscii("Labyrinths:"), ColorTranslator.FromHtml("#aab64c")),
                new ColorfulText(figlet.ToAscii("Black Gate"), ColorTranslator.FromHtml("#4640ff"))
            };

            var height = 16;
            PrintFrame(height, false, false, text, height + 2);
            PrintMessage("Hello " + playerName, false);
            PrintMessage("Welcome to " + gameName + ".", false);
        }

        public static void PrintHUD(IEnumerable<Hero> heroes, IEnumerable<Enemy> enemies, IEnumerable<IItem> itemBag)
        {
            // Print frame and headers
            List<ColorfulText> text = GenerateHeaders();
            PrintFrame(hudRows, true, true, text, -1);

            // Print columns
            PrintColumns(heroes, enemies, itemBag);

            // Return cursor
            Colorful.Console.SetCursorPosition(origCol, origRow);
        }

        private static List<ColorfulText> GenerateHeaders()
        {
            var textLines = new string[hudRows];
            var columnHeaders = new string[hudColumns] { " Heroes", " Enemies", " Item Bag", " Avalaible Commands" };

            foreach (var header in columnHeaders)
            {
                textLines[0] += header + ' '.Repeat(lineLength / hudColumns - header.Length);
            }

            var text = new List<ColorfulText>
            {
                new ColorfulText(textLines[0], Color.BlueViolet)
            };
            return text;
        }

        private static void PrintColumns(IEnumerable<Hero> heroes, IEnumerable<Enemy> enemies, IEnumerable<IItem> itemBag)
        {
            var items = itemBag
                            .GroupBy(item => item.Name)
                            .Select(itemGroup => new { Name = itemGroup.Key, Count = itemGroup.Count() });
            var availableCommands = GetAvailableCommands();

            // Save the top and left coordinates first
            origRow = Colorful.Console.CursorTop;
            origCol = Colorful.Console.CursorLeft;

            PrintColumn(heroes, 0, (hero) => hero.Name + ": " + PrintHealthBar(hero.HealthMeter), Color.Aqua);
            PrintColumn(enemies, 1, (enemy) => enemy.Name + ": " + PrintHealthBar(enemy.HealthMeter), Color.Aqua);
            PrintColumn(items, 2, (item) => item.Count + ": " + item.Name, Color.Yellow);
            PrintColumn(availableCommands, 3, (command) => command, Color.Green);
        }

        static void PrintColumn<T>(IEnumerable<T> enumerable, int rowIndex, Func<T, string> rowTextConstructor, Color color)
        {
            var count = 0;
            foreach (var item in enumerable)
            {
                Colorful.Console.SetCursorPosition(rowIndex * (lineLength / hudColumns) + 1, origRow + 3 + count);
                Colorful.Console.WriteLine(rowTextConstructor(item), color);
                count++;
            }
        }

        static void PrintFrame(int height, bool clear, bool rewrite, List<ColorfulText> text, int returnCursorToRow)
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

        static void WriteAt(string s, int x, int y)
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

        static string PrintHealthBar(HealthMeter healthMeter)
        {
            var healthBar = "[";
            int emChars = Convert.ToInt32((healthMeter.Health / healthMeter.MaxHealth) * 10);
            healthBar += '\u25A1'.Repeat(emChars);
            healthBar += '-'.Repeat(10 - emChars);
            healthBar += "]";
            healthBar += healthMeter.Health;

            return healthBar;
        }

        static IEnumerable<string> GetAvailableCommands()
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
