using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using SLZ.Marrow.Data;
using SLZ.Marrow.Warehouse;
using UnityEngine;

namespace SLZ.Marrow.Pool
{
	public class AssetSpawner : MonoBehaviour
	{
		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private static void Instantiate()
		{
		}

		public static ulong GetNewID()
		{
			return 0UL;
		}

		public static void Register(Spawnable spawnable)
		{
		}

		public static UniTask RegisterAsync(Barcode barcode)
		{
			return default(UniTask);
		}

		public static UniTask InstantiateForWarmupAsync(Spawnable spawnable)
		{
			return default(UniTask);
		}

		public static void Spawn(Spawnable spawnable,  Vector3 position, Quaternion rotation,  Vector3? scale, Transform parent, bool ignorePolicy,  int? groupID, Action<GameObject> spawnCallback,  Action<GameObject> despawnCallback)
		{
		}

		public static UniTask<Poolee> SpawnAsync(Spawnable spawnable, [Optional] Vector3 position, [Optional] Quaternion rotation, [Optional] Vector3? scale, [Optional] Transform parent, bool ignorePolicy, [Optional] int? groupID, [Optional] Action<GameObject> spawnCallback, [Optional] Action<GameObject> despawnCallback, [Optional] Action<GameObject> recycleCallback)
		{
			return default(UniTask<Poolee>);
		}

		public static void Clear(Poolee poolee)
		{
		}

		public static void Despawn(Poolee poolee)
		{
		}

		public AssetSpawner()
		{
		}

		private static bool _hasInstance;

		private static AssetSpawner _instance;

		private Dictionary<Barcode, Pool> _barcodeToPool;

		private Dictionary<Poolee, SpawnPolicy> _pooleeToPolicy;

		private Dictionary<int, SpawnPolicy> _policySpawns;

		private List<Pool> _poolList;

		private static ulong _spawnCount;
	}
}
