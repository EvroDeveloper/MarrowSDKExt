#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using SLZ.Marrow.Circuits;

namespace SLZ.MarrowEditor
{
    [CustomEditor(typeof(AddCircuit))]
    public class AddCircuitEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            string VISUALTREE_PATH = AssetDatabase.GUIDToAssetPath("21f31679846b59f4c82cbe6766ce28b4");
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
                Application.OpenURL("https://github.com/StressLevelZero/MarrowSDK/wiki/AddCircuit");
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
