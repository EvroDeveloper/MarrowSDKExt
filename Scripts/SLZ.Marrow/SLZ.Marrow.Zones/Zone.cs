 
 
using SLZ.Marrow.Interaction;
using UnityEngine;

namespace SLZ.Marrow.Zones
{
    [AddComponentMenu("MarrowSDK/Zones/Zone")]
    public class Zone : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Reset()
        {
            gameObject.layer = (int)MarrowLayers.EntityTrigger;
            UnityEditor.EditorUtility.SetDirty(gameObject);
        }
#endif
    }
}