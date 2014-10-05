﻿namespace transfluent
{
	[Route("hello", RestRequestType.GET, "http://transfluent.com/backend-api/#Hello")]
	public class Hello : WebServiceParameters
	{
		public Hello(string name)
		{
			getParameters.Add("name", name);
		}

		public string Parse(string text)
		{
			return GetResponse<string>(text);
		}
	}
}