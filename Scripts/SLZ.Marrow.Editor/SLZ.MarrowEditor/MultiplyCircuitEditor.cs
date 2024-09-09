#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using SLZ.Marrow.Circuits;

namespace SLZ.MarrowEditor
{
    [CustomEditor(typeof(MultiplyCircuit))]
    public class MultiplyCircuitEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            string VISUALTREE_PATH = AssetDatabase.GUIDToAssetPath("f6fa257b8f65b2a4296c1410e0f6b126");
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(VISUALTREE_PATH);
            VisualElement tree = visualTree.Instantiate();
            VisualElement validationFeedback = tree.Q<VisualElement>("validationFeedback");
            IMGUIContainer imguiValidationContainer = tree.Q<IMGUIContainer>("imguiValidationContainer");
            imguiValidationContainer.onGUIHandler = () =>
            {
            };
            Button marrowDocsButton = tree.Q<Button>("marrowDocsButton");
            marrowDocsButton.clickable.clicked += () =>
            {
                Application.OpenURL("https://github.com/StressLevelZero/MarrowSDK/wiki/MultiplyCircuit");
            };
            IMGUIContainer imguiInspector = tree.Q<IMGUIContainer>("imguiInspector");
            imguiInspector.onGUIHandler = () =>
            {
                DrawDefaultInspector();
            };
            return tree;
        }
    }
}
#endif
