 
 
 
 
 
 
 
using UnityEngine;
using SLZ.Marrow.Combat;
using SLZ.Data;
 
using SLZ.Marrow.Interaction;
using SLZ.Marrow.Warehouse;
 
using UnityEditor;

namespace SLZ.Marrow
{
    [AddComponentMenu("MarrowSDK/Impact Properties")]
    public class ImpactProperties : MonoBehaviour, IAttackReceiver
    {
        [Header("References")]
        [SerializeField]
        private DataCardReference<SurfaceDataCard> _surfaceDataCard;
        public DataCardReference<SurfaceDataCard> SurfaceDataCard { get => _surfaceDataCard; set => _surfaceDataCard = value; }
        public SurfaceData surfaceData;
        public enum DecalType
        {
            None = -1,
            Collider = 0,
        };
        public DecalType decalType;
#if UNITY_EDITOR
        [HideInInspector]
        public static SurfaceDataCard surfaceDataBrush;
        [HideInInspector]
        public static SurfaceDataCard surfaceDataBrushTarget;
        [HideInInspector]
        public static bool showMissingImpactProperties;
#endif
#if UNITY_EDITOR
        private void ApplyPhysMatsRecursive(Transform p)
        {
            Debug.Log($"Surface Data Card Changed, ApplyPhysMatsRecursive on {p.name}");
            var isDynamic = gameObject.TryGetComponent<Rigidbody>(out var rb) && !rb.isKinematic;
            var colliders = p.GetComponents<Collider>();
            if (ScannableReference.IsValid(SurfaceDataCard) && SurfaceDataCard.TryGetDataCard(out var dataCard))
            {
                foreach (var col in colliders)
                {
                    if (col.isTrigger)
                        continue;
                    col.sharedMaterial = (isDynamic) ? dataCard.PhysicsMaterial.EditorAsset : dataCard.PhysicsMaterialStatic.EditorAsset;
                    EditorUtility.SetDirty(col);
                    if (PrefabUtility.GetPrefabAssetType(col) == PrefabAssetType.Regular || PrefabUtility.GetPrefabAssetType(col) == PrefabAssetType.Variant)
                    {
                        PrefabUtility.RecordPrefabInstancePropertyModifications(col);
                    }
                }
            }

            for (int i = 0; i < p.childCount; i++)
            {
                var child = p.GetChild(i);
                var isATracker = child.gameObject.layer is (int)MarrowLayers.ObserverTracker or (int)MarrowLayers.BeingTracker or (int)MarrowLayers.EntityTracker;
                var hasImpactProps = child.TryGetComponent<ImpactProperties>(out _);
                var hasRigidbody = child.TryGetComponent<Rigidbody>(out _);
                if (hasImpactProps || hasRigidbody || isATracker)
                    continue;
                ApplyPhysMatsRecursive(child);
            }
        }
#endif
    }
}
