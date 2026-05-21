using UnityEngine;

namespace SandFall
{
    public struct Pixel
    {
        public Color Color;
        public bool CanMove;

        public Pixel(Color color, bool canMove = true)
        {
            Color = color;
            CanMove = canMove;
        }
    }
}
