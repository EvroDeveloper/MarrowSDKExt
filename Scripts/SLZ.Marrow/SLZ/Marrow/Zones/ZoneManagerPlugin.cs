using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using SLZ.Marrow.Interaction;
using SLZ.Marrow.Plugins;
using SLZ.Marrow.Warehouse;

namespace SLZ.Marrow.Zones
{
	[MarrowPlugin("SLZ.Marrow.NewZones", "Zone Manager Plugin", null)]
	internal class ZoneManagerPlugin : IMarrowPluginLevelCallbacks, IMarrowPlugin
	{
		public static ZoneLinkManager<MarrowEntity, ZoneLink> ZoneLinkManager
		{
			[CompilerGenerated]
			get
			{
				return null;
			}
			[CompilerGenerated]
			private set
			{
			}
		}

		public static ZoneCullManager ZoneCullManager
		{
			[CompilerGenerated]
			get
			{
				return null;
			}
			[CompilerGenerated]
			private set
			{
			}
		}

		public static InactiveObjectManager InactiveManager
		{
			[CompilerGenerated]
			get
			{
				return null;
			}
			[CompilerGenerated]
			private set
			{
			}
		}

		UniTask IMarrowPluginLevelCallbacks.OnBeforeLevelLoad(LevelCrateReference level)
		{
			return default(UniTask);
		}

		public ZoneManagerPlugin()
		{
		}
	}
}
