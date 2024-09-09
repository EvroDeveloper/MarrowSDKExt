using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UltEvents;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[Support(SupportFlags.Supported, "<b>Reminder</b>: Is there a better way to do this?")]
	[AddComponentMenu("VoidLogic/Sinks/VoidLogic Event Adapter")]
	public sealed class EventAdapter : MonoBehaviour, IVoidLogicSink, IVoidLogicNode, IVoidLogicActuator
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

		private void _invokeInputUpdated(IVoidLogicSource source, float f)
		{
		}

		private void _invokeInputRose(IVoidLogicSource source, float f)
		{
		}

		private void _invokeInputRoseOneShot(IVoidLogicSource source, float f)
		{
		}

		private void _invokeInputFell(IVoidLogicSource source, float f)
		{
		}

		private void _invokeInputHeld(IVoidLogicSource source, float f)
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

		public EventAdapter()
		{
		}

		[SerializeField]
		[HideInInspector]
		private bool _deprecated;

		[Obsolete("Dead Field: Please remove")]
		[Tooltip("Dead Field: Please remove")]
		[NonReorderable]
		[SerializeField]
		protected internal MonoBehaviour _previousNode;

		[SerializeField]
		[Tooltip("Previous node in the chain")]
		private OutputPortReference _previousConnection;

		[SerializeField]
		private float lowThreshold;

		[SerializeField]
		private float highThreshold;

		[Header("Events")]
		[Tooltip("When the input value changes (EXPENSIVE, runs all callbacks on every value update)")]
		public UltEvent<EventAdapter, IVoidLogicSource, float> InputUpdated;

		[Tooltip("When the input value rises above the high threshold")]
		public UltEvent<EventAdapter, IVoidLogicSource, float> InputRose;

		[Tooltip("When the input value holds above the high threshold")]
		public UltEvent<EventAdapter, IVoidLogicSource, float> InputHeld;

		[Tooltip("When the input value lowers beneath the low threshold")]
		public UltEvent<EventAdapter, IVoidLogicSource, float> InputFell;

		[Tooltip("When the input value rises above the high threshold (for the first time only)")]
		public UltEvent<EventAdapter, IVoidLogicSource, float> InputRoseOneShot;

		private float _priorValue;

		private bool _isHigh;

		private bool _hasBeenHigh;

		private static readonly PortMetadata _portMetadata;
	}
}
