using UnityEngine;

namespace SandFall
{
    public class SandRenderer : MonoBehaviour
    {
        private Texture2D _texture;
        private Color32[] _buffer;

        public Texture2D Texture => _texture;

        public void Initialize(int width, int height)
        {
            _texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode   = TextureWrapMode.Clamp
            };
            _buffer = new Color32[width * height];
        }

        private Color32 _emptyColor;

        public void SetEmptyColor(Color color) => _emptyColor = (Color32)color;

        public void Render(SandGrid grid)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    PixelContainer cell = grid.Get(x, y);
                    _buffer[x + y * grid.Width] = cell.IsEmpty ? _emptyColor : (Color32)cell.Pixel.Color;
                }
            }

            _texture.SetPixels32(_buffer);
            _texture.Apply(false);
        }
    }
}
