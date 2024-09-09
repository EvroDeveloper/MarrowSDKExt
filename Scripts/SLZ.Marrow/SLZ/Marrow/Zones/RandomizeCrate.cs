using System;
using Cysharp.Threading.Tasks;
using SLZ.Marrow.Warehouse;
using UnityEngine;

namespace SLZ.Marrow.Zones
{
	public class RandomizeCrate : SpawnDecorator
	{
		private void Start()
		{
		}

		public SpawnableCrateReference SelectRandomCrate()
		{
			return null;
		}

		public void SelectAndSpawnRandomCrate(bool useSpawnEffect = false)
		{
		}

		private UniTaskVoid SelectAndSpawnRandomCrateAsync(bool useSpawnEffect = false)
		{
			return default(UniTaskVoid);
		}

		public RandomizeCrate()
		{
		}

		public SpawnableCrateReference[] crates;

		[Tooltip("If this is part of a CrateSpawnSequencer set this to false")]
		public bool spawnOnStart;
	}
}
