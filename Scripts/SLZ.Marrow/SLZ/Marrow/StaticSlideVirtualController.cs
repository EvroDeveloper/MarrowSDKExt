using System;
using UnityEngine;

namespace SLZ.Marrow
{
	public class StaticSlideVirtualController : VirtualControllerOverride
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

		public StaticSlideVirtualController()
		{
		}

		public ConfigurableJoint joint;

		private float _lastPosInJoint;
	}
}
