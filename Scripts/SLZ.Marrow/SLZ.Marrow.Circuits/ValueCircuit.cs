using UnityEngine;

namespace SLZ.Marrow.Circuits
{
    [AddComponentMenu("MarrowSDK/Circuits/Nodes/Value Node")]
    public class ValueCircuit : Circuit
    {
        [Tooltip("The Value Node outputs the specified constant value")]
        [SerializeField]
        private float _value = 1f;
        public float Value
        {
            get => _value;
            set
            {
                UnityEngine.Debug.Log("Hollowed Property Setter: SLZ.Marrow.Circuits.ValueCircuit.Value");
                throw new System.NotImplementedException();
            }
        }
#if UNITY_EDITOR
#endif
    }
}