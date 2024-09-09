using System;
using Cysharp.Threading.Tasks;
using SLZ.Marrow.Data;
using UnityEngine;

namespace SLZ.Marrow
{
	[Obsolete("Deprecated. Use the new plug and socket system instead.")]
	public class AmmoSocket : Socket
	{
		public override bool IsClearOnInsert
		{
			get
			{
				return default(bool);
			}
		}

		public bool HasMagazine
		{
			get
			{
				return default(bool);
			}
		}

		protected override void Awake()
		{
		}

		public InteractableHost GetHost()
		{
			return null;
		}

		private void OnPlugLocked(Plug plug)
		{
		}

		private void OnPlugUnlocked()
		{
		}

		protected override void OnPlugEnter(Plug plug)
		{
		}

		protected override void OnPlugExit(Plug plug)
		{
		}

		private void UpdateProxyGripState(Hand hand)
		{
		}

		public void EjectMagazine()
		{
		}

		private void UpdateMagGripPriority(int ammoCount)
		{
		}

		private void OnAttachedHandDelegate(InteractableHost host, Hand hand)
		{
		}

		private void OnDetachedHandDelegate(InteractableHost host, Hand hand)
		{
		}

		public UniTask ForceLoadAsync(MagazineData magazineData)
		{
			return default(UniTask);
		}

		public override void OnDespawn()
		{
		}

		public AmmoSocket()
		{
		}

		[Header("Magazine Socket")]
		public string platform;

		[Header("References")]
		public Grip primaryGrip;

		public Gun gun;

		public bool despawnOnInsert;

		private bool _isHostGrabbed;

		private bool _isMagazineInserted;

		private bool _isProxyGripState;

		private AmmoPlug _magazinePlug;

		public Action<Gun> onUpdateProxyGripState;
	}
}
