using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace SLZ.Marrow.VoidLogic
{
	[Serializable]
	public sealed class EdgeDetector
	{
		public float HighThreshold
		{
			[CompilerGenerated]
			get
			{
				return 0f;
			}
			[CompilerGenerated]
			internal set
			{
			}
		}

		public float LowThreshold
		{
			[CompilerGenerated]
			get
			{
				return 0f;
			}
			[CompilerGenerated]
			internal set
			{
			}
		}

		public TriggerBehavior TriggerBehavior
		{
			[CompilerGenerated]
			get
			{
				return TriggerBehavior.DontTrigger;
			}
			[CompilerGenerated]
			set
			{
			}
		}

		public float MaximumRepeatRate
		{
			[CompilerGenerated]
			get
			{
				return 0f;
			}
			[CompilerGenerated]
			set
			{
			}
		}

		public EdgeDetector(float lowThreshold = 0.05f, float highThreshold = 0.95f, TriggerBehavior triggerBehavior = TriggerBehavior.OnRisingEdge, float maximumRepeatRate = 60f)
		{
		}

		private EdgeType Tick(float nextInput)
		{
			return EdgeType.NotAnEdge;
		}

		private ValueType TestValue(float value)
		{
			return ValueType.IndeterminateValue;
		}

		[PublicAPI]
		public bool TickAndTest(float value)
		{
			return default(bool);
		}

		[PublicAPI]
		public bool TickAndTest(float value, [Out] EdgeType edgeType)
		{
			return default(bool);
		}

		private bool _inputWasNotLow;

		private bool _inputWasNotHigh;

		private float _nextAvailableTick;
	}
}
