using System;
using System.Runtime.InteropServices;
using SLZ.Marrow.Pool;
using SLZ.Marrow.Utilities;
using SLZ.Marrow.VFX;
using UnityEngine;

namespace SLZ.Marrow
{
	public class ParticleTint : SpawnEvents
	{
		public static ComponentCache<ParticleTint> Cache
		{
			get
			{
				return null;
			}
		}

		protected override void Reset()
		{
		}

		public override void OnPoolInitialize()
		{
		}

		public void PlaySystems()
		{
		}

		public void PlaySFX()
		{
		}

		public void TintParticles(Color TintColor)
		{
		}

		public void SetTargetMesh(Mesh mesh)
		{
		}

		public void SetTargetMesh(Mesh mesh, Vector3 scale)
		{
		}

		public void SetTargetMeshRenderer(MeshRenderer meshRenderer)
		{
		}

		public void SetTargetSkinnedMesh(SkinnedMeshRenderer meshRenderer)
		{
		}

		public void ScaleParticleSphere(float Radius)
		{
		}

		public void SetParticleBurstCount(int count)
		{
		}

		public void ScaleParticleBurstCount(float scale)
		{
		}

		public void ResetParticleBurstCount()
		{
		}

		public void SetMeshAndPlay(Color TintColor, SkinnedMeshRenderer mesh, float volumeSize, Vector3 scale, [Optional] Vector3? velocity)
		{
		}

		public void SetForceField(ParticleSystemForceField forceField, float lifeTime)
		{
		}

		private static Vector3 RandomPointInTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
		{
			return default(Vector3);
		}

		public void SetMeshAndPlay(Color TintColor, Mesh mesh, float volumeSize, Vector3 scale, [Optional] Vector3? velocity, [Optional] Vector3? angularVelocity, [Optional] Vector3? centerOfMass)
		{
		}

		public void SetParticlesFromBodiesAndPlay(Color tintColor, RBInfo[] bodies, Vector3 scale)
		{
		}

		private void EmitParticleWithVelocity(ParticleSystem particleSystem, ParticleSystem.EmitParams emitParams, RBInfo rbInfo)
		{
		}

		private Vector3 AngularVelocityFromRigidbodyInfo(Vector3 particlePosition, RBInfo rbInfo)
		{
			return default(Vector3);
		}

		private float GetVolumeSurface(Vector3 size)
		{
			return 0f;
		}

		private float CalculateEllipsoidSurfaceArea(Vector3 axes)
		{
			return 0f;
		}

		private Vector3 RandomPointInBounds(Bounds bounds)
		{
			return default(Vector3);
		}

		private Vector3 RandomPointInBox(Matrix4x4 boxMatrixTrans)
		{
			return default(Vector3);
		}

		private Vector3 RandomPointInSphere(SphereCollider sphereCollider)
		{
			return default(Vector3);
		}

		private Vector3 RandomPointInEllipsoid(Matrix4x4 capsMatrixTrans)
		{
			return default(Vector3);
		}

		private float GetVolume(Vector3 size)
		{
			return 0f;
		}

		protected override void Awake()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDestroy()
		{
		}

		public ParticleTint()
		{
		}

		private static ComponentCache<ParticleTint> _cache;

		[Space]
		[SerializeField]
		[Header("This is used to tint particles when they are spawned in")]
		private ParticleSystem[] ParticleSystems;

		[SerializeField]
		private Renderer[] Renderers;

		[SerializeField]
		private AudioSource audioSource;

		private int[] burstCount;

		private ParticleSystem.MinMaxCurve[] _startMinMaxCurves;
	}
}
