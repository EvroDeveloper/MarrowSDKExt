using System;
using UnityEngine;

namespace SLZ.Marrow.Circuits
{
	public class AngularXSensor : CircuitSocket
	{

		private float InternalRead(float lastSensorValue)
		{
			return 0f;
		}

		public AngularXSensor()
		{
		}

		public AngularXSensor.Output output;

		[Range(0f, 1f)]
		[Header("Binary01 Settings")]
		[SerializeField]
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
