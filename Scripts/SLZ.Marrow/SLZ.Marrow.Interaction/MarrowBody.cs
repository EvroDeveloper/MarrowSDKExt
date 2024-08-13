using System.Collections.Generic;
using SLZ.Marrow.Utilities;
using SLZ.Marrow.Data;
using SLZ.Marrow.Warehouse;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;
#endif

namespace SLZ.Marrow.Interaction
{
    [DisallowMultipleComponent]
    public partial class MarrowBody : MonoBehaviour, ITaggable
    {
        [field: SerializeField]
        public MarrowEntity Entity { get; private set; }

        [SerializeField]
        private TagList _tags;
        public TagList Tags => _tags;

		public static ComponentCache<MarrowBody> Cache
		{
			get
			{
				return null;
			}
		}

        [field: SerializeField]
		public bool HasRigidbody { get; private set; }

        [field: SerializeField]
		public bool isCenterOfMassOverride { get; private set; }
        
        [field: SerializeField]
		public Vector3 CenterOfMass { get; private set; }

		public float Mass => 0f;

		public Vector3 CenterOfMassInWorld => default(Vector3);

		public Collider[] Colliders
		{
			get
			{
				return null;
			}
		}

		public Tracker[] Trackers
		{
			get
			{
				return null;
			}
		}

		public Collider[] Triggers
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
			private set
			{
			}
		}

		public bool IsPacked
		{
			get
			{
				return default(bool);
			}
			private set
			{
			}
		}

		public MarrowBody Host
		{
			get
			{
				return null;
			}
			private set
			{
			}
		}

        [field: SerializeField]
		public SimpleTransform InitInEntityTransform { get; private set; }

		public Bounds Bounds
		{
			get
			{
				return default(Bounds);
			}
		}

		public IReadOnlyList<MarrowJoint> InJoints
		{
			get
			{
				return null;
			}
		}

		public IReadOnlyList<MarrowJoint> OutJoints
		{
			get
			{
				return null;
			}
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void DestroyRigidbody()
		{
		}

		private void CreateRigidbody()
		{
		}

		internal void SetFromDefaultConfig()
		{
		}

		public void SetToDefaultConfig()
		{
		}

		public bool TryGetRigidbody(out Rigidbody rigidbody)
		{
            rigidbody = default(Rigidbody);
			return default(bool);
		}

		internal void INTHibernate(bool isHibernating = true)
		{
		}

		internal void INTEnableTrackers(bool isEnabled)
		{
		}

		private void EnableTrackersForIT(bool isEnabled)
		{
		}

		internal void INTEnableColliders(bool isEnabled)
		{
		}

		internal void INTEnableTriggers(bool isEnabled)
		{
		}

		internal void INTGhost(bool isGhosted)
		{
		}

		internal void INTPack(MarrowBody parasiteBody)
		{
		}

		internal void INTUnpack()
		{
		}

		internal void INTResetMass()
		{
		}

		private void CalculateMass(ref float mass, ref Vector3 wCom)
		{
		}

		public void Pack(MarrowBody parasite)
		{
		}

		public void Unpack()
		{
		}

		public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
		{
		}

		public void AddTorque(Vector3 torque, ForceMode mode = ForceMode.Force)
		{
		}

		public void IgnoreCollision(MarrowBody bodyToIgnore, bool doIgnore = true)
		{
		}

		public void IgnoreCollision(Collider otherCollider, bool doIgnore = true)
		{
		}

		internal void CleanupIgnoredCollisions()
		{
		}

		internal void ConnectJoint(MarrowJoint joint)
		{
		}

		internal void DisconnectJoint(MarrowJoint joint)
		{
		}

		internal void Validate(Rigidbody rb, MarrowEntity entity)
		{
		}

		private void CopyRigidbodyInfo(Rigidbody rb)
		{
		}

		private void SetEntity(MarrowEntity marrowEntity)
		{
		}

		private void GatherColliders()
		{
		}

		private void CalculateBounds()
		{
		}

		private void GenerateTrackers()
		{
		}

		private List<Tracker> GetTrackers(Transform transform)
		{
			return null;
		}

		private static ComponentCache<MarrowBody> _cache;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private RigidbodyInfo _defaultRigidbodyInfo;

		private readonly RigidbodyInfo _cachedRigidbodyInfo;

		[SerializeField]
		private Collider[] _colliders;

		private bool[] _colliderStates;

		[SerializeField]
		private Tracker[] _trackers = new Tracker[0];
		
		[SerializeField]
		private Collider[] _triggers;

		private bool[] _triggerStates;

		[SerializeField]
		private MarrowBody[] _bodiesToIgnore;

		[SerializeField]
		private Collider[] _collidersToIgnore;

		private HibernationBodyInfo _hibernationBodyInfo;

		private Transform _lastParent;

		internal readonly List<MarrowBody> INTParasites;

		[SerializeField]
		private Bounds _bounds;

		public TrackerSettingsGroup trackerSettings;

		private HashSet<MarrowJoint> _connectedJoints;

		private List<MarrowJoint> _inJoints;

		private List<MarrowJoint> _outJoints;

		private Dictionary<Collider, int> _ignoredColliders;

#if UNITY_EDITOR
		public Bounds GetBounds()
		{
			return CollectBounds(gameObject);
		}
		
		//credits slz
		private static Bounds CollectBounds(GameObject target)
        {
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
            PreviewRenderUtility fakeScene = new PreviewRenderUtility();

            GameObject root = new GameObject("root");
            fakeScene.AddSingleGO(root);

            GameObject gameobject = Object.Instantiate(target, Vector3.zero, Quaternion.identity, root.transform);
			MarrowBody body = gameobject.GetComponent<MarrowBody>();
			if(body == null)
			{
            	DestroyImmediate(gameobject);
            	DestroyImmediate(root);
            	fakeScene.Cleanup();
				return bounds;
			}
            Collider[] colliders = gameobject.GetComponentsInChildren<Collider>(true);
            bool firstcollider = true;
            foreach (Collider collider in colliders)
            {
				if(collider.GetComponentInParent<MarrowBody>(true) == body && collider.GetComponent<Tracker>() == null)
				{
					if(!collider.isTrigger)
					{
                		if (firstcollider)
						{
							firstcollider = false;
                		    bounds = collider.bounds;
						}
                		else
						{
                		    bounds.Encapsulate(collider.bounds);
						}
					}
				}
            }

            bounds.center = bounds.center - gameobject.transform.position;
            DestroyImmediate(gameobject);
            DestroyImmediate(root);
            fakeScene.Cleanup();

			return bounds;
        }
#endif

		public void ValidateBounds()
		{
#if UNITY_EDITOR
			Bounds bounds = GetBounds();
			if (bounds.size == Vector3.zero)
			{
				bounds.size = new Vector3(0.025f,0.025f,0.05f);
			}
			_bounds = bounds;
#endif
		}

		public void GenerateNewTracker()
		{
			List<Tracker> trackers = new List<Tracker>(_trackers);

			GameObject trackerGO = new GameObject("Tracker[GameObject]");
			trackerGO.transform.SetParent(transform, false);
			Tracker tracker = trackerGO.AddComponent<Tracker>();
			tracker.ValidateComponent();
			
			trackers.Add(tracker);
			_trackers = trackers.ToArray();
		}
		
		public void ValidateComponent()
		{
			//InteractableHost check	
			if (GetComponent<InteractableHost>() == null)
			{
				Object[] grips = gameObject.GetComponentsInChildren<Grip>(true);
				int bodygrip = 0;
				foreach (Object grip in grips)
				{
					if(((Component)grip).gameObject.GetComponentInParent<MarrowBody>(true) == this)
					{
						bodygrip++;
					}
				}
				if (bodygrip > 0)
				{
					gameObject.AddComponent<InteractableHost>();
				}
			}

			//MarrowEntity check
			if (Entity == null)
			{
				MarrowEntity rootEntity = GetComponentInParent<MarrowEntity>(true);
				MarrowEntity selfEntity = GetComponent<MarrowEntity>();
				if (selfEntity != null)
				{
					Entity = selfEntity;
				}
				else if(rootEntity != null)
				{
					Entity = rootEntity;
				}
			}
			if (Entity != null)
			{
				InitInEntityTransform = SimpleTransform.Create(Entity.transform).InverseTransform(SimpleTransform.Create(transform));
			}

			//Rigidbody check
			Rigidbody rigidbody = GetComponent<Rigidbody>();
			if (rigidbody != null)
			{
				_rigidbody = rigidbody;
				_defaultRigidbodyInfo = new RigidbodyInfo();
				_defaultRigidbodyInfo.CopyFrom(rigidbody);
				HasRigidbody = true;
			}
			else
			{
				_rigidbody = null;
				HasRigidbody = false;
			}

			//colliders, trigger and trackers
			Collider[] colliders = GetComponentsInChildren<Collider>(true);
			List<Collider> linkedColliders = new List<Collider>();
			List<Tracker> linkedTracker = new List<Tracker>();
			List<Collider> linkedTrigger = new List<Collider>();
			foreach (Collider collider in colliders)
			{
				if (collider.GetComponentInParent<MarrowBody>(true) == this)
				{
					Tracker tracker = collider.gameObject.GetComponent<Tracker>();
					if (tracker != null && !linkedTracker.Contains(tracker))
					{
						linkedTracker.Add(tracker);
					}
					else
					{
						if(collider.isTrigger)
						{
							linkedTrigger.Add(collider);
						}
						else
						{
							linkedColliders.Add(collider);
						}
					}
				}
			}

			_colliders = linkedColliders.ToArray();
			_trackers = linkedTracker.ToArray();
			_triggers = linkedTrigger.ToArray();

			if (linkedTracker.Count == 0)
			{
				GenerateNewTracker();
			}

			ValidateBounds();
			
#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
#endif
		}

		public void OnDrawGizmos()
    	{
    	}
    }

#if UNITY_EDITOR
	[CustomEditor(typeof(MarrowBody))]
	[DisallowMultipleComponent]
	public class MarrowBodyEditor : Editor 
	{
    	public override VisualElement CreateInspectorGUI()
    	{
    	    var root = new VisualElement();
			MarrowBody behaviour = (MarrowBody)target;
			if(!PrefabUtility.IsPartOfPrefabAsset(behaviour.gameObject))
			{
				var validatebutton = new Button(() => { behaviour.ValidateComponent(); }) { text = "Validate" };
				var Trackerbutton = new Button(() => { behaviour.GenerateNewTracker(); }) { text = "Add Tracker" };
				root.Add(validatebutton);
				root.Add(Trackerbutton);
			}

    	    UnityEditor.UIElements.InspectorElement.FillDefaultInspector(root, serializedObject, this);
    	    return root;
    	}

/*
	    public override void OnInspectorGUI()
	    {
			MarrowBody behaviour = (MarrowBody)target;

			if (!PrefabUtility.IsPartOfPrefabAsset(behaviour.gameObject))
			{
    	    	if(GUILayout.Button("Validate"))
        		{
					behaviour.ValidateComponent();
        		}

    	    	if(GUILayout.Button("Add Tracker"))
        		{
					behaviour.GenerateNewTracker();
        		}
			}
	
        	DrawDefaultInspector();
	    }
		*/
	}
#endif
}