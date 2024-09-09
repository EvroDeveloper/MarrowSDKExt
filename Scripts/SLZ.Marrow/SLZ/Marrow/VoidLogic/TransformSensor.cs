using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[Support(SupportFlags.BetaSupported, null)]
	[AddComponentMenu("VoidLogic/Sources/VoidLogic Transform Sensor")]
	public sealed class TransformSensor : MonoBehaviour, IVoidLogicSource, IVoidLogicNode, IVoidLogicSensor
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

		void IVoidLogicSensor.ReadSensors(NodeState nodeState)
		{
		}

		private float _wrap(float angleDegrees)
		{
			return 0f;
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

		public TransformSensor()
		{
		}

		[HideInInspector]
		[SerializeField]
		private bool _deprecated;

		[SerializeField]
		private Transform _anchor;

		[SerializeField]
		private Transform _connectedTransform;

		[SerializeField]
		private bool _negate;

		private static readonly PortMetadata _portMetadata;
	}
}
