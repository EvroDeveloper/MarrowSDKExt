using UnityEngine;

namespace SLZ.Marrow.Circuits
{
    public class ExternalCircuit : Circuit
    {
        [SerializeField]
        private Circuit _input;
        public Circuit input
        {
            get
            {
                UnityEngine.Debug.Log("Hollowed Property Getter: SLZ.Marrow.Circuits.ExternalCircuit.input");
                throw new System.NotImplementedException();
            }

            set
            {
                UnityEngine.Debug.Log("Hollowed Property Setter: SLZ.Marrow.Circuits.ExternalCircuit.input");
                throw new System.NotImplementedException();
            }
        }

        public delegate float InitializeDelegate();
        public delegate float ReadSensorDelegate(double fixedTime, float lastSensorValue);
        public delegate float InlineCalculateDelegate(float sensorValue);
    }
}