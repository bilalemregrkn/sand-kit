using UnityEngine;
using UnityEngine.UI;

namespace SandFall
{
    /// Attach to any GameObject. Wire controller in the Inspector (done automatically
    /// by SandFall/Create Scene Setup). Left-click to spawn sand, Space to clear.
    public class SandFallExample : MonoBehaviour
    {
        [SerializeField] private SandFallController controller;
        [SerializeField] private Color sandColor = new Color(0.94f, 0.85f, 0.55f);
        [SerializeField] private int spawnRadius = 3;

        private RectTransform _displayRect;

        private void Start()
        {
            if (controller.displayTarget != null)
                _displayRect = controller.displayTarget.rectTransform;
            else
                Debug.LogWarning("[SandFall] SandFallExample: displayTarget not assigned on controller.");
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
                SpawnAtMouse();

            if (Input.GetKeyDown(KeyCode.Space))
                controller.Clear();
        }

        private void SpawnAtMouse()
        {
            if (_displayRect == null) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _displayRect, Input.mousePosition, null, out Vector2 local);

            Rect rect = _displayRect.rect;
            float nx = (local.x - rect.xMin) / rect.width;
            float ny = (local.y - rect.yMin) / rect.height;

            SandGrid grid = controller.Simulation.Grid;
            int cx = Mathf.RoundToInt(nx * (grid.Width  - 1));
            int cy = Mathf.RoundToInt(ny * (grid.Height - 1));

            for (int dx = -spawnRadius; dx <= spawnRadius; dx++)
                for (int dy = -spawnRadius; dy <= spawnRadius; dy++)
                    controller.Spawn(cx + dx, cy + dy, sandColor);
        }
    }
}
