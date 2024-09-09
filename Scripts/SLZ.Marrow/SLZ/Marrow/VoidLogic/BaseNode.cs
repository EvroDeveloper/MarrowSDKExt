using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	public abstract class BaseNode : MonoBehaviour, IVoidLogicSink, IVoidLogicNode, IVoidLogicSource
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

		protected virtual void Awake()
		{
		}

		protected virtual void OnEnable()
		{
		}

		protected virtual void OnDisable()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		public abstract PortMetadata PortMetadata
		{
			get;
		}

		protected static bool EqualWithTolerance(float term1, float term2, float tolerance)
		{
			return default(bool);
		}

		public int InputCount
		{
			get
			{
				return 0;
			}
		}

		public virtual int OutputCount
		{
			get
			{
				return 0;
			}
		}


		public abstract void Initialize(NodeState nodeState);

		public abstract void Calculate(NodeState nodeState);

        public bool TryGetInputAtIndex(uint idx, out IVoidLogicSource input)
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

        protected BaseNode()
		{
		}

		[SerializeField]
		[HideInInspector]
		private bool _deprecated;

		[Tooltip("Dead Field: Please remove")]
		[SerializeField]
		[Obsolete("Dead Field: Please remove")]
		[NonReorderable]
		protected internal MonoBehaviour[] _previous;

		[SerializeField]
		[NonReorderable]
		[Tooltip("Previous node(s) in the chain")]
		protected internal OutputPortReference[] _previousConnections;
	}
}
