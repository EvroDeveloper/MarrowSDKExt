using System;
using SLZ.Marrow.Utilities;
using SLZ.Marrow.Warehouse;

namespace SLZ.Marrow.Blueprints
{
	[Serializable]
	public struct SpawnData
	{
		public SpawnableCrateReference crateRef;

		public SimpleTransform transform;
	}
}
