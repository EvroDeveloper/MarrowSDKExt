using System;
using SLZ.Marrow.Circuits;
using UnityEngine;

namespace SLZ.Marrow.Redacted
{
	public class Battery : Circuit, IPlugable
	{
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

		public void OnSocketAttached(Socket socket)
		{
		}

		public void OnSocketDetached(Socket socket)
		{
		}

		public Battery()
		{
		}

		[SerializeField]
		private Plug _plug;

		private BatteryHolder _batteryHolder;

		[SerializeField]
		private Circuit _input;
	}
}
