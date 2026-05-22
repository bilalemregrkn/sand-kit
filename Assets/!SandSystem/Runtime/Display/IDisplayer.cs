using UnityEngine;

namespace SandFall
{
    public interface IDisplayer
    {
        void SetTexture(Texture2D texture);
        bool IsPointerOver(Vector2 screenPos);
        bool TryGetUV(Vector2 screenPos, out Vector2 uv);
    }
}
