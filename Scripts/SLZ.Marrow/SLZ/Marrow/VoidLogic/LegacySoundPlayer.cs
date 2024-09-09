using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/Sinks/VoidLogic Legacy Sound Player")]
	[Support(SupportFlags.Deprecated, "It's unclear how exactly we want to properly support playing sound. This could break at any time.")]
	public sealed class LegacySoundPlayer : MonoBehaviour, IVoidLogicSink, IVoidLogicNode, IVoidLogicActuator
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

		public LegacySoundPlayer()
		{
		}

		[SerializeField]
		[HideInInspector]
		private bool _deprecated;

		[NonReorderable]
		[SerializeField]
		[Obsolete("Dead Field: Please remove")]
		[Tooltip("Dead Field: Please remove")]
		protected internal MonoBehaviour _previousNode;

		[Tooltip("Previous node in the chain")]
		[SerializeField]
		private OutputPortReference _previousConnection;

		[SerializeField]
		private AnimationCurve _curve;

		private float _priorValue;

		protected bool _wasOn;

		[Header("Audio")]
		[SerializeField]
		private AudioClip _onSound;

		[SerializeField]
		private AudioClip _offSound;

		private static readonly PortMetadata _portMetadata;
	}
}
