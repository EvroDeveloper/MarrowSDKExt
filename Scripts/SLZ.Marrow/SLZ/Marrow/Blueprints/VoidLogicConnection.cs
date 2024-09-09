using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLZ.Marrow.Pool;

namespace SLZ.Marrow.Blueprints
{
	[Serializable]
	public struct VoidLogicConnection : IConnection
	{
		public bool Restore(BlueprintSpawner spawner, ValueTuple<SpawnData, Poolee>[] spawns, Dictionary<Poolee, IBillOfMaterials> bomCache)
		{
			return default(bool);
		}

		public VoidLogicEdge[] edges;
	}
}
