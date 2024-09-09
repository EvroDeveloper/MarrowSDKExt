using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SLZ.Marrow.Interaction;
using UnityEngine;

namespace SLZ.Marrow
{
	[Serializable]
	public class Servo : MonoBehaviour
	{
		public ConfigurableJoint joint
		{
			get
			{
				return null;
			}
		}

		public MarrowBody marrowBody
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

		public JointDrive xDriveBase
		{
			[CompilerGenerated]
			get
			{
				return default(JointDrive);
			}
			[CompilerGenerated]
			set
			{
			}
		}

		public JointDrive yDriveBase
		{
			[CompilerGenerated]
			get
			{
				return default(JointDrive);
			}
			[CompilerGenerated]
			set
			{
			}
		}

		public JointDrive zDriveBase
		{
			[CompilerGenerated]
			get
			{
				return default(JointDrive);
			}
			[CompilerGenerated]
			set
			{
			}
		}

		public JointDrive angularXDriveBase
		{
			[CompilerGenerated]
			get
			{
				return default(JointDrive);
			}
			[CompilerGenerated]
			set
			{
			}
		}

		public JointDrive angularYZDriveBase
		{
			[CompilerGenerated]
			get
			{
				return default(JointDrive);
			}
			[CompilerGenerated]
			set
			{
			}
		}

		public JointDrive slerpDriveBase
		{
			[CompilerGenerated]
			get
			{
				return default(JointDrive);
			}
			[CompilerGenerated]
			set
			{
			}
		}

		public Quaternion initialRotation
		{
			get
			{
				return default(Quaternion);
			}
		}

		public Quaternion toJointSpace
		{
			get
			{
				return default(Quaternion);
			}
		}

		public Quaternion toJointSpaceFromCa
		{
			get
			{
				return default(Quaternion);
			}
		}

		private void Start()
		{
		}

		public virtual void Initiate()
		{
		}

		public Vector3 GetConnectedAnchorInWorld()
		{
			return default(Vector3);
		}

		public float GetTwistInDegrees()
		{
			return 0f;
		}

		public float GetTwistInDegrees(Quaternion rotationToTest)
		{
			return 0f;
		}

		public float GetSwingInDegrees()
		{
			return 0f;
		}

		public float GetSwingInDegrees(Quaternion rotationToTest)
		{
			return 0f;
		}

		public float GetSwingInDegrees([Out] Vector2 yPositiveAlignsToSecAxis)
		{
			return 0f;
		}

		public float GetSwingInDegrees(Quaternion rotationToTest, [Out] Vector2 yPositiveAlignsToSecAxis)
		{
			return 0f;
		}

		public float GetSwingInLimitPercent([Out] Vector2 yPositiveAlignsToSecAxis)
		{
			return 0f;
		}

		public float GetSwingInLimitPercent(Quaternion rotationToTest, [Out] Vector2 yPositiveAlignsToSecAxis)
		{
			return 0f;
		}

		public float GetLinearXInMeters()
		{
			return 0f;
		}

		public float GetTwistInLimitPercent()
		{
			return 0f;
		}

		public void SetXDrive(float springMult, float damperMult, float maxForceMult)
		{
		}

		public void SetYDrive(float springMult, float damperMult, float maxForceMult)
		{
		}

		public void SetZDrive(float springMult, float damperMult, float maxForceMult)
		{
		}

		public void SetAngularXDrive(float springMult, float damperMult, float maxForceMult)
		{
		}

		public void SetAngularYZDrive(float springMult, float damperMult, float maxForceMult)
		{
		}

		public void SetSlerpDrive(float springMult, float damperMult, float maxForceMult)
		{
		}

		public void SetXDriveBase(float spring, float damper, float maxForce)
		{
		}

		public void SetYDriveBase(float spring, float damper, float maxForce)
		{
		}

		public void SetZDriveBase(float spring, float damper, float maxForce)
		{
		}

		public void SetAngularXDriveBase(float spring, float damper, float maxForce)
		{
		}

		public void SetAngularYZDriveBase(float spring, float damper, float maxForce)
		{
		}

		public void SetSlerpDriveBase(float spring, float damper, float maxForce)
		{
		}

		public void SetLinearLimitSpring(float spring, float damper)
		{
		}

		public void SetAngularXLimitSpring(float spring, float damper)
		{
		}

		public void SetAngularYZLimitSpring(float spring, float damper)
		{
		}

		private JointDrive ComputeJointDrive(JointDrive driveBase, float springMult, float damperMult, float maxForceMult)
		{
			return default(JointDrive);
		}

		public void FreeAllConstraints()
		{
		}

		private JointDrive SetJointDrive(float spring, float damper, float maxForce)
		{
			return default(JointDrive);
		}

		private SoftJointLimitSpring SetLimitSpring(float spring, float damper)
		{
			return default(SoftJointLimitSpring);
		}

		public void SetJointLimitsAll(float linearLimit, Vector4 angularLimits)
		{
		}

		public void SetTargetRotationLocal(Quaternion targetLocal)
		{
		}

		public void SetTargetRotationAndAngVelocityLocal(Quaternion targetLocal, float deltaTime)
		{
		}

		public void SetTargetPositionLocal(Vector3 targetLocalPosition, Quaternion targetLocalRotation)
		{
		}

		public Servo()
		{
		}

		[SerializeField]
		private ConfigurableJoint _joint;

		private Rigidbody _rb;

		private Quaternion _initialRotation;

		private Quaternion _toJointSpace;

		private Quaternion _toJointSpaceInv;

		private Quaternion _toJointSpaceFromCa;

		private Servo.RotationJointType _rotationJointType;

		private enum RotationJointType
		{
			None,
			Revolute,
			ZeroTwist,
			Spherical
		}
	}
}
