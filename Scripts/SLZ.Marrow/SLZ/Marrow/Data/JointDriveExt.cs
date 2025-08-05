using System;
using UnityEngine;

namespace SLZ.Marrow.Data
{
	[Serializable]
	public struct JointDriveExt
	{
		public float positionSpring;

		public float positionDamper;

		public float maximumForce;


		public JointDrive ToUnityJointDrive()
		{
			return new JointDrive()
			{
				positionSpring = positionSpring,
				positionDamper = positionDamper,
				maximumForce = maximumForce
			};
		}
	}
}
