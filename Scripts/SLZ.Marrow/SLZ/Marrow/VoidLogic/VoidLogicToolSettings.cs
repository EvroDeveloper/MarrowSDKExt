using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SLZ.Marrow.VoidLogic
{
	[CreateAssetMenu(fileName = "VoidLogicToolSettings.asset", menuName = "StressLevelZero/VoidLogic/VoidLogic Tool Settings")]
	public class VoidLogicToolSettings : ScriptableObject
	{
		public Material LineMaterial
		{
			[CompilerGenerated]
			get
			{
				return null;
			}
			[CompilerGenerated]
			internal set
			{
			}
		}

		public Material IconMaterial
		{
			[CompilerGenerated]
			get
			{
				return null;
			}
			[CompilerGenerated]
			internal set
			{
			}
		}

		public VoidLogicIcon[] Icons
		{
			[CompilerGenerated]
			get
			{
				return null;
			}
			[CompilerGenerated]
			internal set
			{
			}
		}

		public void OnEnable()
		{
		}

		public bool TryGetIcon(Type type, [Out] VoidLogicIcon icon)
		{
			return default(bool);
		}

		public VoidLogicIcon GetIcon(Type type)
		{
			return null;
		}

		public bool TryGetIcon(string iconName, [Out] VoidLogicIcon icon)
		{
			return default(bool);
		}

		public VoidLogicIcon GetIcon(string iconName)
		{
			return null;
		}

		public VoidLogicToolSettings()
		{
		}

		private Dictionary<string, VoidLogicIcon> _type2Icon;

		private Dictionary<string, VoidLogicIcon> _name2Icon;
	}
}
