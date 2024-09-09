 
using System.Collections.Generic;
using UnityEngine;

namespace SLZ.Marrow.Warehouse
{
    public class Fixture : DataCard
    {
        public override bool IsBundledDataCard()
        {
            return true;
        }

#if UNITY_EDITOR
        public override string GetAssetExtension()
        {
            return "fxt";
        }

#endif
        [SerializeField]
        private SpawnableCrateReference _fixtureSpawnable = new SpawnableCrateReference();
        public SpawnableCrateReference FixtureSpawnable { get => _fixtureSpawnable; set => _fixtureSpawnable = value; }

        [SerializeField]
        private MarrowAssetT<GameObject> _staticFixturePrefab = new MarrowAssetT<GameObject>();
        public MarrowAssetT<GameObject> StaticFixturePrefab { get => _staticFixturePrefab; set => _staticFixturePrefab = value; }

        [SerializeField]
        private List<MarrowMonoScript> _decorators = new List<MarrowMonoScript>();
        public List<MarrowMonoScript> Decorators { get => _decorators; set => _decorators = value; }
    }
}