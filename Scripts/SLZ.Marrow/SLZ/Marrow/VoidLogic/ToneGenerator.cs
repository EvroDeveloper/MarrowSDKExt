using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[Support(SupportFlags.AlphaSupported, "This is intended mainly for debugging, and as such its serialization compatibility is not guaranteed.")]
	[AddComponentMenu("VoidLogic/Sinks/VoidLogic Tone Generator")]
	public sealed class ToneGenerator : MonoBehaviour, IVoidLogicSink, IVoidLogicNode, IVoidLogicActuator
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

		public float Volume
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

		private void Start()
		{
		}

		void IVoidLogicNode.Initialize(NodeState nodeState)
		{
		}

		void IVoidLogicActuator.Actuate(NodeState nodeState)
		{
		}

		private void OnAudioFilterRead(float[] data, int channels)
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

		public ToneGenerator()
		{
		}

		[SerializeField]
		[HideInInspector]
		private bool _deprecated;

		[Obsolete("Dead Field: Please remove")]
		[Tooltip("Dead Field: Please remove")]
		[SerializeField]
		[NonReorderable]
		protected internal MonoBehaviour[] _previous;

		[Tooltip("Previous node(s) in the chain")]
		[NonReorderable]
		[SerializeField]
		protected internal OutputPortReference[] _previousConnections;

		[Range(0f, 1f)]
		[SerializeField]
		private float _volume;

		[SerializeField]
		private AnimationCurve _curve;

		private float _frequency;

		private int _sampleRate;

		private float _phase;

		private bool _running;

		private static readonly PortMetadata _portMetadata;
	}
}
