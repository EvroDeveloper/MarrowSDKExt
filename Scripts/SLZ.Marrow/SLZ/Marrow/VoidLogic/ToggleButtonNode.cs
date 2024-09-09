using System;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu(null)]
	[Support(SupportFlags.Unsupported, null)]
	[Support(SupportFlags.Deprecated, "Use Button + Toggle instead")]
	[Obsolete("Use Button + Toggle instead", true)]
	public sealed class ToggleButtonNode : ButtonNode
	{
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

		public ToggleButtonNode()
		{
		}

		private float _multiplier;

		private static readonly PortMetadata _portMetadata;
	}
}
