using System;
using UnityEngine;

namespace SLZ.Marrow.Circuits
{
	public class AngularZSensor : CircuitSocket
	{

		private float InternalRead(float lastSensorValue)
		{
			return 0f;
		}

		public AngularZSensor()
		{
		}

		public AngularZSensor.Output output;

		[Header("Binary01 Settings")]
		[SerializeField]
		[Range(0f, 1f)]
		private float lowThreshold;

		[Range(0f, 1f)]
		[SerializeField]
		private float highThreshold;

		public enum Output
		{
			ZeroToOne,
			NegativeOneToOne,
			ZeroToDegrees,
			NegativeDegreesToDegrees,
			Binary01
		}
	}
}
