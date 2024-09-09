using UnityEngine;

namespace SLZ.Marrow.Circuits
{
    [AddComponentMenu("MarrowSDK/Circuits/Nodes/Multiply Node")]
    public class MultiplyCircuit : Circuit
    {
        [Tooltip("The values of the listed Input circuits will be multiplied by this node.  The Multiply Node can be used as the Input circuit for a target circuit or actuator and the multipled total input will be supplied to the target.  The target's threshold values should be adjsuted for the appropriate total input.")]
        [SerializeField]
        private Circuit[] _input;
        public Circuit[] input
        {
            get
            {
                UnityEngine.Debug.Log("Hollowed Property Getter: SLZ.Marrow.Circuits.MultiplyCircuit.input");
                throw new System.NotImplementedException();
            }

            set
            {
                UnityEngine.Debug.Log("Hollowed Property Setter: SLZ.Marrow.Circuits.MultiplyCircuit.input");
                throw new System.NotImplementedException();
            }
        }
    }
}