namespace SandFall
{
    public class SandGrid
    {
        public readonly int Width;
        public readonly int Height;

        private readonly PixelContainer[] _cells;

        public SandGrid(int width, int height)
        {
            Width = width;
            Height = height;
            _cells = new PixelContainer[width * height];

            for (int i = 0; i < _cells.Length; i++)
                _cells[i] = new PixelContainer();
        }

        public bool InBounds(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

        public PixelContainer Get(int x, int y)
        {
            if (!InBounds(x, y)) return null;
            return _cells[x + y * Width];
        }

        internal PixelContainer GetUnchecked(int x, int y) => _cells[x + y * Width];

        public void Set(int x, int y, Pixel? pixel)
        {
            if (!InBounds(x, y)) return;
            _cells[x + y * Width].Pixel = pixel;
        }
    }
}
