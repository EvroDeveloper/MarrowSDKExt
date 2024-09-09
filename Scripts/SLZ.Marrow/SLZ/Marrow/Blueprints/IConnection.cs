using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLZ.Marrow.Pool;

namespace SLZ.Marrow.Blueprints
{
	public interface IConnection
	{
		bool Restore(BlueprintSpawner spawner, ValueTuple<SpawnData, Poolee>[] spawns, Dictionary<Poolee, IBillOfMaterials> bomCache);
	}
}
