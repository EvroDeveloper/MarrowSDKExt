using System;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/VoidLogic Ratchet")]
	[Support(SupportFlags.AlphaSupported, "Included with reservations, because this is a convenient component.")]
	public sealed class RatchetNode : BaseNode
	{
		public override void Initialize(NodeState nodeState)
		{
		}

		public override void Calculate(NodeState nodeState)
		{
		}

		public void ResetRatchet()
		{
		}

		public override PortMetadata PortMetadata
		{
			get
			{
				return default(PortMetadata);
			}
		}

		public RatchetNode()
		{
		}

		[SerializeField]
		private float lowThreshold;

		private float _maximumValueEverReached;

		private static readonly PortMetadata _portMetadata;
	}
}
