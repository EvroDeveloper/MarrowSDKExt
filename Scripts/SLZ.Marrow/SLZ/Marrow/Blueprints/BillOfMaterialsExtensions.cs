using System;

namespace SLZ.Marrow.Blueprints
{
	public static class BillOfMaterialsExtensions
	{
		public static T Resolve<T>(this IBillOfMaterials billOfMaterials, string key)
		{
			return default(T);
		}
	}
}
