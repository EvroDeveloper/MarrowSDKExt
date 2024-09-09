#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using SLZ.Marrow.Circuits;

namespace SLZ.MarrowEditor
{
    [CustomEditor(typeof(FlipflopCircuit))]
    public class FlipflopCircuitEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            string VISUALTREE_PATH = AssetDatabase.GUIDToAssetPath("85f0b6f32b6e2a04e97a9bfeb7024fa4");
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
                Application.OpenURL("https://github.com/StressLevelZero/MarrowSDK/wiki/FlipFlopCircuit");
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
