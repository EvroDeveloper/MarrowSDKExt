using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SLZ.Marrow.Interaction
{
	[RequireComponent(typeof(SplineContainer))]
	public class PolyLine : MonoBehaviour
	{
		private static readonly float LINEAR_LOOKUP_MIN_DIST;

		[SerializeField]
		public PolyLineData lineData;

		private PolyLineVert[] _polyVerts;

		private float _segmentDistance;

		private float _totalDistance;

		private bool _isBaked;

		[SerializeField]
		private SplineContainer _splineContainer;

		public SplineContainer SplineContainer => _splineContainer;

		private void Awake()
		{
		}

		public int GetNearestPoint(float3 worldPos, out float3 position, out float3 tangent, out float3 normal)
		{
			position = default(float3);
			tangent = default(float3);
			normal = default(float3);
			return 0;
		}

		public int GetPointAtLinearDistance(float3 worldPos, float distance, out float3 position, out float3 tangent, out float3 normal, int startIndex = 0)
		{
			position = default(float3);
			tangent = default(float3);
			normal = default(float3);
			return 0;
		}

		public int GetNearestPointFromIndex(float3 worldPos, out float3 position, out float3 tangent, out float3 normal, int startIndex = 0)
		{
			position = default(float3);
			tangent = default(float3);
			normal = default(float3);
			return 0;
		}

		public bool IsACappedIndex(int index)
		{
			return false;
		}

		public void GetPointAtIndex(int index, out float3 position, out float3 tangent, out float3 normal)
		{
			position = default(float3);
			tangent = default(float3);
			normal = default(float3);
		}

		private int GetNearestVertWithinRange(float3 localPos, int start, int end, out int interval, uint intervalDivision = 1u)
		{
			interval = default(int);
			return 0;
		}

		public static int Mod(int i, int max)
		{
            int r = i % max;
            return r < 0 ? r + max : r;
        }

		private void Reset()
		{
            _splineContainer = GetComponent<SplineContainer>();
        }

		private void OnDrawGizmos()
		{
		}

		private void DrawSplineGizmo()
		{
		}
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(PolyLine))]
    [CanEditMultipleObjects]
    public class PolyLineInspector : Editor
    {
        private PolyLineData.SegmentResolution _currentResolution = PolyLineData.SegmentResolution.Decimeter;
        private bool _hasBeenLoaded = false;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PolyLine pl = (PolyLine) target;

            GUILayout.Space(15);
            GUILayout.Label("Bake Settings");

            if (!_hasBeenLoaded && pl.lineData != null)
            {
                _currentResolution = pl.lineData.segmentResolution;
                _hasBeenLoaded = true;
            }

            _currentResolution = (PolyLineData.SegmentResolution) EditorGUILayout.EnumPopup("Segment Resolution", _currentResolution);

            if (GUILayout.Button("Bake PolyLine"))
            {
                BakePolyLine();
            }
        }

        private PolyLineData MakePolyLineAsset()
        {
            PolyLine pl = (PolyLine) target;

            string instanceID = pl.GetInstanceID().ToString();
            var polyLineDataAssets = AssetDatabase.FindAssets($"t:{typeof(PolyLineData)} {pl.name}_PolyLine_{instanceID}");

            if (polyLineDataAssets.Length <= 0)
            {
                PolyLineData asset = ScriptableObject.CreateInstance<PolyLineData>();
                AssetDatabase.CreateAsset(asset, $"Assets/{pl.name}_PolyLine_{instanceID}.asset");
                AssetDatabase.SaveAssets();
                return asset;
            }

            var assetPath = AssetDatabase.GUIDToAssetPath(polyLineDataAssets[0]);
            return AssetDatabase.LoadAssetAtPath<PolyLineData>(assetPath) as PolyLineData;
        }

        private void BakePolyLine()
        {
            PolyLine pl = (PolyLine) target;

            if (pl.lineData == null)
            {
                pl.lineData = MakePolyLineAsset();
                EditorUtility.SetDirty(pl);
                AssetDatabase.SaveAssetIfDirty(pl);
            }

            pl.lineData.segmentResolution = _currentResolution;


            pl.lineData.totalDistance = pl.SplineContainer.Spline.GetLength();
            int vertCount = Mathf.FloorToInt(pl.lineData.totalDistance / pl.lineData.SegmentDistance());
            float remainder = pl.lineData.totalDistance - (vertCount * pl.lineData.SegmentDistance());
            float edgeLength = pl.lineData.SegmentDistance() + (remainder / vertCount);

            pl.lineData.polyVerts = new PolyLineVert[vertCount];

            for (int i = 0; i < vertCount; i++)
            {
                float distance = i * edgeLength;
                float percentage = distance / pl.lineData.totalDistance;

                var vert = new PolyLineVert();

                pl.SplineContainer.Evaluate(percentage, out vert.position, out vert.tangent, out vert.normal);


                vert.tangent = math.normalize(vert.tangent);
                var cross = math.cross(vert.tangent, vert.normal);
                vert.normal = math.normalize(math.cross(cross, vert.tangent));

                pl.lineData.polyVerts[i] = vert;
            }


            for (int i = 0; i < vertCount; i++)
            {
                var vert = pl.lineData.polyVerts[i];
                var nextVert = pl.lineData.polyVerts[PolyLine.Mod(i + 1, vertCount)];

                if (i == (vertCount - 1) && !pl.SplineContainer.Spline.Closed)
                {
                    pl.lineData.polyVerts[i].forward = pl.lineData.polyVerts[i - 1].forward;
                }
                else
                {
                    pl.lineData.polyVerts[i].forward = math.normalize(nextVert.position - vert.position);
                }
            }


            for (int i = vertCount - 1; i >= 0; i--)
            {
                var vert = pl.lineData.polyVerts[i];
                var nextVert = pl.lineData.polyVerts[PolyLine.Mod(i - 1, vertCount)];

                if (i == 0 && !pl.SplineContainer.Spline.Closed)
                {
                    pl.lineData.polyVerts[i].backward = pl.lineData.polyVerts[i + 1].backward;
                }
                else
                {
                    pl.lineData.polyVerts[i].backward = math.normalize(nextVert.position - vert.position);
                }
            }


            for (int i = 0; i < vertCount; i++)
            {
                var vert = pl.lineData.polyVerts[i];

                vert.position = pl.transform.InverseTransformPoint(vert.position);
                vert.tangent = pl.transform.InverseTransformDirection(vert.tangent);
                vert.normal = pl.transform.InverseTransformDirection(vert.normal);
                vert.forward = pl.transform.InverseTransformDirection(vert.forward);
                vert.backward = pl.transform.InverseTransformDirection(vert.backward);

                pl.lineData.polyVerts[i] = vert;
            }


            EditorUtility.SetDirty(pl.lineData);
            AssetDatabase.SaveAssetIfDirty(pl.lineData);
            AssetDatabase.Refresh();



        }

    }
#endif
}
