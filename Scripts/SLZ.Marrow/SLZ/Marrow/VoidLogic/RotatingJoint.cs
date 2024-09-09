using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/Sinks/VoidLogic Rotating Joint")]
	[Support(SupportFlags.BetaSupported, "This works, but uses ConfigurableJoint instead of Marrow primitives.")]
	public sealed class RotatingJoint : MonoBehaviour, IVoidLogicSink, IVoidLogicNode, IVoidLogicActuator
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

		private void SETJOINT(float voltage = 1f)
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

		public RotatingJoint()
		{
		}

		[SerializeField]
		[HideInInspector]
		private bool _deprecated;

		[SerializeField]
		[NonReorderable]
		[Obsolete("Dead Field: Please remove")]
		[Tooltip("Dead Field: Please remove")]
		protected internal MonoBehaviour _previousNode;

		[Tooltip("Previous node in the chain")]
		[SerializeField]
		private OutputPortReference _previousConnection;

		private float? _priorValue;

		[SerializeField]
		private ConfigurableJoint _configurableJoint;

		private Rigidbody _rigidBody;

		[Header("Joint Control")]
		[SerializeField]
		private bool _varyTargetRotation;

		[SerializeField]
		private float _minDegreesX;

		[SerializeField]
		private float _maxDegreesX;

		[SerializeField]
		private bool _varyTargetAngVelocity;

		[SerializeField]
		private Vector3 _minAngVelocity;

		[SerializeField]
		private Vector3 _maxAngVelocity;

		[SerializeField]
		private bool _varyAngularDrive;

		[SerializeField]
		private Vector3 _xAngMinSpringDamperForce;

		[SerializeField]
		private Vector3 _xAngMaxSpringDamperForce;

		private static readonly PortMetadata _portMetadata;
	}
}
