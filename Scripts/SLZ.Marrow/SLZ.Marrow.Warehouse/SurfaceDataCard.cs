 
using System.Collections.Generic;
using SLZ.Data;
 
 
using UnityEngine;

namespace SLZ.Marrow.Warehouse
{
    public class SurfaceDataCard : DataCard
    {
        [Header("Options")]
        [Range(.001f, 1f)]
        public float PenetrationResistance = 0.9f;
        public float megaPascal = 100f;
        [SerializeField]
        private MarrowAssetT<PhysicMaterial> _physicsMaterial;
        public MarrowAssetT<PhysicMaterial> PhysicsMaterial { get => _physicsMaterial; set => _physicsMaterial = value; }

        [SerializeField]
        private MarrowAssetT<PhysicMaterial> _physicsMaterialStatic;
        public MarrowAssetT<PhysicMaterial> PhysicsMaterialStatic { get => _physicsMaterialStatic; set => _physicsMaterialStatic = value; }

        [SerializeField]
        private MarrowAssetT<SurfaceData> _surfaceData;
        public MarrowAssetT<SurfaceData> SurfaceData { get => _surfaceData; set => _surfaceData = value; }

        public override bool IsBundledDataCard()
        {
            return true;
        }

        public override void ImportPackedAssets(Dictionary<string, PackedAsset> packedAssets)
        {
            base.ImportPackedAssets(packedAssets);
            if (packedAssets.TryGetValue("PhysicsMaterial", out var packedAsset))
                PhysicsMaterial = new MarrowAssetT<PhysicMaterial>(packedAsset.marrowAsset.AssetGUID);
            if (packedAssets.TryGetValue("PhysicsMaterialStatic", out packedAsset))
                PhysicsMaterialStatic = new MarrowAssetT<PhysicMaterial>(packedAsset.marrowAsset.AssetGUID);
            if (packedAssets.TryGetValue("SurfaceData", out packedAsset))
                SurfaceData = new MarrowAssetT<SurfaceData>(packedAsset.marrowAsset.AssetGUID);
#if false
#endif
        }

        public override List<PackedAsset> ExportPackedAssets()
        {
            base.ExportPackedAssets();
            PackedAssets.Add(new PackedAsset("PhysicsMaterial", PhysicsMaterial, PhysicsMaterial.AssetType, "_physicsMaterial"));
            PackedAssets.Add(new PackedAsset("PhysicsMaterialStatic", PhysicsMaterialStatic, PhysicsMaterialStatic.AssetType, "_physicsMaterialStatic"));
            PackedAssets.Add(new PackedAsset("SurfaceData", SurfaceData, SurfaceData.AssetType, "_surfaceData"));
#if false
#endif
            return PackedAssets;
        }
    }
}