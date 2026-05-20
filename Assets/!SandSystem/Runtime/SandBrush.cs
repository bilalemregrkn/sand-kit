using UnityEngine;

namespace SandFall
{
    /// Applies pixels to a SandGrid according to the active BrushType.
    public class SandBrush
    {
        public BrushType BrushType;
        public int       Radius = 3;

        // Sprite brush data — set via SetSprite().
        private Color[]  _spritePixels;
        private int      _spriteWidth;
        private int      _spriteHeight;

        // ── Freeform ──────────────────────────────────────────────────────────

        /// Paint a filled circle of one colour at (cx, cy).
        public void PaintFreeform(SandFallController controller, int cx, int cy, Color color)
        {
            for (int dx = -Radius; dx <= Radius; dx++)
                for (int dy = -Radius; dy <= Radius; dy++)
                    if (dx * dx + dy * dy <= Radius * Radius)
                        controller.Spawn(cx + dx, cy + dy, color);
        }

        // ── Sprite ────────────────────────────────────────────────────────────

        /// Call once to upload a sprite's texture into the brush.
        public void SetSprite(Sprite sprite)
        {
            if (sprite == null)
            {
                _spritePixels = null;
                return;
            }

            Texture2D tex = sprite.texture;

            // Extract the rect that belongs to this sprite (handles sprite atlases).
            Rect r = sprite.textureRect;
            _spriteWidth  = Mathf.RoundToInt(r.width);
            _spriteHeight = Mathf.RoundToInt(r.height);
            _spritePixels = tex.GetPixels(
                Mathf.RoundToInt(r.x),
                Mathf.RoundToInt(r.y),
                _spriteWidth,
                _spriteHeight);
        }

        /// Stamp the sprite centred at (cx, cy).
        /// Transparent pixels (alpha < 0.1) are skipped.
        /// Each opaque pixel is tinted by `tint`.
        public void PaintSprite(SandFallController controller, int cx, int cy)
        {
            if (_spritePixels == null) return;

            int originX = cx - _spriteWidth  / 2;
            int originY = cy - _spriteHeight / 2;

            for (int py = 0; py < _spriteHeight; py++)
            {
                for (int px = 0; px < _spriteWidth; px++)
                {
                    Color c = _spritePixels[px + py * _spriteWidth];
                    if (c.a < 0.1f) continue;
                    controller.Spawn(originX + px, originY + py, c);
                }
            }
        }

        // ── Erase ─────────────────────────────────────────────────────────────

        public void PaintErase(SandFallController controller, int cx, int cy)
        {
            SandGrid grid = controller.Simulation.Grid;
            for (int dx = -Radius; dx <= Radius; dx++)
                for (int dy = -Radius; dy <= Radius; dy++)
                    if (dx * dx + dy * dy <= Radius * Radius)
                        grid.Set(cx + dx, cy + dy, null);
        }

        // ── Unified entry point ───────────────────────────────────────────────

        public void Paint(SandFallController controller, int cx, int cy, Color color)
        {
            switch (BrushType)
            {
                case BrushType.Freeform: PaintFreeform(controller, cx, cy, color); break;
                case BrushType.Sprite:   PaintSprite  (controller, cx, cy);        break;
                case BrushType.Erase:    PaintErase   (controller, cx, cy);        break;
            }
        }
    }
}
