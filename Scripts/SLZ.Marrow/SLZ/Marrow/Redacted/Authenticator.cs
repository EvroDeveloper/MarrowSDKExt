using System;
using SLZ.Marrow.Warehouse;
using UnityEngine;

namespace SLZ.Marrow.Redacted
{
	public class Authenticator : MonoBehaviour, IPlugable, ITaggable
	{
		public TagList Tags
		{
			get
			{
				return null;
			}
		}

		public Plug Plug
		{
			get
			{
				return null;
			}
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		public void OnSocketAttached(Socket socket)
		{
		}

		public void OnSocketDetached(Socket socket)
		{
		}

		internal void OnAuthenticated(AuthenticatorDock dock)
		{
		}

		internal void OnDeauthenticated(AuthenticatorDock dock)
		{
		}

		public Authenticator()
		{
		}

		[SerializeField]
		private TagList _authCodeTag;

		[SerializeField]
		private Plug _plug;
	}
}
