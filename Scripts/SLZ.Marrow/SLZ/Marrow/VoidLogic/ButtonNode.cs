using System;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/VoidLogic Button")]
	[Support(SupportFlags.Supported, null)]
	public class ButtonNode : BaseNode, IVoidLogicSensor, IVoidLogicNode, IVoidLogicActuator
	{
		protected override void Awake()
		{
		}

		protected override void OnEnable()
		{
		}

		void IVoidLogicSensor.ReadSensors(NodeState nodeState)
		{
		}

		public override void Initialize(NodeState nodeState)
		{
		}

		public override void Calculate(NodeState nodeState)
		{
		}

		void IVoidLogicActuator.Actuate(NodeState nodeState)
		{
		}

		public override PortMetadata PortMetadata
		{
			get
			{
				return default(PortMetadata);
			}
		}

		public ButtonNode()
		{
		}

		[SerializeField]
		protected float _lowThreshold;

		[SerializeField]
		protected float _highThreshold;

		[SerializeField]
		protected ConfigurableJoint _joint;

		[SerializeField]
		protected Transform _endTransform;

		[Header("Audio")]
		[SerializeField]
		[Tooltip("Clip(s) to be played on button press")]
		protected AudioClip[] _pressClips;

		[SerializeField]
		[Tooltip("Clip(s) to be played on button unpress")]
		protected AudioClip[] _depressClips;

		[SerializeField]
		[Tooltip("Colliders that the button shaft collider will ignore")]
		protected Collider[] _ignoreColliders;

		[SerializeField]
		protected Collider _buttonShaftCollider;

		protected Rigidbody _rigidBody;

		private Vector3 _initialDisplacement;

		protected bool _isPressed;

		private bool _performedInitialRead;

		private static readonly PortMetadata _portMetadata;
	}
}
