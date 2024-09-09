using System;
using System.Collections;
using System.Runtime.CompilerServices;
using SLZ.Marrow.Circuits;
using SLZ.Marrow.Warehouse;
using UltEvents;
using UnityEngine;

namespace SLZ.Marrow.Redacted
{
	[RequireComponent(typeof(Socket))]
	public class AuthenticatorDock : Circuit, ISocketable
	{
		public bool IsAuthenticated
		{
			[CompilerGenerated]
			private get
			{
				return default(bool);
			}
			[CompilerGenerated]
			set
			{
			}
		}

		public Circuit input
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		public void OnPlugAttached(Plug plug)
		{
		}

		public void OnPlugDetached(Plug plug)
		{
		}

		private IEnumerator CoAuthenticate()
		{
			return null;
		}

		private IEnumerator CoDeauthenticate()
		{
			return null;
		}

		private void SetMaterial(Material material)
		{
		}

		public void SetStaticReferences(Renderer[] renderers, int[] materialIndicies, Material onMaterial, Material offMaterial, Material successMaterial, Material failMaterial, Collider[] collidersToIgnore)
		{
		}

		private float InternalRead()
		{
			return 0f;
		}

		public AuthenticatorDock()
		{
		}

		public MarrowQuery authenticationCodes;

		public bool doBindWhenAuthenticated;

		public bool doEjectWhenDeauthenticated;

		[Space(15f)]
		public AuthenticatorDock.AuthenticatorEventCallback onAuthenticated;

		[Space(15f)]
		public AuthenticatorDock.AuthenticatorEventCallback onDeauthenticated;

		[SerializeField]
		private Circuit _input;

		[SerializeField]
		private Socket _socket;

		[SerializeField]
		private Renderer[] _renderers;

		[SerializeField]
		private int[] _materialIndicies;

		[SerializeField]
		private AudioClip[] _authenticatedClips;

		[SerializeField]
		private AudioClip[] _deauthenticatedClips;

		[SerializeField]
		private Material _offMaterial;

		[SerializeField]
		private Material _onMaterial;

		[SerializeField]
		private Material _successMaterial;

		[SerializeField]
		private Material _failMaterial;

		private Authenticator _authInSocket;

		private Coroutine _coAnimateAuthenticate;

		[Serializable]
		public class AuthenticatorEventCallback : UltEvent<Authenticator>
		{
			public AuthenticatorEventCallback()
			{
			}
		}
	}
}
