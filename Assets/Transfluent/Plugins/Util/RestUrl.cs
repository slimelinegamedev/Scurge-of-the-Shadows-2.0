﻿using System;

namespace transfluent
{
	public class RestUrl
	{
		//public enum TransfluentMethodType { Hello, Authenticate, Register, Languages, Text, Texts, TextStatus,
		//TextsTranslate, TextWordCount, CombinedTexts_Send, CombinedTexts_Translate };
		private static string baseServiceUrl = "https://transfluent.com/v2/";

		public static Route GetRouteAttribute(Type callToGetUrlFor)
		{
			object[] routeable = callToGetUrlFor.GetCustomAttributes(typeof(Route), true);
			if(routeable.Length == 0)
			{
				throw new Exception("tried to get a url from a non-routable object:" + callToGetUrlFor.GetType());
			}
			return (routeable[0] as Route);
		}

		public static string GetURL(ITransfluentParameters callToGetUrlFor)
		{
			return baseServiceUrl + GetRouteAttribute(callToGetUrlFor.GetType()).route;
		}
	}
}