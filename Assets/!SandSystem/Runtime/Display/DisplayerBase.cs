using UnityEngine;

namespace SandFall
{
    public abstract class DisplayerBase : MonoBehaviour, IDisplayer
    {
        public abstract void SetTexture(Texture2D texture);
        public abstract bool IsPointerOver(Vector2 screenPos);
        public abstract bool TryGetUV(Vector2 screenPos, out Vector2 uv);
    }
}
