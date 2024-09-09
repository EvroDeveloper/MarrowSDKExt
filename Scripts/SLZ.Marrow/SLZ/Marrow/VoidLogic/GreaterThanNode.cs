using System;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[Support(SupportFlags.Supported, null)]
	[AddComponentMenu("VoidLogic/VoidLogic Greater Than")]
	public sealed class GreaterThanNode : BaseNode
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

		public GreaterThanNode()
		{
		}

		[Tooltip("Amount of approximation allowed in the equality check.\n0 will use Mathf.Approximately/Mathf.Epsilon to approximate.\nMake sure your tolerances match for consistent results!")]
		[SerializeField]
		private float _tolerance;

		private static readonly PortMetadata _portMetadata;
	}
}
