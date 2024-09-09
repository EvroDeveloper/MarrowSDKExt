using System;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[Support(SupportFlags.Supported, null)]
	[AddComponentMenu("VoidLogic/VoidLogic Add")]
	public sealed class AddNode : BaseNode
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

		public AddNode()
		{
		}

		private static readonly PortMetadata _portMetadata;
	}
}
