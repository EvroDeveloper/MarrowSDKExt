 
 
using SLZ.Marrow.Warehouse;
using SLZ.Marrow.Zones;
using UnityEngine;

namespace SLZ.Marrow
{
    public class MarrowEntityPoseDecorator : SpawnDecorator
    {
        [SerializeField]
        private DataCardReference<EntityPose> _marrowEntityPose;
        public DataCardReference<EntityPose> MarrowEntityPose { get => _marrowEntityPose; set => _marrowEntityPose = value; }
        public CrateSpawner CrateSpawner => _crateSpawner;
    }
}