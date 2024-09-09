using System;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/VoidLogic Greater Than or Equal")]
	[Support(SupportFlags.Supported, null)]
	public sealed class GreaterThanEqualNode : BaseNode
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

		public GreaterThanEqualNode()
		{
		}

		[Tooltip("Amount of approximation allowed in the equality check.\n0 will use Mathf.Approximately/Mathf.Epsilon to approximate.\nMake sure your tolerances match for consistent results!")]
		[SerializeField]
		private float _tolerance;

		private static readonly PortMetadata _portMetadata;
	}
}
