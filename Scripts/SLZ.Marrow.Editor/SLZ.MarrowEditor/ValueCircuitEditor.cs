#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using SLZ.Marrow.Circuits;

namespace SLZ.MarrowEditor
{
    [CustomEditor(typeof(ValueCircuit))]
    public class ValueCircuitEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            string VISUALTREE_PATH = AssetDatabase.GUIDToAssetPath("1a1e3948fb7ac484b8ace6d8ed8ec753");
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
                Application.OpenURL("https://github.com/StressLevelZero/MarrowSDK/wiki/ValueCircuit");
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
