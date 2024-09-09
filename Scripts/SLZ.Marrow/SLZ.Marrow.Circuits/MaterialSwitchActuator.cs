using UnityEngine;

 

namespace SLZ.Marrow.Circuits
{
    [AddComponentMenu("MarrowSDK/Circuits/Actuators/Material Switch Actuator")]
    public class MaterialSwitchActuator : Actuator
    {
        [Tooltip("The circuit supplying the input that will change the listed Renderer materials when the Low or High threshold is reached.")]
        [SerializeField]
        private Circuit _input;
        [Tooltip("Renderers that will have their materials switched")]
        [SerializeField]
        private Renderer[] _renderers;
        [Tooltip("If the Renderer has multiple materials, the Material Index list allows specific materials to be targeted for switching based on their material index.")]
        [SerializeField]
        private int[] _materialIndex;
        [Tooltip("The material to switch to when the Low threshold is reached.")]
        [SerializeField]
        private Material _offMat;
        [Tooltip("The material to switch to when the High threshold is reached.")]
        [SerializeField]
        private Material _onMat;
        [Tooltip("The input threshold value required to trigger a switch to the Off Material.")]
        [SerializeField]
        private float lowThreshold = 0.05f;
        [Tooltip("The input threshold value required to trigger a switch to the On Material.")]
        [SerializeField]
        private float highThreshold = 0.95f;
        private float _priorValue;
        private bool _isHigh;
        public Circuit input
        {
            get
            {
                UnityEngine.Debug.Log("Hollowed Property Getter: SLZ.Marrow.Circuits.MaterialSwitchActuator.input");
                throw new System.NotImplementedException();
            }

            set
            {
                UnityEngine.Debug.Log("Hollowed Property Setter: SLZ.Marrow.Circuits.MaterialSwitchActuator.input");
                throw new System.NotImplementedException();
            }
        }

        private void Reset()
        {
            var renderer = GetComponent<Renderer>();
            if (renderer)
            {
                _renderers = new[]
                {
                    renderer
                };
                _materialIndex = new[]
                {
                    0
                };
            }
            else
            {
                _materialIndex = new[]
                {
                    0
                };
            }
        }
    }
}