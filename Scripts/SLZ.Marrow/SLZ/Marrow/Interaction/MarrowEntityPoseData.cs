using System;
using UnityEngine;

namespace SLZ.Marrow.Interaction
{
	[Obsolete("Use EntityPose DataCard and MarrowEntityPoseDecorator instead")]
	public class MarrowEntityPoseData : ScriptableObject
	{
		public MarrowEntityPoseData()
		{
		}

		[SerializeField]
		public MarrowEntityPose data;
	}
}
