using System;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/VoidLogic Multiply")]
	[Support(SupportFlags.Supported, null)]
	public sealed class MultiplyNode : BaseNode
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

		public MultiplyNode()
		{
		}

		private static readonly PortMetadata _portMetadata;
	}
}
