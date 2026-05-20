using UnityEngine;

namespace SandFall
{
    public class SandSimulation
    {
        public readonly SandGrid Grid;
        public bool EnableDiagonalSpread = true;

        private readonly System.Random _random = new System.Random();

        public SandSimulation(int width, int height)
        {
            Grid = new SandGrid(width, height);
        }

        public void Step()
        {
            // Scan bottom-to-top so settled sand doesn't cascade multiple cells per step.
            for (int y = 0; y < Grid.Height - 1; y++)
            {
                for (int x = 0; x < Grid.Width; x++)
                {
                    PixelContainer cell = Grid.Get(x, y);
                    if (!cell.IsEmpty) continue;

                    // 1. Pull pixel straight down from directly above.
                    PixelContainer above = Grid.Get(x, y + 1);
                    if (!above.IsEmpty && above.Pixel.CanMove)
                    {
                        cell.Pixel = above.Pixel;
                        above.Pixel = null;
                        continue;
                    }

                    if (!EnableDiagonalSpread) continue;

                    // 2. Check diagonal-up-left and diagonal-up-right.
                    //    A diagonal is valid only when the same-row neighbor already has a pixel
                    //    (pile-slope constraint).
                    bool leftValid = false;
                    bool rightValid = false;

                    PixelContainer neighborLeft  = Grid.Get(x - 1, y);
                    PixelContainer diagUpLeft    = Grid.Get(x - 1, y + 1);
                    if (neighborLeft != null && !neighborLeft.IsEmpty &&
                        diagUpLeft   != null && !diagUpLeft.IsEmpty && diagUpLeft.Pixel.CanMove)
                    {
                        leftValid = true;
                    }

                    PixelContainer neighborRight = Grid.Get(x + 1, y);
                    PixelContainer diagUpRight   = Grid.Get(x + 1, y + 1);
                    if (neighborRight != null && !neighborRight.IsEmpty &&
                        diagUpRight   != null && !diagUpRight.IsEmpty && diagUpRight.Pixel.CanMove)
                    {
                        rightValid = true;
                    }

                    if (leftValid && rightValid)
                    {
                        bool pickLeft = _random.NextDouble() < 0.5;
                        PixelContainer source = pickLeft ? diagUpLeft : diagUpRight;
                        cell.Pixel = source.Pixel;
                        source.Pixel = null;
                    }
                    else if (leftValid)
                    {
                        cell.Pixel = diagUpLeft.Pixel;
                        diagUpLeft.Pixel = null;
                    }
                    else if (rightValid)
                    {
                        cell.Pixel = diagUpRight.Pixel;
                        diagUpRight.Pixel = null;
                    }
                }
            }
        }

        public void Spawn(int x, int y, Color color)
        {
            if (!Grid.InBounds(x, y)) return;
            PixelContainer cell = Grid.Get(x, y);
            if (cell.IsEmpty)
                cell.Pixel = new Pixel(color);
        }

        public void Clear()
        {
            for (int y = 0; y < Grid.Height; y++)
                for (int x = 0; x < Grid.Width; x++)
                    Grid.Set(x, y, null);
        }
    }
}
