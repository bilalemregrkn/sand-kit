using UnityEngine;

namespace SandFall
{
    [CreateAssetMenu(fileName = "SandSetting", menuName = "SandFall/SandSetting")]
    public class SandSetting : ScriptableObject
    {
        [Header("Grid")]
        public int width  = 128;
        public int height = 128;

        [Header("Simulation")]
        public int  stepsPerFixedUpdate = 1;
        public bool enableDiagonalSpread = true;

        [Header("Rendering")]
        public Color emptyPixelColor = Color.clear;
    }
}
