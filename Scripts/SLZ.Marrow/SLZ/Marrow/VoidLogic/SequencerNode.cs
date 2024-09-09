using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[Support(SupportFlags.AlphaSupported, "This needs to be updated to use sensors and actuators.")]
	[AddComponentMenu("VoidLogic/VoidLogic Sequencer")]
	public sealed class SequencerNode : BaseNode
	{
		private AnimationCurve Sequence
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

		public bool RealTime
		{
			[CompilerGenerated]
			get
			{
				return default(bool);
			}
			[CompilerGenerated]
			private set
			{
			}
		}

		public float TimeScale
		{
			[CompilerGenerated]
			get
			{
				return 0f;
			}
			[CompilerGenerated]
			private set
			{
			}
		}

		public bool Loop
		{
			[CompilerGenerated]
			get
			{
				return default(bool);
			}
			[CompilerGenerated]
			private set
			{
			}
		}

		public bool ResetTimeOnSequenceCompletion
		{
			[CompilerGenerated]
			get
			{
				return default(bool);
			}
			[CompilerGenerated]
			private set
			{
			}
		}

		private EdgeDetector StartEdgeDetector
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

		private EdgeDetector ResetEdgeDetector
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

		private void Start()
		{
		}

		private void Update()
		{
		}

		public override void Initialize(NodeState nodeState)
		{
		}

		public override void Calculate(NodeState nodeState)
		{
		}

		public override PortMetadata PortMetadata
		{
			get
			{
				return default(PortMetadata);
			}
		}

		public SequencerNode()
		{
		}

		private bool _isRunning;

		private float _time;

		private float _cachedEndKeyTime;

		private float _cachedValue;

		private static readonly PortMetadata _portMetadata;
	}
}
