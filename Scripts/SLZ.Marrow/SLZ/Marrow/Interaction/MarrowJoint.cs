using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SLZ.Marrow.Data;
using SLZ.Marrow.Utilities;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SLZ.Marrow.Interaction
{
	[Serializable]
	public class MarrowJoint : MonoBehaviour
	{
		[SerializeField]
		private MarrowBody _bodyA;

		[SerializeField]
		private MarrowBody _bodyB;

		[SerializeField]
		private ConfigurableJoint _configurableJoint;

		[SerializeField]
		private MarrowEntity _entity;

		[SerializeField]
		[ReadOnly(false)]
		private ConfigurableJointInfo _defaultConfigJointInfo;

		private SimpleTransform _jointSpace;

		private readonly ConfigurableJointInfo _cjTempInfo;

		private bool _doResetConnectedAnchor;

		private bool _hasBeenEnabled;

		private DisabledJointInfo _disableJointInfo;

		private List<Action<MarrowJoint>> _jointBreakActions;

		private List<Action<MarrowJoint>> _jointDestroyActions;

		public MarrowBody BodyA => _bodyA;

		public MarrowBody BodyB => _bodyB;

		public bool SwapBodies => false;

		public bool ConfiguredInWorld => false;

		public Quaternion StartRotation
		{
			get
			{
				return default(Quaternion);
			}
			private set
			{
			}
		}

		public Quaternion ToJointSpace
		{
			get
			{
				return default(Quaternion);
			}
			private set
			{
			}
		}

		public Quaternion ToJointSpaceInv
		{
			get
			{
				return default(Quaternion);
			}
			private set
			{
			}
		}

		public Quaternion ToJointSpaceFromDefault
		{
			get
			{
				return default(Quaternion);
			}
			private set
			{
			}
		}

		public bool IsCreatedAtRuntime
		{
			get
			{
				return false;
			}
			private set
			{
			}
		}

		public bool HasConfigJoint => false;

		public JointDrive xDriveBase
		{
			get
			{
				return default(JointDrive);
			}
			set
			{
			}
		}

		public JointDrive yDriveBase
		{
			get
			{
				return default(JointDrive);
			}
			set
			{
			}
		}

		public JointDrive zDriveBase
		{
			get
			{
				return default(JointDrive);
			}
			set
			{
			}
		}

		public JointDrive angularXDriveBase
		{
			get
			{
				return default(JointDrive);
			}
			set
			{
			}
		}

		public JointDrive angularYZDriveBase
		{
			get
			{
				return default(JointDrive);
			}
			set
			{
			}
		}

		public JointDrive slerpDriveBase
		{
			get
			{
				return default(JointDrive);
			}
			set
			{
			}
		}

		private void Awake()
		{
		}

		public void CreateJoint(ConfigurableJointInfo configJointInfo, MarrowBody bodyA, MarrowBody bodyB)
		{
		}

		private void OnDestroy()
		{
		}

		public void Destroy()
		{
		}

		private void OnJointBreak(float breakForce)
		{
		}

		private UniTaskVoid JointCleanupCheckAsync()
		{
			return default(UniTaskVoid);
		}

		public bool TryGetConfigurableJoint(out ConfigurableJoint joint)
		{
			joint = null;
			return false;
		}

		internal void SetFromDefaultConfig()
		{
		}

		public void SetToDefaultConfig()
		{
		}

		private void Build(ConfigurableJointInfo info)
		{
		}

		private void ReadJointSpace()
		{
		}

		private void WriteJointSpace()
		{
		}

		private void RestoreStartRotation()
		{
		}

		internal void OnBodyEnable()
		{
		}

		internal void OnBodyDisable()
		{
		}

		public void RegisterOnBreakEvent(Action<MarrowJoint> jointBreakAction)
		{
		}

		public void UnregisterOnBreakEvent(Action<MarrowJoint> jointBreakAction)
		{
		}

		public void RegisterOnDestroyEvent(Action<MarrowJoint> jointDestroyAction)
		{
		}

		public void UnregisterOnDestroyEvent(Action<MarrowJoint> jointDestroyAction)
		{
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

		private JointDrive ComputeJointDrive(JointDrive driveBase, float springMult, float damperMult, float maxForceMult)
		{
			return default(JointDrive);
		}

		public void FreeAllConstraints()
		{
		}

		public JointDrive SetJointDrive(float spring, float damper, float maxForce)
		{
			return default(JointDrive);
		}

		public void SetJointLimitsAll(float linearLimit, Vector4 angularLimits)
		{
		}

		private void Validate(ConfigurableJoint cj, MarrowEntity entity)
		{
		}

		private void CopyJointInfo(ConfigurableJoint cj)
		{
		}

		public void SetEntity(MarrowEntity marrowEntity)
		{
			_entity = marrowEntity;
		}

		public void ValidateComponent()
		{
			ConfigurableJoint joint = GetComponent<ConfigurableJoint>();
			if (joint != null)
			{
				MarrowBody marrowBody = GetComponent<MarrowBody>();
				if (marrowBody != null)
				{
					_bodyA = marrowBody;
				}
				if(joint.connectedBody != null)
				{
					MarrowBody connectedMarrowBody = joint.connectedBody.GetComponent<MarrowBody>();
					if(connectedMarrowBody != null)
					{
						_bodyB = connectedMarrowBody;
					}
				}
				_configurableJoint = joint; 

				_defaultConfigJointInfo = new ConfigurableJointInfo();
				_defaultConfigJointInfo.CopyFrom(joint);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(this);
			}
#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
#endif
		}
	}
	
#if UNITY_EDITOR
	[CustomEditor(typeof(MarrowJoint))]
	[DisallowMultipleComponent]
	public class MarrowJointEditor : Editor 
	{
	    public override void OnInspectorGUI()
	    {
			MarrowJoint behaviour = (MarrowJoint)target;

			if (!PrefabUtility.IsPartOfPrefabAsset(behaviour.gameObject))
			{
    	    	if(GUILayout.Button("Validate"))
        		{
					behaviour.ValidateComponent();
        		}
			}
	
        	DrawDefaultInspector();
	    }
	}
#endif
}
