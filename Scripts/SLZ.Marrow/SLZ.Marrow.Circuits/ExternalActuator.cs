using UnityEngine;

namespace SLZ.Marrow.Circuits
{
    public class ExternalActuator : Actuator
    {
        [SerializeField]
        private Circuit _input;
        public Circuit input
        {
            get
            {
                UnityEngine.Debug.Log("Hollowed Property Getter: SLZ.Marrow.Circuits.ExternalActuator.input");
                throw new System.NotImplementedException();
            }

            set
            {
                UnityEngine.Debug.Log("Hollowed Property Setter: SLZ.Marrow.Circuits.ExternalActuator.input");
                throw new System.NotImplementedException();
            }
        }

        public delegate void ActuateDelegate(double fixedTime, bool isInitializing);
    }
}