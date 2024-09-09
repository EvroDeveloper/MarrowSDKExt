using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SLZ.Marrow.VoidLogic
{
	internal static class DictionaryCountedSetFunctions
	{
		public static bool AddOne<T>(this Dictionary<T, int> dict, T obj, [Out] int count, [Optional] Func<T, bool> onFirstAdded) where T : class
		{
			return default(bool);
		}

		public static bool TryRemoveOne<T>(this Dictionary<T, int> dict, T obj, [Out] int count, [Optional] Action<T, int> onLastRemoved) where T : class
		{
			return default(bool);
		}

		public static bool Clear<T>(this Dictionary<T, int> dict, T obj, [Out] int count, [Optional] Action<T, int> onRemoved) where T : class
		{
			return default(bool);
		}
	}
}
