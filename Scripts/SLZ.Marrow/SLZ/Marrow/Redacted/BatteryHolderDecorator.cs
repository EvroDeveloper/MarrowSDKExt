using System;
using SLZ.Marrow.Circuits;
using SLZ.Marrow.Warehouse;
using SLZ.Marrow.Zones;
using UnityEngine;

namespace SLZ.Marrow.Redacted
{
	public class BatteryHolderDecorator : ExternalCircuit, ISpawnListenable
	{
		protected void OnEnable()
		{
		}

		protected void OnDisable()
		{
		}

		public void OnSpawn(GameObject go)
		{
		}

		public void OnDespawn(GameObject go)
		{
		}

		public BatteryHolderDecorator()
		{
		}

		[SerializeField]
		protected CrateSpawner _crateSpawner;

		[SerializeField]
		private bool _isAutobindOnAttached;
	}
}
