using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/VoidLogic Slider")]
	[Support(SupportFlags.AlphaSupported, "Refer to comment.")]
	public sealed class SliderNode : BaseNode, IVoidLogicSensor, IVoidLogicNode, IVoidLogicActuator
	{
		public ConfigurableJoint SliderConfigurableJoint
		{
			get
			{
				return null;
			}
			private set
			{
			}
		}

		public Rigidbody SliderRigidBody
		{
			[CompilerGenerated]
			get
			{
				return null;
			}
			[CompilerGenerated]
			private set
			{
			}
		}

		public Haptor Haptor
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		protected override void Awake()
		{
		}

		private void Start()
		{
		}

		public override void Initialize(NodeState nodeState)
		{
		}

		void IVoidLogicSensor.ReadSensors(NodeState nodeState)
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

		public SliderNode()
		{
		}

		[SerializeField]
		[Tooltip("Output response curve (multiplied by input)")]
		private AnimationCurve _curve;

		[Min(0f)]
		[Tooltip("Slider Types:\n0 => Free\n1 => Momentary\n2+ => Stepped")]
		[SerializeField]
		private int _steps;

		[SerializeField]
		[Tooltip("Slider joint that drives the output power value")]
		private ConfigurableJoint _sliderConfigurableJoint;

		[SerializeField]
		[Tooltip("Interactable host i.e. for running haptics")]
		private InteractableHost _interactableHost;

		[Header("Force")]
		[SerializeField]
		private float force_Spring;

		[SerializeField]
		private float force_Damper;

		[SerializeField]
		private float force_Max;

		[SerializeField]
		[Tooltip("Only measures value and do not drive joint")]
		private bool justMeasure;

		[Header("Audio")]
		[SerializeField]
		private AudioClip clip_clickOn;

		[SerializeField]
		private AudioClip clip_clickOff;

		private Haptor _haptor;

		private bool _canHaptic;

		private bool _sliderThresholdHit;

		private float _localPowerValue;

		private bool _performedInitialRead;

		private static readonly PortMetadata _portMetadata;
	}
}
