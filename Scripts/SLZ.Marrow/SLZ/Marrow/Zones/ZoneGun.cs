using System;
using SLZ.Marrow.Audio;
using SLZ.Marrow.Interaction;
using TMPro;
using UnityEngine;

namespace SLZ.Marrow.Zones
{
	public class ZoneGun : MonoBehaviour
	{
		private void Awake()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void OnHandAttached(Hand hand)
		{
		}

		private void OnHandDetached(Hand hand)
		{
		}

		private void OnHandAttachedUpdate(Hand hand)
		{
		}

		private void ShowZoneLinkInfo(ZoneLink curZoneLink)
		{
		}

		public ZoneGun()
		{
		}

		public TargetGrip triggerGrip;

		public Transform firePoint;

		public float range;

		public TextMeshPro mainUIPanel;

		public GameObject laserPointer;

		public AudioClip[] chargeSFX;

		public AudioClip[] modeSFX;

		private ManagedAudioPlayer _mapCharge;

		private ManagedAudioPlayer _mapMode;

		private ZoneGunMode _gunMode;

		private GameObject _probeRoot;

		private ZoneCullManager zcm;

		private MarrowEntity[] allEntities;

		private int totalEntities;

		private LayerMask layerZoneCollideAllowed;
	}
}
