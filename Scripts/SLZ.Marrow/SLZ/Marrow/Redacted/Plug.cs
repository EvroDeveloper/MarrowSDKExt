using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLZ.Marrow.Interaction;
using SLZ.Marrow.Warehouse;
using UnityEngine;

namespace SLZ.Marrow.Redacted
{
	public class Plug : Tracker, ITaggable
	{
		public TagList Tags
		{
			get
			{
				return null;
			}
		}

		public bool IsAttached
		{
			[CompilerGenerated]
			get
			{
				return default(bool);
			}
			[CompilerGenerated]
			private set
			{
			}
		}

		public bool IsBound
		{
			[CompilerGenerated]
			get
			{
				return default(bool);
			}
			[CompilerGenerated]
			private set
			{
			}
		}

		public void Register(IPlugable socketable)
		{
		}

		public void Unregister(IPlugable socketable)
		{
		}

		private void OnHandAttached(InteractableHost host, Hand hand)
		{
		}

		internal void OnSocketHoverEnter(Socket socket)
		{
		}

		internal void OnSocketHoverExit(Socket socket)
		{
		}

		private void FixedUpdate()
		{
		}

		private Socket Validate()
		{
			return null;
		}

		private void Attach(Socket socket)
		{
		}

		private void MakeJoint(MarrowBody bodyB)
		{
		}

		private void DestroyJoint()
		{
		}

		public void Detach()
		{
		}

		internal void OnBind()
		{
		}

		internal void OnUnbind()
		{
		}

		public Plug()
		{
		}

		[SerializeField]
		private TagList _interfaceTags;

		[SerializeField]
		private InteractableHost _host;

		private readonly List<Socket> _sockets;

		private Socket _attachedSocket;

		private List<IPlugable> _plugables;

		private Socket _lastDetachedSocket;

		private MarrowJoint _joint;

		private static float _minAttachDistSqr;

		private static float _maxAttachDistSqr;
	}
}
