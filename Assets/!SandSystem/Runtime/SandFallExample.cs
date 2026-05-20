using UnityEngine;
using UnityEngine.InputSystem;

namespace SandFall
{
    /// Attach to any GameObject. Wire controller in the Inspector.
    /// Left-click on the canvas to spawn sand, Space to clear.
    public class SandFallExample : MonoBehaviour
    {
        [SerializeField] private SandFallController controller;

        private BrushType    _activeBrushType = BrushType.Freeform;
        private BrushSetting _activeSetting;
        private SandBrush    _brush;

        public BrushType ActiveBrushType
        {
            get => _activeBrushType;
            set
            {
                if (_activeBrushType == value) return;
                _activeBrushType = value;
                RebuildBrush();
                Debug.Log($"[SandFallExample] Brush changed → {value}");
            }
        }

        public void SetActiveSetting(BrushSetting setting)
        {
            _activeSetting   = setting;
            _activeBrushType = setting.brushType;
            RebuildBrush();
            Debug.Log($"[SandFallExample] BrushSetting applied — type:{setting.brushType} radius:{setting.brushRadius}");
        }

        private RectTransform _displayRect;
        private Color         _currentColor;

        private void Start()
        {
            if (controller == null)          { Debug.LogError("[SandFallExample] Controller is not assigned."); return; }
            if (controller.settings == null) { Debug.LogError("[SandFallExample] SandSetting is not assigned on controller."); return; }

            if (controller.displayTarget != null)
                _displayRect = controller.displayTarget.rectTransform;
            else
                Debug.LogWarning("[SandFallExample] displayTarget not assigned on controller.");

            _currentColor = RandomColor();

            // Guarantee a usable brush even if BrushSelector is not wired up yet.
            if (_brush == null)
                _brush = new SandBrush { BrushType = BrushType.Freeform, Radius = 3 };

            Debug.Log("[SandFallExample] Ready.");
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (!IsPointerOverDisplay()) return;

                _currentColor = RandomColor();

                // Pick a fresh random sprite from the pack on each new click.
                if (_activeBrushType == BrushType.Sprite && _activeSetting?.spritePack != null)
                    _brush.SetSprite(_activeSetting.spritePack.Random());
            }

            bool shouldPaint = _activeBrushType == BrushType.Sprite
                ? Mouse.current.leftButton.wasPressedThisFrame
                : Mouse.current.leftButton.isPressed;

            if (shouldPaint && IsPointerOverDisplay())
                PaintAtMouse();

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                controller.Clear();
                Debug.Log("[SandFallExample] Grid cleared.");
            }
        }

        private bool IsPointerOverDisplay()
        {
            if (_displayRect == null)
            {
                Debug.LogWarning("[SandFallExample] _displayRect is null — cannot hit-test.");
                return false;
            }

            Vector2 mousePos = Mouse.current.position.ReadValue();

            // The canvas camera must match the Canvas render mode.
            // ScreenSpaceOverlay uses null; ScreenSpaceCamera/WorldSpace needs the camera.
            Canvas canvas = _displayRect.GetComponentInParent<Canvas>();
            Camera cam = (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                ? canvas.worldCamera
                : null;

            return RectTransformUtility.RectangleContainsScreenPoint(_displayRect, mousePos, cam);
        }

        private void PaintAtMouse()
        {
            if (_displayRect == null || _brush == null)
            {
                Debug.LogWarning($"[SandFallExample] PaintAtMouse skipped — displayRect:{_displayRect != null}  brush:{_brush != null}");
                return;
            }

            Vector2 mousePos = Mouse.current.position.ReadValue();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _displayRect, mousePos, null, out Vector2 local);

            Rect rect = _displayRect.rect;
            float nx = (local.x - rect.xMin) / rect.width;
            float ny = (local.y - rect.yMin) / rect.height;

            SandGrid grid = controller.Simulation.Grid;
            int cx = Mathf.RoundToInt(nx * (grid.Width  - 1));
            int cy = Mathf.RoundToInt(ny * (grid.Height - 1));

            Debug.Log($"[SandFallExample] Paint → grid({cx},{cy})  brush:{_brush.BrushType}  r:{_brush.Radius}");
            _brush.Paint(controller, cx, cy, _currentColor);
        }

        private void RebuildBrush()
        {
            if (_activeSetting == null) return;

            _brush = new SandBrush
            {
                BrushType = _activeBrushType,
                Radius    = _activeSetting.brushRadius,
            };

            if (_activeBrushType == BrushType.Sprite)
                _brush.SetSprite(_activeSetting.spritePack?.Random());

            Debug.Log($"[SandFallExample] Brush built — type:{_activeBrushType} radius:{_activeSetting.brushRadius}");
        }

        private static Color RandomColor()
        {
            return Color.HSVToRGB(Random.value, Random.Range(0.6f, 1f), Random.Range(0.8f, 1f));
        }
    }
}
