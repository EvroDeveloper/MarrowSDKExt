#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using SLZ.Marrow.Circuits;

namespace SLZ.MarrowEditor
{
    [CustomEditor(typeof(MaterialSwitchActuator))]
    public class MaterialSwitchActuatorEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            string VISUALTREE_PATH = AssetDatabase.GUIDToAssetPath("2a8c11288f314fd459446a15cc3770dd");
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
                Application.OpenURL("https://github.com/StressLevelZero/MarrowSDK/wiki/MaterialSwitchActuator");
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
