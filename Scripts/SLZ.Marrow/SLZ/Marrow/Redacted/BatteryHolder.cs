using System;
using SLZ.Marrow.Circuits;
using UnityEngine;

namespace SLZ.Marrow.Redacted
{
	public class BatteryHolder : Circuit, ISocketable
	{
		public Socket Socket
		{
			get
			{
				return null;
			}
		}

		public void OnPlugAttached(Plug plug)
		{
		}

		public void OnPlugDetached(Plug plug)
		{
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

		public BatteryHolder()
		{
		}

		private Circuit _input;

		[SerializeField]
		private Socket _socket;

		private Battery _batInHolder;
	}
}
