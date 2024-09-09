using System;

namespace SLZ.Marrow.Redacted
{
	public interface ISocketable
	{
		void OnPlugAttached(Plug plug);

		void OnPlugDetached(Plug plug);
	}
}
