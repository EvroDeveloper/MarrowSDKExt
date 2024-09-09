using System;
using UnityEngine;

namespace SLZ.Marrow.Circuits
{
	public class AngularYSensor : CircuitSocket
	{

		private float InternalRead(float lastSensorValue)
		{
			return 0f;
		}

		public AngularYSensor()
		{
		}

		public AngularYSensor.Output output;

		[Header("Binary01 Settings")]
		[SerializeField]
		[Range(0f, 1f)]
		private float lowThreshold;

		[SerializeField]
		[Range(0f, 1f)]
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
