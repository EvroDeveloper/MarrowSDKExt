#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using SLZ.Marrow.Warehouse;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace SLZ.Utilities
{
    [InitializeOnLoad]
    public static class CrateDragHandler
    {
        static CrateDragHandler()
        {
            DragAndDrop.RemoveDropHandler(SceneDrop);
            DragAndDrop.AddDropHandler(SceneDrop);
            DragAndDrop.RemoveDropHandler(HierarchyDrop);
            DragAndDrop.AddDropHandler(HierarchyDrop);
        }

        private static SpawnableCrate[] DraggedCrates;
        private static bool dragPreviewsCreated = false;
        private static GameObject dragPreviewHolder;
        private static Vector3 dropPosition;
        private static DragAndDropVisualMode HierarchyDrop(int droptargetinstanceid, HierarchyDropFlags dropmode, Transform parentfordraggedobjects, bool perform)
        {
            if (!QualifyObjects(out var anObjectPassedChecks, out var anObjectFailedChecks))
            {
                if (anObjectPassedChecks && anObjectFailedChecks)
                {
                    return DragAndDropVisualMode.Rejected;
                }

                return DragAndDropVisualMode.None;
            }

            if (!perform)
            {
                return DragAndDropVisualMode.Generic;
            }

            Undo.SetCurrentGroupName("Place Spawnable Crate");
            var undoGroup = Undo.GetCurrentGroup();
            (GameObject go, Scene scene) dropTarget = (null, default);
            if (droptargetinstanceid != 0)
            {
                var target = EditorUtility.InstanceIDToObject(droptargetinstanceid) as GameObject;
                if (target)
                {
                    dropTarget = (target, default);
                }
                else
                {
                    for (var i = 0; i < SceneManager.sceneCount; i++)
                    {
                        var scene = SceneManager.GetSceneAt(i);
                        if (scene.handle != droptargetinstanceid)
                        {
                            continue;
                        }

                        dropTarget = (null, scene);
                        break;
                    }
                }
            }

            GameObject fosterParent = null;
            try
            {
                fosterParent = new GameObject("Temporary Spawnable Parent");
                fosterParent.SetActive(false);
                if (parentfordraggedobjects)
                {
                    fosterParent.transform.SetParent(parentfordraggedobjects);
                }

                if (dropTarget.scene.IsValid())
                {
                    SceneManager.MoveGameObjectToScene(fosterParent, dropTarget.scene);
                }

                var collectionList = Instantiate(fosterParent);
                RegisterCreatedObjectsUndo(collectionList);
                ReparentRepositionAndReorder(collectionList, parentfordraggedobjects, dropmode, dropTarget);
                Enable(collectionList);
            }
            catch (Exception e)
            {
                Debug.LogException(e, DraggedCrates.First());
                Debug.LogError($"Error during spawnable instantiation: {e.Message}", DraggedCrates.First());
                return DragAndDropVisualMode.Rejected;
            }
            finally
            {
                if (fosterParent)
                {
                    Object.DestroyImmediate(fosterParent);
                }

                Undo.CollapseUndoOperations(undoGroup);
            }

            return DragAndDropVisualMode.Generic;
        }

        private static DragAndDropVisualMode SceneDrop(Object dropupon, Vector3 worldposition, Vector2 viewportposition, Transform parentfordraggedobjects, bool perform)
        {
            if (!QualifyObjects(out var anObjectPassedChecks, out var anObjectFailedChecks))
            {
                if (anObjectPassedChecks && anObjectFailedChecks)
                {
                    return DragAndDropVisualMode.Rejected;
                }

                return DragAndDropVisualMode.None;
            }

            if (!perform)
            {
                return DragAndDropVisualMode.Generic;
            }

            Undo.SetCurrentGroupName("Place Spawnable Crate");
            var undoGroup = Undo.GetCurrentGroup();
            GameObject fosterParent = null;
            try
            {
                fosterParent = new GameObject("Temporary Spawnable Parent");
                fosterParent.SetActive(false);
                if (parentfordraggedobjects)
                {
                    fosterParent.transform.SetParent(parentfordraggedobjects);
                }

                var instantiationList = Instantiate(fosterParent);
                RegisterCreatedObjectsUndo(instantiationList);
                if (dropPosition != null && worldposition != dropPosition)
                {
                    worldposition = dropPosition;
                }

                ReparentRepositionAndReorder(instantiationList, parentfordraggedobjects, worldposition);
                Enable(instantiationList);
            }
            catch (Exception e)
            {
                Debug.LogException(e, DraggedCrates.First());
                Debug.LogError($"Error during spawnable instantiation: {e.Message}", DraggedCrates.First());
                if (dragPreviewHolder != null)
                {
                    DestroyObjectPreviews();
                }

                return DragAndDropVisualMode.Rejected;
            }
            finally
            {
                if (fosterParent)
                {
                    Object.DestroyImmediate(fosterParent);
                }

                Undo.CollapseUndoOperations(undoGroup);
                if (dragPreviewHolder != null)
                {
                    DestroyObjectPreviews();
                }
            }

            return DragAndDropVisualMode.Generic;
        }

        private static bool QualifyObjects(out bool anObjectPassedChecks, out bool anObjectFailedChecks)
        {
            DraggedCrates ??= Array.Empty<SpawnableCrate>();
            anObjectPassedChecks = false;
            anObjectFailedChecks = false;
            if (!AssetWarehouse.ready)
            {
                if (dragPreviewHolder != null)
                {
                    DestroyObjectPreviews();
                }

                return false;
            }

            if (DragAndDrop.objectReferences == null)
            {
                if (dragPreviewHolder != null)
                {
                    DestroyObjectPreviews();
                }

                return false;
            }

            if (DragAndDrop.objectReferences.Length != DraggedCrates.Length)
            {
                DraggedCrates = new SpawnableCrate[DragAndDrop.objectReferences.Length];
            }

            for (var i = 0; i < DragAndDrop.objectReferences.Length; i++)
            {
                var obj = DragAndDrop.objectReferences[i];
                if (!obj)
                {
                    goto FailCheck;
                }

                if (obj is not SpawnableCrate sc)
                {
                    goto FailCheck;
                }

                if (!AssetWarehouse.Instance.HasCrate<SpawnableCrate>(sc.Barcode))
                {
                    goto FailCheck;
                }

                anObjectPassedChecks = true;
                DraggedCrates[i] = sc;
                goto QuitEarlyCheck;
            FailCheck:
                anObjectFailedChecks = true;
                DraggedCrates[i] = null;
            QuitEarlyCheck:
                if (anObjectPassedChecks && anObjectFailedChecks)
                {
                    Debug.LogError($"Mixed results in drag and drop for {obj.name}. Rejecting drag.", obj);
                    if (dragPreviewHolder != null)
                    {
                        DestroyObjectPreviews();
                    }

                    return false;
                }
            }

            return anObjectPassedChecks;
        }

        [InitializeOnLoadMethod]
        static void InitSceneGUICallback()
        {
            SceneView.duringSceneGui += OnGlobalSceneGUI;
        }

        private static void OnGlobalSceneGUI(SceneView sceneView)
        {
            var evt = Event.current;
            if (evt.type == EventType.DragUpdated || evt.type == EventType.Repaint)
            {
                if (evt.type == EventType.DragUpdated && dragPreviewHolder == null && dragPreviewsCreated == false)
                {
                    if (DragAndDrop.objectReferences.Any(a => a is SpawnableCrate))
                    {
                        CreateDragObjectPreviewHolder();
                    }
                }

                if (DragAndDrop.objectReferences != null && dragPreviewsCreated == true)
                {
                    UpdateDragObjectPreviews();
                }
            }

            if (evt.type == EventType.DragPerform)
            {
                if (dragPreviewHolder != null)
                {
                    dropPosition = dragPreviewHolder.transform.position;
                }
            }

            if (evt.type == EventType.DragExited)
            {
                if (dragPreviewHolder != null)
                {
                    DestroyObjectPreviews();
                }
            }
        }

        private static void UpdateDragObjectPreviews()
        {
            if (dragPreviewHolder != null)
            {
                bool s_PlaceObject;
                Vector3 s_PlaceObjectPoint;
                Vector3 s_PlaceObjectNormal;
                Vector3 mousePosition = Event.current.mousePosition;
                Vector3 point, normal;
                float offset = 0;
                s_PlaceObject = HandleUtility.PlaceObject(mousePosition, out s_PlaceObjectPoint, out s_PlaceObjectNormal);
                point = s_PlaceObjectPoint;
                normal = s_PlaceObjectNormal;
                if (s_PlaceObject)
                {
                    dragPreviewHolder.transform.position = Matrix4x4.identity.MultiplyPoint(point + (normal * offset));
                }
                else
                {
                    dragPreviewHolder.transform.position = HandleUtility.GUIPointToWorldRay(mousePosition).GetPoint(10);
                }
            }
        }

        private static void CreateDragObjectPreviewHolder()
        {
            dragPreviewHolder = new GameObject("DragPreviewHolder");
            dragPreviewHolder.hideFlags = HideFlags.HideInHierarchy;
            if (dragPreviewHolder.transform.childCount == 0)
            {
                for (int p = 0; p < DragAndDrop.objectReferences.Length; p++)
                {
                    if (DragAndDrop.objectReferences[p] is not SpawnableCrate)
                    {
                        return;
                    }

                    SpawnableCrate draggedCrate = DragAndDrop.objectReferences[p] as SpawnableCrate;
                    if (draggedCrate != null)
                    {
                        var go = new GameObject(draggedCrate.Title);
                        go.transform.parent = dragPreviewHolder.transform;
                        var spawner = go.AddComponent<CrateSpawner>();
                        spawner.spawnableCrateReference = new SpawnableCrateReference(draggedCrate.Barcode);
                        spawner.EditorUpdateName();
                        GameObjectUtility.EnsureUniqueNameForSibling(go);
                    }
                }

                if (PrefabStageUtility.GetCurrentPrefabStage() != null)
                {
                    SceneManager.MoveGameObjectToScene(dragPreviewHolder, PrefabStageUtility.GetCurrentPrefabStage().scene);
                }
            }

            dragPreviewsCreated = true;
        }

        private static void DestroyObjectPreviews()
        {
            if (dragPreviewHolder)
            {
                GameObject.DestroyImmediate(dragPreviewHolder);
                dragPreviewHolder = null;
            }

            dragPreviewsCreated = false;
        }

        private static List<(GameObject, SpawnableCrate)> Instantiate(GameObject fosterParent)
        {
            var instantiationList = new List<(GameObject, SpawnableCrate)>();
            foreach (var draggedCrate in DraggedCrates)
            {
                var go = new GameObject("Auto CrateSpawner");
                Undo.RegisterCreatedObjectUndo(go, "Auto CrateSpawner Created");
                go.SetActive(false);
                var spawner = Undo.AddComponent<CrateSpawner>(go);
                go.transform.parent = fosterParent.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                spawner.spawnableCrateReference = new SpawnableCrateReference(draggedCrate.Barcode);
                spawner.EditorUpdateName();
                instantiationList.Add((go, draggedCrate));
            }

            return instantiationList;
        }

        private static void RegisterCreatedObjectsUndo(List<(GameObject, SpawnableCrate)> instantiationList)
        {
            foreach (var (instantiation, _) in instantiationList)
            {
                Undo.RegisterCreatedObjectUndo(instantiation, "Instantiate CrateSpawner from Crate");
                Undo.RegisterFullObjectHierarchyUndo(instantiation, "Instantiate CrateSpawner from Crate");
            }
        }

        private static void ReparentRepositionAndReorder(List<(GameObject, SpawnableCrate)> instantiationList, Transform parentfordraggedobjects, Vector3 worldposition)
        {
            foreach (var (instantiation, _) in instantiationList)
            {
                instantiation.transform.SetParent(parentfordraggedobjects);
                instantiation.transform.position = worldposition;
                instantiation.transform.SetAsLastSibling();
            }
        }

        private static void ReparentRepositionAndReorder(List<(GameObject, SpawnableCrate)> instantiationList, Transform parentfordraggedobjects, HierarchyDropFlags dropmode, (GameObject go, Scene scene) dropTarget)
        {
            if (PrefabStageUtility.GetCurrentPrefabStage() == null)
            {
                if (dropmode.HasFlag(HierarchyDropFlags.DropUpon) && dropTarget.scene.IsValid())
                {
                    foreach (var (instantiation, _) in instantiationList)
                    {
                        instantiation.transform.SetParent(null);
                        instantiation.transform.position = Vector3.zero;
                        instantiation.transform.SetAsLastSibling();
                    }

                    Debug.Log("Case 1");
                    return;
                }

                if (dropmode.HasFlag(HierarchyDropFlags.DropUpon) && dropTarget.go)
                {
                    Undo.RecordObject(dropTarget.go, "Record object");
                    foreach (var (instantiation, _) in instantiationList)
                    {
                        Undo.SetTransformParent(instantiation.transform, dropTarget.go.transform, true, "Set parent");
                        instantiation.transform.SetAsLastSibling();
                    }

                    Debug.Log("Case 2");
                    return;
                }

                if (dropmode.HasFlag(HierarchyDropFlags.DropAfterParent) && dropmode.HasFlag(HierarchyDropFlags.DropBetween) && dropmode.HasFlag(HierarchyDropFlags.DropAbove))
                {
                    var parentTransform = dropTarget.go.transform.parent;
                    Undo.RecordObject(parentTransform.gameObject, "Record parent object");
                    for (var i = instantiationList.Count - 1; i >= 0; i--)
                    {
                        var (instantiation, _) = instantiationList[i];
                        Undo.SetTransformParent(instantiation.transform, parentTransform, true, "Set parent");
                        instantiation.transform.SetAsFirstSibling();
                    }

                    Debug.Log("Case 3");
                    return;
                }

                if (dropmode.HasFlag(HierarchyDropFlags.DropBetween))
                {
                    var parentTransform = dropTarget.go.transform.parent;
                    var index = dropTarget.go.transform.GetSiblingIndex();
                    if (parentTransform)
                    {
                        Undo.RecordObject(parentTransform.gameObject, "Record parent object");
                    }

                    for (var i = instantiationList.Count - 1; i >= 0; i--)
                    {
                        var (instantiation, _) = instantiationList[i];
                        Undo.SetTransformParent(instantiation.transform, parentTransform, true, "Set parent");
                        instantiation.transform.SetSiblingIndex(index + 1);
                    }

                    Debug.Log("Case 4");
                    return;
                }
            }
            else
            {
                PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                GameObject prefabStageRoot = prefabStage.prefabContentsRoot;
                foreach (var (instantiation, _) in instantiationList)
                {
                    instantiation.transform.parent = null;
                    SceneManager.MoveGameObjectToScene(instantiation, prefabStage.scene);
                    instantiation.transform.SetParent(prefabStageRoot.transform);
                    instantiation.transform.position = Vector3.zero;
                    instantiation.transform.SetAsLastSibling();
                }
            }
        }

        private static void Enable(List<(GameObject, SpawnableCrate)> instantiationList)
        {
            foreach (var (instantiation, _) in instantiationList)
            {
                instantiation.SetActive(true);
            }
        }
    }
}
#endif
