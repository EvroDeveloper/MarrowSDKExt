using System;
using SLZ.Marrow.Warehouse;

namespace SLZ.Marrow.Blueprints
{
	public class Blueprint : DataCard
	{
		public override bool IsBundledDataCard()
		{
			return default(bool);
		}

		public Blueprint()
		{
		}

		public SpawnData[] spawnables;

		public VoidLogicConnection voidLogicConnection;
	}
}
