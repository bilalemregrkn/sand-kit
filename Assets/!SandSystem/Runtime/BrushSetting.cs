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
        [Tooltip("Pixel in the cursor that acts as the click point.")]
        public Vector2   cursorHotspot = Vector2.zero;
    }
}
