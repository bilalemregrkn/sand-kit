using UnityEngine;
using UnityEngine.UI;

namespace SandFall
{
    /// Add this to a UI Button GameObject. Assign a BrushSetting asset in the Inspector.
    [RequireComponent(typeof(Button))]
    public class BrushButton : MonoBehaviour
    {
        [SerializeField] public BrushSetting brushSetting;

        public BrushType  BrushType     => brushSetting.brushType;
        public Texture2D  CursorTexture => brushSetting.cursorTexture;
        public Vector2    CursorHotspot => brushSetting.cursorHotspot;
        public BrushSetting Setting     => brushSetting;

        private Button _button;
        private Image  _image;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _image  = GetComponent<Image>();

            if (_button       == null) Debug.LogError($"[BrushButton] No Button component on {gameObject.name}.");
            if (_image        == null) Debug.LogWarning($"[BrushButton] No Image component on {gameObject.name} — highlight won't work.");
            if (brushSetting  == null) Debug.LogError($"[BrushButton] No BrushSetting assigned on {gameObject.name}.");
        }

        public void RegisterSelector(BrushSelector selector)
        {
            _button.onClick.AddListener(() =>
            {
                Debug.Log($"[BrushButton] Clicked → {brushSetting.brushType}");
                selector.Select(this);
            });

            Debug.Log($"[BrushButton] Registered '{brushSetting.brushType}' button on {gameObject.name}.");
        }

        public void SetActive(bool active)
        {
            if (_image != null)
                _image.color = active ? brushSetting.activeColor : brushSetting.inactiveColor;
        }
    }
}
