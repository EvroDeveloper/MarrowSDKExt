using System;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using SLZ.Marrow.Data;
using SLZ.Marrow.Interaction;
using SLZ.Marrow.Warehouse;
using UnityEngine;

namespace SLZ.Marrow.VFX
{
	public static class SpawnEffects
	{
		public static void CallSpawnEffect(MarrowEntity ThisEntity)
		{
		}

		public static void CallDespawnEffect(MarrowEntity ThisEntity)
		{
		}

		private static UniTaskVoid FireSFXAsync(MarrowEntity ThisEntity, MarrowAssetT<AudioClip> clipRef, float volume)
		{
			return default(UniTaskVoid);
		}

		private static UniTaskVoid FireEffectAsync(MarrowEntity ThisEntity, Spawnable effect, [Optional] Color? color)
		{
			return default(UniTaskVoid);
		}

		private static float GetVolumeSurface(Vector3 size)
		{
			return 0f;
		}
	}
}
