using UnityEngine;

namespace SLZ.Marrow.Circuits
{
    [AddComponentMenu("MarrowSDK/Circuits/Nodes/FlipFlop Node")]
    public class FlipflopCircuit : Circuit
    {
        [Tooltip("If the Set Input reaches the Set Threshold, the FlipFlop Node will output a value of 1")]
        [SerializeField]
        private Circuit _setInput;
        [Tooltip("If the Reset Input reaches the Reset Threshold, the FlipFlop Node will output a value of 0")]
        [SerializeField]
        private Circuit _resetInput;
        [Tooltip("The required threshold to trigger an output of 1")]
        [SerializeField]
        private float setThreshold = 1f;
        [Tooltip("The required threshold to trigger an output of 0")]
        [SerializeField]
        private float resetThreshold = 1f;
        public Circuit setInput
        {
            get
            {
                UnityEngine.Debug.Log("Hollowed Property Getter: SLZ.Marrow.Circuits.FlipflopCircuit.setInput");
                throw new System.NotImplementedException();
            }

            set
            {
                UnityEngine.Debug.Log("Hollowed Property Setter: SLZ.Marrow.Circuits.FlipflopCircuit.setInput");
                throw new System.NotImplementedException();
            }
        }

        public Circuit resetInput
        {
            get
            {
                UnityEngine.Debug.Log("Hollowed Property Getter: SLZ.Marrow.Circuits.FlipflopCircuit.resetInput");
                throw new System.NotImplementedException();
            }

            set
            {
                UnityEngine.Debug.Log("Hollowed Property Setter: SLZ.Marrow.Circuits.FlipflopCircuit.resetInput");
                throw new System.NotImplementedException();
            }
        }
    }
}