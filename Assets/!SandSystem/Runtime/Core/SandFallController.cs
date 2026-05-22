using UnityEngine;

namespace SandFall
{
    [RequireComponent(typeof(SandRenderer))]
    public class SandFallController : MonoBehaviour
    {
        [SerializeField] private SandSetting  _settings;
        [SerializeField] private DisplayerBase _displayer;

        public SandSetting   Settings  => _settings;
        public DisplayerBase Displayer => _displayer;

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
            _simulation = new SandSimulation(_settings.width, _settings.height)
            {
                EnableDiagonalSpread = _settings.enableDiagonalSpread
            };

            _renderer = GetComponent<SandRenderer>();
            _renderer.Initialize(_settings.width, _settings.height);
            _renderer.SetEmptyColor(_settings.emptyPixelColor);

            if (_displayer != null)
                _displayer.SetTexture(_renderer.Texture);
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _settings.stepsPerFixedUpdate; i++)
                _simulation.Step();

            _renderer.Render(_simulation.Grid);
        }

        public void Spawn(int x, int y, Color color) => _simulation.Spawn(x, y, color);

        public void Clear() => _simulation.Clear();
    }
}
