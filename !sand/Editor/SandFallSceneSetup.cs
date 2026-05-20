#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using SandFall;

namespace SandFall.Editor
{
    public static class SandFallSceneSetup
    {
        [MenuItem("SandFall/Create Scene Setup")]
        public static void CreateSceneSetup()
        {
            // ── Canvas ────────────────────────────────────────────────────────
            GameObject canvasGO = new GameObject("SandFallCanvas");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            // ── RawImage (full-screen display) ────────────────────────────────
            GameObject rawImageGO = new GameObject("SandDisplay");
            rawImageGO.transform.SetParent(canvasGO.transform, false);

            RawImage rawImage = rawImageGO.AddComponent<RawImage>();
            RectTransform rt = rawImage.rectTransform;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            // ── SandFall controller object ────────────────────────────────────
            GameObject controllerGO = new GameObject("SandFallController");
            SandFallController controller = controllerGO.AddComponent<SandFallController>();
            controllerGO.AddComponent<SandRenderer>();

            // Wire the display target via SerializedObject so it shows in Inspector.
            SerializedObject so = new SerializedObject(controller);
            so.FindProperty("displayTarget").objectReferenceValue = rawImage;
            so.ApplyModifiedPropertiesWithoutUndo();

            // ── Example spawner ───────────────────────────────────────────────
            GameObject spawnerGO = new GameObject("SandSpawner");
            SandFallExample example = spawnerGO.AddComponent<SandFallExample>();

            SerializedObject exSo = new SerializedObject(example);
            exSo.FindProperty("controller").objectReferenceValue = controller;
            exSo.ApplyModifiedPropertiesWithoutUndo();

            // ── Select the controller in the hierarchy ────────────────────────
            Selection.activeGameObject = controllerGO;

            Debug.Log("[SandFall] Scene setup complete. Hit Play, left-click to spawn sand, Space to clear.");
        }
    }
}
#endif
