using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SLZ.Marrow.Zones;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/Spawn/VoidLogic Spawn Input")]
	[Support(SupportFlags.BetaSupported, null)]
	public sealed class VoidLogicSpawnInput : SpawnDecorator, IVoidLogicSink, IVoidLogicNode, IVoidLogicSource
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

		public VoidLogicSpawnInput()
		{
		}

		[SerializeField]
		[HideInInspector]
		private bool _deprecated;

		[Tooltip("Previous node in the chain")]
		[SerializeField]
		private OutputPortReference _previousConnection;

		private static readonly PortMetadata _portMetadata;
	}
}
