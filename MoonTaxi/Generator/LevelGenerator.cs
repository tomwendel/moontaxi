using Microsoft.Xna.Framework;
using MoonTaxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonTaxi.Generator
{
    class LevelGenerator:ILevelGenerator
    {
        private const int TILE_SIZE = 80;
        public LevelGenerator()
        {
        }
        void RemoveRegion(int x,int y,int width,int height,int levelWidth,int levelHeight,List<int> grid)
        {
            int countX = (int)Math.Ceiling((float)width / TILE_SIZE);
            int countY = (int)Math.Ceiling((float)height / TILE_SIZE);
            for (int i = 0; i < countX; i++)
            {
                for (int j = 0; j < countY; j++)
                {
                    if (x + i < levelWidth)
                        grid.Remove((y + j) * levelWidth + (x + i));
                }
            }
        }
        int FreeTiles(int x,int y,int levelWidth,int levelHeight,List<int> grid)
        {
            for (int i = 1; i < levelWidth - x; i++)
            {
                if (!grid.Contains(y * levelWidth + (x + i)))
                    return Math.Min(i - 1, 4);
            }
            return Math.Min(levelWidth - x - 1, 4);
        }
        public Models.Level CreateLevel(int seed)
        {
            Random random = new Random(seed);
            Level level = new Level();
            int width = (int)(level.Size.X / TILE_SIZE);
            int height = (int)(level.Size.Y/TILE_SIZE);
            List<int> grid =new List<int>(width*height);
            for (int y = 1; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid.Add(y*width+x);
                }
            }


            int blockCount = width * height / 20;

            for (int i = 0; i < blockCount;i++ )
            {
                int index = random.Next(0, grid.Count);
                int position = grid[index];
                grid.RemoveAt(index);

                int x = (position % width) * TILE_SIZE, y = (position / width) * TILE_SIZE;
                int xOffset = random.Next(3, TILE_SIZE / 2 - 3);
                int yOffset = random.Next(0, TILE_SIZE / 4);
                int freeTiles = FreeTiles(x / TILE_SIZE, y / TILE_SIZE, width, height, grid);
                int xSize = random.Next(TILE_SIZE - xOffset, Math.Max(TILE_SIZE * freeTiles, TILE_SIZE - xOffset));
                int ySize = random.Next(TILE_SIZE / 8, TILE_SIZE * 3 / 8);

                RemoveRegion(x / TILE_SIZE, y / TILE_SIZE, xSize + xOffset, ySize + yOffset, width, height, grid);

                Block block = new Block() { Size = new Rectangle(x + xOffset, y + yOffset, xSize, ySize), SpawnPlatform = true };
                level.Blocks.Add(block);
            }

            int pos = grid[random.Next(0, grid.Count)];
            level.TaxiSpawn = new Vector2((pos % width) * TILE_SIZE, (pos / width) * TILE_SIZE);

            /*level.Blocks.AddRange(new[]{
                new Block() { Size = new Rectangle(100, 200, 200, 50), SpawnPlatform = true },
                new Block() { Size = new Rectangle(500, 200, 200, 50), SpawnPlatform = true },
                new Block() { Size = new Rectangle(500, 600, 200, 50), SpawnPlatform = true },
            });*/

            return level;
        }
    }
}
