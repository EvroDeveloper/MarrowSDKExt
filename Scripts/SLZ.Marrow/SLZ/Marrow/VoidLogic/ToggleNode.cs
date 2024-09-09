using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[AddComponentMenu("VoidLogic/VoidLogic Toggle")]
	[Support(SupportFlags.Supported, null)]
	public sealed class ToggleNode : BaseNode
	{
		public bool Value
		{
			[CompilerGenerated]
			get
			{
				return default(bool);
			}
			[CompilerGenerated]
			set
			{
			}
		}

		private EdgeDetector SetEdgeDetector
		{
			[CompilerGenerated]
			get
			{
				return null;
			}
			[CompilerGenerated]
			set
			{
			}
		}

		private EdgeDetector ResetEdgeDetector
		{
			[CompilerGenerated]
			get
			{
				return null;
			}
			[CompilerGenerated]
			set
			{
			}
		}

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

		public ToggleNode()
		{
		}

		private static readonly PortMetadata _portMetadata;
	}
}
