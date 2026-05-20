using UnityEngine;

namespace SandFall
{
    public class PixelContainer
    {
        public Pixel Pixel;
        public readonly Vector2Int Coordinate;

        public bool IsEmpty => Pixel == null;

        public PixelContainer(Vector2Int coordinate)
        {
            Coordinate = coordinate;
        }
    }
}
