using System;
using UnityEngine;

namespace SLZ.Marrow.Data
{
	[Serializable]
	public struct SoftJointLimitSpringExt
	{
		public float spring;

		public float damper;

        public SoftJointLimitSpringExt(SoftJointLimitSpring softJointLimitSpring) : this()
        {
            spring = softJointLimitSpring.spring;
            damper = softJointLimitSpring.damper;
        }


        public SoftJointLimitSpring ToUnitySoftJointLimitSpring()
		{
			return default(SoftJointLimitSpring);
		}
	}
}
