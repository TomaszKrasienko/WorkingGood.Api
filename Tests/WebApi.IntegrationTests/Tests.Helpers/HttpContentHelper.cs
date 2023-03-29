using System;
using Newtonsoft.Json;
using System.Text;

namespace WebApi.IntegrationTests.Tests.Helpers
{
	public static class HttpContentHelper
	{
		public static HttpContent ToJsonContent(this object obj)
		{
            var json = JsonConvert.SerializeObject(obj);
            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            return httpContent;
        }
	}
}

