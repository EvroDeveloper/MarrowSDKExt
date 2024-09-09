using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/Sources/VoidLogic Power")]
	[Support(SupportFlags.Supported, null)]
	public sealed class PowerSource : MonoBehaviour, IVoidLogicSource, IVoidLogicNode
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

		public float OutputValue
		{
			get
			{
				return 0f;
			}
			set
			{
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

		public PortMetadata PortMetadata
		{
			get
			{
				return default(PortMetadata);
			}
		}

		public PowerSource()
		{
		}

		[SerializeField]
		[HideInInspector]
		private bool _deprecated;

		[Tooltip("Amount of power supplied by this source")]
		[SerializeField]
		private float _value;

		private static readonly PortMetadata _portMetadata;
	}
}
