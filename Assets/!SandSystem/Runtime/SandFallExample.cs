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

        private RectTransform _displayRect;
        private Color         _currentColor;

        public void SetActiveSetting(BrushSetting setting)
        {
            _activeSetting = setting;
            _brush = setting.CreateBrush();
            Debug.Log($"[SandFallExample] Brush set — type:{setting.brushType} radius:{setting.brushRadius}");
        }

        private void Start()
        {
            if (controller == null)          { Debug.LogError("[SandFallExample] Controller is not assigned."); return; }
            if (controller.settings == null) { Debug.LogError("[SandFallExample] SandSetting is not assigned on controller."); return; }

            if (controller.displayTarget != null)
                _displayRect = controller.displayTarget.rectTransform;
            else
                Debug.LogWarning("[SandFallExample] displayTarget not assigned on controller.");

            _currentColor = RandomColor();

            if (_brush == null)
                _brush = new FreeformBrush { Radius = 3 };

            Debug.Log("[SandFallExample] Ready.");
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (!IsPointerOverDisplay()) return;

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
            if (!isSprite && Mouse.current.leftButton.isPressed && IsPointerOverDisplay())
                PaintAtMouse();

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                controller.Clear();
                Debug.Log("[SandFallExample] Grid cleared.");
            }
        }

        private bool IsPointerOverDisplay()
        {
            if (_displayRect == null) return false;

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Canvas canvas = _displayRect.GetComponentInParent<Canvas>();
            Camera cam = (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                ? canvas.worldCamera
                : null;

            return RectTransformUtility.RectangleContainsScreenPoint(_displayRect, mousePos, cam);
        }

        private void PaintAtMouse()
        {
            if (_displayRect == null || _brush == null) return;

            Vector2 mousePos = Mouse.current.position.ReadValue();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _displayRect, mousePos, null, out Vector2 local);

            Rect rect = _displayRect.rect;
            float nx = (local.x - rect.xMin) / rect.width;
            float ny = (local.y - rect.yMin) / rect.height;

            SandGrid grid = controller.Simulation.Grid;
            int cx = Mathf.RoundToInt(nx * (grid.Width  - 1));
            int cy = Mathf.RoundToInt(ny * (grid.Height - 1));

            _brush.Paint(controller, cx, cy, _currentColor);
        }

        private static Color RandomColor() =>
            Color.HSVToRGB(Random.value, Random.Range(0.6f, 1f), Random.Range(0.8f, 1f));
    }
}
