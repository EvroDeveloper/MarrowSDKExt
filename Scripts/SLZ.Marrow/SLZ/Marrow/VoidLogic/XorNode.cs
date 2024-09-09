using System;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/VoidLogic Xor")]
	[Support(SupportFlags.Supported, null)]
	public sealed class XorNode : BaseNode
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

		public XorNode()
		{
		}

		private static readonly PortMetadata _portMetadata;
	}
}
