using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SLZ.Marrow.Interaction;
using SLZ.Marrow.Utilities;
using SLZ.Marrow.VoidLogic;
using UltEvents;
using UnityEngine;

namespace SLZ.Marrow.Zones
{
	[Support(SupportFlags.Deprecated, "Replaced by ZoneTriggerSource.")]
	[AddComponentMenu(null)]
	public class ZoneTriggerNode : MonoBehaviour, IVoidLogicSink, IVoidLogicNode, IVoidLogicSource
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

		private void Reset()
		{
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

		private void _OnBodyTriggerEnter(MarrowBody body)
		{
		}

		private void _OnBodyTriggerExit(MarrowBody body)
		{
		}

		private void _OnEntityTriggerEnter(MarrowEntity entity)
		{
		}

		private void _OnEntityTriggerExit(MarrowEntity entity)
		{
		}

		void IVoidLogicNode.Initialize(NodeState nodeState)
		{
		}

		void IVoidLogicSource.Calculate(NodeState nodeState)
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

		public ZoneTriggerNode()
		{
		}

		[SerializeField]
		private Zone _zone;

		[SerializeField]
		[HideInInspector]
		private bool _deprecated;

		[SerializeField]
		[NonReorderable]
		[Tooltip("Dead Field: Please remove")]
		[Obsolete("Dead Field: Please remove")]
		protected internal MonoBehaviour[] _previous;

		[NonReorderable]
		[SerializeField]
		[Tooltip("Previous node(s) in the chain")]
		protected internal OutputPortReference[] _previousConnections;

		public UltEvent<MarrowBody> OnBodyTriggerEnter;

		public UltEvent<MarrowBody> OnBodyTriggerExit;

		public UltEvent<MarrowEntity> OnEntityTriggerEnter;

		public UltEvent<MarrowEntity> OnEntityTriggerExit;

		[SerializeField]
		[ReadOnly(false)]
		private float logicMultiplier;

		private static readonly PortMetadata _portMetadata;
	}
}
