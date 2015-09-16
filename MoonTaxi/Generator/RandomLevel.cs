using Microsoft.Xna.Framework;
using MoonTaxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonTaxi.Generator
{
    internal class RandomLevel : Level
    {
        private const int TILE_SIZE = 80;

        private List<int> grid;
        private Random random;
        private int width;
        private int height;

        private void RemoveRegion(int x, int y, int width, int height)
        {
            int countX = (int)Math.Ceiling((float)width / TILE_SIZE);
            int countY = (int)Math.Ceiling((float)height / TILE_SIZE);
            for (int i = 0; i < countX; i++)
            {
                for (int j = 0; j < countY; j++)
                {
                    if (x + i < width)
                        grid.Remove((y + j) * width + (x + i));
                }
            }
        }
        private int FreeTiles(int x, int y)
        {
            for (int i = 1; i < width - x; i++)
            {
                if (!grid.Contains(y * width + (x + i)))
                    return Math.Min(i - 1, 4);
            }
            return Math.Min(width - x - 1, 4);
        }

        public RandomLevel(Vector2 size, int parallelGuestCounts, int seed)
            : base(size, parallelGuestCounts)
        {
            random = new Random(seed);
            width = (int)(Size.X / TILE_SIZE);
            height = (int)(Size.Y / TILE_SIZE);
            grid = new List<int>(width * height);
            for (int y = 1; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid.Add(y * width + x);
                }
            }


            int blockCount = width * height / 20;

            for (int i = 0; i < blockCount; i++)
            {
                int index = random.Next(0, grid.Count);
                int position = grid[index];
                grid.RemoveAt(index);

                int x = (position % width) * TILE_SIZE, y = (position / width) * TILE_SIZE;
                int xOffset = random.Next(3, TILE_SIZE / 2 - 3);
                int yOffset = random.Next(0, TILE_SIZE / 4);
                int freeTiles = FreeTiles(x / TILE_SIZE, y / TILE_SIZE);
                int xSize = random.Next(TILE_SIZE - xOffset, Math.Max(TILE_SIZE * freeTiles, TILE_SIZE - xOffset));
                int ySize = random.Next(TILE_SIZE / 8, TILE_SIZE * 3 / 8);

                RemoveRegion(x / TILE_SIZE, y / TILE_SIZE, xSize + xOffset, ySize + yOffset);

                Block block = new Block()
                {
                    Size = new Rectangle(x + xOffset, y + yOffset, xSize, ySize),
                    SpawnPlatform = true
                };
                Blocks.Add(block);
            }

            
        }

        public override Vector2 GetTaxiSpawn()
        {
            int pos = grid[random.Next(0, grid.Count)];
            return new Vector2((pos % width) * TILE_SIZE, (pos / width) * TILE_SIZE);
        }
    }
}
