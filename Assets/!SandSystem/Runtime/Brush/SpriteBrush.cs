using UnityEngine;

namespace SandFall
{
    public class SpriteBrush : IBrush
    {
        public int Radius = 3;

        private Color[] _spritePixels;
        private int     _spriteWidth;
        private int     _spriteHeight;

        public void SetSprite(Sprite sprite)
        {
            if (sprite == null) { _spritePixels = null; return; }

            Texture2D tex = sprite.texture;
            Rect r = sprite.textureRect;
            _spriteWidth  = Mathf.RoundToInt(r.width);
            _spriteHeight = Mathf.RoundToInt(r.height);
            _spritePixels = tex.GetPixels(
                Mathf.RoundToInt(r.x), Mathf.RoundToInt(r.y),
                _spriteWidth, _spriteHeight);
        }

        public void Paint(SandFallController controller, int cx, int cy, Color color)
        {
            if (_spritePixels == null) return;

            int originX = cx - _spriteWidth  / 2;
            int originY = cy - _spriteHeight / 2;

            for (int py = 0; py < _spriteHeight; py++)
                for (int px = 0; px < _spriteWidth; px++)
                {
                    Color c = _spritePixels[px + py * _spriteWidth];
                    if (c.a < 0.1f) continue;
                    controller.Spawn(originX + px, originY + py, c);
                }
        }
    }
}
