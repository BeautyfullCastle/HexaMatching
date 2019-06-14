using UnityEngine.UI;

namespace HexaMatching
{
	public static class Extensions
	{
		public static void SetText(this Text text, string str, params object[] args)
		{
			text.text = string.Format(str, args);
		}

		public static void SetText(this Text text, string str)
		{
			text.text = str;
		}

		public static void Clear(this Text text)
		{
			text.text = string.Empty;
		}
	}
}