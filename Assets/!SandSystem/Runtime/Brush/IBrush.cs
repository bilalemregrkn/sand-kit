using UnityEngine;

namespace SandFall
{
    public interface IBrush
    {
        void Paint(SandFallController controller, int cx, int cy, Color color);
    }
}
