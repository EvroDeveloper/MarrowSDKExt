using UnityEngine;

namespace SLZ.Marrow.Circuits
{
    [AddComponentMenu("MarrowSDK/Circuits/Nodes/Add Node")]
    public class AddCircuit : Circuit
    {
        [Tooltip("The listed Input circuits that will be added together by this node")]
        [SerializeField]
        private Circuit[] _input;
        public Circuit[] input
        {
            get
            {
                UnityEngine.Debug.Log("Hollowed Property Getter: SLZ.Marrow.Circuits.AddCircuit.input");
                throw new System.NotImplementedException();
            }

            set
            {
                UnityEngine.Debug.Log("Hollowed Property Setter: SLZ.Marrow.Circuits.AddCircuit.input");
                throw new System.NotImplementedException();
            }
        }
    }
}