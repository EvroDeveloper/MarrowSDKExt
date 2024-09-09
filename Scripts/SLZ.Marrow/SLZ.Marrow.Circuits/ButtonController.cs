 
using UnityEngine;

namespace SLZ.Marrow.Circuits
{
    public class ButtonController : CircuitSocket
    {
        [SerializeField]
        private ButtonMode _buttonMode = ButtonMode.ClickUp;
        public enum ButtonMode
        {
            ClickUp,
            ClickDown,
            Toggle,
            ContinuousDown,
            ContinuousFloat
        }
    }
}