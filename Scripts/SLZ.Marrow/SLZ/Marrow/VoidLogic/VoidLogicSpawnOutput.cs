using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SLZ.Marrow.Utilities;
using SLZ.Marrow.Zones;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[Support(SupportFlags.BetaSupported, null)]
	[AddComponentMenu("VoidLogic/Spawn/VoidLogic Spawn Output")]
	public sealed class VoidLogicSpawnOutput : SpawnDecorator, IVoidLogicSource, IVoidLogicNode, IVoidLogicSink
	{
		public VoidLogicSubgraph Subgraph
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

		public bool Deprecated
		{
			get
			{
				return default(bool);
			}
		}

		public int OutputCount
		{
			get
			{
				return 0;
			}
		}

		public int InputCount
		{
			get
			{
				return 0;
			}
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		void IVoidLogicNode.Initialize(NodeState nodeState)
		{
		}

		void IVoidLogicSource.Calculate(NodeState nodeState)
		{
		}

		public bool TryGetInputConnection(uint inputIndex, [Out] OutputPortReference connectedPort)
		{
			return default(bool);
		}

		public bool TryConnectPortToInput(OutputPortReference output, uint inputIndex)
		{
			return default(bool);
		}

		public PortMetadata PortMetadata
		{
			get
			{
				return default(PortMetadata);
			}
		}

		public VoidLogicSpawnOutput()
		{
		}

		[HideInInspector]
		[SerializeField]
		private bool _deprecated;

		[ReadOnly(false)]
		[SerializeField]
		[Tooltip("Previous node in the chain")]
		private OutputPortReference _previousConnection;

		private static readonly PortMetadata _portMetadata;
	}
}
