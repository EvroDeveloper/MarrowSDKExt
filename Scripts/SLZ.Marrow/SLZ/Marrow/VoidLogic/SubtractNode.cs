using System;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/VoidLogic Subtract")]
	[Support(SupportFlags.Supported, null)]
	public sealed class SubtractNode : BaseNode
	{
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

		public SubtractNode()
		{
		}

		private static readonly PortMetadata _portMetadata;
	}
}
