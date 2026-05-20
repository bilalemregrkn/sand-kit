using UnityEngine;

namespace SandFall
{
    /// Place on any GameObject. Assign all BrushButtons and the SandFallExample.
    public class BrushSelector : MonoBehaviour
    {
        [SerializeField] private BrushButton[]   buttons;
        [SerializeField] private SandFallExample example;

        public BrushType ActiveBrushType { get; private set; }

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

            ApplyCursor(chosen);

            Debug.Log($"[BrushSelector] Active brush → {chosen.BrushType}");
        }

        private void OnDestroy()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        private static void ApplyCursor(BrushButton chosen)
        {
            if (chosen.CursorTexture != null)
            {
                Cursor.SetCursor(chosen.CursorTexture, chosen.CursorHotspot, CursorMode.Auto);
                Debug.Log($"[BrushSelector] Cursor set to '{chosen.CursorTexture.name}' (hotspot {chosen.CursorHotspot}).");
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                Debug.LogWarning($"[BrushSelector] No cursor texture on '{chosen.gameObject.name}' — using default cursor.");
            }
        }
    }
}
