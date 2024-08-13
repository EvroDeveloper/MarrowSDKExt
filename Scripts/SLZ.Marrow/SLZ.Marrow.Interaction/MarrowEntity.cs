using System.Collections.Generic;
using SLZ.Marrow.Utilities;
using SLZ.Marrow.Warehouse;
using SLZ.Marrow.Pool;
using SLZ.Marrow.Zones;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;
#endif

namespace SLZ.Marrow.Interaction
{
    [SelectionBase]
    public partial class MarrowEntity : MonoBehaviour, ITaggable
    {
        [field: Header("Marrow Entity")]
        [SerializeField]
        private TagList _tags;
        public TagList Tags => _tags;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
#endif


		public static ComponentCache<MarrowEntity> Cache
		{
			get
			{
				return null;
			}
		}

		public bool IsDestroyed
		{
			get
			{
				return default(bool);
			}
			private set
			{
			}
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnLevelLoadComplete()
		{
		}

		public void Teleport(Vector3 position, Quaternion rotation, bool doResetPose = false)
		{
		}

		public bool IsInactive
		{
			get
			{
				return default(bool);
			}
		}

		public bool IsCulled
		{
			get
			{
				return default(bool);
			}
		}

		public bool IsHidden
		{
			get
			{
				return default(bool);
			}
		}

		public bool IsDespawned
		{
			get
			{
				return default(bool);
			}
		}

		public void SetActive(bool isActive)
		{
		}

		public void PreventDisableOnCull(bool isPrevented = true)
		{
		}

		public void RegisterEventHandler(IMarrowEntityCullable cullable)
		{
		}

		public void UnregisterEventHandler(IMarrowEntityCullable cullable)
		{
		}

		public void RegisterEventHandler(IMarrowEntityDespawnable despawnable)
		{
		}

		public void UnregisterEventHandler(IMarrowEntityDespawnable despawnable)
		{
		}

		public void RegisterEventHandler(IMarrowEntityHideable hideable)
		{
		}

		public void UnregisterEventHandler(IMarrowEntityHideable hideable)
		{
		}

		private void InitializeInactiveState()
		{
		}

		private void DisableEvents_Awake()
		{
		}

		public void Hide(bool isHidden = true, bool doApplyImmediately = false)
		{
		}

		private void OnCullResolve(InactiveStatus status, bool isInactive)
		{
		}

		private void OnCullApply(InactiveStatus status, bool isInactive)
		{
		}

		private void OnDespawnApply(InactiveStatus status, bool isInactive)
		{
		}

		private void OnHideApply(InactiveStatus status, bool isInactive)
		{
		}

		private void Activate(InactiveStatus status)
		{
		}

		public IReadOnlyList<MarrowEntity> ConnectedEntities
		{
			get
			{
				return null;
			}
		}

		internal void AddConnection(MarrowEntity entity)
		{
		}

		internal void RemoveConnection(MarrowEntity entity)
		{
		}

		private void DFS(MarrowEntity entity)
		{
		}

		public MarrowBody AnchorBody
		{
			get
			{
				return null;
			}
		}

		public MarrowBody[] Bodies
		{
			get
			{
				return null;
			}
		}

		public MarrowJoint[] Joints
		{
			get
			{
				return null;
			}
		}

		public bool IsHibernating
		{
			get
			{
				return default(bool);
			}
		}

		public bool IsGhosting
		{
			get
			{
				return default(bool);
			}
			private set
			{
			}
		}

		public bool IsCollidersEnabled
		{
			get
			{
				return default(bool);
			}
			private set
			{
			}
		}

		public bool IsTriggersEnabled
		{
			get
			{
				return default(bool);
			}
			private set
			{
			}
		}

		public bool IsTrackersEnabled
		{
			get
			{
				return default(bool);
			}
			private set
			{
			}
		}

		public float ScaleRatio
		{
			get
			{
				return 0f;
			}
			private set
			{
			}
		}

		public IReadOnlyList<MarrowJoint> RuntimeJoints
		{
			get
			{
				return null;
			}
		}

		public Poolee Poolee
		{
			get
			{
				return _poolee;
			}
		}

		private void Physics_Awake()
		{
		}

		private void Physics_OnCullDisabled()
		{
		}

		private void Physics_OnCullEnabled()
		{
		}

		private void Physics_OnPoolInitialize()
		{
		}

		private void Physics_OnSpawn()
		{
		}

		private void Physics_OnPoolDeinitialize()
		{
		}

		private void Physics_OnLevelLoadComplete()
		{
		}

		public void SetPhysicsDefaults()
		{
		}

		public void Hibernate(bool setHibernating = true, MarrowEntity.HibernationSources source = MarrowEntity.HibernationSources.Default)
		{
		}

		private void ClearHibernation()
		{
		}

		private void ClearGhost()
		{
		}

		private void CleanupIgnoredCollisions()
		{
		}

		public void Ghost(bool isGhosting)
		{
		}

		public void Unpack(MarrowBody hostBody, MarrowBody parasiteBody)
		{
		}

		public void Pack(MarrowBody hostBody, MarrowBody parasiteBody)
		{
		}

		public void EnableColliders(bool isEnabled = true)
		{
		}

		private void ForceEnableColliders(bool isEnabled = true)
		{
		}

		public void EnableTriggers(bool isEnabled = true)
		{
		}

		private void ForceEnableTriggers(bool isEnabled = true)
		{
		}

		public void EnableTrackers(bool isEnabled = true)
		{
		}

		private void ForceEnableTrackers(bool isEnabled)
		{
		}

		public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
		{
		}

		public void AddTorque(Vector3 torque, ForceMode mode = ForceMode.Force)
		{
		}

		public Vector3 GetCenterOfMassInWorld()
		{
			return default(Vector3);
		}
		
		public void ResetMass()
		{
		}

		public void IgnoreCollision(MarrowEntity otherEntity, bool isIgnoring = true)
		{
		}

		public void IgnoreCollision(Collider colliderToIgnore, bool isIgnoring = true)
		{
		}

		internal void AddRuntimeJoint(MarrowJoint joint)
		{
		}

		internal void RemoveRuntimeJoint(MarrowJoint joint)
		{
		}

		private void SpawnEvents_Awake()
		{
		}

		public void RegisterEventHandler(IPoolable poolable)
		{
		}

		public void UnregisterEventHandler(IPoolable poolable)
		{
		}

		public void Despawn()
		{
		}

		public void OnPoolInitialize()
		{
		}

		public void OnPoolSpawn()
		{
		}

		public void OnPoolDeInitialize()
		{
		}

		private void Validate()
		{
		}

		private void ValidatePoolee()
		{
		}

		private void ValidateBodies()
		{
		}

		[ContextMenu("ValidatePoseCache")]
		private void ValidatePoseCache()
		{
		}

		private void ValidateBody(MarrowBody mBody, Rigidbody rBody)
		{
		}

		private void ValidateJoints()
		{
		}

		private void ValidateJoint(MarrowJoint mJoint, ConfigurableJoint cJoint)
		{
		}

		private void GatherMarrowBehaviours()
		{
		}

		private static ComponentCache<MarrowEntity> _cache;

		[SerializeField]
		private MarrowBehaviour[] _behaviours;

		private List<IMarrowEntityCullable> _cullables;

		private List<IMarrowEntityHideable> _hideables;

		private List<IMarrowEntityDespawnable> _despawnables;

		private bool _hasInactiveState;

		internal InactiveStatus _inactiveStatus;

		private bool _isDisableOnCullPrevented;

		private List<MarrowEntity> _outLinks;

		private List<MarrowEntity> _inLinks;

		private bool _isDirty;

		private HashSet<MarrowEntity> _visited;

		private List<MarrowEntity> _connectedEntities;

		private MarrowEntity.HibernationSources allHibernationSources;

		[SerializeField]
		[Header("Physics")]
		private Vector3 _originalScale;

		[SerializeField]
		private MarrowBody[] _bodies;

		[SerializeField]
		private MarrowJoint[] _joints;

		[SerializeField]
		private MarrowBody _anchorBody;

		private SimpleTransform[] _defaultPoseCache;

		private SimpleTransform[] _teleportTransformCache;

		private MarrowEntity _hostEntity;

		private List<MarrowEntity> _parasites;

		private MarrowEntity.HibernationSources _hibernationFlags;

		private List<MarrowJoint> _runtimeJoints;

		[SerializeField]
		private Poolee _poolee;

		[System.Flags]
		public enum HibernationSources
		{
			Default = 1,
			Culling = 2,
			Loading = 4
		}

		public void ValidateComponent()
		{
			//originalScale
			_originalScale = gameObject.transform.localScale;

			//tracker validation
			Tracker[] trackers = GetComponentsInChildren<Tracker>(true);
			foreach(Tracker tracker in trackers)	
			{
				tracker.ValidateComponent();
			}

			//bodies
			MarrowBody[] marrowBodies = GetComponentsInChildren<MarrowBody>(true);
			List<MarrowBody> validMarrowBodies = new List<MarrowBody>();
			foreach(MarrowBody marrowBody in marrowBodies)	
			{
				marrowBody.ValidateComponent();
				if(marrowBody.Entity == this)
				{
					validMarrowBodies.Add(marrowBody);
				}
			}
			_bodies = validMarrowBodies.ToArray();

			//anchor body
			if(_bodies.Length > 0)
			{
				_anchorBody = _bodies[0];
			}

			//joints
			MarrowJoint[] marrowJoints = GetComponentsInChildren<MarrowJoint>(true);
			List<MarrowJoint> validMarrowJoints = new List<MarrowJoint>();
			foreach(MarrowJoint marrowJoint in marrowJoints)	
			{
				marrowJoint.ValidateComponent();
				if(marrowJoint != null)
				{
					MarrowEntity parentEntity = marrowJoint.GetComponentInParent<MarrowEntity>(true);
					if (this == parentEntity)
					{
						marrowJoint.SetEntity(this);
					}
					validMarrowJoints.Add(marrowJoint);
				}
			}
			_joints = validMarrowJoints.ToArray();

			//marrow behaviours
			MarrowBehaviour[] marrowBehaviours = GetComponentsInChildren<MarrowBehaviour>(true);
			List<MarrowBehaviour> linkedbehaviours = new List<MarrowBehaviour>();
			foreach (MarrowBehaviour behaviour in marrowBehaviours)
			{
				MarrowEntity parentEntity = behaviour.GetComponentInParent<MarrowEntity>(true);
				if (this == parentEntity)
				{
					behaviour.marrowEntity = this;
					linkedbehaviours.Add(behaviour);
				}
			}
			_behaviours = linkedbehaviours.ToArray();

			//poolee
			Poolee poolee = GetComponent<Poolee>();
			//if(poolee == null)
			//{
			//	poolee = gameObject.AddComponent<Poolee>();
			//}
			_poolee = poolee;

			//spawn event components
			SpawnEvents[] spawnEvents = GetComponentsInChildren<SpawnEvents>(true);
			foreach(SpawnEvents spawnEvent in spawnEvents)
			{
				spawnEvent.ValidateComponent();
			}
			
		}
    }

#if UNITY_EDITOR
	[CustomEditor(typeof(MarrowEntity))]
	[DisallowMultipleComponent]
	public class MarrowEntityEditor : Editor 
	{
    	public override VisualElement CreateInspectorGUI()
    	{
    	    var root = new VisualElement();
			GameObject behaviourGO = ((MarrowEntity)target).gameObject;
			if(behaviourGO != null && !PrefabUtility.IsPartOfPrefabAsset(behaviourGO))
			{
				var validatebutton = new Button(() => { PopulateMarrowComponents(behaviourGO); }) { text = "Validate" };
				root.Add(validatebutton);
			}

    	    UnityEditor.UIElements.InspectorElement.FillDefaultInspector(root, serializedObject, this);
    	    return root;
    	}
/*
	    public override void OnInspectorGUI()
	    {
			GameObject behaviourGO = ((MarrowEntity)target).gameObject;

			if(behaviourGO != null && !PrefabUtility.IsPartOfPrefabAsset(behaviourGO))
			{
				if(((MarrowEntity)target).Poolee == null)
				{
					EditorGUILayout.HelpBox("Poolee not defined!", MessageType.Warning);
					if(GUILayout.Button("Add Poolee"))
					{
						if(behaviourGO.GetComponent<Poolee>() == null)
						{
							behaviourGO.AddComponent<Poolee>();
						}
						PopulateMarrowComponents(behaviourGO);
					}
				}
    	    	if(GUILayout.Button("Validate"))
        		{
					PopulateMarrowComponents(behaviourGO);
				}
			}
	
        	DrawDefaultInspector();
	    }
*/
		[MenuItem("GameObject/[Experimental] Delete Marrow Components", false, 0)]
    	public static void DeleteMarrowComponents(MenuCommand menuCommand) {
    	    var selected = Selection.gameObjects[0];
			if(selected == null)
			{
				return;
			}

			if( EditorUtility.DisplayDialog("Confirmation Dialog", "Delete ALL the Marrow Components?", "Yes", "No") )
			{
    	    	MarrowBody[] marrowBodies = selected.GetComponentsInChildren<MarrowBody>();
    	    	Tracker[] trackers = selected.GetComponentsInChildren<Tracker>();
    	    	MarrowJoint[] marrowJoints = selected.GetComponentsInChildren<MarrowJoint>();
    	    	MarrowEntity[] marrowEntities = selected.GetComponentsInChildren<MarrowEntity>();
    	    	MarrowBehaviour[] marrowBehaviours = selected.GetComponentsInChildren<MarrowBehaviour>();
    	    	Poolee[] poolees = selected.GetComponentsInChildren<Poolee>();
				Undo.RecordObjects(marrowBodies, "undo bodies");
				Undo.RecordObjects(marrowJoints, "undo joints");
				Undo.RecordObjects(marrowEntities, "undo entities");
				//Undo.RecordObjects(marrowBehaviours, "undo behaviours");
				Undo.RecordObjects(poolees, "undo poolees");

				foreach(MarrowBody body in marrowBodies)
				{
					Object.DestroyImmediate(body);
				}
				foreach(Tracker tracker in trackers)
				{
					Undo.RecordObject(tracker.gameObject, tracker.gameObject.name);
					Object.DestroyImmediate(tracker.gameObject);
				}
				foreach(MarrowJoint joint in marrowJoints)
				{
					Object.DestroyImmediate(joint);
				}
				foreach(MarrowEntity entity in marrowEntities)
				{
					Object.DestroyImmediate(entity);
				}
				//foreach(MarrowBehaviour behaviour in marrowBehaviours)
				//{
				//	Object.DestroyImmediate(behaviour);
				//}
				foreach(Poolee poolee in poolees)
				{
					Object.DestroyImmediate(poolee);
				}
				
#if UNITY_EDITOR
				EditorUtility.SetDirty(selected);
#endif
			}
    	}

		[MenuItem("GameObject/[Experimental] Populate Marrow Components", false, 0)]
    	public static void PopulateMarrowComponent_button(MenuCommand menuCommand) {
			GameObject target = Selection.activeGameObject;
			if(target != null && EditorUtility.DisplayDialog("Confirmation Dialog", "This will populate necessary components for the entities. Make sure you backup this object", "Yes", "No") )
			{
				if( target.GetComponent<Poolee>() == null )
				{
					target.AddComponent<Poolee>();
				}
				PopulateMarrowComponents(target);
			}
		}

    	public static void PopulateMarrowComponents(GameObject go) {
			if(go == null)
			{
				return;
			}

			MarrowEntity marrowEntity = go.GetComponent<MarrowEntity>();
			if( marrowEntity == null )
			{
				go.AddComponent<MarrowEntity>();
			}

			Rigidbody[] rigidbodies = go.GetComponentsInChildren<Rigidbody>();
			foreach(Rigidbody rigidbody in rigidbodies)
			{
				MarrowBody body = rigidbody.GetComponent<MarrowBody>();
				if( body == null )
				{
					body = rigidbody.gameObject.AddComponent<MarrowBody>();
				}
			}
			
			ConfigurableJoint[] joints = go.GetComponentsInChildren<ConfigurableJoint>();
			foreach(ConfigurableJoint joint in joints)
			{
				MarrowJoint marrowJoint = joint.GetComponent<MarrowJoint>();
				if( marrowJoint == null )
				{
					marrowJoint = joint.gameObject.AddComponent<MarrowJoint>();
				}
			}

			MarrowEntity[] marrowEntities = go.GetComponentsInChildren<MarrowEntity>();
			foreach(MarrowEntity entity in marrowEntities)
			{
				entity.ValidateComponent();
			}

#if UNITY_EDITOR
			EditorUtility.SetDirty(go);
#endif
    	}
	}
#endif
}