using System;
using UnityEngine;

namespace SLZ.Marrow.Circuits
{
	public class HingeController : CircuitSocket
	{

		public HingeController()
		{
		}

		[Min(0f)]
		[Tooltip("Friction Types:\n0 => Continuous\n1 => Momentary\n2+ => Stepped")]
		[SerializeField]
		protected int _steps;

		[SerializeField]
		[Tooltip("Interactable host i.e. for running haptics")]
		protected InteractableHost _interactableHost;

		[SerializeField]
		protected AudioClip clip_clickOn;

		[Header("Force")]
		[SerializeField]
		private float force_Spring;

		[SerializeField]
		private float force_Damper;

		[SerializeField]
		private float force_Max;
	}
}
