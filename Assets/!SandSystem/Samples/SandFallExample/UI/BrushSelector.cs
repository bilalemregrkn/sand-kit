using UnityEngine;

namespace SandFall
{
    /// Place on any GameObject. Assign all BrushButtons and the SandFallExample.
    public class BrushSelector : MonoBehaviour
    {
        [SerializeField] private BrushButton[]   buttons;
        [SerializeField] private SandFallExample example;

        public BrushType ActiveBrushType { get; private set; }

        private Texture2D _scaledCursor;

        private void Start()
        {
            if (buttons == null || buttons.Length == 0)
            {
                Debug.LogError("[BrushSelector] No buttons assigned.");
                return;
            }

            if (example == null)
            {
                Debug.LogError("[BrushSelector] SandFallExample is not assigned.");
                return;
            }

            Debug.Log($"[BrushSelector] Registering {buttons.Length} brush button(s).");

            foreach (BrushButton btn in buttons)
            {
                if (btn == null) { Debug.LogWarning("[BrushSelector] A button slot is null — skipping."); continue; }
                btn.RegisterSelector(this);
            }

            Select(buttons[0]);
        }

        public void Select(BrushButton chosen)
        {
            if (chosen == null) { Debug.LogWarning("[BrushSelector] Select called with null button."); return; }

            ActiveBrushType = chosen.BrushType;
            example.SetActiveSetting(chosen.Setting);

            foreach (BrushButton btn in buttons)
                if (btn != null) btn.SetActive(btn == chosen);

            Debug.Log($"[BrushSelector] Active brush → {chosen.BrushType}");
            ApplyCursor(chosen);
        }

        private void OnDestroy()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            if (_scaledCursor != null) Destroy(_scaledCursor);
        }

        private void ApplyCursor(BrushButton chosen)
        {
            if (chosen.CursorTexture == null)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                return;
            }

            Texture2D src   = chosen.CursorTexture;
            float     scale = Mathf.Max(0.01f, chosen.Setting.cursorScale);
            int       w     = Mathf.RoundToInt(src.width  * scale);
            int       h     = Mathf.RoundToInt(src.height * scale);

            // Destroy previous scaled copy to avoid leaking textures.
            if (_scaledCursor != null) Destroy(_scaledCursor);

            if (Mathf.Approximately(scale, 1f))
            {
                _scaledCursor = null;
            }
            else
            {
                // Blit src into a RenderTexture at the new size, then read back.
                RenderTexture rt = RenderTexture.GetTemporary(w, h, 0, RenderTextureFormat.ARGB32);
                Graphics.Blit(src, rt);
                RenderTexture prev = RenderTexture.active;
                RenderTexture.active = rt;
                _scaledCursor = new Texture2D(w, h, TextureFormat.ARGB32, false);
                _scaledCursor.ReadPixels(new Rect(0, 0, w, h), 0, 0);
                _scaledCursor.Apply(false);
                RenderTexture.active = prev;
                RenderTexture.ReleaseTemporary(rt);
            }

            Texture2D cursor = _scaledCursor != null ? _scaledCursor : src;
            Vector2   n      = chosen.CursorHotspot;
            Vector2   pixel  = new Vector2(n.x * cursor.width, (1f - n.y) * cursor.height);
            Cursor.SetCursor(cursor, pixel, CursorMode.Auto);
        }
    }
}
