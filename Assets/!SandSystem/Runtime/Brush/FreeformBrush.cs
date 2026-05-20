using UnityEngine;

namespace SandFall
{
    public class FreeformBrush : IBrush
    {
        public int Radius = 3;

        public void Paint(SandFallController controller, int cx, int cy, Color color)
        {
            for (int dx = -Radius; dx <= Radius; dx++)
                for (int dy = -Radius; dy <= Radius; dy++)
                    if (dx * dx + dy * dy <= Radius * Radius)
                        controller.Spawn(cx + dx, cy + dy, color);
        }
    }
}
