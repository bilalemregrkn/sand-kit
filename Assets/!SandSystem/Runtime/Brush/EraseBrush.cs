using UnityEngine;

namespace SandFall
{
    public class EraseBrush : IBrush
    {
        public int Radius = 3;

        public void Paint(SandFallController controller, int cx, int cy, Color color)
        {
            SandGrid grid = controller.Simulation.Grid;
            for (int dx = -Radius; dx <= Radius; dx++)
                for (int dy = -Radius; dy <= Radius; dy++)
                    if (dx * dx + dy * dy <= Radius * Radius)
                        grid.Set(cx + dx, cy + dy, null);
        }
    }
}
