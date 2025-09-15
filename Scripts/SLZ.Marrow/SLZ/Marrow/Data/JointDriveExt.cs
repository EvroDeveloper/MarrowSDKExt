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

        public JointDriveExt(JointDrive drive) : this()
        {
            positionSpring = drive.positionSpring;
            positionDamper = drive.positionDamper;
            maximumForce = drive.maximumForce;
        }


        public JointDrive ToUnityJointDrive()
		{
			return default(JointDrive);
		}
	}
}
