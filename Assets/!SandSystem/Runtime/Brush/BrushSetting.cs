using UnityEngine;

namespace SandFall
{
    [CreateAssetMenu(fileName = "BrushSetting", menuName = "SandFall/BrushSetting")]
    public class BrushSetting : ScriptableObject
    {
        [Header("Brush")]
        public BrushType brushType   = BrushType.Freeform;
        public int       brushRadius = 3;
        public SpritePack spritePack;

        [Header("Button Colors")]
        public Color activeColor   = new Color(1f, 0.85f, 0.2f);
        public Color inactiveColor = Color.white;

        [Header("Cursor")]
        [Tooltip("Texture2D used as the cursor when this brush is active. Read/Write must be enabled. Recommended 32x32 or 64x64.")]
        public Texture2D cursorTexture;
        [Tooltip("Normalized hotspot (0,0) = bottom-left, (1,1) = top-right.")]
        public Vector2 cursorHotspot = Vector2.zero;
        [Tooltip("Scale multiplier for the cursor texture. 1 = original size.")]
        public float cursorScale = 1f;

        public IBrush CreateBrush()
        {
            switch (brushType)
            {
                case BrushType.Sprite:
                    var sb = new SpriteBrush();
                    sb.SetSprite(spritePack?.Random());
                    return sb;
                case BrushType.Erase:
                    return new EraseBrush { Radius = brushRadius };
                default:
                    return new FreeformBrush { Radius = brushRadius };
            }
        }
    }
}
