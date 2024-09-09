using SLZ.Marrow.Utilities;
using SLZ.Marrow.Warehouse;
 
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace SLZ.MarrowEditor
{
    [CustomEditor(typeof(CrateSpawner))]
    [CanEditMultipleObjects]
    public class CrateSpawnerEditor : Editor
    {
        SerializedProperty spawnableCrateReferenceProperty;
        SerializedProperty policyDataProperty;
        SerializedProperty crateQueryProperty;
        SerializedProperty useQueryProperty;
        SerializedProperty manualModeProperty;
        SerializedProperty onPlaceEventProperty;
        private static GUIContent previewMeshGizmoIcon = null;
        private static GUIContent colliderBoundsGizmoIcon = null;
        private static GUIContent materialIconOn = null;
        private static GUIContent materialIconOff = null;
        private CrateSpawner script;
        private static SceneAsset mainSceneAsset;
        private static bool levelIsMultiScene;
        private static SceneAsset spawnerSceneAsset;
        private bool fixtureHasBase = false;
        public virtual void OnEnable()
        {
            EditorApplication.contextualPropertyMenu += OnPropertyContextMenu;
            spawnableCrateReferenceProperty = serializedObject.FindProperty("spawnableCrateReference");
            policyDataProperty = serializedObject.FindProperty("policyData");
            crateQueryProperty = serializedObject.FindProperty("crateQuery");
            useQueryProperty = serializedObject.FindProperty("useQuery");
            manualModeProperty = serializedObject.FindProperty("manualMode");
            onPlaceEventProperty = serializedObject.FindProperty("onSpawnEvent");
            script = (CrateSpawner)target;
            if (previewMeshGizmoIcon == null)
            {
                previewMeshGizmoIcon = new GUIContent(EditorGUIUtility.IconContent("d_GizmosToggle On@2x"));
                previewMeshGizmoIcon.tooltip = "Toggle Preview Mesh Gizmo";
            }

            if (colliderBoundsGizmoIcon == null)
            {
                colliderBoundsGizmoIcon = new GUIContent(EditorGUIUtility.IconContent("d_BoxCollider2D Icon"));
                colliderBoundsGizmoIcon.tooltip = "Toggle Collider Bounds";
            }

            if (materialIconOn == null)
            {
                materialIconOn = new GUIContent(EditorGUIUtility.IconContent("d_Material Icon"));
                materialIconOn.tooltip = "Swap Preview Mesh Material";
            }

            if (materialIconOff == null)
            {
                materialIconOff = new GUIContent(EditorGUIUtility.IconContent("d_Material On Icon"));
                materialIconOff.tooltip = "Swap Preview Mesh Material";
            }

            SceneAsset activeSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorSceneManager.GetActiveScene().path);
            if (activeSceneAsset == null)
            {
                return;
            }

            AssetWarehouse.OnReady(() =>
            {
                if (AssetWarehouse.Instance.EditorObjectCrateLookup.TryGetValue(activeSceneAsset, out Crate crate) && crate is LevelCrate levelCrate)
                {
                    spawnerSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(script.gameObject.scene.path);
                    mainSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GetAssetPath(levelCrate.MainScene.EditorAsset));
                    levelIsMultiScene = levelCrate.MultiScene;
                }

                List<DataCard> awDataCards = AssetWarehouse.Instance.GetDataCards();
                if (awDataCards != null)
                {
                    foreach (DataCard dataCard in awDataCards)
                    {
                        if (dataCard != null && dataCard is Fixture)
                        {
                            Fixture awFixture = (Fixture)dataCard;
                            if (awFixture != null && awFixture.FixtureSpawnable != null)
                            {
                                if (awFixture.FixtureSpawnable.Barcode == script.spawnableCrateReference?.Barcode)
                                {
                                    if (awFixture.StaticFixturePrefab != null && !String.IsNullOrEmpty(awFixture.StaticFixturePrefab.AssetGUID))
                                    {
                                        fixtureHasBase = true;
                                    }
                                }
                            }
                        }
                    }
                }
            });
            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        private void OnHierarchyChanged()
        {
            spawnerSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(script.gameObject.scene.path);
        }

        void OnDestroy()
        {
            EditorApplication.contextualPropertyMenu -= OnPropertyContextMenu;
            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
        }

        public override VisualElement CreateInspectorGUI()
        {
            string VISUALTREE_PATH = AssetDatabase.GUIDToAssetPath("de672589595164d43bfb622a18720ba6");
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(VISUALTREE_PATH);
            VisualElement tree = visualTree.Instantiate();
            VisualElement validationFeedback = tree.Q<VisualElement>("validationFeedback");
            IMGUIContainer imguiValidationContainer = tree.Q<IMGUIContainer>("imguiValidationContainer");
            imguiValidationContainer.onGUIHandler = () =>
            {
                if (levelIsMultiScene && spawnerSceneAsset != mainSceneAsset)
                {
                    EditorGUILayout.HelpBox($"{script.gameObject.name} not in persistent scene!", MessageType.Error);
                }
            };
            Button marrowDocsButton = tree.Q<Button>("marrowDocsButton");
            marrowDocsButton.clickable.clicked += () =>
            {
                Application.OpenURL("https://github.com/StressLevelZero/MarrowSDK/wiki/Spawnables");
            };
            IMGUIContainer imguiInspector = tree.Q<IMGUIContainer>("imguiInspector");
            imguiInspector.onGUIHandler = () =>
            {
                DrawDefaultInspector();
            };
            Toggle useQueryToggle = tree.Q<Toggle>("useQueryToggle");
            PropertyField crateQuery = tree.Q<PropertyField>("crateQuery");
            PropertyField spawnableCrateReference = tree.Q<PropertyField>("spawnableCrateReference");
            spawnableCrateReference.RegisterValueChangeCallback(evt =>
            {
                if (ScannableReference.IsValid(script.spawnableCrateReference))
                {
                    script.EditorUpdateName(true);
                }
            });
            if (serializedObject.FindProperty("policyData") == null)
            {
                tree.Q<PropertyField>("policyData").style.display = DisplayStyle.None;
            }

            useQueryToggle.RegisterValueChangedCallback((evt) =>
            {
                useQueryToggle.style.display = DisplayStyle.None;
                spawnableCrateReference.style.display = DisplayStyle.Flex;
            });
            useQueryToggle.style.display = DisplayStyle.None;
            spawnableCrateReference.style.display = DisplayStyle.Flex;
            IMGUIContainer imguiRuntimeDebugContainer = tree.Q<IMGUIContainer>("imguiRuntimeDebugContainer");
#if false
#endif
            Button selectFixtureStaticButton = tree.Q<Button>("selectFixtureStaticButton");
            selectFixtureStaticButton.clickable.clicked += () =>
            {
                List<GameObject> selObjs = new List<GameObject>();
                Selection.objects = null;
                foreach (var target in targets)
                {
                    CrateSpawner targetCS = (CrateSpawner)target;
                    selObjs.Add(targetCS.gameObject);
                    foreach (Transform gameObjectTrans in UnityEngine.Object.FindObjectsOfType<Transform>())
                    {
                        if (gameObjectTrans.name.ToLower().Contains("spawnpoint"))
                        {
                            if (Vector3.Distance(gameObjectTrans.transform.position, targetCS.transform.position) < 0.05f)
                            {
                                selObjs.Add(gameObjectTrans.transform.parent.gameObject);
                            }
                        }
                    }

                    selObjs.Add(targetCS.gameObject);
                }

                Selection.objects = selObjs.ToArray();
            };
            Button alignFixtureToBaseButton = tree.Q<Button>("alignFixtureToBaseButton");
            alignFixtureToBaseButton.clickable.clicked += () =>
            {
                CrateSpawner targetCS = script;
                GameObject fixtureBaseParent = null;
                if (Selection.gameObjects.Length != 2)
                {
                    Debug.Log($"{Selection.gameObjects.Length}, Select ONLY the CrateSpawner Fixture AND the Fixture Base GameObject, then try again");
                    return;
                }

                foreach (GameObject selObj in Selection.gameObjects)
                {
                    if (selObj != script.gameObject)
                    {
                        fixtureBaseParent = selObj;
                    }
                }

                if (fixtureBaseParent == null)
                {
                    return;
                }

                foreach (Transform gameObjectTrans in fixtureBaseParent.GetComponentsInChildren<Transform>())
                {
                    if (gameObjectTrans.name.ToLower().Contains("spawnpoint"))
                    {
                        targetCS.transform.position = gameObjectTrans.transform.position;
                        targetCS.transform.rotation = gameObjectTrans.transform.rotation;
                    }
                }
            };
            selectFixtureStaticButton.style.display = DisplayStyle.None;
            alignFixtureToBaseButton.style.display = DisplayStyle.None;
            if (fixtureHasBase)
            {
                selectFixtureStaticButton.style.display = DisplayStyle.Flex;
                alignFixtureToBaseButton.style.display = DisplayStyle.Flex;
            }

            ToolbarToggle showPreviewMeshToolbarToggle = tree.Q<ToolbarToggle>("showPreviewMeshToolbarToggle");
            Image showPreviewMeshIconImage = new Image
            {
                image = previewMeshGizmoIcon.image
            };
            showPreviewMeshToolbarToggle.Add(showPreviewMeshIconImage);
            showPreviewMeshToolbarToggle.RegisterValueChangedCallback(evt =>
            {
                CrateSpawner.showPreviewMesh = showPreviewMeshToolbarToggle.value;
                InternalEditorUtility.RepaintAllViews();
            });
            ToolbarToggle showColliderBoundsToolbarToggle = tree.Q<ToolbarToggle>("showColliderBoundsToolbarToggle");
            Image showColliderBoundsIconImage = new Image
            {
                image = colliderBoundsGizmoIcon.image
            };
            showColliderBoundsToolbarToggle.Add(showColliderBoundsIconImage);
            showColliderBoundsToolbarToggle.RegisterValueChangedCallback(evt =>
            {
                CrateSpawner.showColliderBounds = showColliderBoundsToolbarToggle.value;
                InternalEditorUtility.RepaintAllViews();
            });
            ToolbarToggle showLitMaterialPreviewToolbarToggle = tree.Q<ToolbarToggle>("showLitMaterialPreviewToolbarToggle");
            Image showLitMaterialPreviewIconOnImage = new Image
            {
                image = materialIconOn.image
            };
            Image showLitMaterialPreviewIconOffImage = new Image
            {
                image = materialIconOff.image
            };
            showLitMaterialPreviewToolbarToggle.Add(showLitMaterialPreviewIconOnImage);
            showLitMaterialPreviewToolbarToggle.Add(showLitMaterialPreviewIconOffImage);
            showLitMaterialPreviewToolbarToggle.RegisterValueChangedCallback(evt =>
            {
                CrateSpawner.showLitMaterialPreview = showLitMaterialPreviewToolbarToggle.value;
                if (CrateSpawner.showLitMaterialPreview)
                {
                    showLitMaterialPreviewIconOnImage.style.display = DisplayStyle.None;
                    showLitMaterialPreviewIconOffImage.style.display = DisplayStyle.Flex;
                }
                else
                {
                    showLitMaterialPreviewIconOnImage.style.display = DisplayStyle.Flex;
                    showLitMaterialPreviewIconOffImage.style.display = DisplayStyle.None;
                }

                InternalEditorUtility.RepaintAllViews();
            });
            SliderInt gizmoVisRangeSlider = tree.Q<SliderInt>("gizmoVisRangeSlider");
            gizmoVisRangeSlider.RegisterValueChangedCallback(evt =>
            {
                CrateSpawner.gizmoVisRange = gizmoVisRangeSlider.value;
            });
            showPreviewMeshToolbarToggle.SetValueWithoutNotify(CrateSpawner.showPreviewMesh);
            showColliderBoundsToolbarToggle.SetValueWithoutNotify(CrateSpawner.showColliderBounds);
            showLitMaterialPreviewToolbarToggle.SetValueWithoutNotify(CrateSpawner.showLitMaterialPreview);
            if (CrateSpawner.showLitMaterialPreview)
            {
                showLitMaterialPreviewIconOnImage.style.display = DisplayStyle.None;
                showLitMaterialPreviewIconOffImage.style.display = DisplayStyle.Flex;
            }
            else
            {
                showLitMaterialPreviewIconOnImage.style.display = DisplayStyle.Flex;
                showLitMaterialPreviewIconOffImage.style.display = DisplayStyle.None;
            }

            gizmoVisRangeSlider.SetValueWithoutNotify((int)CrateSpawner.gizmoVisRange);
            return tree;
        }

        void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
        {
            if (property.type != "CrateQuery")
                return;
            menu.AddItem(new GUIContent("Run Query"), false, () =>
            {
                foreach (var targetObject in serializedObject.targetObjects)
                {
                    if (targetObject is CrateSpawner spawner)
                    {
                        if (spawner.useQuery)
                            spawner.crateQuery.RunQuery();
                    }
                }
            });
        }

        [MenuItem("GameObject/MarrowSDK/Crate Spawner", priority = 1)]
        private static void MenuCreateSpawner(MenuCommand menuCommand)
        {
            GameObject go = CrateSpawner.EditorCreateCrateSpawner();
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Selection.activeObject = go;
            bool overlayShown = false;
            bool overlayCollapsed = false;
            AWSpawnerOverlayToolbar.DoWithInstances(instance => overlayShown = instance.displayed);
            AWSpawnerOverlayToolbar.DoWithInstances(instance => overlayCollapsed = instance.collapsed);
            if (overlayShown == false)
            {
                if (UnityEngine.Object.FindObjectsOfType<CrateSpawner>().Length == 1)
                {
                    AWSpawnerOverlayToolbar.DoWithInstances(instance => instance.displayed = true);
                    AWSpawnerOverlayToolbar.DoWithInstances(instance => instance.collapsed = false);
                }
            }

            if (overlayCollapsed == true)
            {
                if (UnityEngine.Object.FindObjectsOfType<CrateSpawner>().Length == 1)
                {
                    AWSpawnerOverlayToolbar.DoWithInstances(instance => instance.collapsed = false);
                }
            }
        }

        private static void ShowInChunkSceneWarning(CrateSpawner crateSpawner)
        {
            if (levelIsMultiScene == false)
            {
                return;
            }

            SceneAsset activeSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorSceneManager.GetActiveScene().path);
            if (activeSceneAsset == null)
            {
                return;
            }

            AssetWarehouse.OnReady(() =>
            {
                if (spawnerSceneAsset == null || mainSceneAsset == null)
                {
                    return;
                }

                if (spawnerSceneAsset != mainSceneAsset && AssetDatabase.LoadAssetAtPath<SceneAsset>(crateSpawner.gameObject.scene.path) == spawnerSceneAsset)
                {
                    Gizmos.color = Color.red;
                    DrawGizmoHelper.DrawText($"WARNING: {crateSpawner.gameObject.name} not in persistent scene!", crateSpawner.transform.position);
                }
            });
        }

        [DrawGizmo(GizmoType.Selected)]
        static void DrawSelectedCrateSpawnerGizmo(CrateSpawner crateSpawner, GizmoType gizmoType)
        {
            ShowInChunkSceneWarning(crateSpawner);
        }

        [DrawGizmo(GizmoType.NonSelected)]
        static void DrawNonSelectedCrateSpawnerGizmo(CrateSpawner crateSpawner, GizmoType gizmoType)
        {
            ShowInChunkSceneWarning(crateSpawner);
        }

        [DrawGizmo(GizmoType.Pickable | GizmoType.Selected)]
        static void DrawPickableSelectedCrateSpawnerGizmo(CrateSpawner crateSpawner, GizmoType gizmoType)
        {
            ShowInChunkSceneWarning(crateSpawner);
        }

        [DrawGizmo(GizmoType.Pickable | GizmoType.NonSelected)]
        static void DrawPickableNonSelectedCrateSpawnerGizmo(CrateSpawner crateSpawner, GizmoType gizmoType)
        {
            ShowInChunkSceneWarning(crateSpawner);
        }
    }
}