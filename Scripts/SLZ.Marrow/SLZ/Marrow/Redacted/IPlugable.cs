using System;

namespace SLZ.Marrow.Redacted
{
	public interface IPlugable
	{
		void OnSocketAttached(Socket socket);

		void OnSocketDetached(Socket socket);
	}
}
