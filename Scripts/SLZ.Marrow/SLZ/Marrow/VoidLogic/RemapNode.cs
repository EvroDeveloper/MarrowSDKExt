using System;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/VoidLogic Remap")]
	[Support(SupportFlags.Supported, null)]
	public sealed class RemapNode : BaseNode
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

		public RemapNode()
		{
		}

		[SerializeField]
		[Tooltip("Output response curve")]
		private AnimationCurve _remapCurve;

		private static readonly PortMetadata _portMetadata;
	}
}
