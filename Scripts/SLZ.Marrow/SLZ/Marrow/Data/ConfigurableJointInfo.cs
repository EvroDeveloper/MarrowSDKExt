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
			joint.axis = axis;
			joint.secondaryAxis = secondaryAxis;
			joint.anchor = anchor;
			joint.connectedAnchor = connectedAnchor;
			joint.autoConfigureConnectedAnchor = autoConfigureConnectedAnchor;
			joint.breakForce = breakForce;
			joint.breakTorque = breakTorque;
			joint.enableCollision = enableCollision;
			joint.enablePreprocessing = enablePreprocessing;
			joint.massScale = massScale;
			joint.connectedMassScale = connectedMassScale;
			joint.projectionAngle = projectionAngle;
			joint.projectionDistance = projectionDistance;
			joint.projectionMode = (JointProjectionMode)projectionModeExt;
			joint.slerpDrive = slerpDriveExt.ToUnityJointDrive();
			joint.angularYZDrive = angularYZDriveExt.ToUnityJointDrive();
			joint.angularXDrive = angularXDriveExt.ToUnityJointDrive();
			joint.rotationDriveMode = rotationDriveMode;
			joint.targetAngularVelocity = targetAngularVelocity;
			joint.targetRotation = targetRotation;
			joint.zDrive = zDriveExt.ToUnityJointDrive();
			joint.yDrive = yDriveExt.ToUnityJointDrive();
			joint.xDrive = xDriveExt.ToUnityJointDrive();
			joint.targetVelocity = targetVelocity;
			joint.targetPosition = targetPosition;
			joint.angularZLimit = angularZLimitExt.ToUnitySoftJointLimit();
			joint.angularYLimit = angularYLimitExt.ToUnitySoftJointLimit();
			joint.highAngularXLimit = highAngularXLimitExt.ToUnitySoftJointLimit();
			joint.lowAngularXLimit = lowAngularXLimitExt.ToUnitySoftJointLimit();
			joint.linearLimit = linearLimitExt.ToUnitySoftJointLimit();
			joint.angularYZLimitSpring = angularYZLimitSpringExt.ToUnitySoftJointLimitSpring();
			joint.angularXLimitSpring = angularXLimitSpringExt.ToUnitySoftJointLimitSpring();
			joint.linearLimitSpring = linearLimitSpringExt.ToUnitySoftJointLimitSpring();
			joint.angularZMotion = angularZMotion;
			joint.angularYMotion = angularYMotion;
			joint.angularXMotion = angularXMotion;
			joint.zMotion = zMotion;
			joint.yMotion = yMotion;
			joint.xMotion = xMotion;
			joint.configuredInWorldSpace = configuredInWorldSpace;
			joint.swapBodies = swapBodies;
		}

		public void CopyFrom(ConfigurableJoint joint)
		{
			axis = joint.axis;
			secondaryAxis = joint.secondaryAxis;
			anchor = joint.anchor;
			connectedAnchor = joint.connectedAnchor;
			autoConfigureConnectedAnchor = joint.autoConfigureConnectedAnchor;
			breakForce = joint.breakForce;
			breakTorque = joint.breakTorque;
			enableCollision = joint.enableCollision;
			enablePreprocessing = joint.enablePreprocessing;
			massScale = joint.massScale;
			connectedMassScale = joint.connectedMassScale;
			projectionAngle = joint.projectionAngle;
			projectionDistance = joint.projectionDistance;
			projectionModeExt = (JointProjectionModeExt)joint.projectionMode;
			slerpDriveExt = new JointDriveExt()
			{
				positionSpring = joint.slerpDrive.positionSpring,
				positionDamper = joint.slerpDrive.positionDamper,
				maximumForce = joint.slerpDrive.maximumForce,
			};
			angularYZDriveExt = new JointDriveExt()
			{
				positionSpring = joint.angularYZDrive.positionSpring,
				positionDamper = joint.angularYZDrive.positionDamper,
				maximumForce = joint.angularYZDrive.maximumForce
			};
			angularXDriveExt = new JointDriveExt()
			{
				positionSpring = joint.angularXDrive.positionSpring,
				positionDamper = joint.angularXDrive.positionDamper,
				maximumForce = joint.angularXDrive.maximumForce
			};
			rotationDriveMode = joint.rotationDriveMode;
			targetAngularVelocity = joint.targetAngularVelocity;
			targetRotation = joint.targetRotation;
			zDriveExt = new JointDriveExt()
			{
				positionSpring = joint.zDrive.positionSpring,
				positionDamper = joint.zDrive.positionDamper,
				maximumForce = joint.zDrive.maximumForce
			};
            		yDriveExt = new JointDriveExt()
            		{
                		positionSpring = joint.yDrive.positionSpring,
                		positionDamper = joint.yDrive.positionDamper,
                		maximumForce = joint.yDrive.maximumForce
            		};
            		xDriveExt = new JointDriveExt()
            		{
                		positionSpring = joint.xDrive.positionSpring,
                		positionDamper = joint.xDrive.positionDamper,
                		maximumForce = joint.xDrive.maximumForce
            		};
			targetVelocity = joint.targetVelocity;
			targetPosition = joint.targetPosition;
			angularZLimitExt = new SoftJointLimitExt()
			{
				limit = joint.angularZLimit.limit,
				bounciness = joint.angularZLimit.bounciness,
				contactDistance = joint.angularZLimit.contactDistance,
			};
			angularYLimitExt = new SoftJointLimitExt()
			{
				limit = joint.angularYLimit.limit,
				bounciness = joint.angularYLimit.bounciness,
				contactDistance = joint.angularYLimit.contactDistance
			};
			highAngularXLimitExt = new SoftJointLimitExt()
			{
				limit = joint.highAngularXLimit.limit,
				bounciness = joint.highAngularXLimit.bounciness,
				contactDistance = joint.highAngularXLimit.contactDistance
			};
			lowAngularXLimitExt = new SoftJointLimitExt()
			{
				limit = joint.lowAngularXLimit.limit,
				bounciness = joint.lowAngularXLimit.bounciness,
				contactDistance = joint.lowAngularXLimit.bounciness
			};
			linearLimitExt = new SoftJointLimitExt()
			{
				limit = joint.linearLimit.limit,
				bounciness = joint.linearLimit.bounciness,
				contactDistance = joint.linearLimit.contactDistance
			};
			angularYZLimitSpringExt = new SoftJointLimitSpringExt()
			{
				spring = joint.angularYZLimitSpring.spring,
				damper = joint.angularYZLimitSpring.damper,
			};
			angularXLimitSpringExt = new SoftJointLimitSpringExt()
			{
				spring = joint.angularXLimitSpring.spring,
				damper = joint.angularXLimitSpring.damper
			};
			linearLimitSpringExt = new SoftJointLimitSpringExt()
			{
				spring = joint.linearLimitSpring.spring,
				damper = joint.linearLimitSpring.damper
			};
			angularZMotion = joint.angularZMotion;
			angularYMotion = joint.angularYMotion;
			angularXMotion = joint.angularXMotion;
			zMotion = joint.zMotion;
			yMotion = joint.yMotion;
			xMotion = joint.xMotion;
			configuredInWorldSpace = joint.configuredInWorldSpace;
			swapBodies = joint.swapBodies;
        	}

		public Quaternion GetJointSpace()
		{
			return default(Quaternion);
		}
	}
}
