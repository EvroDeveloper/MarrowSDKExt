using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SLZ.Marrow.Pool
{
	public abstract class SpawnEvents : MonoBehaviour, IPoolable
	{
		[SerializeField]
		protected Poolee _poolee;

		public ulong ID => 0uL;

		public bool IsDespawned => false;

		protected virtual void Awake()
		{
		}

		protected virtual void Reset()
		{
		}

		public void Despawn()
		{
		}

		public virtual void OnPoolInitialize()
		{
		}

		public virtual void OnPoolSpawn()
		{
		}

		public virtual void OnPoolDeInitialize()
		{
		}
		
		public virtual void ValidateComponent()
		{
			_poolee = GetComponentInParent<Poolee>(true);
#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
#endif
		}
	}
	
	
#if UNITY_EDITOR
	[CustomEditor(typeof(SpawnEvents))]
	[DisallowMultipleComponent]
	public class SpawnEventsEditor : Editor 
	{
	    public override void OnInspectorGUI()
	    {
			SpawnEvents behaviour = (SpawnEvents)target;
			
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
