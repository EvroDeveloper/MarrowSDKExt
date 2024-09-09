using System;
using SLZ.Marrow.Interaction;
using SLZ.Marrow.Zones;
using UnityEngine;

namespace SLZ.Marrow.Circuits
{
	public class ZoneCircuit : ZoneLinkItem
	{
		private void Awake()
		{
		}

		private float ReadSensor(double fixedTime, float lastSensorValue)
		{
			return 0f;
		}

		public ZoneCircuit()
		{
		}

		[SerializeField]
		private ExternalCircuit _externalCircuit;

		private float _sensorValue;
	}
}
