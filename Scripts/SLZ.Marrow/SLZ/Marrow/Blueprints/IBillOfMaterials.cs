using System;
using System.Collections.Generic;
using UnityEngine;

namespace SLZ.Marrow.Blueprints
{
	public interface IBillOfMaterials
	{
		object Resolve(string key);

		Dictionary<string, Component> AllComponents
		{
			get;
		}
	}
}
