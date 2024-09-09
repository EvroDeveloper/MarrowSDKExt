using System;
using UnityEngine;

namespace SLZ.Marrow.Circuits
{
	public class ActuatorSocket : Actuator
	{
		public Circuit input
		{
			get
			{
				return null;
			}
			set
			{
			}
		}


		protected virtual void Reset()
		{
		}

		public ActuatorSocket()
		{
		}

		[SerializeField]
		private Circuit _input;

		[SerializeField]
		protected Servo _servo;

		[SerializeField]
		protected Rigidbody _rb;
	}
}
