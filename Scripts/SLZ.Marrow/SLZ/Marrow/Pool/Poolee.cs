using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLZ.Marrow.Utilities;
using SLZ.Marrow.Warehouse;
using UnityEngine;

namespace SLZ.Marrow.Pool
{
	[DefaultExecutionOrder(1000)]
	public class Poolee : MonoBehaviour
	{
		public static ComponentCache<Poolee> Cache
		{
			get
			{
				return null;
			}
		}

		public SpawnableCrate SpawnableCrate
		{
			[CompilerGenerated]
			get
			{
				return null;
			}
			[CompilerGenerated]
			internal set
			{
			}
		}

		public ulong ID
		{
			[CompilerGenerated]
			get
			{
				return 0UL;
			}
			[CompilerGenerated]
			internal set
			{
			}
		}

		public bool IsInPool
		{
			get
			{
				return default(bool);
			}
		}

		private void Awake()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDestroy()
		{
		}

		internal void OnInitialize()
		{
		}

		internal void OnSpawnEvent()
		{
		}

		internal void OnSpawn()
		{
		}

		internal void OnDespawnEvent()
		{
		}

		internal void OnRecycleEvent()
		{
		}

		internal void OnDeinitialize()
		{
		}

		public void Despawn()
		{
		}

		public void RegisterPoolable(IPoolable poolable)
		{
		}

		public void DeregisterPoolable(IPoolable poolable)
		{
		}

		public Poolee()
		{
		}

		private static ComponentCache<Poolee> _cache;

		private bool _isInPool;

		private readonly List<IPoolable> _poolables;

		public Action<GameObject> OnSpawnDelegate;

		public Action<GameObject> OnDespawnDelegate;

		public Action<GameObject> OnRecycleDelegate;

		private bool _hasBeenInitialized;
	}
}
