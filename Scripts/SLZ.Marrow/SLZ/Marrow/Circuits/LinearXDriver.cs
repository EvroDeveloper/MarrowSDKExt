using System;
using UnityEngine;

namespace SLZ.Marrow.Circuits
{
	public class LinearXDriver : ActuatorSocket
	{
		private void FixedUpdate()
		{
		}

		public LinearXDriver()
		{
		}

		[SerializeField]
		private float _minSpeed;

		[SerializeField]
		private float _maxSpeed;

		[SerializeField]
		private float _smoothTime;

		[Header("Force")]
		[SerializeField]
		private float force_Spring;

		[SerializeField]
		private float force_Damper;

		[SerializeField]
		private float force_Max;

		private float _target;

		private float _vel;
	}
}
