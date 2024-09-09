using System;
using System.Runtime.InteropServices;

namespace SLZ.Marrow.Blueprints
{
	[Serializable]
	public struct VoidLogicEdge
	{
		public void Deconstruct([Out] VoidLogicPort from, [Out] VoidLogicPort to)
		{
		}

		public VoidLogicPort from;

		public VoidLogicPort to;
	}
}
