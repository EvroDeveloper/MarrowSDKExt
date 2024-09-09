using System;
using UnityEngine;

namespace SLZ.Marrow
{
	public class SwingVirtualController : VirtualControllerOverride
	{
		protected void Reset()
		{
		}

		public override void OnVirtualControllerStart(VirtualControlerPayload payload)
		{
		}

		public override void OnVirtualControllerSolve(VirtualControlerPayload payload)
		{
		}

		public SwingVirtualController()
		{
		}

		public Servo servo;

		public bool allowTwist;

		private Vector3 _inputMagCache;

		private Vector3 _lastSwing;
	}
}
