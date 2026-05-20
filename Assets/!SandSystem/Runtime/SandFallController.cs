using UnityEngine;
using UnityEngine.UI;

namespace SandFall
{
    [RequireComponent(typeof(SandRenderer))]
    public class SandFallController : MonoBehaviour
    {
        [SerializeField] public SandSetting settings;
        [SerializeField] public RawImage displayTarget;

        private SandSimulation _simulation;
        private SandRenderer   _renderer;

        public SandSimulation Simulation => _simulation;

        public bool EnableDiagonalSpread
        {
            get => _simulation.EnableDiagonalSpread;
            set => _simulation.EnableDiagonalSpread = value;
        }

        private void Awake()
        {
            _simulation = new SandSimulation(settings.width, settings.height)
            {
                EnableDiagonalSpread = settings.enableDiagonalSpread
            };

            _renderer = GetComponent<SandRenderer>();
            _renderer.Initialize(settings.width, settings.height);

            if (displayTarget != null)
                displayTarget.texture = _renderer.Texture;
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < settings.stepsPerFixedUpdate; i++)
                _simulation.Step();

            _renderer.Render(_simulation.Grid);
        }

        public void Spawn(int x, int y, Color color) => _simulation.Spawn(x, y, color);

        public void Clear() => _simulation.Clear();
    }
}
