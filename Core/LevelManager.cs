using System;
using System.Collections.Generic;

namespace Labyrinths.Core
{
    public class LevelManager
    {
        public Level CurrentLevel { get; set; }
        private float DifficultyFactor { get; set; }

        const int maxFactorToPassLevel = 3;

        public LevelManager(float difficultyFactor, List<Entity> entities)
        {
            DifficultyFactor = difficultyFactor;
            CurrentLevel = new Level(1, 1 * DifficultyFactor, WalkDirection.South, entities);
        }

        public void MoveToChamber(WalkDirection direction, float speed)
        {
            float chanceFactor = 8 - speed;

            int[] seedArray = new int[5];
            Random rand = new Random((int)direction);
            for (int i = 0; i < 5; i++)
            {
                seedArray[i] = rand.Next(1, 4);
            }

            rand = new Random();
            chanceFactor += seedArray[rand.Next(0, 5)];

            chanceFactor = rand.Next(1, Convert.ToInt32(chanceFactor));

            DecideLocationToMove(chanceFactor, direction);
        }
        private void DecideLocationToMove(float chanceFactor, WalkDirection direction)
        {
            direction = GetInvertedDirection(direction);
            if (chanceFactor <= maxFactorToPassLevel)
            {
                // Move to the next level!
                CurrentLevel = new Level(CurrentLevel.Id + 1, (CurrentLevel.Difficulty + 1) * DifficultyFactor, direction, CurrentLevel.CurrentChamber.Entities);
            }
            else
            {
                // Generate a new chamber in the same level.
                CurrentLevel.CurrentChamber = new Chamber(CurrentLevel.CurrentChamber.Id + 1, direction, CurrentLevel, CurrentLevel.CurrentChamber.Entities);
            }
        }

        private WalkDirection GetInvertedDirection(WalkDirection walkDirection)
        {
            int index = (int)walkDirection;
            index = (index + 1) % 4;
            index += 1;
            return (WalkDirection)index;
        }

    }
}
