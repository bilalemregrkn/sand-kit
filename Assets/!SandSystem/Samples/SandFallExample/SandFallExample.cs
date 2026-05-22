using UnityEngine;
using UnityEngine.InputSystem;

namespace SandFall
{
    /// Attach to any GameObject. Wire controller in the Inspector.
    /// Left-click on the canvas to spawn sand, Space to clear.
    public class SandFallExample : MonoBehaviour
    {
        [SerializeField] private SandFallController controller;

        private BrushSetting _activeSetting;
        private IBrush       _brush;
        private Color        _currentColor;

        public void SetActiveSetting(BrushSetting setting)
        {
            _activeSetting = setting;
            _brush = setting.CreateBrush();
            Debug.Log($"[SandFallExample] Brush set — type:{setting.brushType} radius:{setting.brushRadius}");
        }

        private void Start()
        {
            if (controller == null)           { Debug.LogError("[SandFallExample] Controller is not assigned."); return; }
            if (controller.Settings == null) { Debug.LogError("[SandFallExample] SandSetting is not assigned on controller."); return; }

            if (controller.Displayer == null)
                Debug.LogWarning("[SandFallExample] Displayer not assigned on controller.");

            _currentColor = RandomColor();

            if (_brush == null)
                _brush = new FreeformBrush { Radius = 3 };

            Debug.Log("[SandFallExample] Ready.");
        }

        private void Update()
        {
            bool overDisplay = IsPointerOverDisplay();

            if (Mouse.current.leftButton.wasPressedThisFrame && overDisplay)
            {
                _currentColor = RandomColor();

                // Sprite brush: rebuild to pick a fresh random sprite, paint once on click.
                if (_activeSetting != null && _activeSetting.brushType == BrushType.Sprite)
                {
                    _brush = _activeSetting.CreateBrush();
                    PaintAtMouse();
                    return;
                }
            }

            // Freeform and Erase paint while held; Sprite is click-only (handled above).
            bool isSprite = _activeSetting != null && _activeSetting.brushType == BrushType.Sprite;
            if (!isSprite && Mouse.current.leftButton.isPressed && overDisplay)
                PaintAtMouse();

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                controller.Clear();
                Debug.Log("[SandFallExample] Grid cleared.");
            }
        }

        private bool IsPointerOverDisplay()
        {
            if (controller.Displayer == null) return false;
            return controller.Displayer.IsPointerOver(Mouse.current.position.ReadValue());
        }

        private void PaintAtMouse()
        {
            if (controller.Displayer == null || _brush == null) return;

            Vector2 screenPos = Mouse.current.position.ReadValue();
            if (!controller.Displayer.TryGetUV(screenPos, out Vector2 uv)) return;

            SandGrid grid = controller.Simulation.Grid;
            int cx = Mathf.RoundToInt(uv.x * (grid.Width  - 1));
            int cy = Mathf.RoundToInt(uv.y * (grid.Height - 1));

            _brush.Paint(controller, cx, cy, _currentColor);
        }

        private static Color RandomColor() =>
            Color.HSVToRGB(Random.value, Random.Range(0.6f, 1f), Random.Range(0.8f, 1f));
    }
}
