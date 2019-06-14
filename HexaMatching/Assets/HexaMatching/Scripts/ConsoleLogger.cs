using UnityEngine;

namespace HexaMatching
{
	public class ConsoleLogger
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void Init()
		{
#if !DEBUG_CONSOLE
			Debug.unityLogger.logEnabled = false;
#endif
		}
	}
}