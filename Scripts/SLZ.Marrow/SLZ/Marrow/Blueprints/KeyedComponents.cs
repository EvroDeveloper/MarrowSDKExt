using System;
using SLZ.Marrow.Utilities;
using UnityEngine;

namespace SLZ.Marrow.Blueprints
{
	[Serializable]
	public sealed class KeyedComponents : SerializableDictionary<string, Component>
	{
		public KeyedComponents()
		{
		}
	}
}
