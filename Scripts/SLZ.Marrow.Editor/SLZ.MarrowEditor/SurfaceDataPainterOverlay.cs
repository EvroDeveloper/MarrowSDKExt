#if UNITY_EDITOR
using SLZ.Marrow;
using SLZ.Marrow.Utilities;
using SLZ.Marrow.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SLZ.MarrowEditor
{
    [Overlay(typeof(SceneView), id: ID_OVERLAY_SURFACEDATA, displayName: "Surface Painter")]
    public class SurfaceDataPainterOverlay : Overlay
    {
        private const string ID_OVERLAY_SURFACEDATA = "surface-data-painter-overlay";
        private string VISUALTREE_PATH = AssetDatabase.GUIDToAssetPath("c1a8729d34b51f548897ccb3aa51c153");
        private ListView surfaceDataCardListView;
        private ListView surfaceDataCardErrorsListView;
        public SurfaceDataCard surfaceDataBrushOverlay;
        private ToolbarToggle surfacePaintModeToggle;
        private TextField sdBrushTargetTextField;
        private Task checkBrushTarg;
        private VisualElement surfacePaintColorsListViewContainer;
        private VisualElement missingImpactPropsListViewContainer;
        private Button surfaceDataPaintSelectionButton;
        private Toggle applyPhysMatsToggle;
        private List<GameObject> allGameObjects = new List<GameObject>();
        private List<GameObject> unattachedColsMissingImpactProperties = new List<GameObject>();
        private List<GameObject> rbAttachedColsMissingImpactProperties = new List<GameObject>();
        private List<Label> allLabels;
        public override VisualElement CreatePanelContent()
        {
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(VISUALTREE_PATH);
            VisualElement tree = visualTree.Instantiate();
            VisualElement rootVisualElement = new VisualElement();
            rootVisualElement.Add(tree);
            this.displayedChanged -= SurfaceDataPainterOverlay_displayedChanged;
            this.displayedChanged += SurfaceDataPainterOverlay_displayedChanged;
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeDomainReload;
            if (allLabels == null)
            {
                allLabels = new List<Label>();
            }
            else
            {
                allLabels.Clear();
            }

            allLabels = tree.Query<Label>("sdLabelField").ToList();
            SceneVisibilityManager svm = SceneVisibilityManager.instance;
            surfacePaintColorsListViewContainer = tree.Q<VisualElement>("surfacePaintColorsListViewContainer");
            missingImpactPropsListViewContainer = tree.Q<VisualElement>("missingImpactPropsListViewContainer");
            surfacePaintModeToggle = rootVisualElement.Q<ToolbarToggle>("surfacePaintModeToggle");
            surfacePaintModeToggle.RegisterValueChangedCallback(evt =>
            {
                if (surfacePaintModeToggle.value)
                {
                    EnableSurfacePaintMode();
                }
                else
                {
                    DisableSurfacePaintMode();
                }
            });
            surfaceDataPaintSelectionButton = rootVisualElement.Q<Button>("surfaceDataPaintSelectionButton");
            applyPhysMatsToggle = rootVisualElement.Q<Toggle>("applyPhysMatsToggle");
            applyPhysMatsToggle.tooltip = "If enabled, the suggested Physics Materials of the Surface Data Card will automatically be applied when the surface is painted.  If disabled, only the Surface Data will be painted.";
            surfaceDataPaintSelectionButton.clickable.clicked += () => PaintSelectedSurfaces();
            Button surfaceDataPainterCloseButton = rootVisualElement.Q<Button>("surfaceDataPainterCloseButton");
            surfaceDataPainterCloseButton.clickable.clicked += () =>
            {
                surfaceDataBrushOverlay = null;
                ImpactProperties.surfaceDataBrush = null;
                ImpactPropertiesMeshGizmo.ShowGizmo = false;
                DoWithInstances(instance => instance.displayed = false);
            };
            ToolbarToggle sdShowMissingImpactPropCols = rootVisualElement.Q<ToolbarToggle>("sdShowMissingImpactPropCols");
            sdShowMissingImpactPropCols.RegisterValueChangedCallback(evt =>
            {
                if (sdShowMissingImpactPropCols.value)
                {
                    ShowMissingImpactProps();
                }
                else
                {
                    HideMissingImpactProps();
                }
            });
            string sdImagePath = MarrowSDK.GetPackagePath("Editor/Assets/Icons/Warehouse/surfacedata.png");
            Texture2D surfaceDataCardIconImage = EditorGUIUtility.Load(sdImagePath) as Texture2D;
            Image surfaceDataImage = new Image();
            surfaceDataImage.image = surfaceDataCardIconImage;
            Button surfaceDataPainterImpactPropsUtilsButton = rootVisualElement.Q<Button>("surfaceDataPainterImpactPropsUtilsButton");
            surfaceDataPainterImpactPropsUtilsButton.style.visibility = Visibility.Visible;
            surfaceDataPainterImpactPropsUtilsButton.Add(surfaceDataImage);
            surfaceDataPainterImpactPropsUtilsButton.clickable.clicked += () =>
            {
                ImpactPropUtilsNewEditorWindow.ShowImpactPropUtilsWindow();
            };
            AssetWarehouse.OnReady(() =>
            {
                ImpactPropertiesMeshGizmo.GetAWSurfaceDataCards();
                CreateSurfaceDataListView(tree);
                surfacePaintColorsListViewContainer.Add(surfaceDataCardListView);
                surfacePaintColorsListViewContainer.Add(surfaceDataCardErrorsListView);
                surfacePaintModeToggle.SetValueWithoutNotify(true);
                DelaySurfacePaintModeAsync(800);
            });
            sdBrushTargetTextField = new TextField();
            sdBrushTargetTextField.value = ImpactProperties.surfaceDataBrushTarget?.Title;
            sdBrushTargetTextField.style.width = 0;
            sdBrushTargetTextField.style.height = 0;
            sdBrushTargetTextField.isDelayed = false;
            sdBrushTargetTextField.style.visibility = Visibility.Hidden;
            tree.Add(sdBrushTargetTextField);
            sdBrushTargetTextField.RegisterValueChangedCallback(evt =>
            {
                UpdateTargetHover(tree, allLabels);
            });
            checkBrushTarg = PeriodicAsync(async () =>
            {
                try
                {
                    await CheckBrushTarget();
                }
                catch (Exception ex)
                {
                }
            }, TimeSpan.FromSeconds(0.5));
            surfaceDataPaintSelectionButton.style.visibility = Visibility.Visible;
            tree.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                if (allLabels.Count == 0)
                {
                    allLabels = tree.Query<Label>("sdLabelField").ToList();
                }

                if (sdBrushTargetTextField.value != null)
                {
                    UpdateTargetHover(tree, allLabels);
                }
            });
            return rootVisualElement;
        }

        private void OnBeforeDomainReload()
        {
            SurfaceDataPainterOverlay.DoWithInstances(instance => instance.displayed = false);
        }

        private void SurfaceDataPainterOverlay_displayedChanged(bool overlayEnabled)
        {
            if (overlayEnabled)
            {
                if (ImpactPropertiesMeshGizmo.ShowGizmo == false)
                {
                    ImpactPropertiesMeshGizmo.ShowGizmo = true;
                }
            }
            else
            {
                ImpactPropertiesMeshGizmo.ShowGizmo = false;
                surfaceDataBrushOverlay = null;
                ImpactProperties.surfaceDataBrush = null;
                ImpactProperties.showMissingImpactProperties = false;
                if (checkBrushTarg != null && checkBrushTarg.Status == TaskStatus.Running)
                {
                    checkBrushTarg.Dispose();
                }

                SceneVisibilityManager.instance.EnableAllPicking();
            }
        }

        private void UpdateTargetHover(VisualElement tree, List<Label> allLabels)
        {
            foreach (Label label in allLabels)
            {
                label.parent?.RemoveFromClassList("forceHover");
            }

            if (sdBrushTargetTextField != null && String.IsNullOrEmpty(sdBrushTargetTextField.value) == false)
            {
                Label targetLabel = tree.Query<Label>("sdLabelField").Where(elem => elem.text?.Replace("Surface Data: ", "") == sdBrushTargetTextField.value).First();
                if (targetLabel != null)
                {
                    targetLabel.parent.AddToClassList("forceHover");
                }
            }
        }

        private void HideMissingImpactProps()
        {
            ImpactProperties.showMissingImpactProperties = false;
            EnableSurfacePaintMode();
            surfacePaintColorsListViewContainer.style.display = DisplayStyle.Flex;
            missingImpactPropsListViewContainer.style.display = DisplayStyle.None;
            surfaceDataPaintSelectionButton.style.visibility = Visibility.Visible;
            if (Selection.activeGameObject == null)
            {
                return;
            }

            ImpactProperties selectedIPChild = Selection.activeGameObject.GetComponentInChildren<ImpactProperties>();
            if (selectedIPChild == null)
            {
                Selection.objects = null;
            }
            else
            {
                Selection.objects = new List<GameObject>
                {
                    selectedIPChild.gameObject
                }.ToArray();
            }
        }

        private void ShowMissingImpactProps()
        {
            SceneVisibilityManager.instance.EnableAllPicking();
            ImpactProperties.showMissingImpactProperties = true;
            surfacePaintColorsListViewContainer.style.display = DisplayStyle.None;
            missingImpactPropsListViewContainer.style.display = DisplayStyle.Flex;
            surfaceDataPaintSelectionButton.style.visibility = Visibility.Hidden;
            if (Selection.activeGameObject == null)
            {
                Debug.Log("Select any Impact Properties object and re-toggle this button to see if other objects are missing their Impact Props components");
                return;
            }

            missingImpactPropsListViewContainer.Clear();
            if (allGameObjects == null)
            {
                allGameObjects = new List<GameObject>();
                unattachedColsMissingImpactProperties = new List<GameObject>();
                rbAttachedColsMissingImpactProperties = new List<GameObject>();
            }
            else
            {
                allGameObjects.Clear();
                unattachedColsMissingImpactProperties.Clear();
                rbAttachedColsMissingImpactProperties.Clear();
            }

            allGameObjects = GameObject.FindObjectsOfType<GameObject>().ToList();
            foreach (GameObject gameObj in allGameObjects)
            {
                if (gameObj.TryGetComponent<Collider>(out var gameObjHasCol) && !gameObjHasCol.isTrigger && !gameObj.GetComponent<ImpactProperties>())
                {
                    if (gameObjHasCol.attachedRigidbody == null)
                    {
                        if (!unattachedColsMissingImpactProperties.Contains(gameObj))
                        {
                            unattachedColsMissingImpactProperties.Add(gameObj);
                        }
                    }
                    else
                    {
                        if (gameObjHasCol.attachedRigidbody.gameObject.GetComponent<ImpactProperties>() == null)
                        {
                            if (!rbAttachedColsMissingImpactProperties.Contains(gameObjHasCol.attachedRigidbody.gameObject))
                            {
                                rbAttachedColsMissingImpactProperties.Add(gameObjHasCol.attachedRigidbody.gameObject);
                            }
                        }
                    }
                }
            }

            Label missingIPListLabel = new Label();
            missingIPListLabel.text = "Non-Attached Cols<br>Missing Impact Properties";
            missingIPListLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            missingIPListLabel.style.marginBottom = 5;
            missingImpactPropsListViewContainer.Add(missingIPListLabel);
            ListView missingImpactPropsListView = new ListView();
            missingImpactPropsListView.fixedItemHeight = 20;
            missingImpactPropsListView.showAlternatingRowBackgrounds = AlternatingRowBackground.None;
            missingImpactPropsListView.style.marginBottom = 10;
            missingImpactPropsListView.showBoundCollectionSize = false;
            missingImpactPropsListView.selectionType = SelectionType.Multiple;
            missingImpactPropsListView.itemsSource = unattachedColsMissingImpactProperties;
            float missingIPListViewHeightMax = unattachedColsMissingImpactProperties.Count * missingImpactPropsListView.fixedItemHeight;
            if (missingIPListViewHeightMax > 250)
            {
                missingIPListViewHeightMax = 250;
            }

            missingImpactPropsListView.style.height = missingIPListViewHeightMax;
            Func<VisualElement> makeMIPItem = () => new VisualElement();
            Action<VisualElement, int> bindMIPItem = (mipContainer, i) =>
            {
                mipContainer.Clear();
                mipContainer.style.flexDirection = FlexDirection.Row;
                mipContainer.style.justifyContent = Justify.FlexStart;
                mipContainer.style.width = 130;
                Label mipLabelField = new Label();
                mipLabelField.name = "mipLabelField";
                mipLabelField.style.marginLeft = 5;
                mipContainer.Add(mipLabelField);
                mipLabelField.text = unattachedColsMissingImpactProperties[i].name;
                mipLabelField.tooltip = unattachedColsMissingImpactProperties[i].name;
            };
            missingImpactPropsListView.makeItem = makeMIPItem;
            missingImpactPropsListView.bindItem = bindMIPItem;
            missingImpactPropsListView.onSelectionChange -= OverlayMissingIPListSelectionChanged;
            missingImpactPropsListView.onSelectionChange += OverlayMissingIPListSelectionChanged;
            Button addMissingImpactPropsButton = new Button();
            addMissingImpactPropsButton.text = "Add ImpactProp to Col";
            addMissingImpactPropsButton.style.marginBottom = 5;
            addMissingImpactPropsButton.clickable.clicked += () =>
            {
                foreach (var selObj in missingImpactPropsListView.selectedItems)
                {
                    GameObject selGO = selObj as GameObject;
                    if (selGO.GetComponent<ImpactProperties>() == null)
                    {
                        Undo.RecordObject(selGO, "ImpactProperties Added");
                        ImpactPropertiesMeshGizmo.DisableGizmo(selGO.GetComponent<Collider>());
                        ImpactProperties addedIP = selGO.AddComponent<ImpactProperties>();
                        if (addedIP.SurfaceDataCard == null)
                        {
                            addedIP.SurfaceDataCard = new DataCardReference<SurfaceDataCard>();
                        }

                        if (ImpactPropertiesMeshGizmo.surfaceDataCardList[2] != null && ImpactPropertiesMeshGizmo.surfaceDataCardList[2].IsValid())
                        {
                            addedIP.SurfaceDataCard.Barcode = ImpactPropertiesMeshGizmo.surfaceDataCardList[2];
                        }

                        EditorUtility.SetDirty(selGO);
                        if (PrefabUtility.GetPrefabAssetType(selGO) == PrefabAssetType.Regular || PrefabUtility.GetPrefabAssetType(selGO) == PrefabAssetType.Variant)
                        {
                            PrefabUtility.RecordPrefabInstancePropertyModifications(selGO);
                        }
                    }
                }
            };
            missingImpactPropsListViewContainer.Add(addMissingImpactPropsButton);
            missingImpactPropsListViewContainer.Add(missingImpactPropsListView);
            Label rbMissingIPListLabel = new Label();
            rbMissingIPListLabel.text = "Rigid Bodies Missing<br>Impact Properties";
            rbMissingIPListLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            rbMissingIPListLabel.style.marginTop = 5;
            rbMissingIPListLabel.style.marginBottom = 5;
            missingImpactPropsListViewContainer.Add(rbMissingIPListLabel);
            ListView rbMissingImpactPropsListView = new ListView();
            rbMissingImpactPropsListView.fixedItemHeight = 20;
            rbMissingImpactPropsListView.showAlternatingRowBackgrounds = AlternatingRowBackground.None;
            rbMissingImpactPropsListView.showBoundCollectionSize = false;
            rbMissingImpactPropsListView.selectionType = SelectionType.Multiple;
            rbMissingImpactPropsListView.itemsSource = rbAttachedColsMissingImpactProperties;
            float rbMissingIPListViewHeightMax = rbAttachedColsMissingImpactProperties.Count * rbMissingImpactPropsListView.fixedItemHeight;
            if (rbMissingIPListViewHeightMax > 250)
            {
                rbMissingIPListViewHeightMax = 250;
            }

            rbMissingImpactPropsListView.style.height = rbMissingIPListViewHeightMax;
            Func<VisualElement> makeMRBItem = () => new VisualElement();
            Action<VisualElement, int> bindMRBItem = (mrbContainer, i) =>
            {
                mrbContainer.Clear();
                mrbContainer.style.flexDirection = FlexDirection.Row;
                mrbContainer.style.justifyContent = Justify.FlexStart;
                mrbContainer.style.width = 130;
                Label mrbLabelField = new Label();
                mrbLabelField.name = "mrbLabelField";
                mrbLabelField.style.marginLeft = 5;
                mrbContainer.Add(mrbLabelField);
                mrbLabelField.text = rbAttachedColsMissingImpactProperties[i].name;
                mrbLabelField.tooltip = rbAttachedColsMissingImpactProperties[i].name;
            };
            rbMissingImpactPropsListView.makeItem = makeMRBItem;
            rbMissingImpactPropsListView.bindItem = bindMRBItem;
            rbMissingImpactPropsListView.onSelectionChange -= OverlayRBMissingIPListSelectionChanged;
            rbMissingImpactPropsListView.onSelectionChange += OverlayRBMissingIPListSelectionChanged;
            Button rbAddMissingImpactPropsButton = new Button();
            rbAddMissingImpactPropsButton.text = "Add ImpactProp to RB";
            rbAddMissingImpactPropsButton.clickable.clicked += () =>
            {
                foreach (var selObj in rbMissingImpactPropsListView.selectedItems)
                {
                    GameObject selGO = selObj as GameObject;
                    if (selGO.GetComponent<ImpactProperties>() == null)
                    {
                        Undo.RecordObject(selGO, "ImpactProperties Added");
                        ImpactProperties addedIP = selGO.AddComponent<ImpactProperties>();
                        if (addedIP.SurfaceDataCard == null)
                        {
                            addedIP.SurfaceDataCard = new DataCardReference<SurfaceDataCard>();
                        }

                        if (ImpactPropertiesMeshGizmo.surfaceDataCardList[2] != null && ImpactPropertiesMeshGizmo.surfaceDataCardList[2].IsValid())
                        {
                            addedIP.SurfaceDataCard.Barcode = ImpactPropertiesMeshGizmo.surfaceDataCardList[2];
                        }

                        ImpactPropertiesMeshGizmo.DisableGizmo(addedIP);
                        EditorUtility.SetDirty(selGO);
                        if (PrefabUtility.GetPrefabAssetType(selGO) == PrefabAssetType.Regular || PrefabUtility.GetPrefabAssetType(selGO) == PrefabAssetType.Variant)
                        {
                            PrefabUtility.RecordPrefabInstancePropertyModifications(selGO);
                        }
                    }
                }
            };
            missingImpactPropsListViewContainer.Add(rbAddMissingImpactPropsButton);
            missingImpactPropsListViewContainer.Add(rbMissingImpactPropsListView);
        }

        private void OverlayRBMissingIPListSelectionChanged(IEnumerable<object> enumerable)
        {
            List<GameObject> selGameObjs = new List<GameObject>();
            foreach (object selObj in enumerable)
            {
                GameObject selGO = selObj as GameObject;
                selGameObjs.Add(selGO);
            }

            Selection.objects = selGameObjs.ToArray();
        }

        private void OverlayMissingIPListSelectionChanged(IEnumerable<object> enumerable)
        {
            List<GameObject> selGameObjs = new List<GameObject>();
            foreach (object selObj in enumerable)
            {
                GameObject selGO = selObj as GameObject;
                selGameObjs.Add(selGO);
            }

            Selection.objects = selGameObjs.ToArray();
        }

        private bool ImpactPropertyIsMissing(GameObject selGO)
        {
            if (selGO != null && selGO.GetComponent<ImpactProperties>() == null && selGO.TryGetComponent<Collider>(out var selectedCol))
            {
                if (selectedCol != null && selectedCol.attachedRigidbody == null)
                {
                    if (selectedCol.GetComponent<ImpactProperties>() == null)
                    {
                        return true;
                    }
                }
                else if (selectedCol.attachedRigidbody.gameObject.GetComponent<ImpactProperties>() == null)
                {
                    return true;
                }
            }

            return false;
        }

        private GameObject AddMissingImpactProps(GameObject selGO)
        {
            if (selGO != null && selGO.GetComponent<ImpactProperties>() == null && selGO.TryGetComponent<Collider>(out var selectedCol))
            {
                if (selectedCol != null && selectedCol.attachedRigidbody == null)
                {
                    if (selectedCol.GetComponent<ImpactProperties>() == null)
                    {
                        Undo.RecordObject(selectedCol, "ImpactProperties Added");
                        ImpactProperties addedIP = selectedCol.gameObject.AddComponent<ImpactProperties>();
                        if (addedIP.SurfaceDataCard == null)
                        {
                            addedIP.SurfaceDataCard = new DataCardReference<SurfaceDataCard>();
                        }

                        ImpactPropertiesMeshGizmo.DisableGizmo(selectedCol);
                        EditorUtility.SetDirty(selectedCol);
                        if (PrefabUtility.GetPrefabAssetType(selectedCol) == PrefabAssetType.Regular || PrefabUtility.GetPrefabAssetType(selectedCol) == PrefabAssetType.Variant)
                        {
                            PrefabUtility.RecordPrefabInstancePropertyModifications(selectedCol);
                        }

                        return addedIP.gameObject;
                    }
                }
                else if (selectedCol.attachedRigidbody.gameObject.GetComponent<ImpactProperties>() == null)
                {
                    Undo.RecordObject(selectedCol.attachedRigidbody.gameObject, "ImpactProperties Added");
                    ImpactProperties addedIP = selectedCol.attachedRigidbody.gameObject.AddComponent<ImpactProperties>();
                    if (addedIP.SurfaceDataCard == null)
                    {
                        addedIP.SurfaceDataCard = new DataCardReference<SurfaceDataCard>();
                    }

                    foreach (var attachedCol in selectedCol.attachedRigidbody.GetComponentsInChildren<Collider>())
                    {
                        ImpactPropertiesMeshGizmo.DisableGizmo(attachedCol);
                    }

                    EditorUtility.SetDirty(selectedCol.attachedRigidbody.gameObject);
                    if (PrefabUtility.GetPrefabAssetType(selectedCol.attachedRigidbody.gameObject) == PrefabAssetType.Regular || PrefabUtility.GetPrefabAssetType(selectedCol.attachedRigidbody.gameObject) == PrefabAssetType.Variant)
                    {
                        PrefabUtility.RecordPrefabInstancePropertyModifications(selectedCol.attachedRigidbody.gameObject);
                    }

                    return addedIP.gameObject;
                }
            }

            return null;
        }

        private void PaintSelectedSurfaces()
        {
            if (surfaceDataBrushOverlay == null)
            {
                Debug.Log("Please select a Surface Brush before 'painting'");
                return;
            }

            List<GameObject> selectedObjs = Selection.gameObjects.ToList();
            foreach (var selObj in Selection.gameObjects)
            {
                GameObject goWithImpactProp = selObj;
                if (selObj != null && selObj.TryGetComponent<ImpactPropertiesMeshGizmo>(out var impactPropGizmoNoIP))
                {
                    if (impactPropGizmoNoIP)
                    {
                        ImpactProperties impactPropGizmoParent = impactPropGizmoNoIP.GetComponentInParent<ImpactProperties>();
                        if (impactPropGizmoParent == null)
                        {
                            Transform gizmoParent = impactPropGizmoNoIP.transform.parent;
                            if (gizmoParent != null)
                            {
                                selectedObjs.Insert(selectedObjs.IndexOf(selObj), gizmoParent.gameObject);
                                selectedObjs.Remove(selObj);
                                goWithImpactProp = AddMissingImpactProps(gizmoParent.gameObject);
                            }
                        }
                        else
                        {
                            goWithImpactProp = impactPropGizmoParent.gameObject;
                        }
                    }
                }
                else if (selObj != null && selObj.TryGetComponent<Collider>(out var gameObjHasCol) && !gameObjHasCol.isTrigger && !selObj.GetComponent<ImpactProperties>())
                {
                    goWithImpactProp = AddMissingImpactProps(selObj);
                }

                if (goWithImpactProp != null)
                {
                }

                if (goWithImpactProp != null && goWithImpactProp.TryGetComponent<ImpactProperties>(out var impactProp))
                {
                    if (impactProp)
                    {
                        Undo.RecordObject(impactProp, "ImpactProperties SurfaceData Painted");
                        impactProp.SurfaceDataCard.Barcode = surfaceDataBrushOverlay.Barcode;
                        EditorUtility.SetDirty(impactProp);
                        if (PrefabUtility.GetPrefabAssetType(impactProp) == PrefabAssetType.Regular || PrefabUtility.GetPrefabAssetType(impactProp) == PrefabAssetType.Variant)
                        {
                            PrefabUtility.RecordPrefabInstancePropertyModifications(impactProp);
                        }

                        ImpactPropertiesMeshGizmo.DisableGizmo(impactProp);
                        if (applyPhysMatsToggle.value == true)
                        {
                            AddEmptyPhysMats(impactProp);
                        }
                    }
                }
                else if (goWithImpactProp != null && goWithImpactProp.TryGetComponent<ImpactPropertiesMeshGizmo>(out var impactPropGizmo))
                {
                    if (impactPropGizmo)
                    {
                        ImpactProperties impactProperty = impactPropGizmo.GetComponentInParent<ImpactProperties>();
                        if (impactProperty != null)
                        {
                            Undo.RecordObject(impactProperty, "ImpactProperties SurfaceData Painted");
                            impactProperty.SurfaceDataCard.Barcode = surfaceDataBrushOverlay.Barcode;
                            EditorUtility.SetDirty(impactProperty);
                            if (PrefabUtility.GetPrefabAssetType(impactProperty) == PrefabAssetType.Regular || PrefabUtility.GetPrefabAssetType(impactProperty) == PrefabAssetType.Variant)
                            {
                                PrefabUtility.RecordPrefabInstancePropertyModifications(impactProperty);
                            }

                            ImpactPropertiesMeshGizmo.DisableGizmo(impactProperty);
                            selectedObjs.Remove(goWithImpactProp);
                            selectedObjs.Add(impactProperty.gameObject);
                            if (applyPhysMatsToggle.value == true)
                            {
                                AddEmptyPhysMats(impactProperty);
                            }
                        }
                    }
                }
                else
                {
                }
            }

            Selection.objects = null;
            DelayMultiSelectionAsync(selectedObjs.ToArray());
        }

        private void AddEmptyPhysMats(ImpactProperties impactProp)
        {
            SurfaceDataCard sdc = impactProp.SurfaceDataCard.DataCard;
            PhysicMaterial sdcStaticMaterial = (PhysicMaterial)AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(sdc.PhysicsMaterialStatic.AssetGUID));
            PhysicMaterial sdcDynamicMaterial = (PhysicMaterial)AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(sdc.PhysicsMaterial.AssetGUID));
            PhysicMaterial suggestedPhysMat = null;
            if (impactProp.TryGetComponent<Collider>(out var siblingCol))
            {
                if (siblingCol.attachedRigidbody == null)
                {
                    suggestedPhysMat = sdcStaticMaterial;
                }
                else
                {
                    suggestedPhysMat = sdcDynamicMaterial;
                }

                Undo.RecordObject(siblingCol, "Empty Physics Material Painted");
                siblingCol.sharedMaterial = suggestedPhysMat;
                EditorUtility.SetDirty(siblingCol);
                if (PrefabUtility.GetPrefabAssetType(siblingCol) == PrefabAssetType.Regular || PrefabUtility.GetPrefabAssetType(siblingCol) == PrefabAssetType.Variant)
                {
                    PrefabUtility.RecordPrefabInstancePropertyModifications(siblingCol);
                }
            }
            else if (impactProp.TryGetComponent<Rigidbody>(out var ipRigidBody))
            {
                foreach (Collider rbCol in ipRigidBody.GetComponentsInChildren<Collider>())
                {
                    if (rbCol.attachedRigidbody == ipRigidBody)
                    {
                        suggestedPhysMat = sdcDynamicMaterial;
                        Undo.RecordObject(siblingCol, "Empty Physics Material Painted");
                        siblingCol.sharedMaterial = suggestedPhysMat;
                        EditorUtility.SetDirty(siblingCol);
                        if (PrefabUtility.GetPrefabAssetType(siblingCol) == PrefabAssetType.Regular || PrefabUtility.GetPrefabAssetType(siblingCol) == PrefabAssetType.Variant)
                        {
                            PrefabUtility.RecordPrefabInstancePropertyModifications(siblingCol);
                        }
                    }
                }
            }
        }

        private Task CheckBrushTarget()
        {
            sdBrushTargetTextField.value = ImpactProperties.surfaceDataBrushTarget?.Title;
            return Task.CompletedTask;
        }

        private void CreateSurfaceDataListView(VisualElement tree)
        {
            ImpactPropertiesMeshGizmo.BuildSurfaceDataColorDict();
            if (surfaceDataCardListView == null)
            {
                surfaceDataCardListView = new ListView();
            }
            else
            {
                surfaceDataCardListView.Clear();
            }

            surfaceDataCardListView.fixedItemHeight = 20;
            surfaceDataCardListView.showAlternatingRowBackgrounds = AlternatingRowBackground.None;
            surfaceDataCardListView.showBoundCollectionSize = false;
            surfaceDataCardListView.selectionType = SelectionType.Single;
            surfaceDataCardListView.style.marginBottom = 10;
            float sdcListViewHeightMax = ImpactPropertiesMeshGizmo.surfaceDataCardList.Count * surfaceDataCardListView.fixedItemHeight;
            if (sdcListViewHeightMax > 500)
            {
                sdcListViewHeightMax = 500;
            }

            surfaceDataCardListView.style.height = sdcListViewHeightMax;
            surfaceDataCardListView.itemsSource = ImpactPropertiesMeshGizmo.surfaceDataCardList;
            Func<VisualElement> makeItem = () => new VisualElement();
            Action<VisualElement, int> bindItem = (sdContainer, i) =>
            {
                sdContainer.Clear();
                sdContainer.style.flexDirection = FlexDirection.Row;
                sdContainer.style.alignItems = Align.Center;
                sdContainer.style.justifyContent = Justify.FlexStart;
                sdContainer.style.width = 130;
                VisualElement sdColorContainer = new VisualElement();
                sdColorContainer.style.width = 30;
                sdColorContainer.style.height = 15;
                sdColorContainer.style.backgroundColor = ImpactPropertiesMeshGizmo.surfaceDataCardColorDict[ImpactPropertiesMeshGizmo.surfaceDataCardList[i]];
                sdColorContainer.style.borderBottomColor = Color.gray;
                sdColorContainer.style.borderTopColor = Color.gray;
                sdColorContainer.style.borderLeftColor = Color.gray;
                sdColorContainer.style.borderRightColor = Color.gray;
                sdColorContainer.style.borderBottomWidth = 1;
                sdColorContainer.style.borderTopWidth = 1;
                sdColorContainer.style.borderLeftWidth = 1;
                sdColorContainer.style.borderRightWidth = 1;
                sdColorContainer.style.borderTopLeftRadius = 2;
                sdColorContainer.style.borderTopRightRadius = 2;
                sdColorContainer.style.borderBottomLeftRadius = 2;
                sdColorContainer.style.borderBottomRightRadius = 2;
                sdColorContainer.style.paddingBottom = 2;
                sdColorContainer.style.paddingTop = 2;
                sdContainer.Add(sdColorContainer);
                Label sdLabelField = new Label();
                sdLabelField.name = "sdLabelField";
                sdLabelField.style.marginLeft = 5;
                sdContainer.Add(sdLabelField);
                if (AssetWarehouse.ready && !Application.isPlaying && AssetWarehouse.Instance.TryGetDataCard(ImpactPropertiesMeshGizmo.surfaceDataCardList[i], out var dataCard))
                {
                    sdLabelField.text = dataCard.Title;
                    sdLabelField.tooltip = dataCard.Title;
                }
            };
            surfaceDataCardListView.makeItem = makeItem;
            surfaceDataCardListView.bindItem = bindItem;
            surfaceDataCardListView.onSelectionChange -= OverlayListSelectionChanged;
            surfaceDataCardListView.onSelectionChange += OverlayListSelectionChanged;
            List<string> surfaceDataErrorsList = new List<string>
            {
                "Missing IP",
                "Missing SD"
            };
            List<string> surfaceDataErrorsTooltipList = new List<string>
            {
                "Missing Impact Properties",
                "Missing Surface Data Card"
            };
            string missingImpactPropImagePath = MarrowSDK.GetPackagePath("Editor/Assets/Icons/Warehouse/magenta_checkerboard.png");
            Texture2D missingImpactPropImageTex = EditorGUIUtility.Load(missingImpactPropImagePath) as Texture2D;
            string missingSurfaceDataCardImagePath = MarrowSDK.GetPackagePath("Editor/Assets/Icons/Warehouse/cyan_checkerboard.png");
            Texture2D missingSurfaceDataCardImageTex = EditorGUIUtility.Load(missingSurfaceDataCardImagePath) as Texture2D;
            Dictionary<string, Texture2D> surfaceDataErrorsDict = new Dictionary<string, Texture2D>();
            foreach (var sdError in surfaceDataErrorsList)
            {
                if (sdError == "Missing IP")
                {
                    surfaceDataErrorsDict.TryAdd(sdError, missingImpactPropImageTex);
                }
                else
                {
                    surfaceDataErrorsDict.TryAdd(sdError, missingSurfaceDataCardImageTex);
                }
            }

            surfaceDataCardErrorsListView = new ListView();
            surfaceDataCardErrorsListView.fixedItemHeight = 20;
            surfaceDataCardErrorsListView.horizontalScrollingEnabled = false;
            surfaceDataCardErrorsListView.showBoundCollectionSize = false;
            surfaceDataCardErrorsListView.selectionType = SelectionType.None;
            surfaceDataCardErrorsListView.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
            float sdcErrorsListViewHeightMax = surfaceDataErrorsList.Count * surfaceDataCardErrorsListView.fixedItemHeight;
            if (sdcErrorsListViewHeightMax > 500)
            {
                sdcErrorsListViewHeightMax = 500;
            }

            surfaceDataCardErrorsListView.style.height = sdcErrorsListViewHeightMax;
            surfaceDataCardErrorsListView.itemsSource = surfaceDataErrorsList;
            Func<VisualElement> makeErrorsItem = () => new VisualElement();
            Action<VisualElement, int> bindErrorsItem = (sdContainer, i) =>
            {
                sdContainer.Clear();
                sdContainer.style.flexDirection = FlexDirection.Row;
                sdContainer.style.alignItems = Align.Center;
                sdContainer.style.justifyContent = Justify.FlexStart;
                sdContainer.style.width = 130;
                VisualElement sdColorContainer = new VisualElement();
                sdColorContainer.style.width = 30;
                sdColorContainer.style.height = 15;
                sdColorContainer.style.backgroundImage = surfaceDataErrorsDict[surfaceDataErrorsList[i]];
                sdColorContainer.style.borderBottomColor = Color.gray;
                sdColorContainer.style.borderTopColor = Color.gray;
                sdColorContainer.style.borderLeftColor = Color.gray;
                sdColorContainer.style.borderRightColor = Color.gray;
                sdColorContainer.style.borderBottomWidth = 1;
                sdColorContainer.style.borderTopWidth = 1;
                sdColorContainer.style.borderLeftWidth = 1;
                sdColorContainer.style.borderRightWidth = 1;
                sdColorContainer.style.borderTopLeftRadius = 2;
                sdColorContainer.style.borderTopRightRadius = 2;
                sdColorContainer.style.borderBottomLeftRadius = 2;
                sdColorContainer.style.borderBottomRightRadius = 2;
                sdColorContainer.style.paddingBottom = 2;
                sdColorContainer.style.paddingTop = 2;
                sdContainer.Add(sdColorContainer);
                Label sdLabelField = new Label();
                sdLabelField.name = "sdLabelField";
                sdLabelField.style.marginLeft = 5;
                sdContainer.Add(sdLabelField);
                sdLabelField.text = surfaceDataErrorsList[i];
                sdLabelField.tooltip = surfaceDataErrorsTooltipList[i];
            };
            surfaceDataCardErrorsListView.makeItem = makeErrorsItem;
            surfaceDataCardErrorsListView.bindItem = bindErrorsItem;
        }

        private void OverlayListSelectionChanged(IEnumerable<object> objects)
        {
            if (AssetWarehouse.ready && !Application.isPlaying && AssetWarehouse.Instance.TryGetDataCard(ImpactPropertiesMeshGizmo.surfaceDataCardList[surfaceDataCardListView.selectedIndex], out var dataCard))
            {
                surfaceDataBrushOverlay = dataCard as SurfaceDataCard;
                ImpactProperties.surfaceDataBrush = surfaceDataBrushOverlay;
                if (ImpactPropertiesMeshGizmo.ShowGizmo == false)
                {
                    ImpactPropertiesMeshGizmo.ShowGizmo = true;
                }
            }
        }

        private void EnableSurfacePaintMode()
        {
            List<GameObject> nonIPGizmoGameObjects = new List<GameObject>();
            nonIPGizmoGameObjects.Clear();
            foreach (GameObject gameObj in UnityEngine.Object.FindObjectsOfType<GameObject>())
            {
                if (!gameObj.GetComponent<ImpactProperties>())
                {
                    if (gameObj.TryGetComponent<Collider>(out var gameObjCol) && gameObjCol.isTrigger == false)
                    {
                        continue;
                    }
                    else
                    {
                        nonIPGizmoGameObjects.Add(gameObj);
                    }
                }
            }

            SceneVisibilityManager.instance.DisablePicking(nonIPGizmoGameObjects.ToArray(), false);
            surfacePaintModeToggle.label = "Only Impact<br>Properties Pickable";
        }

        private void DisableSurfacePaintMode()
        {
            SceneVisibilityManager.instance.EnableAllPicking();
            surfacePaintModeToggle.label = "All Objects<br>Pickable";
            surfaceDataBrushOverlay = null;
            ImpactProperties.surfaceDataBrush = null;
        }

        async void DelaySurfacePaintModeAsync(int delay)
        {
            await Task.Delay(delay);
            EnableSurfacePaintMode();
        }

        async void DelayUpdateImpactPropsGizmoChildAsync(int delay, ImpactProperties impactProp)
        {
            await Task.Delay(delay);
            ImpactPropertiesMeshGizmo.DisableGizmo(impactProp);
        }

        public static async Task PeriodicAsync(Func<Task> action, TimeSpan interval, CancellationToken cancellationToken = default)
        {
            while (true)
            {
                Task delayTask = Task.Delay(interval, cancellationToken);
                await action();
                await delayTask;
            }
        }

        async static void DelayMultiSelectionAsync(GameObject[] selObjects)
        {
            await Task.Delay(800);
            if (selObjects != null)
            {
                Selection.objects = selObjects;
            }
        }

        private static List<SurfaceDataPainterOverlay> instances = new List<SurfaceDataPainterOverlay>();
        public override void OnCreated()
        {
            instances.Add(this);
        }

        public override void OnWillBeDestroyed()
        {
            instances.Remove(this);
        }

        public static void DoWithInstances(Action<SurfaceDataPainterOverlay> doWithInstance)
        {
            foreach (var instance in instances)
            {
                doWithInstance(instance);
            }
        }
    }
}
#endif
