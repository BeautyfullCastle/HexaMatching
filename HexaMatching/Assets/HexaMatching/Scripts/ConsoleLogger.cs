using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class ConsoleLogger
{
	static ConsoleLogger()
	{
#if !DEBUG_CONSOLE
		Debug.unityLogger.logEnabled = false;
#endif
	}
}