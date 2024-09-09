using SLZ.Marrow.Warehouse;
using SLZ.Marrow.Zones;
using UnityEngine;

namespace SLZ.Marrow.Circuits
{
    public class ActuatorSocketDecorator : ExternalActuator, ISpawnListenable
    {
        [SerializeField]
        protected CrateSpawner _crateSpawner;
#if UNITY_EDITOR
        protected void Reset()
        {
            _crateSpawner = GetComponent<CrateSpawner>();
        }
#endif
    }
}