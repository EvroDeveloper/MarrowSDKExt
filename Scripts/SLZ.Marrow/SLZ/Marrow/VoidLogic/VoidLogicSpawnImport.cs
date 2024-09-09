using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SLZ.Marrow.Utilities;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[Support(SupportFlags.BetaSupported, null)]
	[AddComponentMenu("VoidLogic/Spawn/VoidLogic Spawn Import")]
	public sealed class VoidLogicSpawnImport : MonoBehaviour, IVoidLogicSink, IVoidLogicNode, IVoidLogicSource
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

		private void OnEnable()
		{
		}

		private void OnDisable()
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

		public VoidLogicSpawnImport()
		{
		}

		[SerializeField]
		[HideInInspector]
		private bool _deprecated;

		[Tooltip("Previous node in the chain")]
		[ReadOnly(false)]
		[SerializeField]
		private OutputPortReference _previousConnection;

		private static readonly PortMetadata _portMetadata;
	}
}
