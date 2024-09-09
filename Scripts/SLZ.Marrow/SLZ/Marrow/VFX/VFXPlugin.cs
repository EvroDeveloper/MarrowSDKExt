using System;
using Cysharp.Threading.Tasks;
using SLZ.Marrow.Plugins;
using SLZ.Marrow.Warehouse;

namespace SLZ.Marrow.VFX
{
	[MarrowPlugin("SLZ.Marrow.VFX", "VFX", null)]
	public class VFXPlugin : IMarrowPluginLevelCallbacks, IMarrowPlugin
	{
		UniTask IMarrowPluginLevelCallbacks.OnBeforeLevelLoad(LevelCrateReference level)
		{
			return default(UniTask);
		}

		public VFXPlugin()
		{
		}
	}
}
