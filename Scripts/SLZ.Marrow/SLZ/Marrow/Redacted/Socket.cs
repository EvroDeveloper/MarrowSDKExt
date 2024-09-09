using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLZ.Marrow.Interaction;
using SLZ.Marrow.Warehouse;
using SLZ.Marrow.Zones;
using UltEvents;
using UnityEngine;

namespace SLZ.Marrow.Redacted
{
	public class Socket : Zone, IZoneTrackerListenable
	{
		public MarrowBody Body
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

		private void OnEnable()
		{
		}


		public void Register(ISocketable socketable)
		{
		}

		public void Unregister(ISocketable socketable)
		{
		}

		public void OnZoneTrackerEnter(Tracker tracker)
		{
		}

		public void OnZoneTrackerExit(Tracker tracker)
		{
		}

		internal void OnPlugAttached(Plug plug)
		{
		}

		internal void OnPlugDetached(Plug plug)
		{
		}

		public void Bind()
		{
		}

		public void Unbind()
		{
		}

		public void Detach()
		{
		}

		internal void SetStaticReferences(Collider[] collidersToIgnore)
		{
		}

		public Socket()
		{
		}

		[SerializeField]
		private MarrowQuery _interfaceTags;

		[SerializeField]
		private bool _isAutobindOnAttached;

		[SerializeField]
		private AudioClip[] _attachedClips;

		[SerializeField]
		private AudioClip[] _detachedClips;

		[SerializeField]
		private AudioClip[] _boundClips;

		[SerializeField]
		private AudioClip[] _unboundClips;

		[Space(15f)]
		public Socket.PlugEventCallback onPlugAttached;

		[Space(15f)]
		public Socket.PlugEventCallback onPlugDetached;

		[Space(15f)]
		public Socket.PlugEventCallback onPlugBound;

		[Space(15f)]
		public Socket.PlugEventCallback onPlugUnbound;

		[SerializeField]
		private MarrowBody _body;

		[SerializeField]
		private Collider[] _collidersToIgnore;

		private List<ISocketable> _socketables;

		private Plug _plugInSocket;

		[Serializable]
		public class PlugEventCallback : UltEvent<Plug>
		{
			public PlugEventCallback()
			{
			}
		}
	}
}
