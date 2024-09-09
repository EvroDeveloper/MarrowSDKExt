using System;
using UnityEngine;

namespace SLZ.Marrow.Circuits
{
	public class LinearXSensor : CircuitSocket
	{

		private float InternalRead(float lastSensorValue)
		{
			return 0f;
		}

		public LinearXSensor()
		{
		}

		public LinearXSensor.Output output;

		[SerializeField]
		[Range(0f, 1f)]
		[Header("Binary01 Settings")]
		private float lowThreshold;

		[Range(0f, 1f)]
		[SerializeField]
		private float highThreshold;

		public enum Output
		{
			ZeroToOne,
			NegativeOneToOne,
			ZeroToMeters,
			NegativeMetersToMeters,
			Binary01
		}
	}
}
