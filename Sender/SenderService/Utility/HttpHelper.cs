﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SenderService.Utility
{
    public class HttpHelper
    {
        public static async Task<HttpResponseMessage> SendRequest(HttpMethod method, string endPoint, string accessToken = null, dynamic content = null, string contentType = null)
        {
            HttpResponseMessage response = null;
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(method, endPoint))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (!string.IsNullOrEmpty(accessToken))
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    if (content != null)
                    {
                        string c;
                        if (content is string)
                            c = content;
                        else
                            c = JsonConvert.SerializeObject(content);
                        request.Content = new StringContent(c, Encoding.UTF8, string.IsNullOrEmpty(contentType)? "application/json": contentType);
                    }

                    response = await client.SendAsync(request).ConfigureAwait(false);
                }
            }
            return response;
        }

    }
}
