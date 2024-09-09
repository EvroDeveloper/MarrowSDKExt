using System;
using UnityEngine;

namespace SLZ.Marrow.Circuits
{
	public class SliderController : CircuitSocket
	{
		public AudioClip NotchClick
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public SliderController()
		{
		}

		[SerializeField]
		[Min(0f)]
		[Tooltip("Friction Types:\n0 => Continuous\n1 => Momentary\n2+ => Stepped")]
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
