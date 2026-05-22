using UnityEngine;
using UnityEngine.UI;

namespace SandFall
{
    [RequireComponent(typeof(RawImage))]
    public class RawImageDisplayer : DisplayerBase
    {
        private RawImage      _rawImage;
        private RectTransform _rect;

        private void EnsureComponents()
        {
            if (_rawImage != null) return;
            _rawImage = GetComponent<RawImage>();
            _rect     = _rawImage.rectTransform;
        }

        public override void SetTexture(Texture2D texture)
        {
            EnsureComponents();
            _rawImage.texture = texture;
        }

        public override bool IsPointerOver(Vector2 screenPos)
        {
            EnsureComponents();
            Camera cam = GetCanvasCamera();
            return RectTransformUtility.RectangleContainsScreenPoint(_rect, screenPos, cam);
        }

        public override bool TryGetUV(Vector2 screenPos, out Vector2 uv)
        {
            EnsureComponents();
            uv = Vector2.zero;
            Camera cam = GetCanvasCamera();

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, screenPos, cam, out Vector2 local))
                return false;

            Rect rect = _rect.rect;
            uv = new Vector2(
                (local.x - rect.xMin) / rect.width,
                (local.y - rect.yMin) / rect.height);
            return true;
        }

        private Camera GetCanvasCamera()
        {
            Canvas canvas = _rect.GetComponentInParent<Canvas>();
            return (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                ? canvas.worldCamera
                : null;
        }
    }
}
