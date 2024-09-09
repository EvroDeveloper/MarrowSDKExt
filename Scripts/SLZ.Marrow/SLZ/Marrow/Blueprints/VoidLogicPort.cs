using System;

namespace SLZ.Marrow.Blueprints
{
	[Serializable]
	public struct VoidLogicPort
	{
		public uint spawnableIndex;

		public string spawnableKey;

		public uint portIndex;

		public bool useInlinePower;

		public float inlinePowerValue;
	}
}
