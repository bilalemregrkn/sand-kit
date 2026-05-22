using UnityEngine;

namespace SandFall
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class QuadDisplayer : DisplayerBase
    {
        [SerializeField] private Camera _camera;

        private MeshRenderer _meshRenderer;
        private MeshCollider _meshCollider;

        private void EnsureComponents()
        {
            if (_meshRenderer != null) return;
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshCollider = GetComponent<MeshCollider>();
            if (_camera == null)
                _camera = Camera.main;
        }

        public override void SetTexture(Texture2D texture)
        {
            EnsureComponents();
            _meshRenderer.material.mainTexture = texture;
        }

        public override bool IsPointerOver(Vector2 screenPos)
        {
            EnsureComponents();
            Ray ray = _camera.ScreenPointToRay(screenPos);
            return _meshCollider.Raycast(ray, out _, Mathf.Infinity);
        }

        public override bool TryGetUV(Vector2 screenPos, out Vector2 uv)
        {
            EnsureComponents();
            uv = Vector2.zero;
            Ray ray = _camera.ScreenPointToRay(screenPos);

            if (!_meshCollider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                return false;

            uv = hit.textureCoord;
            return true;
        }
    }
}
