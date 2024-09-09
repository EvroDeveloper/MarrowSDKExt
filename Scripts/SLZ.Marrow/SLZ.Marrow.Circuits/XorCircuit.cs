using UnityEngine;

namespace SLZ.Marrow.Circuits
{
    [AddComponentMenu("MarrowSDK/Circuits/Nodes/Xor Node")]
    public class XorCircuit : Circuit
    {
        [Tooltip("The Xor Node will only activate when an odd number of its inputs are active.")]
        [SerializeField]
        private Circuit[] _input;
        public Circuit[] input
        {
            get
            {
                UnityEngine.Debug.Log("Hollowed Property Getter: SLZ.Marrow.Circuits.XorCircuit.input");
                throw new System.NotImplementedException();
            }

            set
            {
                UnityEngine.Debug.Log("Hollowed Property Setter: SLZ.Marrow.Circuits.XorCircuit.input");
                throw new System.NotImplementedException();
            }
        }
    }
}