using UnityEngine;

namespace SLZ.Marrow.Circuits
{
    [AddComponentMenu("MarrowSDK/Circuits/Nodes/Remap Node")]
    public class RemapCircuit : Circuit
    {
        [Tooltip("The Input circuit's values will be remapped to the curve")]
        [SerializeField]
        private Circuit _input;
        [Tooltip("Keys can be added to the curve to sculpt complex shapes.")]
        [SerializeField]
        private AnimationCurve _remapCurve = new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f));
        public Circuit input
        {
            get
            {
                UnityEngine.Debug.Log("Hollowed Property Getter: SLZ.Marrow.Circuits.RemapCircuit.input");
                throw new System.NotImplementedException();
            }

            set
            {
                UnityEngine.Debug.Log("Hollowed Property Setter: SLZ.Marrow.Circuits.RemapCircuit.input");
                throw new System.NotImplementedException();
            }
        }
    }
}