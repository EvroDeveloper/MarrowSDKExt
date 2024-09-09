using SLZ.Marrow.Warehouse;
using SLZ.Marrow.Zones;
using UnityEngine;

namespace SLZ.Marrow.Circuits
{
    [AddComponentMenu("MarrowSDK/Circuits/Decorators/Button Decorator")]
    public class ButtonDecorator : ExternalCircuit, ISpawnListenable
    {
        [Min(0)]
        public ButtonController.ButtonMode buttonMode = ButtonController.ButtonMode.ClickUp;
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