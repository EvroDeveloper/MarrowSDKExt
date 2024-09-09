using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/Sinks/VoidLogic Material Switcher")]
	[Support(SupportFlags.Supported, null)]
	public sealed class MaterialSwitcher : MonoBehaviour, IVoidLogicSink, IVoidLogicNode, IVoidLogicActuator
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

		void IVoidLogicActuator.Actuate(NodeState nodeState)
		{
		}

		private void UpdateMats(Material mat)
		{
		}

		public int InputCount
		{
			get
			{
				return 0;
			}
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

		public MaterialSwitcher()
		{
		}

		[SerializeField]
		[HideInInspector]
		private bool _deprecated;

		[Obsolete("Dead Field: Please remove")]
		[Tooltip("Dead Field: Please remove")]
		[NonReorderable]
		[SerializeField]
		protected internal MonoBehaviour _previousNode;

		[SerializeField]
		[Tooltip("Previous node in the chain")]
		private OutputPortReference _previousConnection;

		private float _priorValue;

		[SerializeField]
		private Renderer[] _renderers;

		[SerializeField]
		private int[] _materialIndex;

		[SerializeField]
		private Material _offMat;

		[SerializeField]
		private Material _onMat;

		[SerializeField]
		private float lowThreshold;

		[SerializeField]
		private float highThreshold;

		private bool _isHigh;

		private static readonly PortMetadata _portMetadata;
	}
}
