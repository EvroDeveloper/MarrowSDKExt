using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SLZ.Marrow.Blueprints
{
	public class BillOfMaterials : MonoBehaviour, IBillOfMaterials
	{
		public Dictionary<string, Component> AllComponents
		{
			get
			{
				return null;
			}
		}

		public KeyedComponents Components
		{
			[CompilerGenerated]
			get
			{
				return null;
			}
			[CompilerGenerated]
			private set
			{
			}
		}

		object IBillOfMaterials.Resolve(string key)
		{
			return null;
		}

		public BillOfMaterials()
		{
		}
	}
}
