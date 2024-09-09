using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using SLZ.Marrow.Pool;
using UnityEngine;

namespace SLZ.Marrow.Blueprints
{
	public class BlueprintSpawner : MonoBehaviour
	{
		public Blueprint Blueprint
		{
			[CompilerGenerated]
			get
			{
				return null;
			}
			[CompilerGenerated]
			set
			{
			}
		}

		private void Awake()
		{
		}

		private void OnValidate()
		{
		}

		private void Reset()
		{
		}

		[ContextMenu("Reset Name")]
		public void ResetName()
		{
		}

		[Conditional("UNITY_EDITOR")]
		public void EditorUpdateName()
		{
		}

		[ContextMenu("Spawn Blueprint")]
		private void DebugSpawnBlueprint()
		{
		}

		public void SpawnBlueprint(bool ignoreCondition = false)
		{
		}

		public UniTask SpawnBlueprintAsync(bool ignoreCondition = false)
		{
			return default(UniTask);
		}

		private static UniTask<ValueTuple<SpawnData, Poolee>> DoSpawn(SpawnData spawnData, GameObject parentGO)
		{
			return default(UniTask<ValueTuple<SpawnData, Poolee>>);
		}

		private UniTask<bool> DoRestore(ValueTuple<SpawnData, Poolee>[] spawns)
		{
			return default(UniTask<bool>);
		}

		public BlueprintSpawner()
		{
		}

		public Func<bool> ShouldSpawn;
	}
}
