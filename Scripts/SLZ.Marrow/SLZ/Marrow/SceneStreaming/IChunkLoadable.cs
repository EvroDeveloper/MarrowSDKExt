using System;

namespace SLZ.Marrow.SceneStreaming
{
	public interface IChunkLoadable
	{
		void OnChunkLoad();

		void OnChunkUnload();
	}
}
