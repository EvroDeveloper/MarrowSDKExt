using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	public static class VL
	{
		public static void AddNode(IVoidLogicNode node)
		{
		}

		public static void RemoveNode(IVoidLogicNode node)
		{
		}

		public static void EnableNode(IVoidLogicNode node)
		{
		}

		public static void DisableNode(IVoidLogicNode node)
		{
		}

		public static void UpdateConnection(InputPortReference to, OutputPortReference from)
		{
		}

		public static void UpdateConnection(IVoidLogicSink sink, uint inputIndex, OutputPortReference from)
		{
		}

		public static bool TryGetValue(this OutputPortReference portReference, [Out] float value)
		{
			return default(bool);
		}

		public static float GetValue(this OutputPortReference portReference, float defaultValue = 0f)
		{
			return 0f;
		}

		public static OutputPortReference GetInputAtIndex(this IVoidLogicSink sink, uint index)
		{
			return default(OutputPortReference);
		}

		public static NodeState State(MonoBehaviour sourceMb)
		{
			return default(NodeState);
		}

		public static NodeState State(this IVoidLogicSource source)
		{
			return default(NodeState);
		}

		private static NodeState _badState;
	}
}
