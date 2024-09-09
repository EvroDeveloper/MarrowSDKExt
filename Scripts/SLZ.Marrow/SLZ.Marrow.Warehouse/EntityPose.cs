 
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SLZ.Marrow.Interaction;
using SLZ.Serialize;
using UnityEngine;

 

namespace SLZ.Marrow.Warehouse
{
    public class EntityPose : DataCard
    {
        [SerializeField]
        private MarrowEntityPose _poseData;
        public MarrowEntityPose PoseData { get => _poseData; set => _poseData = value; }

        [SerializeField]
        private SpawnableCrateReference _spawnable = new SpawnableCrateReference();
        public SpawnableCrateReference Spawnable { get => _spawnable; set => _spawnable = value; }

        [SerializeField]
        private MarrowAssetT<Mesh> _posePreviewMesh = new MarrowAssetT<Mesh>();
        public MarrowAssetT<Mesh> PosePreviewMesh { get => _posePreviewMesh; set => _posePreviewMesh = value; }

        [SerializeField]
        public Bounds _colliderBounds;
        public Bounds ColliderBounds { get => _colliderBounds; set => _colliderBounds = value; }

        public override bool IsBundledDataCard()
        {
            return true;
        }

        public override void ImportPackedAssets(Dictionary<string, PackedAsset> packedAssets)
        {
            base.ImportPackedAssets(packedAssets);
            if (packedAssets.TryGetValue("PosePreviewMesh", out var packedAsset))
                PosePreviewMesh = new MarrowAssetT<Mesh>(packedAsset.marrowAsset.AssetGUID);
        }

        public override List<PackedAsset> ExportPackedAssets()
        {
            base.ExportPackedAssets();
            PackedAssets.Add(new PackedAsset("PosePreviewMesh", PosePreviewMesh, PosePreviewMesh.AssetType, "_posePreviewMesh"));
            return PackedAssets;
        }

        public override void Pack(ObjectStore store, JObject json)
        {
            base.Pack(store, json);
            json.Add("spawnable", Spawnable.Barcode.ID);
            json.Add(new JProperty("colliderBounds", new JObject { { "center", new JObject { { "x", _colliderBounds.center.x }, { "y", _colliderBounds.center.y }, { "z", _colliderBounds.center.z } } }, { "extents", new JObject { { "x", _colliderBounds.extents.x }, { "y", _colliderBounds.extents.y }, { "z", _colliderBounds.extents.z } } } }));
        }

        public override void Unpack(ObjectStore store, string objectId)
        {
            base.Unpack(store, objectId);
            if (store.TryGetJSON("spawnable", objectId, out JToken spawnableValue))
            {
                Spawnable = new SpawnableCrateReference(spawnableValue.ToString());
            }

            if (store.TryGetJSON("colliderBounds", objectId, out JToken colliderBoundsToken))
            {
                _colliderBounds = colliderBoundsToken.ToObject<Bounds>();
            }
        }

#if UNITY_EDITOR
        public string EntityPoseSpawnableFilter(Scannable scannable)
        {
            if (scannable != null && scannable is EntityPose entityPose && ScannableReference.IsValid(entityPose.Spawnable))
            {
                return entityPose.Spawnable.Barcode.ToString();
            }

            return string.Empty;
        }
#endif
    }
}