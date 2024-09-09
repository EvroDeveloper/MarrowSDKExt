using System;
using System.Runtime.CompilerServices;
using SLZ.Marrow.Interaction;
using SLZ.Marrow.Utilities;
using SLZ.Marrow.VoidLogic;
using UnityEngine;

namespace SLZ.Marrow.Zones
{
	[AddComponentMenu("VoidLogic/Sources/VoidLogic Zone Trigger Source")]
	[Support(SupportFlags.BetaSupported, null)]
	public sealed class ZoneTriggerSource : ZoneItem, IVoidLogicSource, IVoidLogicNode
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

		private void Awake()
		{
		}


		private void OnDestroy()
		{
		}


		private void Increment()
		{
		}

		private void Decrement()
		{
		}

		void IVoidLogicNode.Initialize(NodeState nodeState)
		{
		}

		void IVoidLogicSource.Calculate(NodeState nodeState)
		{
		}

		public PortMetadata PortMetadata
		{
			get
			{
				return default(PortMetadata);
			}
		}

		public ZoneTriggerSource()
		{
		}

		[Tooltip("Trigger Behavior:\nAlways => Enter and Exit\nOnce => Enter and Exit, once\nPinned => Enter (pin to \"on\" state)")]
		[SerializeField]
		private ZoneTriggerSource.TriggerBehavior _triggerBehavior;

		[HideInInspector]
		[SerializeField]
		private bool _deprecated;

		[SerializeField]
		[ReadOnly(false)]
		private float _count;

		[ReadOnly(false)]
		[SerializeField]
		private bool _blockIncrement;

		[ReadOnly(false)]
		[SerializeField]
		private bool _blockDecrement;

		private static readonly PortMetadata _portMetadata;

		public enum TriggerBehavior
		{
			Always,
			Once,
			Pinned
		}
	}
}
