using System;
using System.Collections.Generic;
using SLZ.Marrow.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SLZ.Marrow
{
	[DisallowMultipleComponent]
	public class BoxGrip : TargetGrip
	{
		private class GrabConfiguration
		{
			public bool isZoneValid;

			public bool isSandwich;

			public bool isFace;

			public Faces face;

			public bool isEdge;

			public Edges edge;

			public bool isCorner;

			public Corners corner;

			public Vector3 gripWorld;

			public Vector3 handleWorld;

			public Vector3 normal;

			public Vector3 localPosition;

			public Quaternion localRotation;
		}

		[Flags]
		public enum Faces
		{
			PositiveX = 1,
			NegativeX = 2,
			PositiveY = 4,
			NegativeY = 8,
			PositiveZ = 0x10,
			NegativeZ = 0x20
		}

		[Flags]
		public enum Edges
		{
			PositiveXPositiveY = 1,
			NegativeXPositiveY = 2,
			PositiveXNegativeY = 4,
			NegativeXNegativeY = 8,
			PositiveXPositiveZ = 0x10,
			NegativeXPositiveZ = 0x20,
			PositiveXNegativeZ = 0x40,
			NegativeXNegativeZ = 0x80,
			PositiveYPositiveZ = 0x100,
			NegativeYPositiveZ = 0x200,
			PositiveYNegativeZ = 0x400,
			NegativeYNegativeZ = 0x800
		}
		[Flags]
		public enum Corners
		{
			PositiveXPositiveYPositiveZ = 1,
			NegativeXPositiveYPositiveZ = 2,
			PositiveXNegativeYPositiveZ = 4,
			NegativeXNegativeYPositiveZ = 8,
			PositiveXPositiveYNegativeZ = 0x10,
			NegativeXPositiveYNegativeZ = 0x20,
			PositiveXNegativeYNegativeZ = 0x40,
			NegativeXNegativeYNegativeZ = 0x80
		}

		private struct HandToBoxGripState
		{
			public float radius;

			public HandPose handPose;
		}

		[Header("BoxGrip Options")]
		[FormerlySerializedAs("sandwitchSize")]
		public float sandwichSize = 0.15f;

		public float edgePadding = 0.2f;

		public float faceInsetdistance;

		public float faceDepth;

		[FormerlySerializedAs("sandwitchHandPose")]
		public HandPose sandwichHandPose;

		public bool canBeSandwichedGrabbed;

		public HandPose edgeHandPose;

		public float edgeHandPoseRadius = 0.05f;

		public bool canBeEdgeGrabbed;

		public HandPose cornerHandPose;

		public float cornerHandPoseRadius = 0.05f;

		public bool canBeCornerGrabbed;

		public HandPose faceHandPose;

		public float faceHandPoseRadius = 1;

		public bool canBeFaceGrabbed;

		private Bounds _bounds;

		private Dictionary<Hand, GrabConfiguration> _grabConfig;

		[Header("BoxGrip Face Options")]
		#if UNITY_EDITOR
			[Tooltip("Whether or not to render the custom Gizmos and Handles. The 'Can Be ____ Grabbed' bool must be enabled for each Gizmo/Handle to render.")]
			public bool renderGizmos = true;
			[NonSerialized]
			[HideInInspector]
			public bool renderHandles = false;
		#endif
        [SerializeField]
		[EnumFlags]
		public Faces enabledFaces;
		[EnumFlags]
		[SerializeField]
		public Edges enabledEdges;
		[SerializeField]
		[EnumFlags]
		public Corners enabledCorners;

		[Tooltip("Defines the primary face for force grab orentation")]
		[SerializeField]
		private Faces forceGrabFace;

		[Tooltip("Defines the secondary face for force grab orentation")]
		[SerializeField]
		private Faces forceGrabTop;

		[SerializeField]
		[Header("References")]
		public BoxCollider _boxCollider;

		private Dictionary<Hand, HandToBoxGripState> boxHandStates;

		private float _45DegSin;

		private Vector3 _boxCenter;

		private Vector3 _boxSize;

		private Vector3 _boxExtents;

		protected override void Awake()
		{
		}

		private float GetDistanceBetweenFace(Faces face)
		{
			return 0f;
		}

		private Vector3 FaceToVector(Faces face)
		{
			return default(Vector3);
		}

		public override SimpleTransform GetForcePullTransform(Hand hand)
		{
			return default(SimpleTransform);
		}

		public void UpdateForcePullTransform(Hand hand)
		{
		}

		public float GetEdgePadding()
		{
			return 0f;
		}

		public override void OnFarHandHoverBegin(Hand hand)
		{
		}

		public override void OnHandHoverUpdate(Hand hand)
		{
		}

		public override bool OnHandHoverUpdate(Hand hand, bool isOverride)
		{
			return false;
		}

		public override void OnHandAttachedUpdate(Hand hand)
		{
		}

		protected override void UpdateJointConfiguration(Hand hand)
		{
		}

		public override (float, float, Vector3, Vector3) ValidateGripScore(Hand hand, SimpleTransform handTransform)
		{
			return default((float, float, Vector3, Vector3));
		}

#if UNITY_EDITOR

		[ContextMenu("Populate Collider and Grip Poses")]
        private void Reset()
        {
            _boxCollider = GetComponent<BoxCollider>();
			sandwichHandPose = AssetDatabase.LoadAssetAtPath<HandPose>("Assets/Marrow-ExtendedSDK-MAINTAINED-main/Data/HandPose/BoxSandwichGrip.asset");
			edgeHandPose = AssetDatabase.LoadAssetAtPath<HandPose>("Assets/Marrow-ExtendedSDK-MAINTAINED-main/Data/HandPose/BoxEdgeGrip.asset");
			cornerHandPose = AssetDatabase.LoadAssetAtPath<HandPose>("Assets/Marrow-ExtendedSDK-MAINTAINED-main/Data/HandPose/BoxCornerGrip.asset");
			faceHandPose = AssetDatabase.LoadAssetAtPath<HandPose>("Assets/Marrow-ExtendedSDK-MAINTAINED-main/Data/HandPose/BoxFaceGrip.asset");
        }

        private void OnDrawGizmos()
        {
            if (Selection.activeGameObject != gameObject)
            {
                renderHandles = false;
            }
        }

        private void OnDrawGizmosSelected()
		{
			if (_boxCollider && renderGizmos && Selection.activeGameObject == gameObject)
			{
				//Correctly setting the Gizmo matrix for the rest of the Gizmos. Thank you Cam for the base setup of this!
				Vector3 halfSize = _boxCollider.size * 0.5f;
				Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.rotation * Vector3.Scale(_boxCollider.center, transform.lossyScale), transform.rotation, transform.lossyScale);

				if (canBeFaceGrabbed)
				{
					float faceCubeDepth = 0.01f * Mathf.Clamp01(halfSize.magnitude);
					//PositiveX Gizmo
					if (enabledFaces.HasFlag(Faces.PositiveX))
					{
						Gizmos.color = new Color(1f, 0f, 0f, 0.6f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.right), new Vector3(faceCubeDepth / transform.lossyScale.x, halfSize.y, halfSize.z));
					}
					//NegativeX Gizmo
					if (enabledFaces.HasFlag(Faces.NegativeX))
					{
						Gizmos.color = new Color(1f, 0f, 0.5f, 0.6f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.left), new Vector3(faceCubeDepth / transform.lossyScale.x, halfSize.y, halfSize.z));
					}
					//PositiveY Gizmo
					if (enabledFaces.HasFlag(Faces.PositiveY))
					{
						Gizmos.color = new Color(0f, 1f, 0f, 0.6f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.up), new Vector3(halfSize.x, faceCubeDepth / transform.lossyScale.y, halfSize.z));
					}
					//NegativeY Gizmo
					if (enabledFaces.HasFlag(Faces.NegativeY))
					{
						Gizmos.color = new Color(0f, 1f, 0.5f, 0.6f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.down), new Vector3(halfSize.x, faceCubeDepth / transform.lossyScale.y, halfSize.z));
					}
					//PositiveZ Gizmo
					if (enabledFaces.HasFlag(Faces.PositiveZ))
					{
						Gizmos.color = new Color(0f, 0f, 1f, 0.6f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.forward), new Vector3(halfSize.x, halfSize.y, faceCubeDepth / transform.lossyScale.z));
					}
					//NegativeZ Gizmo
					if (enabledFaces.HasFlag(Faces.NegativeZ))
					{
						Gizmos.color = new Color(0f, 0.5f, 1f, 0.6f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.back), new Vector3(halfSize.x, halfSize.y, faceCubeDepth / transform.lossyScale.z));
					}
				}
				if (canBeEdgeGrabbed)
				{
					float edgeCubeDepth = 0.02f * halfSize.magnitude;
					//XY Axis Edges
					if (enabledEdges.HasFlag(Edges.PositiveXPositiveY))
					{
						Gizmos.color = new Color(1f, 1f, 0f, 0.8f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.right + Vector3.up), new Vector3(edgeCubeDepth, edgeCubeDepth, halfSize.z * 1.25f));
					}
					if (enabledEdges.HasFlag(Edges.NegativeXPositiveY))
					{
						Gizmos.color = new Color(0f, 1f, 0f, 0.8f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.left + Vector3.up), new Vector3(edgeCubeDepth, edgeCubeDepth, halfSize.z * 1.25f));
					}
					if (enabledEdges.HasFlag(Edges.PositiveXNegativeY))
					{
						Gizmos.color = new Color(1f, 0f, 0f, 0.8f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.right + Vector3.down), new Vector3(edgeCubeDepth, edgeCubeDepth, halfSize.z * 1.25f));
					}
					if (enabledEdges.HasFlag(Edges.NegativeXNegativeY))
					{
						Gizmos.color = new Color(0f, 0.1f, 0.3f, 0.8f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.left + Vector3.down), new Vector3(edgeCubeDepth, edgeCubeDepth, halfSize.z * 1.25f));
					}
					//XZ Axis Edges
					if (enabledEdges.HasFlag(Edges.PositiveXPositiveZ))
					{
						Gizmos.color = new Color(1f, 0f, 1f, 0.8f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.right + Vector3.forward), new Vector3(edgeCubeDepth, halfSize.y * 1.25f, edgeCubeDepth));
					}
					if (enabledEdges.HasFlag(Edges.NegativeXPositiveZ))
					{
						Gizmos.color = new Color(0f, 0f, 1f, 0.8f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.left + Vector3.forward), new Vector3(edgeCubeDepth, halfSize.y * 1.25f, edgeCubeDepth));
					}
					if (enabledEdges.HasFlag(Edges.PositiveXNegativeZ))
					{
						Gizmos.color = new Color(1f, 0f, 0f, 0.8f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.right + Vector3.back), new Vector3(edgeCubeDepth, halfSize.y * 1.25f, edgeCubeDepth));
					}
					if (enabledEdges.HasFlag(Edges.NegativeXNegativeZ))
					{
						Gizmos.color = new Color(0f, 0.5f, 0.35f, 0.8f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.left + Vector3.back), new Vector3(edgeCubeDepth, halfSize.y * 1.25f, edgeCubeDepth));
					}
					//YZ Axis Edges
					if (enabledEdges.HasFlag(Edges.PositiveYPositiveZ))
					{
						Gizmos.color = new Color(0f, 1f, 1f, 0.8f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.up + Vector3.forward), new Vector3(halfSize.x * 1.25f, edgeCubeDepth, edgeCubeDepth));
					}
					if (enabledEdges.HasFlag(Edges.NegativeYPositiveZ))
					{
						Gizmos.color = new Color(0f, 0f, 1f, 0.8f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.down + Vector3.forward), new Vector3(halfSize.x * 1.25f, edgeCubeDepth, edgeCubeDepth));
					}
					if (enabledEdges.HasFlag(Edges.PositiveYNegativeZ))
					{
						Gizmos.color = new Color(0f, 1f, 0f, 0.8f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.up + Vector3.back), new Vector3(halfSize.x * 1.25f, edgeCubeDepth, edgeCubeDepth));
					}
					if (enabledEdges.HasFlag(Edges.NegativeYNegativeZ))
					{
						Gizmos.color = new Color(0.4f, 0f, 0.3f, 0.8f);
						Gizmos.DrawCube(Vector3.Scale(halfSize, Vector3.down + Vector3.back), new Vector3(halfSize.x * 1.25f, edgeCubeDepth, edgeCubeDepth));
					}
				}
				if (canBeCornerGrabbed)
				{
					float cornerRadius = 0.03f * halfSize.magnitude;
                    if (enabledCorners.HasFlag(Corners.PositiveXPositiveYPositiveZ))
					{
						Gizmos.color = new Color(1f, 1f, 1f, 1f);
						Gizmos.DrawSphere(Vector3.Scale(halfSize, Vector3.right + Vector3.up + Vector3.forward), cornerRadius);
					}
					if (enabledCorners.HasFlag(Corners.NegativeXPositiveYPositiveZ))
					{
						Gizmos.color = new Color(0f, 1f, 1f, 1f);
						Gizmos.DrawSphere(Vector3.Scale(halfSize, Vector3.left + Vector3.up + Vector3.forward), cornerRadius);
					}
					if (enabledCorners.HasFlag(Corners.PositiveXNegativeYPositiveZ))
					{
						Gizmos.color = new Color(1f, 0f, 1f, 1);
						Gizmos.DrawSphere(Vector3.Scale(halfSize, Vector3.right + Vector3.down + Vector3.forward), cornerRadius);
					}
					if (enabledCorners.HasFlag(Corners.NegativeXNegativeYPositiveZ))
					{
						Gizmos.color = new Color(0f, 0f, 1f, 1);
						Gizmos.DrawSphere(Vector3.Scale(halfSize, Vector3.left + Vector3.down + Vector3.forward), cornerRadius);
					}
					if (enabledCorners.HasFlag(Corners.PositiveXPositiveYNegativeZ))
					{
						Gizmos.color = new Color(1f, 1f, 0f, 1);
						Gizmos.DrawSphere(Vector3.Scale(halfSize, Vector3.right + Vector3.up + Vector3.back), cornerRadius);
					}
					if (enabledCorners.HasFlag(Corners.NegativeXPositiveYNegativeZ))
					{
						Gizmos.color = new Color(0f, 1f, 0f, 1);
						Gizmos.DrawSphere(Vector3.Scale(halfSize, Vector3.left + Vector3.up + Vector3.back), cornerRadius);
					}
					if (enabledCorners.HasFlag(Corners.PositiveXNegativeYNegativeZ))
					{
						Gizmos.color = new Color(1f, 0f, 0f, 1);
						Gizmos.DrawSphere(Vector3.Scale(halfSize, Vector3.right + Vector3.down + Vector3.back), cornerRadius);
					}
					if (enabledCorners.HasFlag(Corners.NegativeXNegativeYNegativeZ))
					{
						Gizmos.color = new Color(0.1f, 0.1f, 0.1f, 1);
						Gizmos.DrawSphere(Vector3.Scale(halfSize, Vector3.left + Vector3.down + Vector3.back), cornerRadius);
					}
				}
			}
		}
    }
		[ExecuteAlways]
        [CustomEditor(typeof(BoxGrip))]
        public class GripHandles : Editor
        {

        public override void OnInspectorGUI()
        {
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            }

            EditorGUILayout.Space();

            BoxGrip behaviour = (BoxGrip)target;

            Color originalColor = GUI.backgroundColor;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
			buttonStyle.fixedWidth = 40;
			buttonStyle.fixedHeight = 30;

            GUIContent buttonContent = new GUIContent();
			buttonContent.image = EditorGUIUtility.IconContent("d_GizmosToggle On@2x").image as Texture2D;
			buttonContent.tooltip = "Toggles the Handle Gizmos for selecting which faces, edges, and corners are enabled. Each Handle will only show if the corresponding 'Can Be ____ Grabbed' bool is enabled.";

            GUILayout.Label("Toggle Grip Handles");
            if (behaviour.renderHandles)
			{
				GUI.backgroundColor = new Color (1.15f, 1.15f, 1.15f, 1f);
			}
			else
			{
				GUI.backgroundColor = new Color(0.75f, 0.75f, 0.75f, 1f);
            }

			//Button for the handles
			if (GUILayout.Button(buttonContent, buttonStyle))
			{
				behaviour.renderHandles = !behaviour.renderHandles;
			}

            GUI.backgroundColor = originalColor;

            EditorGUILayout.Space();

            DrawPropertiesExcluding(serializedObject, new string[] { "m_Script" });

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnSceneGUI()
            {
                BoxGrip boxGripRef = (BoxGrip)target;
				Transform boxTransform = boxGripRef.transform;

                if (boxGripRef._boxCollider && boxGripRef.renderGizmos && boxGripRef.renderHandles)
				{
					Vector3 halfSize = boxGripRef._boxCollider.size * 0.5f;
					Handles.matrix = Matrix4x4.TRS(boxTransform.position + boxTransform.rotation * Vector3.Scale(boxGripRef._boxCollider.center, boxTransform.lossyScale), boxTransform.rotation, Vector3.Scale(boxTransform.lossyScale, halfSize));
					Color handlesColorMult = new Color(2f, 2f, 2f, 1f);
                    Handles.matrix = Matrix4x4.TRS(boxTransform.position + boxTransform.rotation * Vector3.Scale(boxGripRef._boxCollider.center, boxTransform.lossyScale), boxTransform.rotation, Vector3.Scale(boxTransform.lossyScale, halfSize));
					if (boxGripRef.canBeFaceGrabbed)
					{
                    Handles.lighting = false;
                        //Face handles
                        float cubeSize = 0.5f;
				        Handles.color = new Color(1f, 0f, 0f, 1f) * handlesColorMult;
						if (Handles.Button(Vector3.right, Quaternion.AngleAxis(90, Vector3.up), cubeSize, cubeSize, Handles.RectangleHandleCap))
						{
							Undo.RecordObject(boxGripRef, "Change Faces");
							boxGripRef.enabledFaces ^= BoxGrip.Faces.PositiveX;
						}
						Handles.color = new Color(1f, 0f, 0.5f, 1f) * handlesColorMult;
						if (Handles.Button(Vector3.left, Quaternion.AngleAxis(90, Vector3.down), cubeSize, cubeSize, Handles.RectangleHandleCap))
						{
							Undo.RecordObject(boxGripRef, "Change Faces");
							boxGripRef.enabledFaces ^= BoxGrip.Faces.NegativeX;
						}
						Handles.color = new Color(0f, 1f, 0f, 1f) * handlesColorMult;
						if (Handles.Button(Vector3.up, Quaternion.AngleAxis(90, Vector3.right), cubeSize, cubeSize, Handles.RectangleHandleCap))
						{
							Undo.RecordObject(boxGripRef, "Change Faces");
							boxGripRef.enabledFaces ^= BoxGrip.Faces.PositiveY;
						}
						Handles.color = new Color(0f, 1f, 0.5f, 1f) * handlesColorMult;
						if (Handles.Button(Vector3.down, Quaternion.AngleAxis(90, Vector3.left), cubeSize, cubeSize, Handles.RectangleHandleCap))
						{
							Undo.RecordObject(boxGripRef, "Change Faces");
							boxGripRef.enabledFaces ^= BoxGrip.Faces.NegativeY;
						}
						Handles.color = new Color(0f, 0f, 1f, 1f) * handlesColorMult;
						if (Handles.Button(Vector3.forward, Quaternion.identity, cubeSize, cubeSize, Handles.RectangleHandleCap))
						{
							Undo.RecordObject(boxGripRef, "Change Faces");
							boxGripRef.enabledFaces ^= BoxGrip.Faces.PositiveZ;
						}
						Handles.color = new Color(0f, 0.5f, 1f, 1f) * handlesColorMult;
						if (Handles.Button(Vector3.back, Quaternion.AngleAxis(180, Vector3.up), cubeSize, cubeSize, Handles.RectangleHandleCap))
						{
							Undo.RecordObject(boxGripRef, "Change Faces");
							boxGripRef.enabledFaces ^= BoxGrip.Faces.NegativeZ;
						}
					}
                    Handles.matrix = Matrix4x4.TRS(boxTransform.position + boxTransform.rotation * Vector3.Scale(boxGripRef._boxCollider.center, boxTransform.lossyScale), boxTransform.rotation, boxTransform.lossyScale);
					if (boxGripRef.canBeEdgeGrabbed)
					{
                    //Edge handles
                    float cubeSize = 0.03f * halfSize.magnitude;
						Handles.lighting = true;
                        //XY Axis Edges
                        Handles.color = new Color(1f, 1f, 0f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.right + Vector3.up, halfSize), Quaternion.identity, cubeSize, cubeSize, Handles.CubeHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Edges");
                            boxGripRef.enabledEdges ^= BoxGrip.Edges.PositiveXPositiveY;
                        }
                        Handles.color = new Color(0f, 1f, 0f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.left + Vector3.up, halfSize), Quaternion.identity, cubeSize, cubeSize, Handles.CubeHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Edges");
                            boxGripRef.enabledEdges ^= BoxGrip.Edges.NegativeXPositiveY;
                        }
                        Handles.color = new Color(1f, 0f, 0f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.right + Vector3.down, halfSize), Quaternion.identity, cubeSize, cubeSize, Handles.CubeHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Edges");
                            boxGripRef.enabledEdges ^= BoxGrip.Edges.PositiveXNegativeY;
                        }
                        Handles.color = new Color(0f, 0.1f, 0.3f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.left + Vector3.down, halfSize), Quaternion.identity, cubeSize, cubeSize, Handles.CubeHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Edges");
                            boxGripRef.enabledEdges ^= BoxGrip.Edges.NegativeXNegativeY;
                        }
                        //XZ Axis Edges
                        Handles.color = new Color(1f, 0f, 1f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.right + Vector3.forward, halfSize), Quaternion.identity, cubeSize, cubeSize, Handles.CubeHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Edges");
                            boxGripRef.enabledEdges ^= BoxGrip.Edges.PositiveXPositiveZ;
                        }
                        Handles.color = new Color(0f, 0f, 1f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.left + Vector3.forward, halfSize), Quaternion.identity, cubeSize, cubeSize, Handles.CubeHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Edges");
                            boxGripRef.enabledEdges ^= BoxGrip.Edges.NegativeXPositiveZ;
                        }
                        Handles.color = new Color(1f, 0f, 0f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.right + Vector3.back, halfSize), Quaternion.identity, cubeSize, cubeSize, Handles.CubeHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Edges");
                            boxGripRef.enabledEdges ^= BoxGrip.Edges.PositiveXNegativeZ;
                        }
                        Handles.color = new Color(0f, 0.5f, 0.35f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.left + Vector3.back, halfSize), Quaternion.identity, cubeSize, cubeSize, Handles.CubeHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Edges");
                            boxGripRef.enabledEdges ^= BoxGrip.Edges.NegativeXNegativeZ;
                        }
                        //YZ Axis Edges
                        Handles.color = new Color(0f, 1f, 1f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.up + Vector3.forward, halfSize), Quaternion.identity, cubeSize, cubeSize, Handles.CubeHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Edges");
                            boxGripRef.enabledEdges ^= BoxGrip.Edges.PositiveYPositiveZ;
                        }
                        Handles.color = new Color(0f, 0f, 1f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.down + Vector3.forward, halfSize), Quaternion.identity, cubeSize, cubeSize, Handles.CubeHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Edges");
                            boxGripRef.enabledEdges ^= BoxGrip.Edges.NegativeYPositiveZ;
                        }
                        Handles.color = new Color(0f, 1f, 0f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.up + Vector3.back, halfSize), Quaternion.identity, cubeSize, cubeSize, Handles.CubeHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Edges");
                            boxGripRef.enabledEdges ^= BoxGrip.Edges.PositiveYNegativeZ;
                        }
                        Handles.color = new Color(0.4f, 0f, 0.3f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.down + Vector3.back, halfSize), Quaternion.identity, cubeSize, cubeSize, Handles.CubeHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Edges");
                            boxGripRef.enabledEdges ^= BoxGrip.Edges.NegativeYNegativeZ;
                        }
                    }
                    if (boxGripRef.canBeCornerGrabbed)
					{
                        Handles.lighting = true;
                        float cornerRadius = 0.035f * halfSize.magnitude;
                        //Corner handles
                        Handles.color = new Color(1f, 1f, 1f, 1f) * handlesColorMult;
						if (Handles.Button(Vector3.Scale(Vector3.right + Vector3.up + Vector3.forward, halfSize), Quaternion.identity, cornerRadius, cornerRadius, Handles.SphereHandleCap))
						{
							Undo.RecordObject(boxGripRef, "Change Corners");
							boxGripRef.enabledCorners ^= BoxGrip.Corners.PositiveXPositiveYPositiveZ;
						}
                        Handles.color = new Color(0f, 1f, 1f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.left + Vector3.up + Vector3.forward, halfSize), Quaternion.identity, cornerRadius, cornerRadius, Handles.SphereHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Corners");
                            boxGripRef.enabledCorners ^= BoxGrip.Corners.NegativeXPositiveYPositiveZ;
                        }
                        Handles.color = new Color(1f, 0f, 1f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.right + Vector3.down + Vector3.forward, halfSize), Quaternion.identity, cornerRadius, cornerRadius, Handles.SphereHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Corners");
                            boxGripRef.enabledCorners ^= BoxGrip.Corners.PositiveXNegativeYPositiveZ;
                        }
                        Handles.color = new Color(0f, 0f, 1f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.left + Vector3.down + Vector3.forward, halfSize), Quaternion.identity, cornerRadius, cornerRadius, Handles.SphereHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Corners");
                            boxGripRef.enabledCorners ^= BoxGrip.Corners.NegativeXNegativeYPositiveZ;
                        }
                        Handles.color = new Color(1f, 1f, 0f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.right + Vector3.up + Vector3.back, halfSize), Quaternion.identity, cornerRadius, cornerRadius, Handles.SphereHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Corners");
                            boxGripRef.enabledCorners ^= BoxGrip.Corners.PositiveXPositiveYNegativeZ;
                        }
                        Handles.color = new Color(0f, 1f, 0f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.left + Vector3.up + Vector3.back, halfSize), Quaternion.identity, cornerRadius, cornerRadius, Handles.SphereHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Corners");
                            boxGripRef.enabledCorners ^= BoxGrip.Corners.NegativeXPositiveYNegativeZ;
                        }
                        Handles.color = new Color(1f, 0f, 0f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.right + Vector3.down + Vector3.back, halfSize), Quaternion.identity, cornerRadius, cornerRadius, Handles.SphereHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Corners");
                            boxGripRef.enabledCorners ^= BoxGrip.Corners.PositiveXNegativeYNegativeZ;
                        }
                        Handles.color = new Color(0.25f, 0.25f, 0.25f, 1f) * handlesColorMult;
                        if (Handles.Button(Vector3.Scale(Vector3.left + Vector3.down + Vector3.back, halfSize), Quaternion.identity, cornerRadius, cornerRadius, Handles.SphereHandleCap))
                        {
							Undo.RecordObject(boxGripRef, "Change Corners");
                            boxGripRef.enabledCorners ^= BoxGrip.Corners.NegativeXNegativeYNegativeZ;
                        }
                    }
                }
            }
#endif

        public bool CheckZones(Hand hand)
		{
			return false;
		}
	}
}
