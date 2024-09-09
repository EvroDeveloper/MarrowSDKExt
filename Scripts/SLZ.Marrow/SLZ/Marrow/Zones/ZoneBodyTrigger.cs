using System;
using System.Runtime.CompilerServices;
using SLZ.Algorithms.Unity;
using SLZ.Marrow.Interaction;
using SLZ.Marrow.Utilities;
using SLZ.Marrow.VoidLogic;
using UltEvents;
using UnityEngine;

namespace SLZ.Marrow.Zones
{
	[Support(SupportFlags.Unsupported, "Not (yet?) supported.")]
	[AddComponentMenu("VoidLogic/Sinks/VoidLogic Zone Body Trigger")]
	public class ZoneBodyTrigger : MonoBehaviour, IVoidLogicSink, IVoidLogicNode, ISerializationCallbackReceiver, IVoidLogicSource
	{
		[SerializeField]
		private Zone _zone;

		public EntityAggregator aggregator;

		[Obsolete("Replace with `_previousConnections`")]
		[Tooltip("Previous node(s) in the chain")]
		[SerializeField]
		[Interface(typeof(IVoidLogicSource), false)]
		private MonoBehaviour[] _previous;

		[SerializeField]
		[NonReorderable]
		[Tooltip("Previous node(s) in the chain")]

		public UltEvent<MarrowBody> OnBodyTriggerEnter;

		public UltEvent<MarrowBody> OnBodyTriggerExit;

		[SerializeField]
		[ReadOnly(false)]
		private float logicMultiplier;

		protected internal float _cachedValue;

		private static readonly PortMetadata _portMetadata;

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

		protected bool IsCachedInternal
		{
			[CompilerGenerated]
			get
			{
				return false;
			}
			[CompilerGenerated]
			private set
			{
			}
		}

		public int OutputCount => 0;

		public int InputCount => 0;

		public PortMetadata PortMetadata => default(PortMetadata);

		private void UnityEngine_002EISerializationCallbackReceiver_002EOnBeforeSerialize()
		{
		}

		private void UnityEngine_002EISerializationCallbackReceiver_002EOnAfterDeserialize()
		{
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

		private void SLZ_002EMarrow_002EVoidLogic_002EIVoidLogicSource_002ECalculate(ref NodeState nodeState)
		{
		}

        public bool TryGetInputAtIndex(uint idx, out IVoidLogicSource input)
        {
            throw new NotImplementedException();
        }

        public void OnBeforeSerialize()
        {
            throw new NotImplementedException();
        }

        public void OnAfterDeserialize()
        {
            throw new NotImplementedException();
        }

        public void Calculate(ref NodeState nodeState)
        {
            throw new NotImplementedException();
        }

        public bool TryGetInputConnection(uint inputIndex, OutputPortReference connectedPort)
        {
            throw new NotImplementedException();
        }

        public bool TryConnectPortToInput(OutputPortReference output, uint inputIndex)
        {
            throw new NotImplementedException();
        }

        public void Initialize(NodeState nodeState)
        {
            throw new NotImplementedException();
        }

        public void Calculate(NodeState nodeState)
        {
            throw new NotImplementedException();
        }
    }
}
