using UnityEngine;
using UltEvents;

namespace SLZ.Marrow.Circuits
{
    [AddComponentMenu("MarrowSDK/Circuits/Actuators/Event Actuator")]
    public class EventActuator : Actuator
    {
        [Tooltip("The circuit supplying the input that will activate events.")]
        [SerializeField]
        private Circuit _input;
        [Tooltip("The input threshold required to trigger Input Fell events.")]
        [SerializeField]
        private float lowThreshold = 0.05f;
        [Tooltip("The input threshold required to trigger Input Rose, Input Held and Input Rose OneShot events.")]
        [SerializeField]
        private float highThreshold = 0.95f;
        [Header("Events")]
        [Tooltip("When the input value changes (EXPENSIVE, runs all callbacks on every value update)")]
        public UltEvent<float> InputUpdated;
        [Tooltip("When the input value rises above the high threshold")]
        public UltEvent<float> InputRose;
        [Tooltip("When the input value holds above the high threshold")]
        public UltEvent<float> InputHeld;
        [Tooltip("When the input value lowers beneath the low threshold")]
        public UltEvent<float> InputFell;
        [Tooltip("When the input value rises above the high threshold (for the first time only)")]
        public UltEvent<float> InputRoseOneShot;
        private float _priorValue;
        private bool _isHigh;
        private bool _hasBeenHigh;
        public Circuit input
        {
            get
            {
                UnityEngine.Debug.Log("Hollowed Property Getter: SLZ.Marrow.Circuits.EventActuator.input");
                throw new System.NotImplementedException();
            }

            set
            {
                UnityEngine.Debug.Log("Hollowed Property Setter: SLZ.Marrow.Circuits.EventActuator.input");
                throw new System.NotImplementedException();
            }
        }

        private void Reset()
        {
        }
    }
}