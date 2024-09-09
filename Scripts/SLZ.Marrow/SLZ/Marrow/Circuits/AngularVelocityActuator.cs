using System;
using UnityEngine;

namespace SLZ.Marrow.Circuits
{
	public class AngularVelocityActuator : Actuator
	{
		public Circuit inputX
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Circuit inputY
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Circuit inputZ
		{
			get
			{
				return null;
			}
			set
			{
			}
		}


		public void SetAllCircuits(Circuit x, Circuit y, Circuit z)
		{
		}

		private void Reset()
		{
		}


		public AngularVelocityActuator()
		{
		}

		[SerializeField]
		private Circuit _inputX;

		[SerializeField]
		private Circuit _inputY;

		[SerializeField]
		private Circuit _inputZ;

		[SerializeField]
		private Servo _servo;
	}
}
