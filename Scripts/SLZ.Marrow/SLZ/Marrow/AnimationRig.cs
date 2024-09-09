using System;
using System.Runtime.InteropServices;
using SLZ.Marrow.Utilities;
using SLZ.VRMK;
using UnityEngine;
using UnityEngine.Serialization;

namespace SLZ.Marrow
{
	public class AnimationRig : HeptaRig
	{
		public SLZ_Body body
		{
			get
			{
				return null;
			}
		}

		public SimpleTransform pelvisLocalAnimOff
		{
			get
			{
				return default(SimpleTransform);
			}
		}

		public float suppressChestVelocityTwist
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public override void OnStart()
		{
		}

		public override void OnRigEnable(bool reEnable)
		{
		}

		public override void OnEarlyUpdate()
		{
		}

		private void OnFootSolve(Rig inRig, float deltaTime)
		{
		}

		private void UpdateHeptaBody2(Rig inRig, float deltaTime, Vector2 velocity, Vector2 accel)
		{
		}

		private void ReapplyTorsoProximity(Rig inRig, float deltaTime)
		{
		}

		public override void Teleport(SimpleTransform displace, bool zeroVelocity = false)
		{
		}

		public override void SetAvatar(SLZ.VRMK.Avatar avatar)
		{
		}

		protected Vector3 BodyVelocity(Rig inRig, Vector2 vel, Vector2 accel, float deltaTime)
		{
			return default(Vector3);
		}

		private SimpleTransform AdditiveChest(SLZ.VRMK.Avatar avatar, SimpleTransform head, SimpleTransform handLf, SimpleTransform handRt, Rig inRig, float deltaTime, [Out] SimpleTransform outNeck, float mantle = 0f, float footSupported = 1f, float handSupported = 0f, float physKneeOffset = 0f)
		{
			return default(SimpleTransform);
		}

		private float SolveT1UpOffset(float heightWeight, float mantle, float footSupported, float handSupported, float velocityInLegs)
		{
			return 0f;
		}

		protected SimpleTransform AdditivePelvis(SLZ.VRMK.Avatar avatar, SimpleTransform chest, Rig inRig, float deltaTime, [Out] SimpleTransform outSpine, Quaternion pelvisLocalAnimOff, float mantle = 0f, float footSupported = 1f, float handSupported = 0f, float physKneeOffset = 0f)
		{
			return default(SimpleTransform);
		}

		protected float SolveSacrumUpOffset(float heightWeight, float mantle, float footSupported, float handSupported, float velocityInLegs)
		{
			return 0f;
		}

		public override void SetBodyState(RigManager.BodyState toBodyState, RigManager.BodyState fromBodyState)
		{
		}

		public AnimationRig()
		{
		}

		[Header("AnimationRig")]
		public float spineCrouchOffset;

		[SerializeField]
		private AnimationCurve _t1UpOffsetCurveVelocity;

		[SerializeField]
		private AnimationCurve _t1UpOffsetCurveClimb;

		[SerializeField]
		private AnimationCurve _t1UpOffsetCurveMantle;

		[SerializeField]
		private AnimationCurve _sacrumUpOffsetCurveVelocity;

		[SerializeField]
		private AnimationCurve _sacrumUpOffsetCurveClimb;

		[SerializeField]
		private AnimationCurve _sacrumUpOffsetCurveMantle;

		[FormerlySerializedAs("SwingCounterCurve")]
		public AnimationCurve SwingCounterCurveSeated;

		public AnimationCurve SwingCounterCurveRagdoll;

		private float _lfCurlLerp;

		private float _rtCurlLerp;

		private Vector3 _dragUp;

		[SerializeField]
		private SLZ_Body _body;

		private SimpleTransform _pelvisLocalAnimOff;

		private float _t1ToVelLerp;

		private float _t1ToVelVel;

		private float _sacrumToVelLerp;

		private float _sacrumToVelVel;

		private float _t1UpOffsetLerp;

		private float _t1UpOffsetVel;

		private float _sacrumUpOffsetLerp;

		private float _sacrumUpOffsetVel;

		private float _currentSwing;

		private float _lastInHeadSwing;

		private Vector2 _lastT1FwdV2;

		private Vector2 _lastSacrumFwdV2;

		protected float _suppressChestVelocityTwist;

		private bool _suppressionDirty;

		private bool _suppressionFreshlyFilthy;

		private float _sampleBlendWeight;

		private float _sampleBlendWeightV;

		private float _blendToSampleTime;

		private Quaternion[] _stateBlendSample;
	}
}
