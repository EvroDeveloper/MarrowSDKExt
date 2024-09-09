using System;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/VoidLogic Passthrough")]
	[Support(SupportFlags.Supported, null)]
	public sealed class PassthroughNode : BaseNode
	{
		public override void Initialize(NodeState nodeState)
		{
		}

		public override void Calculate(NodeState nodeState)
		{
		}

		public void Toggle()
		{
		}

		public void TurnOn()
		{
		}

		public void TurnOff()
		{
		}

		public override PortMetadata PortMetadata
		{
			get
			{
				return default(PortMetadata);
			}
		}

		public PassthroughNode()
		{
		}

		[SerializeField]
		private bool _cutoff;

		private static readonly PortMetadata _portMetadata;
	}
}
