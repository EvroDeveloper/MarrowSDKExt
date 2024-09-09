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
    public static class FixtureDragHandler
    {
        static FixtureDragHandler()
        {
            DragAndDrop.RemoveDropHandler(SceneDrop);
            DragAndDrop.AddDropHandler(SceneDrop);
            DragAndDrop.RemoveDropHandler(HierarchyDrop);
            DragAndDrop.AddDropHandler(HierarchyDrop);
        }

        private static Fixture[] DraggedFixtures;
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

            Undo.SetCurrentGroupName("Place Fixture");
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
                fosterParent = new GameObject("Temporary Fixture Parent");
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
                Debug.LogException(e, DraggedFixtures.First());
                Debug.LogError($"Error during fixture instantiation: {e.Message}", DraggedFixtures.First());
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

            if (PrefabStageUtility.GetCurrentPrefabStage() == null && !DroppedOnValidGeo())
            {
                return DragAndDropVisualMode.Rejected;
            }

            if (!perform)
            {
                return DragAndDropVisualMode.Generic;
            }

            Undo.SetCurrentGroupName("Place Fixture Crate");
            var undoGroup = Undo.GetCurrentGroup();
            GameObject fosterParent = null;
            try
            {
                fosterParent = new GameObject("Temporary Fixture Parent");
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
                Debug.LogException(e, DraggedFixtures.First());
                Debug.LogError($"Error during fixture instantiation: {e.Message}", DraggedFixtures.First());
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
            DraggedFixtures ??= Array.Empty<Fixture>();
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

            if (DragAndDrop.objectReferences.Length != DraggedFixtures.Length)
            {
                DraggedFixtures = new Fixture[DragAndDrop.objectReferences.Length];
            }

            for (var i = 0; i < DragAndDrop.objectReferences.Length; i++)
            {
                var obj = DragAndDrop.objectReferences[i];
                if (!obj)
                {
                    goto FailCheck;
                }

                if (obj is not Fixture fx)
                {
                    goto FailCheck;
                }

                if (!AssetWarehouse.Instance.HasDataCard<Fixture>(fx.Barcode))
                {
                    goto FailCheck;
                }

                anObjectPassedChecks = true;
                DraggedFixtures[i] = fx;
                goto QuitEarlyCheck;
            FailCheck:
                anObjectFailedChecks = true;
                DraggedFixtures[i] = null;
            QuitEarlyCheck:
                if (anObjectPassedChecks && anObjectFailedChecks)
                {
                    Debug.LogError($"Mixed results in drag and drop for {obj.name}. Rejecting drag.", obj);
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
                    if (DragAndDrop.objectReferences.Any(a => a is Fixture))
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
                    if (DragAndDrop.objectReferences[p] is not Fixture)
                    {
                        return;
                    }

                    Fixture draggedFixture = DragAndDrop.objectReferences[p] as Fixture;
                    if (draggedFixture == null)
                    {
                        return;
                    }

                    if (draggedFixture.FixtureSpawnable == null)
                    {
                        return;
                    }

                    GameObject staticFixturePrefab = CheckForDraggedMarrowFixture(draggedFixture);
                    if (draggedFixture.FixtureSpawnable.TryGetCrate(out SpawnableCrate spawnCrate))
                    {
                        if (staticFixturePrefab == null)
                        {
                            var go = new GameObject(spawnCrate.Title);
                            go.transform.parent = dragPreviewHolder.transform;
                            var spawner = Undo.AddComponent<CrateSpawner>(go);
                            spawner.spawnableCrateReference = new SpawnableCrateReference(draggedFixture.FixtureSpawnable.Barcode);
                            spawner.EditorUpdateName();
                            GameObjectUtility.EnsureUniqueNameForSibling(go);
                        }
                        else
                        {
                            var go = new GameObject(spawnCrate.Title);
                            go.transform.parent = dragPreviewHolder.transform;
                            var spawner = go.AddComponent<CrateSpawner>();
                            spawner.spawnableCrateReference = new SpawnableCrateReference(draggedFixture.FixtureSpawnable.Barcode);
                            spawner.EditorUpdateName();
                            staticFixturePrefab.name = $"Base of {spawnCrate.name}";
                            staticFixturePrefab.transform.parent = dragPreviewHolder.transform;
                            foreach (var staticFixtureChild in staticFixturePrefab.GetComponentsInChildren<Transform>())
                            {
                                if (staticFixtureChild.name.ToLower().Contains("spawnpoint"))
                                {
                                    go.transform.position = staticFixtureChild.transform.position;
                                    EditorUtility.SetDirty(go);
                                }
                            }

                            foreach (var staticFixtureChild in staticFixturePrefab.GetComponentsInChildren<Collider>())
                            {
                                staticFixtureChild.enabled = false;
                            }

                            GameObjectUtility.EnsureUniqueNameForSibling(staticFixturePrefab);
                            GameObjectUtility.EnsureUniqueNameForSibling(go);
                        }
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

        private static bool DroppedOnValidGeo()
        {
            if (dragPreviewHolder == null)
            {
                return false;
            }

            var evt = Event.current;
            List<GameObject> ignoreFixtureSelf = new List<GameObject>();
            foreach (var gameObjTrans in dragPreviewHolder.GetComponentsInChildren<Transform>())
            {
                if (gameObjTrans == null)
                    continue;
                ignoreFixtureSelf.Add(gameObjTrans.gameObject);
            }

            if (evt.type == EventType.DragUpdated || evt.type == EventType.Repaint || evt.type == EventType.DragPerform)
            {
                GameObject pickedObject = null;
                pickedObject = HandleUtility.PickGameObject(evt.mousePosition, false, ignoreFixtureSelf?.ToArray());
                if (pickedObject != null && pickedObject.isStatic)
                {
                    return true;
                }
            }

            return false;
        }

        private static GameObject CheckForDraggedMarrowFixture(Fixture draggedFixture)
        {
            GameObject fixtureObject = null;
            if (draggedFixture != null)
            {
                if (draggedFixture.StaticFixturePrefab != null && !String.IsNullOrEmpty(draggedFixture.StaticFixturePrefab.AssetGUID))
                {
                    fixtureObject = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(draggedFixture.StaticFixturePrefab.AssetGUID)));
                }
            }

            return fixtureObject;
        }

        private static void CreateStaticFixtureBase(Vector3 worldposition, HierarchyDropFlags dropmode, (GameObject dropTarget, Scene scene) dropTarget, GameObject instantiation, Fixture draggedFixture, Transform parentfordraggedobjects)
        {
            if (draggedFixture != null)
            {
                if (draggedFixture.StaticFixturePrefab != null && draggedFixture.FixtureSpawnable != null)
                {
                    var evt = Event.current;
                    if (draggedFixture.FixtureSpawnable.TryGetCrate(out SpawnableCrate spawnCrate))
                    {
                        if (!String.IsNullOrEmpty(draggedFixture.StaticFixturePrefab.AssetGUID))
                        {
                            GameObject newStaticFixture = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(draggedFixture.StaticFixturePrefab.AssetGUID)));
                            if (newStaticFixture != null)
                            {
                                Undo.RegisterCreatedObjectUndo(newStaticFixture, "Create Static Fixture GO");
                                Undo.RegisterFullObjectHierarchyUndo(newStaticFixture, "Instantiate Static Fixture Prefab");
                                newStaticFixture.transform.position = worldposition;
                                newStaticFixture.name = $"Base of CrateSpawner ({spawnCrate.Title})";
                                foreach (var staticFixtureChild in newStaticFixture.GetComponentsInChildren<Transform>())
                                {
                                    if (staticFixtureChild.name.ToLower().Contains("spawnpoint"))
                                    {
                                        instantiation.transform.position = staticFixtureChild.transform.position;
                                        EditorUtility.SetDirty(instantiation);
                                    }
                                }

                                if (PrefabStageUtility.GetCurrentPrefabStage() == null)
                                {
                                    if (dropmode == HierarchyDropFlags.DropUpon && dropTarget.scene.IsValid())
                                    {
                                        SceneManager.MoveGameObjectToScene(newStaticFixture, instantiation.scene);
                                        newStaticFixture.transform.SetSiblingIndex(instantiation.transform.GetSiblingIndex() + 1);
                                    }

                                    if (dropmode.HasFlag(HierarchyDropFlags.DropUpon) || dropmode.HasFlag(HierarchyDropFlags.DropAfterParent) || dropmode.HasFlag(HierarchyDropFlags.DropBetween) || dropmode.HasFlag(HierarchyDropFlags.DropAbove))
                                    {
                                        newStaticFixture.transform.parent = instantiation.transform.parent;
                                        newStaticFixture.transform.SetSiblingIndex(instantiation.transform.GetSiblingIndex() + 1);
                                    }

                                    List<GameObject> ignoreFixtureSelfAndPreview = new List<GameObject>();
                                    foreach (var gameObjTrans in newStaticFixture.GetComponentsInChildren<Transform>())
                                    {
                                        if (gameObjTrans == null)
                                            continue;
                                        ignoreFixtureSelfAndPreview.Add(gameObjTrans.gameObject);
                                    }

                                    if (dragPreviewHolder)
                                    {
                                        foreach (var previewObjTrans in dragPreviewHolder.GetComponentsInChildren<Transform>())
                                        {
                                            if (previewObjTrans == null)
                                                continue;
                                            ignoreFixtureSelfAndPreview.Add(previewObjTrans.gameObject);
                                        }
                                    }

                                    if (evt.type == EventType.DragPerform)
                                    {
                                        GameObject pickedObject = null;
                                        if (dropTarget.dropTarget == null)
                                        {
                                            if (dropmode != HierarchyDropFlags.DropUpon)
                                            {
                                                pickedObject = HandleUtility.PickGameObject(evt.mousePosition, false, ignoreFixtureSelfAndPreview?.ToArray());
                                                if (pickedObject != null && pickedObject.isStatic)
                                                {
                                                    newStaticFixture.transform.parent = null;
                                                    SceneManager.MoveGameObjectToScene(newStaticFixture, pickedObject.scene);
                                                }
                                            }
                                        }

                                        GameObjectUtility.EnsureUniqueNameForSibling(newStaticFixture);
                                        EditorUtility.SetDirty(newStaticFixture);
                                    }
                                }
                                else
                                {
                                    PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                                    GameObject prefabStageRoot = prefabStage.prefabContentsRoot;
                                    SceneManager.MoveGameObjectToScene(newStaticFixture, prefabStage.scene);
                                    newStaticFixture.transform.SetParent(prefabStageRoot.transform);
                                    newStaticFixture.transform.SetAsLastSibling();
                                }
                            }
                        }

                        GameObjectUtility.EnsureUniqueNameForSibling(instantiation);
                    }
                }
            }
        }

        private static List<(GameObject, Fixture)> Instantiate(GameObject fosterParent)
        {
            var instantiationList = new List<(GameObject, Fixture)>();
            foreach (var draggedFixture in DraggedFixtures)
            {
                if (draggedFixture.FixtureSpawnable == null)
                {
                    continue;
                }

                var go = new GameObject("Auto CrateSpawner");
                Undo.RegisterCreatedObjectUndo(go, "Auto CrateSpawner Created");
                go.SetActive(false);
                var spawner = Undo.AddComponent<CrateSpawner>(go);
                if (draggedFixture.Decorators != null)
                {
                    foreach (var deco in draggedFixture.Decorators)
                    {
                        MonoScript decoMS = AssetDatabase.LoadAssetAtPath<MonoScript>(AssetDatabase.GUIDToAssetPath(deco.AssetGUID));
                        if (decoMS != null)
                        {
                            Type decoType = decoMS.GetClass();
                            Undo.AddComponent(go, decoType);
                        }
                    }
                }

                go.transform.parent = fosterParent.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                spawner.spawnableCrateReference = new SpawnableCrateReference(draggedFixture.FixtureSpawnable.Barcode);
                spawner.EditorUpdateName();
                instantiationList.Add((go, draggedFixture));
            }

            return instantiationList;
        }

        private static void RegisterCreatedObjectsUndo(List<(GameObject, Fixture)> instantiationList)
        {
            foreach (var (instantiation, _) in instantiationList)
            {
                Undo.RegisterCreatedObjectUndo(instantiation, "Instantiate CrateSpawner from Crate");
                Undo.RegisterFullObjectHierarchyUndo(instantiation, "Instantiate CrateSpawner from Crate");
            }
        }

        private static void ReparentRepositionAndReorder(List<(GameObject, Fixture)> instantiationList, Transform parentfordraggedobjects, Vector3 worldposition)
        {
            foreach (var (instantiation, draggedFixture) in instantiationList)
            {
                instantiation.transform.SetParent(parentfordraggedobjects);
                instantiation.transform.position = worldposition;
                instantiation.transform.SetAsLastSibling();
                CreateStaticFixtureBase(worldposition, HierarchyDropFlags.None, (null, default), instantiation, draggedFixture, parentfordraggedobjects);
            }
        }

        private static void ReparentRepositionAndReorder(List<(GameObject, Fixture)> instantiationList, Transform parentfordraggedobjects, HierarchyDropFlags dropmode, (GameObject go, Scene scene) dropTarget)
        {
            if (PrefabStageUtility.GetCurrentPrefabStage() == null)
            {
                if (dropmode.HasFlag(HierarchyDropFlags.DropUpon) && dropTarget.scene.IsValid())
                {
                    foreach (var (instantiation, draggedFixture) in instantiationList)
                    {
                        instantiation.transform.SetParent(null);
                        instantiation.transform.position = Vector3.zero;
                        instantiation.transform.SetAsLastSibling();
                        CreateStaticFixtureBase(Vector3.zero, dropmode, dropTarget, instantiation, draggedFixture, parentfordraggedobjects);
                    }

                    Debug.Log("Fixture Case 1");
                    return;
                }

                if (dropmode.HasFlag(HierarchyDropFlags.DropUpon) && dropTarget.go)
                {
                    Undo.RecordObject(dropTarget.go, "Record object");
                    foreach (var (instantiation, draggedFixture) in instantiationList)
                    {
                        Undo.SetTransformParent(instantiation.transform, dropTarget.go.transform, true, "Set parent");
                        instantiation.transform.SetAsLastSibling();
                        CreateStaticFixtureBase(Vector3.zero, dropmode, dropTarget, instantiation, draggedFixture, parentfordraggedobjects);
                    }

                    Debug.Log("Fixture Case 2");
                    return;
                }

                if (dropmode.HasFlag(HierarchyDropFlags.DropAfterParent) && dropmode.HasFlag(HierarchyDropFlags.DropBetween) && dropmode.HasFlag(HierarchyDropFlags.DropAbove))
                {
                    var parentTransform = dropTarget.go.transform.parent;
                    Undo.RecordObject(parentTransform.gameObject, "Record parent object");
                    for (var i = instantiationList.Count - 1; i >= 0; i--)
                    {
                        var (instantiation, draggedFixture) = instantiationList[i];
                        Undo.SetTransformParent(instantiation.transform, parentTransform, true, "Set parent");
                        instantiation.transform.SetAsFirstSibling();
                        CreateStaticFixtureBase(Vector3.zero, dropmode, dropTarget, instantiation, draggedFixture, parentfordraggedobjects);
                    }

                    Debug.Log("Fixture Case 3");
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
                        var (instantiation, draggedFixture) = instantiationList[i];
                        Undo.SetTransformParent(instantiation.transform, parentTransform, true, "Set parent");
                        instantiation.transform.SetSiblingIndex(index + 1);
                        CreateStaticFixtureBase(Vector3.zero, dropmode, dropTarget, instantiation, draggedFixture, parentfordraggedobjects);
                    }

                    Debug.Log("Fixture Case 4");
                    return;
                }
            }
            else
            {
                PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                GameObject prefabStageRoot = prefabStage.prefabContentsRoot;
                foreach (var (instantiation, draggedFixture) in instantiationList)
                {
                    instantiation.transform.parent = null;
                    SceneManager.MoveGameObjectToScene(instantiation, prefabStage.scene);
                    instantiation.transform.SetParent(prefabStageRoot.transform);
                    instantiation.transform.position = Vector3.zero;
                    instantiation.transform.SetAsLastSibling();
                    CreateStaticFixtureBase(Vector3.zero, dropmode, dropTarget, instantiation, draggedFixture, parentfordraggedobjects);
                }
            }
        }

        private static void Enable(List<(GameObject, Fixture)> instantiationList)
        {
            foreach (var (instantiation, _) in instantiationList)
            {
                instantiation.SetActive(true);
            }
        }
    }
}
#endif
