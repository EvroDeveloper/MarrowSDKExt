#if UNITY_EDITOR
using SLZ.Marrow.Zones;
 
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SLZ.MarrowEditor
{
    public class LevelUtilitiesEditorWin : EditorWindow
    {
        private VisualElement reflectionprobesParentContainer;
        private VisualElement lightprobesParentContainer;
        private VisualElement volumetricsParentContainer;
        ToolbarToggle reflectionsToolbarToggle;
        ToolbarToggle lightprobesToolbarToggle;
        ToolbarToggle volumetricsToolbarToggle;
        private RaycastHit[] _hitsUp;
        private RaycastHit[] _hitsDown;
        private RaycastHit[] _hitsRight;
        private RaycastHit[] _hitsLeft;
        private RaycastHit[] _hitsForward;
        private RaycastHit[] _hitsBack;
        LayerMask layerCollideAllowed = ~0;
        [MenuItem("Stress Level Zero/LevelTools/Level Utilities (EXPERIMENTAL)", false, 4000)]
        public static void ShowLevelUtilitiesEditorWindow()
        {
            EditorWindow zlWin = GetWindow<LevelUtilitiesEditorWin>();
            zlWin.titleContent = new GUIContent("Level Utils (EXPERIMENTAL)");
            zlWin.minSize = new Vector2(485, 485);
        }

        public void CreateGUI()
        {
            var svm = SceneVisibilityManager.instance;
            string VISUALTREE_PATH = AssetDatabase.GUIDToAssetPath("7125f26f72fc0b748874bd7853c28725");
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(VISUALTREE_PATH);
            VisualElement tree = visualTree.Instantiate();
            tree.StretchToParentSize();
            rootVisualElement.Add(tree);
            reflectionsToolbarToggle = rootVisualElement.Q<ToolbarToggle>("reflectionsToolbarToggle");
            lightprobesToolbarToggle = rootVisualElement.Q<ToolbarToggle>("lightprobesToolbarToggle");
            volumetricsToolbarToggle = rootVisualElement.Q<ToolbarToggle>("volumetricsToolbarToggle");
            reflectionprobesParentContainer = rootVisualElement.Q<VisualElement>("reflectionprobesParentContainer");
            lightprobesParentContainer = rootVisualElement.Q<VisualElement>("lightprobesParentContainer");
            volumetricsParentContainer = rootVisualElement.Q<VisualElement>("volumetricsParentContainer");
            reflectionsToolbarToggle.RegisterValueChangedCallback(evt =>
            {
                ToggleToolbarContainers();
                reflectionprobesParentContainer.style.display = DisplayStyle.Flex;
            });
            lightprobesToolbarToggle.RegisterValueChangedCallback(evt =>
            {
                ToggleToolbarContainers();
                lightprobesParentContainer.style.display = DisplayStyle.Flex;
            });
            volumetricsToolbarToggle.RegisterValueChangedCallback(evt =>
            {
                ToggleToolbarContainers();
                volumetricsParentContainer.style.display = DisplayStyle.Flex;
            });
            ToggleToolbarDefaults();
            Toggle reflectionProbeUtilsModeToggle = rootVisualElement.Q<Toggle>("reflectionProbeUtilsModeToggle");
            reflectionProbeUtilsModeToggle.RegisterValueChangedCallback(evt =>
            {
                List<GameObject> nonReflectionProbeGameObjects = new List<GameObject>();
                foreach (GameObject gameObj in FindObjectsOfType<GameObject>())
                {
                    if (!gameObj.GetComponent<ReflectionProbe>())
                    {
                        nonReflectionProbeGameObjects.Add(gameObj);
                    }
                }

                if (reflectionProbeUtilsModeToggle.value)
                {
                    reflectionProbeUtilsModeToggle.text = "  Only Reflect Probes Scene-Selectable";
                    svm.DisablePicking(nonReflectionProbeGameObjects.ToArray(), false);
                }
                else
                {
                    reflectionProbeUtilsModeToggle.text = "  Disabled: All Objects Scene-Selectable";
                    svm.EnableAllPicking();
                }
            });
            Vector3Field reflectionProbeGeoPadding = rootVisualElement.Q<Vector3Field>("reflectionProbeGeoPadding");
            Button createReflectionProbeFromZonesButton = rootVisualElement.Q<Button>("createReflectionProbeFromZonesButton");
            createReflectionProbeFromZonesButton.clickable.clicked += () =>
            {
                CreateReflectionProbeFromZonesOnClick(reflectionProbeGeoPadding.value);
            };
            Button createDefaultReflectionProbeButton = rootVisualElement.Q<Button>("createDefaultReflectionProbeButton");
            createDefaultReflectionProbeButton.clickable.clicked += () =>
            {
                CreateDefaultReflectionProbeOnClick();
            };
            Button createReflectionProbeFromGeoButton = rootVisualElement.Q<Button>("createReflectionProbeFromGeoButton");
            createReflectionProbeFromGeoButton.clickable.clicked += () =>
            {
                CreateReflectionProbeFromGeoOnClick(reflectionProbeGeoPadding.value);
            };
            Button createReflectionProbeEncapsulatesGeoButton = rootVisualElement.Q<Button>("createReflectionProbeEncapsulatesGeoButton");
            createReflectionProbeEncapsulatesGeoButton.clickable.clicked += () =>
            {
                CreateReflectionProbeEncapsulatesGeoOnClick(reflectionProbeGeoPadding.value);
            };
            Button createReflectionProbeFromSelectedGeoDimensions = rootVisualElement.Q<Button>("createReflectionProbeFromSelectedGeoDimensions");
            createReflectionProbeFromSelectedGeoDimensions.clickable.clicked += () =>
            {
                CreateReflectionProbeFromSelectedGeoDimensionsOnClick(reflectionProbeGeoPadding.value);
            };
            Toggle lightProbeUtilsModeToggle = rootVisualElement.Q<Toggle>("lightProbeUtilsModeToggle");
            lightProbeUtilsModeToggle.RegisterValueChangedCallback(evt =>
            {
                List<GameObject> nonLightProbeGameObjects = new List<GameObject>();
                foreach (GameObject gameObj in FindObjectsOfType<GameObject>())
                {
                    if (!gameObj.GetComponent<LightProbeGroup>())
                    {
                        nonLightProbeGameObjects.Add(gameObj);
                    }
                }

                if (lightProbeUtilsModeToggle.value)
                {
                    lightProbeUtilsModeToggle.text = "  Enabled: Only LPGs Scene-Selectable";
                    svm.DisablePicking(nonLightProbeGameObjects.ToArray(), false);
                }
                else
                {
                    lightProbeUtilsModeToggle.text = "  Disabled: All Objects Scene-Selectable";
                    svm.EnableAllPicking();
                }
            });
            FloatField lightProbeGridSpacing = rootVisualElement.Q<FloatField>("lightProbeGridSpacing");
            lightProbeGridSpacing.formatString = "F2";
            Vector3Field lightProbeGeoPadding = rootVisualElement.Q<Vector3Field>("lightProbeGeoPadding");
            Button createLightProbeFromZonesButton = rootVisualElement.Q<Button>("createLightProbeFromZonesButton");
            createLightProbeFromZonesButton.clickable.clicked += () =>
            {
                CreateLightProbeFromZonesOnClick(lightProbeGridSpacing.value, lightProbeGeoPadding.value);
            };
            Button removeOverlappingLightProbesButton = rootVisualElement.Q<Button>("removeOverlappingLightProbesButton");
            removeOverlappingLightProbesButton.clickable.clicked += () =>
            {
                RemoveOverlappingLightProbesOnClick(lightProbeGridSpacing.value, lightProbeGeoPadding.value);
            };
            FloatField lightProbeCheckMaxBackfaceDistance = rootVisualElement.Q<FloatField>("lightProbeCheckMaxBackfaceDistance");
            Toggle cullProbesOutsideMaxDistance = rootVisualElement.Q<Toggle>("cullProbesOutsideMaxDistance");
            Button removeLightProbesInAndBehindGeoButton = rootVisualElement.Q<Button>("removeLightProbesInAndBehindGeoButton");
            removeLightProbesInAndBehindGeoButton.clickable.clicked += () =>
            {
                RemoveLightProbesInAndBehindGeoButtonOnClick(lightProbeGridSpacing.value, lightProbeCheckMaxBackfaceDistance.value, cullProbesOutsideMaxDistance.value);
            };
            Button createDefaultLightProbesButton = rootVisualElement.Q<Button>("createDefaultLightProbesButton");
            createDefaultLightProbesButton.clickable.clicked += () =>
            {
                CreateDefaultLightProbesOnClick();
            };
            Button createLightProbesFromGeoButton = rootVisualElement.Q<Button>("createLightProbesFromGeoButton");
            createLightProbesFromGeoButton.clickable.clicked += () =>
            {
                CreateLightProbesFromGeoOnClick(lightProbeGridSpacing.value, lightProbeGeoPadding.value);
            };
            Button createLightProbesEncapsulatesGeoButton = rootVisualElement.Q<Button>("createLightProbesEncapsulatesGeoButton");
            createLightProbesEncapsulatesGeoButton.clickable.clicked += () =>
            {
                CreateLightProbesEncapsulatesGeoOnClick(lightProbeGridSpacing.value, lightProbeGeoPadding.value);
            };
            Button createLightProbesFromSelectedGeoDimensions = rootVisualElement.Q<Button>("createLightProbesFromSelectedGeoDimensions");
            createLightProbesFromSelectedGeoDimensions.clickable.clicked += () =>
            {
                CreateLightProbesFromSelectedGeoDimensionsOnClick(lightProbeGridSpacing.value, lightProbeGeoPadding.value);
            };
            Toggle volumetricsUtilsModeToggle = rootVisualElement.Q<Toggle>("volumetricsUtilsModeToggle");
            volumetricsUtilsModeToggle.RegisterValueChangedCallback(evt =>
            {
                List<GameObject> nonVolumetricsGameObjects = new List<GameObject>();
                foreach (GameObject gameObj in FindObjectsOfType<GameObject>())
                {
                    if (!gameObj.GetComponent<ReflectionProbe>())
                    {
                        nonVolumetricsGameObjects.Add(gameObj);
                    }
                }

                if (volumetricsUtilsModeToggle.value)
                {
                    volumetricsUtilsModeToggle.text = "  Enabled: Only Volumetrics Scene-Selectable";
                    svm.DisablePicking(nonVolumetricsGameObjects.ToArray(), false);
                }
                else
                {
                    volumetricsUtilsModeToggle.text = "  Disabled: All Objects Scene-Selectable";
                    svm.EnableAllPicking();
                }
            });
            Vector3Field volumetricsGeoPadding = rootVisualElement.Q<Vector3Field>("volumetricsGeoPadding");
            Button createVolumetricsFromZonesButton = rootVisualElement.Q<Button>("createVolumetricsFromZonesButton");
            createVolumetricsFromZonesButton.clickable.clicked += () =>
            {
                CreateVolumetricsFromZonesOnClick(volumetricsGeoPadding.value);
            };
            Button createDefaultVolumetricsButton = rootVisualElement.Q<Button>("createDefaultVolumetricsButton");
            createDefaultVolumetricsButton.clickable.clicked += () =>
            {
                CreateDefaultVolumetricsOnClick();
            };
            Button createVolumetricsFromGeoButton = rootVisualElement.Q<Button>("createVolumetricsFromGeoButton");
            createVolumetricsFromGeoButton.clickable.clicked += () =>
            {
                CreateVolumetricsFromGeoOnClick(volumetricsGeoPadding.value);
            };
            Button createVolumetricsEncapsulatesGeoButton = rootVisualElement.Q<Button>("createVolumetricsEncapsulatesGeoButton");
            createVolumetricsEncapsulatesGeoButton.clickable.clicked += () =>
            {
                CreateVolumetricsEncapsulatesGeoOnClick(volumetricsGeoPadding.value);
            };
            Button createVolumetricsFromSelectedGeoDimensions = rootVisualElement.Q<Button>("createVolumetricsFromSelectedGeoDimensions");
            createVolumetricsFromSelectedGeoDimensions.clickable.clicked += () =>
            {
                CreateVolumetricsFromSelectedGeoDimensionsOnClick(volumetricsGeoPadding.value);
            };
            reflectionProbeUtilsModeToggle.SetValueWithoutNotify(false);
            lightProbeUtilsModeToggle.SetValueWithoutNotify(false);
            volumetricsUtilsModeToggle.SetValueWithoutNotify(false);
            svm.EnableAllPicking();
        }

        private void ToggleToolbarDefaults()
        {
            reflectionsToolbarToggle.value = false;
            lightprobesToolbarToggle.value = true;
            volumetricsToolbarToggle.value = false;
            reflectionprobesParentContainer.style.display = DisplayStyle.None;
            lightprobesParentContainer.style.display = DisplayStyle.Flex;
            volumetricsParentContainer.style.display = DisplayStyle.None;
        }

        private void ToggleToolbarContainers()
        {
            reflectionsToolbarToggle.value = false;
            lightprobesToolbarToggle.value = false;
            volumetricsToolbarToggle.value = false;
            reflectionprobesParentContainer.style.display = DisplayStyle.None;
            lightprobesParentContainer.style.display = DisplayStyle.None;
            volumetricsParentContainer.style.display = DisplayStyle.None;
        }

        private void CreateReflectionProbeFromZonesOnClick(Vector3 reflectionProbeGeoPadding)
        {
            List<GameObject> createdObjects = new List<GameObject>();
            GameObject reflectionProbesHolderParent = CreateReflectProbeGOContainer();
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            foreach (GameObject go in selectedGOs)
            {
                Zone selectedZone = go.GetComponent<Zone>();
                if (selectedZone != null)
                {
                    Gizmos.matrix = selectedZone.transform.localToWorldMatrix;
                    BoxCollider boxCollider = null;
                    SphereCollider sphereCollider = null;
                    CapsuleCollider capsuleCollider = null;
                    MeshCollider meshCollider = null;
                    Collider zoneCol = selectedZone.GetComponent<Collider>();
                    Bounds selectedGOBounds = FindZoneBounds(selectedZone);
                    Vector3 zoneCenter = FindZoneCenter(selectedZone);
                    GameObject cloneGO = new GameObject("Reflection Probe of " + go.name);
                    cloneGO.transform.position = go.transform.position;
                    cloneGO.transform.rotation = go.transform.rotation;
                    if (zoneCol is BoxCollider)
                    {
                        boxCollider = (BoxCollider)zoneCol;
                        BoxCollider addedBoxCol = cloneGO.AddComponent<BoxCollider>();
                        addedBoxCol = boxCollider;
                    }
                    else if (zoneCol is SphereCollider)
                    {
                        sphereCollider = (SphereCollider)zoneCol;
                        SphereCollider addedSphereCol = cloneGO.AddComponent<SphereCollider>();
                        addedSphereCol = sphereCollider;
                    }
                    else if (zoneCol is CapsuleCollider)
                    {
                        capsuleCollider = (CapsuleCollider)zoneCol;
                        CapsuleCollider addedCapsuleCol = cloneGO.AddComponent<CapsuleCollider>();
                        addedCapsuleCol = capsuleCollider;
                    }
                    else if (zoneCol is MeshCollider)
                    {
                        meshCollider = (MeshCollider)zoneCol;
                        MeshCollider addedMeshCollider = cloneGO.AddComponent<MeshCollider>();
                        addedMeshCollider = meshCollider;
                    }

                    if (reflectionProbesHolderParent)
                    {
                        cloneGO.transform.parent = reflectionProbesHolderParent.transform;
                    }

                    Vector3 origRotation = go.transform.rotation.eulerAngles;
                    ReflectionProbe reflectProbe = cloneGO.AddComponent<ReflectionProbe>();
                    reflectProbe.center = Vector3.zero;
                    reflectProbe.size = selectedGOBounds.size + reflectionProbeGeoPadding;
                    cloneGO.transform.position = selectedZone.transform.TransformPoint(zoneCenter);
                    reflectProbe.size = GetRotatedRPSize(reflectProbe, origRotation);
                    DrawDebugCubeWithBounds(reflectProbe.bounds, Color.white, 5f);
                    Collider addedCollider = cloneGO.GetComponent<Collider>();
                    DestroyImmediate(addedCollider);
                    EditorUtility.SetDirty(cloneGO);
                    if (!createdObjects.Contains(cloneGO))
                    {
                        createdObjects.Add(cloneGO);
                    }

                    Undo.RegisterCreatedObjectUndo(cloneGO, "Reflection Probe Created");
                }
            }

            Selection.objects = createdObjects.ToArray();
        }

        private void CreateReflectionProbeFromSelectedGeoDimensionsOnClick(Vector3 reflectionProbeGeoPadding)
        {
            List<GameObject> createdObjects = new List<GameObject>();
            GameObject reflectionProbesHolderParent = CreateReflectProbeGOContainer();
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            GameObject tempParentGO = new GameObject(selectedGOs[0].name + " and neighbors");
            foreach (GameObject go in selectedGOs)
            {
                GameObject clonedSelected = Instantiate(go);
                clonedSelected.transform.position = go.transform.position;
                clonedSelected.transform.rotation = go.transform.rotation;
                clonedSelected.transform.parent = tempParentGO.transform;
            }

            Renderer[] rends = tempParentGO.GetComponentsInChildren<Renderer>();
            Bounds selectedGOBounds = rends[0].bounds;
            foreach (Renderer rend in rends)
            {
                selectedGOBounds = selectedGOBounds.GrowBounds(rend.bounds);
            }

            GameObject cloneGO = new GameObject();
            cloneGO.transform.position = tempParentGO.transform.position;
            cloneGO.transform.rotation = tempParentGO.transform.rotation;
            cloneGO.name = "Reflection Probe of " + tempParentGO.name;
            if (reflectionProbesHolderParent)
            {
                cloneGO.transform.parent = reflectionProbesHolderParent.transform;
            }

            DestroyImmediate(tempParentGO);
            cloneGO.transform.position = cloneGO.transform.TransformPoint(selectedGOBounds.center);
            ReflectionProbe reflectProbe = cloneGO.AddComponent<ReflectionProbe>();
            reflectProbe.size = selectedGOBounds.size + reflectionProbeGeoPadding;
            DrawDebugCubeWithBounds(reflectProbe.bounds, Color.white, 5f);
            EditorUtility.SetDirty(reflectProbe);
            if (!createdObjects.Contains(cloneGO))
            {
                createdObjects.Add(cloneGO);
            }

            Undo.RegisterCreatedObjectUndo(cloneGO, "Reflection Probe Created");
            Selection.objects = createdObjects.ToArray();
        }

        private void CreateReflectionProbeEncapsulatesGeoOnClick(Vector3 reflectionProbeGeoPadding)
        {
            List<GameObject> createdObjects = new List<GameObject>();
            GameObject reflectionProbesHolderParent = CreateReflectProbeGOContainer();
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            foreach (GameObject go in selectedGOs)
            {
                Renderer[] rends = go.GetComponentsInChildren<Renderer>();
                Bounds selectedGOBounds = rends[0].bounds;
                foreach (Renderer rend in rends)
                {
                    selectedGOBounds = selectedGOBounds.GrowBounds(rend.bounds);
                }

                Vector3 origRotation = go.transform.rotation.eulerAngles;
                GameObject cloneGO = new GameObject();
                cloneGO.transform.rotation = Quaternion.identity;
                cloneGO.name = "Reflection Probe of " + go.name;
                cloneGO.AddComponent<ReflectionProbe>();
                ReflectionProbe reflectProbe = cloneGO.GetComponent<ReflectionProbe>();
                reflectProbe.size = selectedGOBounds.size + reflectionProbeGeoPadding;
                cloneGO.transform.position = selectedGOBounds.center;
                if (reflectionProbesHolderParent)
                {
                    cloneGO.transform.parent = reflectionProbesHolderParent.transform;
                }

                reflectProbe.size = GetRotatedRPSize(reflectProbe, origRotation);
                DrawDebugCubeWithBounds(reflectProbe.bounds, Color.white, 5f);
                EditorUtility.SetDirty(reflectProbe);
                if (!createdObjects.Contains(cloneGO))
                {
                    createdObjects.Add(cloneGO);
                }

                Undo.RegisterCreatedObjectUndo(cloneGO, "Reflection Probe Created");
            }

            Selection.objects = createdObjects.ToArray();
        }

        private void CreateReflectionProbeFromGeoOnClick(Vector3 reflectionProbeGeoPadding)
        {
            List<GameObject> createdObjects = new List<GameObject>();
            GameObject reflectionProbesHolderParent = CreateReflectProbeGOContainer();
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            foreach (GameObject go in selectedGOs)
            {
                MeshFilter selectedMeshFilter = go.GetComponent<MeshFilter>();
                if (go.GetComponent<MeshRenderer>() != null && selectedMeshFilter != null)
                {
                    Bounds selectedGOBounds = selectedMeshFilter.sharedMesh.bounds;
                    selectedGOBounds.center = go.transform.TransformPoint(selectedMeshFilter.sharedMesh.bounds.center);
                    selectedGOBounds.size = Vector3.Scale(selectedGOBounds.size, go.transform.lossyScale);
                    Vector3 origRotation = go.transform.rotation.eulerAngles;
                    GameObject cloneGO = new GameObject();
                    cloneGO.transform.position = cloneGO.transform.TransformPoint(selectedGOBounds.center);
                    cloneGO.transform.rotation = Quaternion.identity;
                    cloneGO.name = "Reflection Probe of " + go.name;
                    if (reflectionProbesHolderParent)
                    {
                        cloneGO.transform.parent = reflectionProbesHolderParent.transform;
                    }

                    ReflectionProbe reflectProbe = cloneGO.AddComponent<ReflectionProbe>();
                    reflectProbe.size = selectedGOBounds.size + reflectionProbeGeoPadding;
                    reflectProbe.center = Vector3.zero;
                    reflectProbe.size = GetRotatedRPSize(reflectProbe, origRotation);
                    DrawDebugCubeWithBounds(reflectProbe.bounds, Color.white, 5f);
                    EditorUtility.SetDirty(cloneGO);
                    if (!createdObjects.Contains(cloneGO))
                    {
                        createdObjects.Add(cloneGO);
                    }

                    Undo.RegisterCreatedObjectUndo(cloneGO, "Reflection Probe Created");
                }
            }

            Selection.objects = createdObjects.ToArray();
        }

        private Vector3 GetRotatedRPSize(ReflectionProbe reflectProbe, Vector3 origRotation)
        {
            Vector3 rotatedSize = reflectProbe.size;
            bool xRot = false;
            bool yRot = false;
            bool zRot = false;
            if ((85f < origRotation.x && origRotation.x < 95f) || (265f < origRotation.x && origRotation.x < 275f) || (-85 < origRotation.x && origRotation.x < -95f) || (-265f < origRotation.x && origRotation.x < -275f))
            {
                xRot = true;
            }

            if ((85f < origRotation.y && origRotation.y < 95f) || (265f < origRotation.y && origRotation.y < 275f) || (-85 < origRotation.y && origRotation.y < -95f) || (-265f < origRotation.y && origRotation.y < -275f))
            {
                yRot = true;
            }

            if ((85f < origRotation.z && origRotation.z < 95f) || (265f < origRotation.z && origRotation.z < 275f) || (-85 < origRotation.z && origRotation.z < -95f) || (-265f < origRotation.z && origRotation.z < -275f))
            {
                zRot = true;
            }

            Vector3 origSize = reflectProbe.size;
            if (xRot && !yRot && !zRot)
            {
                rotatedSize = new Vector3(origSize.x, origSize.z, origSize.y);
            }
            else if (!xRot && yRot && !zRot)
            {
                rotatedSize = new Vector3(origSize.z, origSize.y, origSize.x);
            }
            else if (!xRot && !yRot && zRot)
            {
                rotatedSize = new Vector3(origSize.y, origSize.x, origSize.z);
            }
            else if (xRot && yRot && !zRot)
            {
                rotatedSize = new Vector3(origSize.y, origSize.z, origSize.x);
            }
            else if (!xRot && yRot && zRot)
            {
                rotatedSize = new Vector3(origSize.z, origSize.x, origSize.y);
            }
            else if (xRot && !yRot && zRot)
            {
                rotatedSize = new Vector3(origSize.y, origSize.z, origSize.x);
            }
            else if (xRot && yRot && zRot)
            {
                rotatedSize = new Vector3(origSize.x, origSize.z, origSize.y);
            }

            return rotatedSize;
        }

        private Vector3 GetRotatedVolSize(BakedVolumetricArea volumetricArea, Vector3 origRotation)
        {
            Vector3 rotatedSize = volumetricArea.BoxScale;
            bool xRot = false;
            bool yRot = false;
            bool zRot = false;
            if ((85f < origRotation.x && origRotation.x < 95f) || (265f < origRotation.x && origRotation.x < 275f) || (-85 < origRotation.x && origRotation.x < -95f) || (-265f < origRotation.x && origRotation.x < -275f))
            {
                xRot = true;
            }

            if ((85f < origRotation.y && origRotation.y < 95f) || (265f < origRotation.y && origRotation.y < 275f) || (-85 < origRotation.y && origRotation.y < -95f) || (-265f < origRotation.y && origRotation.y < -275f))
            {
                yRot = true;
            }

            if ((85f < origRotation.z && origRotation.z < 95f) || (265f < origRotation.z && origRotation.z < 275f) || (-85 < origRotation.z && origRotation.z < -95f) || (-265f < origRotation.z && origRotation.z < -275f))
            {
                zRot = true;
            }

            Vector3 origSize = volumetricArea.BoxScale;
            if (xRot && !yRot && !zRot)
            {
                rotatedSize = new Vector3(origSize.x, origSize.z, origSize.y);
            }
            else if (!xRot && yRot && !zRot)
            {
                rotatedSize = new Vector3(origSize.z, origSize.y, origSize.x);
            }
            else if (!xRot && !yRot && zRot)
            {
                rotatedSize = new Vector3(origSize.y, origSize.x, origSize.z);
            }
            else if (xRot && yRot && !zRot)
            {
                rotatedSize = new Vector3(origSize.y, origSize.z, origSize.x);
            }
            else if (!xRot && yRot && zRot)
            {
                rotatedSize = new Vector3(origSize.z, origSize.x, origSize.y);
            }
            else if (xRot && !yRot && zRot)
            {
                rotatedSize = new Vector3(origSize.y, origSize.z, origSize.x);
            }
            else if (xRot && yRot && zRot)
            {
                rotatedSize = new Vector3(origSize.x, origSize.z, origSize.y);
            }

            return rotatedSize;
        }

        private void CreateDefaultReflectionProbeOnClick()
        {
            GameObject reflectionProbesHolderParent = CreateReflectProbeGOContainer();
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            List<GameObject> createdReflectionProbes = new List<GameObject>();
            foreach (GameObject go in selectedGOs)
            {
                GameObject cloneGO = new GameObject();
                cloneGO.transform.position = go.transform.position;
                cloneGO.transform.rotation = Quaternion.identity;
                cloneGO.name = "Reflection Probe of " + go.name;
                if (reflectionProbesHolderParent)
                {
                    cloneGO.transform.parent = reflectionProbesHolderParent.transform;
                }

                if (!cloneGO.GetComponent<ReflectionProbe>())
                {
                    cloneGO.AddComponent<ReflectionProbe>();
                    cloneGO.GetComponent<ReflectionProbe>().size = Vector3.one;
                }

                if (!createdReflectionProbes.Contains(cloneGO))
                {
                    createdReflectionProbes.Add(cloneGO);
                }

                EditorUtility.SetDirty(cloneGO);
                Undo.RegisterCreatedObjectUndo(cloneGO, "Reflection Probe Created");
            }

            Selection.objects = createdReflectionProbes.ToArray();
        }

        private void CreateLightProbeFromZonesOnClick(float gridSpacing, Vector3 gridBoundsShrink)
        {
            List<GameObject> createdObjects = new List<GameObject>();
            GameObject lightProbesHolderParent = CreateLightProbeGOContainer();
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            float progressBarStatus = 0;
            foreach (GameObject go in selectedGOs)
            {
                EditorUtility.DisplayCancelableProgressBar("Light Probe Automation", $"Building the LightProbe Grid for {go.name}", progressBarStatus / selectedGOs.Count);
                Zone selectedZone = go.GetComponent<Zone>();
                if (selectedZone != null)
                {
                    Gizmos.matrix = selectedZone.transform.localToWorldMatrix;
                    BoxCollider boxCollider = null;
                    SphereCollider sphereCollider = null;
                    CapsuleCollider capsuleCollider = null;
                    MeshCollider meshCollider = null;
                    Collider zoneCol = selectedZone.GetComponent<Collider>();
                    Bounds selectedGOBounds = FindZoneBounds(selectedZone);
                    Vector3 zoneCenter = FindZoneCenter(selectedZone);
                    GameObject cloneGO = new GameObject("LPG of " + go.name);
                    cloneGO.transform.position = go.transform.position;
                    cloneGO.transform.rotation = go.transform.rotation;
                    if (zoneCol is BoxCollider)
                    {
                        boxCollider = (BoxCollider)zoneCol;
                        BoxCollider addedBoxCol = cloneGO.AddComponent<BoxCollider>();
                        addedBoxCol = boxCollider;
                    }
                    else if (zoneCol is SphereCollider)
                    {
                        sphereCollider = (SphereCollider)zoneCol;
                        SphereCollider addedSphereCol = cloneGO.AddComponent<SphereCollider>();
                        addedSphereCol = sphereCollider;
                    }
                    else if (zoneCol is CapsuleCollider)
                    {
                        capsuleCollider = (CapsuleCollider)zoneCol;
                        CapsuleCollider addedCapsuleCol = cloneGO.AddComponent<CapsuleCollider>();
                        addedCapsuleCol = capsuleCollider;
                    }
                    else if (zoneCol is MeshCollider)
                    {
                        meshCollider = (MeshCollider)zoneCol;
                        MeshCollider addedMeshCollider = cloneGO.AddComponent<MeshCollider>();
                        addedMeshCollider = meshCollider;
                    }

                    cloneGO.layer = 0;
                    if (lightProbesHolderParent)
                    {
                        cloneGO.transform.parent = lightProbesHolderParent.transform;
                    }

                    LightProbeGroup lpGroup = cloneGO.AddComponent<LightProbeGroup>();
                    lpGroup.transform.position = selectedZone.transform.TransformPoint(zoneCenter);
                    lpGroup.dering = true;
                    lpGroup.probePositions = BuildLightProbeGrid(lpGroup, selectedGOBounds, gridSpacing, gridBoundsShrink);
                    CullPointsOutsideZoneTriggerCollider(lpGroup, selectedZone);
                    Collider addedCollider = cloneGO.GetComponent<Collider>();
                    DestroyImmediate(addedCollider);
                    EditorUtility.SetDirty(cloneGO);
                    if (!createdObjects.Contains(cloneGO))
                    {
                        createdObjects.Add(cloneGO);
                    }

                    Undo.RegisterCreatedObjectUndo(cloneGO, "Light Probe Group Created");
                }
            }

            EditorUtility.ClearProgressBar();
            Selection.objects = createdObjects.ToArray();
        }

        private void CreateLightProbesFromSelectedGeoDimensionsOnClick(float gridSpacing, Vector3 gridBoundsShrink)
        {
            List<GameObject> createdObjects = new List<GameObject>();
            GameObject lightProbesHolderParent = CreateLightProbeGOContainer();
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            GameObject tempParentGO = new GameObject(selectedGOs[0].name + " and neighbors");
            foreach (GameObject go in selectedGOs)
            {
                GameObject clonedSelected = Instantiate(go);
                clonedSelected.transform.position = go.transform.position;
                clonedSelected.transform.rotation = go.transform.rotation;
                clonedSelected.transform.parent = tempParentGO.transform;
            }

            Renderer[] rends = tempParentGO.GetComponentsInChildren<Renderer>();
            Bounds selectedGOBounds = rends[0].bounds;
            foreach (Renderer rend in rends)
            {
                selectedGOBounds = selectedGOBounds.GrowBounds(rend.bounds);
            }

            DrawDebugCubeWithBounds(selectedGOBounds, Color.yellow, 5f);
            GameObject cloneGO = new GameObject();
            cloneGO.transform.position = tempParentGO.transform.position;
            cloneGO.transform.rotation = Quaternion.identity;
            cloneGO.name = "LPG of " + tempParentGO.name;
            if (lightProbesHolderParent)
            {
                cloneGO.transform.parent = lightProbesHolderParent.transform;
            }

            DestroyImmediate(tempParentGO);
            cloneGO.transform.position = cloneGO.transform.TransformPoint(selectedGOBounds.center);
            LightProbeGroup lpGroup = cloneGO.AddComponent<LightProbeGroup>();
            lpGroup.dering = true;
            lpGroup.probePositions = BuildLightProbeGrid(lpGroup, selectedGOBounds, gridSpacing, gridBoundsShrink);
            if (!createdObjects.Contains(cloneGO))
            {
                createdObjects.Add(cloneGO);
            }

            Undo.RegisterCreatedObjectUndo(cloneGO, "Light Probe Group Created");
            Selection.objects = createdObjects.ToArray();
        }

        private void CreateLightProbesEncapsulatesGeoOnClick(float gridSpacing, Vector3 gridBoundsShrink)
        {
            List<GameObject> createdObjects = new List<GameObject>();
            GameObject lightProbesHolderParent = CreateLightProbeGOContainer();
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            foreach (GameObject go in selectedGOs)
            {
                Renderer[] rends = go.GetComponentsInChildren<Renderer>();
                Bounds selectedGOBounds = rends[0].bounds;
                foreach (Renderer rend in rends)
                {
                    selectedGOBounds = selectedGOBounds.GrowBounds(rend.bounds);
                }

                DrawDebugCubeWithBounds(selectedGOBounds, Color.yellow, 5f);
                GameObject cloneGO = new GameObject();
                cloneGO.transform.position = cloneGO.transform.TransformPoint(selectedGOBounds.center);
                cloneGO.transform.rotation = Quaternion.identity;
                cloneGO.name = "LPG of " + go.name;
                if (lightProbesHolderParent)
                {
                    cloneGO.transform.parent = lightProbesHolderParent.transform;
                }

                LightProbeGroup lpGroup = cloneGO.AddComponent<LightProbeGroup>();
                lpGroup.dering = true;
                lpGroup.probePositions = BuildLightProbeGrid(lpGroup, selectedGOBounds, gridSpacing, gridBoundsShrink);
                if (!createdObjects.Contains(cloneGO))
                {
                    createdObjects.Add(cloneGO);
                }

                Undo.RegisterCreatedObjectUndo(cloneGO, "Light Probe Group Created");
            }

            Selection.objects = createdObjects.ToArray();
        }

        private void CreateLightProbesFromGeoOnClick(float gridSpacing, Vector3 gridBoundsShrink)
        {
            List<GameObject> createdObjects = new List<GameObject>();
            GameObject lightProbesHolderParent = CreateLightProbeGOContainer();
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            foreach (GameObject go in selectedGOs)
            {
                MeshFilter selectedMeshFilter = go.GetComponent<MeshFilter>();
                if (go.GetComponent<MeshRenderer>() != null && selectedMeshFilter != null)
                {
                    Bounds selectedGOBounds = selectedMeshFilter.sharedMesh.bounds;
                    selectedGOBounds.center = go.transform.TransformPoint(selectedMeshFilter.sharedMesh.bounds.center);
                    selectedGOBounds.size = Vector3.Scale(selectedGOBounds.size, go.transform.lossyScale);
                    DrawDebugCubeWithBounds(selectedGOBounds, Color.yellow, 5f);
                    GameObject cloneGO = new GameObject();
                    cloneGO.transform.position = cloneGO.transform.TransformPoint(selectedGOBounds.center);
                    cloneGO.transform.rotation = Quaternion.identity;
                    cloneGO.name = "LPG of " + go.name;
                    if (lightProbesHolderParent)
                    {
                        cloneGO.transform.parent = lightProbesHolderParent.transform;
                    }

                    LightProbeGroup lpGroup = cloneGO.AddComponent<LightProbeGroup>();
                    lpGroup.dering = true;
                    lpGroup.probePositions = BuildLightProbeGrid(lpGroup, selectedGOBounds, gridSpacing, gridBoundsShrink);
                    cloneGO.transform.rotation = go.transform.rotation;
                    MeshCollider origMeshCollider = go.GetComponent<MeshCollider>();
                    if (origMeshCollider != null && origMeshCollider.isTrigger)
                    {
                        CullPointsOutsideGeoMeshCollider(lpGroup, origMeshCollider);
                    }

                    EditorUtility.SetDirty(cloneGO);
                    if (!createdObjects.Contains(cloneGO))
                    {
                        createdObjects.Add(cloneGO);
                    }

                    Undo.RegisterCreatedObjectUndo(cloneGO, "Light Probe Group Created");
                }
            }

            Selection.objects = createdObjects.ToArray();
        }

        private void CreateDefaultLightProbesOnClick()
        {
            List<GameObject> createdObjects = new List<GameObject>();
            GameObject lightProbesHolderParent = CreateLightProbeGOContainer();
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            List<GameObject> createdZones = new List<GameObject>();
            foreach (GameObject go in selectedGOs)
            {
                GameObject cloneGO = new GameObject();
                cloneGO.transform.position = go.transform.position;
                cloneGO.transform.rotation = Quaternion.identity;
                cloneGO.name = "LPG of " + go.name;
                if (lightProbesHolderParent)
                {
                    cloneGO.transform.parent = lightProbesHolderParent.transform;
                }

                LightProbeGroup lpGroup = cloneGO.AddComponent<LightProbeGroup>();
                lpGroup.dering = true;
                Bounds selectedGOBounds = new Bounds(Vector3.zero, Vector3.one);
                BoxCollider zoneCol = cloneGO.AddComponent<BoxCollider>();
                zoneCol.size = Vector3.one;
                DestroyImmediate(zoneCol);
                EditorUtility.SetDirty(cloneGO);
                if (!createdZones.Contains(cloneGO))
                {
                    createdZones.Add(cloneGO);
                }

                Undo.RegisterCreatedObjectUndo(cloneGO, "Light Probe Group Created");
            }

            Selection.objects = createdZones.ToArray();
        }

        private Vector3[] BuildLightProbeGrid(LightProbeGroup lpGroup, Bounds selectedGOBounds, float gridSpacing, Vector3 gridBoundsShrink)
        {
            List<Vector3> lpgPositionsList = new List<Vector3>();

            Bounds shrunkBounds = selectedGOBounds;
            shrunkBounds.size = shrunkBounds.size - gridBoundsShrink * 2;

            int celProbesX = Mathf.CeilToInt((shrunkBounds.size.x) / gridSpacing);
            int celProbesY = Mathf.CeilToInt((shrunkBounds.size.y) / gridSpacing);
            int celProbesZ = Mathf.CeilToInt((shrunkBounds.size.z) / gridSpacing);

            float xSize = (shrunkBounds.size.x) / celProbesX;
            float ySize = (shrunkBounds.size.y) / celProbesY;
            float zSize = (shrunkBounds.size.z) / celProbesZ;


            for (float x = 0; x <= shrunkBounds.size.x + 0.01; x += xSize)
            {
                for (float y = 0; y <= shrunkBounds.size.y + 0.01; y += ySize)
                {
                    for (float z = 0; z <= shrunkBounds.size.z + 0.01; z += zSize)
                    {

                        Vector3 lprobePoint = new Vector3(x, y, z) - shrunkBounds.extents;
                        if (selectedGOBounds.Contains(lpGroup.transform.TransformPoint(lprobePoint)))
                        {
                            lpgPositionsList.Add(lprobePoint);
                        }
                    }
                }
            }

            return lpgPositionsList.ToArray();
        }

        private void RemoveOverlappingLightProbesOnClick(float gridSpacing, Vector3 gridBoundsShrink)
        {
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            List<Vector3> allWSLightProbePositionsUnique = new List<Vector3>();
            foreach (GameObject go in selectedGOs)
            {
                LightProbeGroup lpGroup = go.GetComponent<LightProbeGroup>();
                foreach (Vector3 probePos in lpGroup.probePositions)
                {
                    Vector3 wsProbePos = lpGroup.transform.TransformPoint(probePos);
                    if (!allWSLightProbePositionsUnique.Contains(wsProbePos))
                    {
                        allWSLightProbePositionsUnique.Add(wsProbePos);
                    }
                }
            }

            List<Vector3> checkedWSProbeList = new List<Vector3>();
            foreach (GameObject go in selectedGOs)
            {
                LightProbeGroup lpGroup = go.GetComponent<LightProbeGroup>();
                List<Vector3> removedProbeList = lpGroup.probePositions.ToList();
                foreach (Vector3 probePos in lpGroup.probePositions)
                {
                    Vector3 wsProbePos = lpGroup.transform.TransformPoint(probePos);
                    if (!checkedWSProbeList.Contains(wsProbePos))
                    {
                        foreach (Vector3 allWSProbePos in allWSLightProbePositionsUnique)
                        {
                            if (wsProbePos.x > allWSProbePos.x - (gridSpacing / 2 + gridBoundsShrink.x) && wsProbePos.x < allWSProbePos.x + (gridSpacing / 2 - gridBoundsShrink.x))
                            {
                                if (wsProbePos.y > allWSProbePos.y - (gridSpacing / 2 + gridBoundsShrink.y) && wsProbePos.y < allWSProbePos.y + (gridSpacing / 2 - gridBoundsShrink.y))
                                {
                                    if (wsProbePos.z > allWSProbePos.z - (gridSpacing / 2 + gridBoundsShrink.z) && wsProbePos.z < allWSProbePos.z + (gridSpacing / 2 - gridBoundsShrink.z))
                                    {
                                        if (wsProbePos != allWSProbePos)
                                        {
                                            removedProbeList.Remove(probePos);
                                            lpGroup.probePositions = removedProbeList.ToArray();
                                            if (!checkedWSProbeList.Contains(allWSProbePos))
                                            {
                                                checkedWSProbeList.Add(allWSProbePos);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void RemoveLightProbesInAndBehindGeoButtonOnClick(float gridSpacing, float maxBackfaceCheck, bool cullProbesOutsideMaxDistance)
        {
            float progressBarStatus = 0;
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            foreach (GameObject go in selectedGOs)
            {
                EditorUtility.DisplayCancelableProgressBar("Removing Light Probes Located Inside/Behind Static Geometry", $" From {go.name}.  This can take a while...", progressBarStatus / selectedGOs.Count);
                LightProbeGroup lpGroup = go.GetComponent<LightProbeGroup>();
                if (lpGroup == null)
                {
                    Debug.Log("Select at least one Light Probe Group");
                    continue;
                }

                List<Vector3> removedProbeList = lpGroup.probePositions.ToList();
                List<GameObject> meshGOsNearLightProbes = GetMeshGOsForBackfaceChecks(lpGroup, gridSpacing);
                List<MeshCollider> addedMeshColliders = new List<MeshCollider>();
                List<BoxCollider> childBoxCols = new List<BoxCollider>();
                foreach (var meshRendererObj in meshGOsNearLightProbes)
                {
                    MeshCollider meshCol = meshRendererObj.GetComponent<MeshCollider>();
                    if (!meshCol)
                    {
                        meshCol = meshRendererObj.AddComponent<MeshCollider>();
                        if (!addedMeshColliders.Contains(meshCol))
                        {
                            addedMeshColliders.Add(meshCol);
                        }

                        foreach (var boxCol in meshRendererObj.GetComponentsInChildren<BoxCollider>())
                        {
                            if (boxCol.isTrigger == false && boxCol.enabled && !childBoxCols.Contains(boxCol))
                            {
                                childBoxCols.Add(boxCol);
                                boxCol.enabled = false;
                            }
                        }
                    }
                }

                for (int p = 0; p < lpGroup.probePositions.Length; p++)
                {
                    Vector3 wsProbePos = lpGroup.transform.TransformPoint(lpGroup.probePositions[p]);
                    if (PointIsBehindBackface(wsProbePos, maxBackfaceCheck, cullProbesOutsideMaxDistance))
                    {
                        if (removedProbeList.Contains(lpGroup.probePositions[p]))
                        {
                            removedProbeList.Remove(lpGroup.probePositions[p]);
                        }
                    }

                    if (PointIsInsideStaticCollider(wsProbePos))
                    {
                        if (removedProbeList.Contains(lpGroup.probePositions[p]))
                        {
                            removedProbeList.Remove(lpGroup.probePositions[p]);
                        }
                    }
                }

                lpGroup.probePositions = removedProbeList.ToArray();
                foreach (var meshCol in addedMeshColliders)
                {
                    DestroyImmediate(meshCol);
                }

                foreach (var boxCol in childBoxCols)
                {
                    boxCol.enabled = true;
                }

                progressBarStatus++;
            }

            EditorUtility.ClearProgressBar();
            Physics.queriesHitBackfaces = false;
        }

        private void DrawDebugCubeWithBounds(Bounds bounds, Color boundsColor, float boundsDuration)
        {
            Vector3 pLeftBottomFront = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
            Vector3 pRightBottomFront = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            Vector3 pRightBottomBack = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            Vector3 pLeftBottomBack = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
            Vector3 pLeftTopFront = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            Vector3 pRightTopFront = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
            Vector3 pRightTopBack = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
            Vector3 pLeftTopBack = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
            Debug.DrawLine(pLeftBottomFront, pRightBottomFront, boundsColor, boundsDuration);
            Debug.DrawLine(pLeftBottomBack, pRightBottomBack, boundsColor, boundsDuration);
            Debug.DrawLine(pLeftTopFront, pRightTopFront, boundsColor, boundsDuration);
            Debug.DrawLine(pLeftTopBack, pRightTopBack, boundsColor, boundsDuration);
            Debug.DrawLine(pLeftTopFront, pLeftTopBack, boundsColor, boundsDuration);
            Debug.DrawLine(pRightTopFront, pRightTopBack, boundsColor, boundsDuration);
            Debug.DrawLine(pLeftBottomFront, pLeftBottomBack, boundsColor, boundsDuration);
            Debug.DrawLine(pRightBottomFront, pRightBottomBack, boundsColor, boundsDuration);
            Debug.DrawLine(pLeftBottomFront, pLeftTopFront, boundsColor, boundsDuration);
            Debug.DrawLine(pRightBottomFront, pRightTopFront, boundsColor, boundsDuration);
            Debug.DrawLine(pLeftBottomBack, pLeftTopBack, boundsColor, boundsDuration);
            Debug.DrawLine(pRightBottomBack, pRightTopBack, boundsColor, boundsDuration);
        }

        private List<GameObject> GetMeshGOsForBackfaceChecks(LightProbeGroup lpGroup, float gridSpacing)
        {
            List<GameObject> meshObjectsNearProbes = new List<GameObject>();
            Bounds selectedProbeBounds = new Bounds();
            selectedProbeBounds.center = lpGroup.transform.position;
            foreach (Vector3 probePos in lpGroup.probePositions)
            {
                selectedProbeBounds.Encapsulate(lpGroup.transform.TransformPoint(probePos));
            }

            selectedProbeBounds.Encapsulate(selectedProbeBounds.min - new Vector3(gridSpacing, gridSpacing, gridSpacing));
            selectedProbeBounds.Encapsulate(selectedProbeBounds.max + new Vector3(gridSpacing, gridSpacing, gridSpacing));
            DrawDebugCubeWithBounds(selectedProbeBounds, Color.red, 5f);
            foreach (MeshRenderer meshRend in FindObjectsOfType<MeshRenderer>())
            {
                if (selectedProbeBounds.Contains(meshRend.transform.position) && meshRend.gameObject.isStatic && !meshRend.GetComponent<MeshCollider>())
                {
                    if (meshRend != null && !meshObjectsNearProbes.Contains(meshRend.gameObject))
                    {
                        meshObjectsNearProbes.Add(meshRend.gameObject);
                    }
                }
            }

            foreach (var localPoint in lpGroup.probePositions)
            {
                Vector3 wsPoint = lpGroup.transform.TransformPoint(localPoint);
                Collider[] collidersNearLightProbes = Physics.OverlapSphere(wsPoint, gridSpacing, layerCollideAllowed, QueryTriggerInteraction.Ignore);
                foreach (var collider in collidersNearLightProbes)
                {
                    MeshRenderer meshRend = collider.GetComponentInParent<MeshRenderer>();
                    if (meshRend != null && !meshObjectsNearProbes.Contains(meshRend.gameObject))
                    {
                        meshObjectsNearProbes.Add(meshRend.gameObject);
                    }
                }
            }

            return meshObjectsNearProbes;
        }

        private bool PointIsBehindBackface(Vector3 wsPoint, float maxBackfaceCheck, bool cullProbesOutsideMaxDistance)
        {
            RaycastHit backfaceHit;
            RaycastHit nonBackfaceHit;
            List<Vector3> directions = new List<Vector3>()
            {
                Vector3.up,
                Vector3.down,
                Vector3.left,
                Vector3.right,
                Vector3.forward,
                Vector3.back
            };
            List<Color> rayColors = new List<Color>()
            {
                Color.green,
                Color.green,
                Color.red,
                Color.red,
                Color.blue,
                Color.blue
            };
            bool hitSomeDirection = false;
            for (int i = 0; i < directions.Count; i++)
            {
                Physics.queriesHitBackfaces = true;
                Vector3 rayDirection = directions[i];
                Color rayColor = rayColors[i];
                if (Physics.Raycast(wsPoint, rayDirection, out backfaceHit, maxBackfaceCheck, layerCollideAllowed, QueryTriggerInteraction.Ignore))
                {
                    hitSomeDirection = true;
                    Physics.queriesHitBackfaces = false;
                    if (Physics.Raycast(wsPoint, rayDirection, out nonBackfaceHit, backfaceHit.distance + 0.00001f, layerCollideAllowed, QueryTriggerInteraction.Ignore))
                    {
                        if (backfaceHit.collider != nonBackfaceHit.collider)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                }
            }

            if (cullProbesOutsideMaxDistance && hitSomeDirection == false)
            {
                return true;
            }

            Physics.queriesHitBackfaces = false;
            return false;
        }

        private void CullPointsOutsideZoneTriggerCollider(LightProbeGroup lpGroup, Zone zone)
        {
            List<Vector3> culledPoints = new List<Vector3>();
            BoxCollider boxCollider = zone.GetComponent<BoxCollider>();
            SphereCollider sphereCollider = zone.GetComponent<SphereCollider>();
            CapsuleCollider capsuleCollider = zone.GetComponent<CapsuleCollider>();
            MeshCollider meshCollider = zone.GetComponent<MeshCollider>();
            Collider zoneCollider = null;
            if (boxCollider != null && boxCollider.isTrigger == true)
            {
                zoneCollider = boxCollider;
            }
            else if (sphereCollider != null && sphereCollider.isTrigger == true)
            {
                zoneCollider = sphereCollider;
            }
            else if (capsuleCollider != null && capsuleCollider.isTrigger == true)
            {
                zoneCollider = capsuleCollider;
            }
            else if (meshCollider != null && meshCollider.isTrigger == true)
            {
                zoneCollider = meshCollider;
            }

            foreach (var localPoint in lpGroup.probePositions)
            {
                Vector3 wsPoint = lpGroup.transform.TransformPoint(localPoint);
                Collider[] collidersAroundLightProbes = Physics.OverlapSphere(wsPoint, 0.001f, layerCollideAllowed, QueryTriggerInteraction.Collide);
                foreach (Collider collider in collidersAroundLightProbes)
                {
                    if (collider != null && collider == zoneCollider && !culledPoints.Contains(wsPoint))
                    {
                        culledPoints.Add(localPoint);
                    }
                }
            }

            lpGroup.probePositions = culledPoints.ToArray();
        }

        private void CullPointsOutsideGeoMeshCollider(LightProbeGroup lpGroup, MeshCollider meshCollider)
        {
            List<Vector3> culledPoints = new List<Vector3>();
            foreach (var localPoint in lpGroup.probePositions)
            {
                Vector3 wsPoint = lpGroup.transform.TransformPoint(localPoint);
                Collider[] collidersAroundLightProbes = Physics.OverlapSphere(wsPoint, 0.001f, ~0, QueryTriggerInteraction.Collide);
                foreach (Collider collider in collidersAroundLightProbes)
                {
                    if (collider != null && collider == meshCollider && !culledPoints.Contains(wsPoint))
                    {
                        culledPoints.Add(localPoint);
                    }
                }
            }

            lpGroup.probePositions = culledPoints.ToArray();
        }

        private bool PointIsInsideStaticCollider(Vector3 wsPoint)
        {
            Collider[] collidersAroundLightProbes = Physics.OverlapSphere(wsPoint, 0.001f, ~0, QueryTriggerInteraction.Ignore);
            foreach (Collider collider in collidersAroundLightProbes)
            {
                if (collider.gameObject.isStatic)
                {
                    return true;
                }
            }

            return false;
        }

        private void CreateVolumetricsFromZonesOnClick(Vector3 volumetricsGeoPadding)
        {
            List<GameObject> createdObjects = new List<GameObject>();
            GameObject volumetricsHolderParent = CreateVolumetricsGOContainer();
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            foreach (GameObject go in selectedGOs)
            {
                Zone selectedZone = go.GetComponent<Zone>();
                if (selectedZone != null)
                {
                    Gizmos.matrix = selectedZone.transform.localToWorldMatrix;
                    Collider zoneCol = selectedZone.GetComponent<Collider>();
                    Bounds selectedGOBounds = FindZoneBounds(selectedZone);
                    Vector3 zoneCenter = FindZoneCenter(selectedZone);
                    GameObject cloneGO = new GameObject("VOLUMETRICS of " + go.name);
                    cloneGO.transform.position = go.transform.position;
                    if (volumetricsHolderParent)
                    {
                        cloneGO.transform.parent = volumetricsHolderParent.transform;
                    }

                    Vector3 origRotation = go.transform.rotation.eulerAngles;
                    BakedVolumetricArea volumetric = cloneGO.AddComponent<BakedVolumetricArea>();
                    volumetric.BoxScale = selectedGOBounds.size + volumetricsGeoPadding;
                    cloneGO.transform.position = selectedZone.transform.TransformPoint(zoneCenter);
                    EditorUtility.SetDirty(cloneGO);
                    if (!createdObjects.Contains(cloneGO))
                    {
                        createdObjects.Add(cloneGO);
                    }

                    Undo.RegisterCreatedObjectUndo(cloneGO, "Volumetric Created");
                }
            }

            Selection.objects = createdObjects.ToArray();
        }

        private void CreateVolumetricsFromSelectedGeoDimensionsOnClick(Vector3 volumetricsGeoPadding)
        {
            List<GameObject> createdObjects = new List<GameObject>();
            GameObject volumetricsHolderParent = CreateVolumetricsGOContainer();
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            GameObject tempParentGO = new GameObject(selectedGOs[0].name + " and neighbors");
            foreach (GameObject go in selectedGOs)
            {
                GameObject clonedSelected = Instantiate(go);
                clonedSelected.transform.position = go.transform.position;
                clonedSelected.transform.rotation = go.transform.rotation;
                clonedSelected.transform.parent = tempParentGO.transform;
            }

            Renderer[] rends = tempParentGO.GetComponentsInChildren<Renderer>();
            Bounds selectedGOBounds = rends[0].bounds;
            foreach (Renderer rend in rends)
            {
                selectedGOBounds = selectedGOBounds.GrowBounds(rend.bounds);
            }

            GameObject cloneGO = new GameObject();
            cloneGO.transform.position = tempParentGO.transform.position;
            cloneGO.transform.rotation = tempParentGO.transform.rotation;
            cloneGO.name = "Volumetric of " + tempParentGO.name;
            if (volumetricsHolderParent)
            {
                cloneGO.transform.parent = volumetricsHolderParent.transform;
            }

            DestroyImmediate(tempParentGO);
            cloneGO.transform.position = cloneGO.transform.TransformPoint(selectedGOBounds.center);
            BakedVolumetricArea volumetric = cloneGO.AddComponent<BakedVolumetricArea>();
            volumetric.BoxScale = selectedGOBounds.size + volumetricsGeoPadding;
            EditorUtility.SetDirty(volumetric);
            if (!createdObjects.Contains(cloneGO))
            {
                createdObjects.Add(cloneGO);
            }

            Undo.RegisterCreatedObjectUndo(cloneGO, "Volumetric Created");
            Selection.objects = createdObjects.ToArray();
        }

        private void CreateVolumetricsEncapsulatesGeoOnClick(Vector3 volumetricsGeoPadding)
        {
            List<GameObject> createdObjects = new List<GameObject>();
            GameObject volumetricsHolderParent = CreateVolumetricsGOContainer();
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            foreach (GameObject go in selectedGOs)
            {
                Renderer[] rends = go.GetComponentsInChildren<Renderer>();
                Bounds selectedGOBounds = rends[0].bounds;
                foreach (Renderer rend in rends)
                {
                    selectedGOBounds = selectedGOBounds.GrowBounds(rend.bounds);
                }

                GameObject cloneGO = new GameObject();
                cloneGO.transform.rotation = go.transform.rotation;
                cloneGO.name = "Volumetric of " + go.name;
                BakedVolumetricArea volumetric = cloneGO.AddComponent<BakedVolumetricArea>();
                volumetric.BoxScale = selectedGOBounds.size + volumetricsGeoPadding;
                cloneGO.transform.position = selectedGOBounds.center;
                if (volumetricsHolderParent)
                {
                    cloneGO.transform.parent = volumetricsHolderParent.transform;
                }

                EditorUtility.SetDirty(volumetric);
                if (!createdObjects.Contains(cloneGO))
                {
                    createdObjects.Add(cloneGO);
                }

                Undo.RegisterCreatedObjectUndo(cloneGO, "Volumetric Created");
            }

            Selection.objects = createdObjects.ToArray();
        }

        private void CreateVolumetricsFromGeoOnClick(Vector3 volumetricsGeoPadding)
        {
            List<GameObject> createdObjects = new List<GameObject>();
            GameObject volumetricsHolderParent = CreateVolumetricsGOContainer();
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            foreach (GameObject go in selectedGOs)
            {
                MeshFilter selectedMeshFilter = go.GetComponent<MeshFilter>();
                if (go.GetComponent<MeshRenderer>() != null && selectedMeshFilter != null)
                {
                    Bounds selectedGOBounds = selectedMeshFilter.sharedMesh.bounds;
                    selectedGOBounds.center = go.transform.TransformPoint(selectedMeshFilter.sharedMesh.bounds.center);
                    selectedGOBounds.size = Vector3.Scale(selectedGOBounds.size, go.transform.lossyScale);
                    Vector3 origRotation = go.transform.rotation.eulerAngles;
                    GameObject cloneGO = new GameObject();
                    cloneGO.transform.position = cloneGO.transform.TransformPoint(selectedGOBounds.center);
                    cloneGO.name = "Volumetric of " + go.name;
                    if (volumetricsHolderParent)
                    {
                        cloneGO.transform.parent = volumetricsHolderParent.transform;
                    }

                    BakedVolumetricArea volumetric = cloneGO.AddComponent<BakedVolumetricArea>();
                    volumetric.BoxScale = selectedGOBounds.size + volumetricsGeoPadding;
                    volumetric.BoxScale = GetRotatedVolSize(volumetric, origRotation);
                    EditorUtility.SetDirty(cloneGO);
                    if (!createdObjects.Contains(cloneGO))
                    {
                        createdObjects.Add(cloneGO);
                    }

                    Undo.RegisterCreatedObjectUndo(cloneGO, "Volumetric Created");
                }
            }

            Selection.objects = createdObjects.ToArray();
        }

        private void CreateDefaultVolumetricsOnClick()
        {
            GameObject volumetricsHolderParent = CreateVolumetricsGOContainer();
            List<GameObject> selectedGOs = Selection.gameObjects.ToList();
            List<GameObject> createdVolumetricss = new List<GameObject>();
            foreach (GameObject go in selectedGOs)
            {
                GameObject cloneGO = new GameObject();
                cloneGO.transform.position = go.transform.position;
                cloneGO.transform.rotation = Quaternion.identity;
                cloneGO.name = "Volumetric of " + go.name;
                if (volumetricsHolderParent)
                {
                    cloneGO.transform.parent = volumetricsHolderParent.transform;
                }

                if (!cloneGO.GetComponent<BakedVolumetricArea>())
                {
                    cloneGO.AddComponent<BakedVolumetricArea>();
                    cloneGO.GetComponent<BakedVolumetricArea>().BoxScale = Vector3.one;
                }

                if (!createdVolumetricss.Contains(cloneGO))
                {
                    createdVolumetricss.Add(cloneGO);
                }

                EditorUtility.SetDirty(cloneGO);
                Undo.RegisterCreatedObjectUndo(cloneGO, "Volumetric Created");
            }

            Selection.objects = createdVolumetricss.ToArray();
        }

        private GameObject CreateLightingGOContainer()
        {
            GameObject lightingParent = GameObject.Find("Generated Lighting");
            if (lightingParent == null)
            {
                lightingParent = new GameObject("Generated Lighting");
                Undo.RegisterCreatedObjectUndo(lightingParent, "Generated Lighting Parent Created");
            }

            return lightingParent;
        }

        private GameObject CreateReflectProbeGOContainer()
        {
            GameObject lightingParent = CreateLightingGOContainer();
            GameObject reflectionProbesHolderParent = GameObject.Find("Generated Reflection Probes");
            if (reflectionProbesHolderParent == null)
            {
                reflectionProbesHolderParent = new GameObject("Generated Reflection Probes");
                Undo.RegisterCreatedObjectUndo(reflectionProbesHolderParent, "Reflection Probes Parent Created");
            }

            if (reflectionProbesHolderParent.transform.parent != lightingParent)
            {
                SetChildParent(reflectionProbesHolderParent, lightingParent);
            }

            return reflectionProbesHolderParent;
        }

        private GameObject CreateLightProbeGOContainer()
        {
            GameObject lightingParent = CreateLightingGOContainer();
            GameObject lightProbesHolderParent = GameObject.Find("Generated Light Probes");
            if (lightProbesHolderParent == null)
            {
                lightProbesHolderParent = new GameObject("Generated Light Probes");
                Undo.RegisterCreatedObjectUndo(lightProbesHolderParent, "Light Probes Parent Created");
            }

            if (lightProbesHolderParent.transform.parent != lightingParent)
            {
                SetChildParent(lightProbesHolderParent, lightingParent);
            }

            return lightProbesHolderParent;
        }

        private GameObject CreateVolumetricsGOContainer()
        {
            GameObject volumetricsHolderParent = GameObject.Find("Generated Volumetrics");
            if (volumetricsHolderParent == null)
            {
                volumetricsHolderParent = new GameObject("Generated Volumetrics");
                Undo.RegisterCreatedObjectUndo(volumetricsHolderParent, "Volumetrics Parent Created");
            }

            return volumetricsHolderParent;
        }

        private void SetChildParent(GameObject lightingChildGO, GameObject lightingParent)
        {
            if (lightingParent != null)
            {
                lightingChildGO.transform.parent = lightingParent.transform;
            }
        }

        public class ZoneLinkNameSorter : IComparer
        {
            int IComparer.Compare(System.Object x, System.Object y)
            {
                return ((new CaseInsensitiveComparer()).Compare(((ZoneLink)x).name, ((ZoneLink)y).name));
            }
        }

        private void RemoveZoneComponents(GameObject cloneGO)
        {
            if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(cloneGO) > 0)
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(cloneGO);
            }

            if (cloneGO.GetComponent<Zone>() != null)
            {
                try
                {
                    Zone zone = cloneGO.GetComponent<Zone>();
                    ZoneLink zoneLink = zone.GetComponent<ZoneLink>();
                    ZoneCuller zoneCuller = zone.GetComponent<ZoneCuller>();
                    if (zoneCuller != null)
                    {
                        try
                        {
                            DestroyImmediate(zoneCuller);
                            DestroyImmediate(zoneLink);
                        }
                        catch
                        {
                        }
                    }

                    if (zoneLink != null)
                    {
                        foreach (Component component in cloneGO.GetComponents<Component>())
                        {
                            if (component != null && component.GetType() != typeof(Transform))
                            {
                                try
                                {
                                    DestroyImmediate(component);
                                }
                                catch
                                {
                                }
                            }
                        }

                        DestroyImmediate(zoneLink);
                    }

                    DestroyImmediate(zone);
                }
                catch
                {
                }
            }

            foreach (Component component in cloneGO.GetComponents<Component>())
            {
                if (component != null && component.GetType() != typeof(Transform))
                {
                    try
                    {
                        DestroyImmediate(component);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private Bounds GetLocalBoundsForObject(GameObject go)
        {
            var referenceTransform = go.transform;
            List<Transform> transformsWithMeshes = new List<Transform>();
            foreach (var childrenWithMeshes in go.GetComponentsInChildren<MeshFilter>().ToList())
            {
                transformsWithMeshes.Add(childrenWithMeshes.transform);
            }

            Vector3 centerOfSelection = FindCenterOfSelection(transformsWithMeshes.ToArray());
            var localBounds = new Bounds(centerOfSelection, Vector3.zero);
            RecurseEncapsulate(referenceTransform, ref localBounds);
            return localBounds;
            void RecurseEncapsulate(Transform child, ref Bounds bounds)
            {
                var mesh = child.GetComponent<MeshFilter>();
                if (mesh)
                {
                    var lsBounds = mesh.sharedMesh.bounds;
                    var wsMin = child.TransformPoint(lsBounds.center - lsBounds.extents);
                    var wsMax = child.TransformPoint(lsBounds.center + lsBounds.extents);
                    bounds.Encapsulate(referenceTransform.InverseTransformPoint(wsMin));
                    bounds.Encapsulate(referenceTransform.InverseTransformPoint(wsMax));
                }

                foreach (Transform grandChild in child.transform)
                {
                    RecurseEncapsulate(grandChild, ref bounds);
                }
            }
        }

        private Bounds FindZoneBounds(Zone zone)
        {
            Bounds selectedGOBounds = new Bounds();
            Collider zoneCol = zone.GetComponent<Collider>();
            BoxCollider boxCollider = null;
            SphereCollider sphereCollider = null;
            CapsuleCollider capsuleCollider = null;
            MeshCollider meshCollider = null;
            if (zoneCol is BoxCollider)
            {
                boxCollider = (BoxCollider)zoneCol;
            }
            else if (zoneCol is SphereCollider)
            {
                sphereCollider = (SphereCollider)zoneCol;
            }
            else if (zoneCol is CapsuleCollider)
            {
                capsuleCollider = (CapsuleCollider)zoneCol;
            }
            else if (zoneCol is MeshCollider)
            {
                meshCollider = (MeshCollider)zoneCol;
            }

            if (boxCollider != null && boxCollider.isTrigger == true)
            {
                selectedGOBounds = boxCollider.bounds;
            }
            else if (sphereCollider != null && sphereCollider.isTrigger == true)
            {
                selectedGOBounds = sphereCollider.bounds;
            }
            else if (capsuleCollider != null && capsuleCollider.isTrigger == true)
            {
                selectedGOBounds = capsuleCollider.bounds;
            }
            else if (meshCollider != null && meshCollider.isTrigger == true)
            {
                selectedGOBounds = meshCollider.sharedMesh.bounds;
            }

            return selectedGOBounds;
        }

        private Vector3 FindZoneCenter(Zone zone)
        {
            Gizmos.matrix = zone.transform.localToWorldMatrix;
            Vector3 zoneCenter = Vector3.zero;
            Collider zoneCol = zone.GetComponent<Collider>();
            BoxCollider boxCollider = null;
            SphereCollider sphereCollider = null;
            CapsuleCollider capsuleCollider = null;
            MeshCollider meshCollider = null;
            if (zoneCol is BoxCollider)
            {
                boxCollider = (BoxCollider)zoneCol;
                zoneCenter = boxCollider.center;
            }
            else if (zoneCol is SphereCollider)
            {
                sphereCollider = (SphereCollider)zoneCol;
                zoneCenter = sphereCollider.center;
            }
            else if (zoneCol is CapsuleCollider)
            {
                capsuleCollider = (CapsuleCollider)zoneCol;
                zoneCenter = capsuleCollider.center;
            }
            else if (zoneCol is MeshCollider)
            {
                meshCollider = (MeshCollider)zoneCol;
                Transform[] zonePosList =
                {
                    zone.transform
                };
                zoneCenter = zone.transform.InverseTransformPoint(FindCenterOfSelection(zonePosList));
            }

            return zoneCenter;
        }

        private Vector3 FindCenterOfSelection(Transform[] transformList)
        {
            if (transformList == null || transformList.Length == 0)
            {
                return Vector3.zero;
            }

            if (transformList.Length == 1)
            {
                return transformList[0].position;
            }

            float minX = Mathf.Infinity;
            float minY = Mathf.Infinity;
            float minZ = Mathf.Infinity;
            float maxX = -Mathf.Infinity;
            float maxY = -Mathf.Infinity;
            float maxZ = -Mathf.Infinity;
            foreach (Transform transformObj in transformList)
            {
                if (transformObj.position.x < minX)
                {
                    minX = transformObj.position.x;
                }

                if (transformObj.position.y < minY)
                {
                    minY = transformObj.position.y;
                }

                if (transformObj.position.z < minZ)
                {
                    minZ = transformObj.position.z;
                }

                if (transformObj.position.x > maxX)
                {
                    maxX = transformObj.position.x;
                }

                if (transformObj.position.y > maxY)
                {
                    maxY = transformObj.position.y;
                }

                if (transformObj.position.z > maxZ)
                {
                    maxZ = transformObj.position.z;
                }
            }

            return new Vector3((minX + maxX) / 2.0f, (minY + maxY) / 2.0f, (minZ + maxZ) / 2.0f);
        }

        private bool ApproxWithinTolerance(float a, float b, float tolerance)
        {
            return (Mathf.Abs(a - b) < tolerance);
        }
    }
}
#endif
