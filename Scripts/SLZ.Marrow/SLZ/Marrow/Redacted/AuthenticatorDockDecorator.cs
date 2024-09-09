using System;
using SLZ.Marrow.Circuits;
using SLZ.Marrow.Warehouse;
using SLZ.Marrow.Zones;
using UnityEngine;

namespace SLZ.Marrow.Redacted
{
	public class AuthenticatorDockDecorator : ExternalCircuit, ISpawnListenable
	{
		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		public void OnSpawn(GameObject go)
		{
		}

		public void OnDespawn(GameObject go)
		{
		}

		public AuthenticatorDockDecorator()
		{
		}

		[SerializeField]
		protected CrateSpawner _crateSpawner;

		[SerializeField]
		private MarrowQuery _authenticationCodes;

		[SerializeField]
		private bool _doBindWhenAuthenticated;

		[SerializeField]
		private bool _doEjectWhenDeauthenticated;

		[SerializeField]
		private Renderer[] _renderers;

		[SerializeField]
		private int[] _materialIndicies;

		[SerializeField]
		private Material _offMaterial;

		[SerializeField]
		private Material _onMaterial;

		[SerializeField]
		private Material _successMaterial;

		[SerializeField]
		private Material _failMaterial;

		[SerializeField]
		private Collider[] _collidersToIgnore;
	}
}
