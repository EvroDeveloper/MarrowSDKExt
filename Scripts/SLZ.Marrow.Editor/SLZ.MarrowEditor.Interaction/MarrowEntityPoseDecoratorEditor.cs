 
 
 
using SLZ.Marrow;
 
 
using SLZ.Marrow.Utilities;
using SLZ.Marrow.Warehouse;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SLZ.MarrowEditor.Interaction
{
    public class MarrowEntityPoseDecoratorEditor
    {
        [CustomEditor(typeof(MarrowEntityPoseDecorator))]
        [CanEditMultipleObjects]
        public class MarrowEntityPoseDecoratorEditorInspector : Editor
        {
            private PropertyField propField;
            private ScannableReferenceElement scannableReferenceElement;
            private ScannableSelector.ScannableSelectorFilter scannableSelectorFilter;
            public override VisualElement CreateInspectorGUI()
            {
                string VISUALTREE_PATH = "Packages/"+MarrowSDK.PACKAGE_NAME+"/Editor/Assets/EditorStyleSheets/MarrowEntityPoseDecorator.uxml";
                VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(VISUALTREE_PATH);
                VisualElement tree = visualTree.Instantiate();
                tree.Bind(serializedObject);
                if (targets.Length == 1)
                {
                    propField = tree.Q<PropertyField>("MarrowEntityPose");
                    propField.RegisterCallback<MouseOverEvent>(evt =>
                    {
                        scannableReferenceElement = propField.Q<VisualElement>("ScannableReferenceElement") as ScannableReferenceElement;
                        scannableReferenceElement.onOpenSelector -= OnOpenSelector;
                        scannableReferenceElement.onOpenSelector += OnOpenSelector;
                        OnOpenSelector();
                    });
                }

                return tree;
            }

            private void OnDestroy()
            {
                if (scannableReferenceElement != null)
                    scannableReferenceElement.onOpenSelector -= OnOpenSelector;
            }

            private void OnOpenSelector()
            {
                if (target is MarrowEntityPoseDecorator poseDecorator && poseDecorator.CrateSpawner != null)
                {
                    scannableReferenceElement.SetScannableQueryFilter(new ScannableSelector.ScannableSelectorFilter("ep", scannable =>
                    {
                        if (scannable is EntityPose entityPose)
                        {
                            return entityPose.Spawnable.Barcode.ToString();
                        }

                        if (scannable.Title == "None")
                        {
                            return poseDecorator.CrateSpawner.spawnableCrateReference.Barcode.ToString();
                        }

                        return string.Empty;
                    }, poseDecorator.CrateSpawner.spawnableCrateReference.Barcode.ID));
                }
            }
        }

        [DrawGizmo(GizmoType.Active | GizmoType.Selected | GizmoType.NonSelected)]
        private static void DrawPreviewGizmo(MarrowEntityPoseDecorator decorator, GizmoType gizmoType)
        {
            bool gizmoInRange = Camera.current != null && Vector3.Dot(decorator.transform.position - Camera.current.transform.position, Camera.current.transform.forward) < CrateSpawner.gizmoVisRange;
            if (!Application.isPlaying && decorator.gameObject.scene != default)
            {
                if (decorator.MarrowEntityPose.TryGetDataCard(out var entityPose))
                {
                    if (decorator.CrateSpawner.showPreviewGizmo)
                        decorator.CrateSpawner.showPreviewGizmo = false;
                    if (CrateSpawner.showLitMaterialPreview && CrateSpawner.defaultLitMat == null)
                    {
                        CrateSpawner.defaultLitMat = AssetDatabase.LoadAssetAtPath<Material>("Packages/com.unity.render-pipelines.universal/Runtime/Materials/Lit.mat");
                    }

                    if (decorator != null && entityPose != null)
                    {
                        Bounds bounds = CrateSpawner.showColliderBounds && gizmoInRange ? entityPose.ColliderBounds : default;
                        Mesh previewMesh = null;
                        var editorAsset = entityPose.PosePreviewMesh.EditorAsset;
                        if (editorAsset)
                        {
                            previewMesh = editorAsset;
                        }
                        else if (entityPose.PosePreviewMesh.Asset)
                        {
                            previewMesh = entityPose.PosePreviewMesh.Asset;
                        }
                        else if (!entityPose.PosePreviewMesh.Asset)
                        {
                            PreviewMeshLoadQueue.QueueLoadMesh(entityPose.PosePreviewMesh);
                        }

                        EditorMeshGizmo.Draw("PosePreviewMesh", decorator.gameObject, previewMesh, CrateSpawner.showLitMaterialPreview ? CrateSpawner.defaultLitMat : MarrowSDK.VoidMaterial, bounds, true);
                    }
                }
                else
                {
                    if (!decorator.CrateSpawner.showPreviewGizmo)
                        decorator.CrateSpawner.showPreviewGizmo = true;
                }
            }
        }
    }
}