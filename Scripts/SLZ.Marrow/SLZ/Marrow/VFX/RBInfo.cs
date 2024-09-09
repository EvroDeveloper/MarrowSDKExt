using System;
using UnityEngine;

namespace SLZ.Marrow.VFX
{
	public struct RBInfo
	{
		public Vector3 worldCenterOfMass;

		public Vector3 velocity;

		public Vector3 angularVelocity;

		public Bounds bounds;

		public Collider[] Colliders;
	}
}
