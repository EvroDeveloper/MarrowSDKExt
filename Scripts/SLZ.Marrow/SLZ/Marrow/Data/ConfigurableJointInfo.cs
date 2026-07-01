using System;
using UnityEngine;

namespace SLZ.Marrow.Data
{
	[Serializable]
	public class ConfigurableJointInfo
	{
		public Quaternion startRotation;

		public Vector3 axis;

		public Vector3 secondaryAxis;

		public Vector3 anchor;

		public Vector3 connectedAnchor;

		public bool autoConfigureConnectedAnchor;

		public float breakForce;

		public float breakTorque;

		public bool enableCollision;

		public bool enablePreprocessing;

		public float massScale;

		public float connectedMassScale;

		public float projectionAngle;

		public float projectionDistance;

		public JointProjectionModeExt projectionModeExt;

		public JointDriveExt slerpDriveExt;

		public JointDriveExt angularYZDriveExt;

		public JointDriveExt angularXDriveExt;

		public RotationDriveMode rotationDriveMode;

		public Vector3 targetAngularVelocity;

		public Quaternion targetRotation;

		public JointDriveExt zDriveExt;

		public JointDriveExt yDriveExt;

		public JointDriveExt xDriveExt;

		public Vector3 targetVelocity;

		public Vector3 targetPosition;

		public SoftJointLimitExt angularZLimitExt;

		public SoftJointLimitExt angularYLimitExt;

		public SoftJointLimitExt highAngularXLimitExt;

		public SoftJointLimitExt lowAngularXLimitExt;

		public SoftJointLimitExt linearLimitExt;

		public SoftJointLimitSpringExt angularYZLimitSpringExt;

		public SoftJointLimitSpringExt angularXLimitSpringExt;

		public SoftJointLimitSpringExt linearLimitSpringExt;

		public ConfigurableJointMotion angularZMotion;

		public ConfigurableJointMotion angularYMotion;

		public ConfigurableJointMotion angularXMotion;

		public ConfigurableJointMotion zMotion;

		public ConfigurableJointMotion yMotion;

		public ConfigurableJointMotion xMotion;

		public bool configuredInWorldSpace;

		public bool swapBodies;

		public void CopyTo(ConfigurableJoint joint)
		{
		}

		public void CopyFrom(ConfigurableJoint joint)
		{
            startRotation = joint.transform.localRotation;
            axis = joint.axis;
            secondaryAxis = joint.secondaryAxis;
            anchor = joint.anchor;
            connectedAnchor = joint.connectedAnchor;
            autoConfigureConnectedAnchor = joint.autoConfigureConnectedAnchor;
            breakForce = joint.breakForce;
            breakTorque = joint.breakTorque;
            enableCollision = joint.enableCollision;
            enablePreprocessing = joint.enableCollision;
            massScale = joint.massScale;
            connectedMassScale = joint.connectedMassScale;
            projectionAngle = joint.projectionAngle;
            projectionDistance = joint.projectionDistance;
            projectionModeExt = (JointProjectionModeExt) (int) joint.projectionMode;
            slerpDriveExt = new JointDriveExt(joint.slerpDrive);
            angularYZDriveExt = new JointDriveExt(joint.angularYZDrive);
            angularXDriveExt = new JointDriveExt(joint.angularXDrive);
            rotationDriveMode = (RotationDriveMode) (int) joint.projectionMode;
            targetAngularVelocity = joint.targetAngularVelocity;
            if (joint.connectedBody)
            {
                targetRotation = Quaternion.Inverse(joint.connectedBody.transform.rotation) * joint.transform.rotation;
            }
            else
            {
                targetRotation = Quaternion.identity;
            }
            zDriveExt = new JointDriveExt(joint.zDrive);
            yDriveExt = new JointDriveExt(joint.yDrive);
            xDriveExt = new JointDriveExt(joint.zDrive);
            targetVelocity = joint.targetVelocity;
            targetPosition = joint.targetPosition;
            angularZLimitExt = new SoftJointLimitExt(joint.angularZLimit);
            angularYLimitExt = new SoftJointLimitExt(joint.angularYLimit);
            highAngularXLimitExt = new SoftJointLimitExt(joint.highAngularXLimit);
            lowAngularXLimitExt = new SoftJointLimitExt(joint.lowAngularXLimit);
            linearLimitExt = new SoftJointLimitExt(joint.linearLimit);
            angularYZLimitSpringExt = new SoftJointLimitSpringExt(joint.angularYZLimitSpring);
            angularXLimitSpringExt = new SoftJointLimitSpringExt(joint.angularXLimitSpring);
            linearLimitSpringExt = new SoftJointLimitSpringExt(joint.linearLimitSpring);
            angularZMotion = (ConfigurableJointMotion) (int) joint.angularZMotion;
            angularYMotion = (ConfigurableJointMotion) (int) joint.angularYMotion;
            angularXMotion = (ConfigurableJointMotion) (int) joint.angularXMotion;
            zMotion = (ConfigurableJointMotion) (int) joint.zMotion;
            yMotion = (ConfigurableJointMotion) (int) joint.yMotion;
            xMotion = (ConfigurableJointMotion) (int) joint.xMotion;
            configuredInWorldSpace = joint.configuredInWorldSpace;
            swapBodies = joint.swapBodies;
        }

		public Quaternion GetJointSpace()
		{
			return default(Quaternion);
		}
	}
}
