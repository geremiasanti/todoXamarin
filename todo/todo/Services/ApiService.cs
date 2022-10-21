using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace todo.Services
{
    class ApiService
    {
        /* Permette di richiamare un API con metodo GET */
        public async Task<String> CallGetApi(string url, string token)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(10000);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var uri = new Uri(string.Format(url, string.Empty));

            HttpResponseMessage response = null;
            response = await client.GetAsync(uri);
            string resp = response.Content.ReadAsStringAsync().Result;

            return resp;
        }

        public async Task<String> CallPostApiNoAuth(string url, Dictionary<string, string> postParameters)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(10000);

            var uri = new Uri(string.Format(url, string.Empty));
            FormUrlEncodedContent formContent = null;
            if (postParameters != null)
            {
                formContent = new FormUrlEncodedContent(postParameters);
            }

            HttpResponseMessage response = null;
            response = await client.PostAsync(uri, formContent);
            string resp = response.Content.ReadAsStringAsync().Result;

            return resp;
        }


        /* Permette di richiamare un API con metodo POST */
        public async Task<String> CallPostApi(string url, string token, Dictionary<string, string> postParameters)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(10000);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var uri = new Uri(string.Format(url, string.Empty));
            FormUrlEncodedContent formContent = null;
            if (postParameters != null)
            {
                formContent = new FormUrlEncodedContent(postParameters);
            }

            HttpResponseMessage response = null;
            response = await client.PostAsync(uri, formContent);
            string resp = response.Content.ReadAsStringAsync().Result;

            return resp;
        }

        /* Permette di richiamare un API con metodo POST */
        public async Task<String> CallPostApiByObject(string url, string token, String DataToSendJsonString)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(10000);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var uri = new Uri(string.Format(url, string.Empty));
            var jsonContent = new StringContent(DataToSendJsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await client.PostAsync(uri, jsonContent);
            string resp = response.Content.ReadAsStringAsync().Result;

            return resp;
        }

        /* Permette di richiamare un API con metodo POST con auth base */
        public async Task<String> CallPostApiWithBasicAuth(string url, string username, string password, String DataToSendJsonString)
        {

            // Create an HTTP web request using the URL:
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(10000);

            string tokenEncoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", tokenEncoded);

            var uri = new Uri(string.Format(url, string.Empty));
            var jsonContent = new StringContent(DataToSendJsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await client.PostAsync(uri, jsonContent);
            string resp = response.Content.ReadAsStringAsync().Result;

            return resp;
        }

        /* Permette di richiamare un API con metodo POST */
        public async Task<String> CallPostWithoutTokenApiByObject(string url, String DataToSendJsonString)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(10000);

            var uri = new Uri(string.Format(url, string.Empty));
            var jsonContent = new StringContent(DataToSendJsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            response = await client.PostAsync(uri, jsonContent);
            string resp = response.Content.ReadAsStringAsync().Result;

            return resp;
        }


        /* Permette di richiamare un API con metodo PUT */
        public async Task<String> CallPutApi(string url, string token, Dictionary<string, string> putParameters)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(10000);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var uri = new Uri(string.Format(url, string.Empty));
            FormUrlEncodedContent formContent = null;
            if (putParameters != null)
            {
                formContent = new FormUrlEncodedContent(putParameters);
            }

            HttpResponseMessage response = null;
            response = await client.PutAsync(uri, formContent);
            string resp = response.Content.ReadAsStringAsync().Result;

            return resp;
        }

        /* Permette di richiamare un API con metodo PUT  */
        public async Task<String> CallPutApiByObject(string url, string token, String DataToSendJsonString)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMilliseconds(10000);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var uri = new Uri(string.Format(url, string.Empty));
                var jsonContent = new StringContent(DataToSendJsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PutAsync(uri, jsonContent);
                string resp = response.Content.ReadAsStringAsync().Result;

                return resp;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        /* Permette di richiamare un API con metodo DELETE */
        public async Task<String> CallDeleteApi(string url, string token)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(10000);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var uri = new Uri(string.Format(url, string.Empty));

            HttpResponseMessage response = null;
            response = await client.DeleteAsync(uri);
            string resp = response.Content.ReadAsStringAsync().Result;

            return resp;
        }


        /* Permette di richiamare un API con metodo POST allegando un immagine */
        public async Task<String> CallMultipartImagePostApi(string url, string token, byte[] image)
        {
            string resp = null;
            var fileContent = new ByteArrayContent(image);
            fileContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("multipart/form-data");
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                FileName = DateTime.Now.Ticks + ".jpg"
            };

            string boundary = "---8d0f01e6b3b5dafaaadaad";
            MultipartFormDataContent multipartContent = new MultipartFormDataContent(boundary);
            multipartContent.Add(fileContent);

            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(1);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.PostAsync(url, multipartContent);
            if (response.IsSuccessStatusCode)
            {
                resp = await response.Content.ReadAsStringAsync();
            }
            return resp;
        }

        public async Task<String> CallGetApiNoAuth(string url)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(10000);

            var uri = new Uri(string.Format(url, string.Empty));

            HttpResponseMessage response = null;
            response = await client.GetAsync(uri);
            string resp = response.Content.ReadAsStringAsync().Result;

            return resp;
        }
    }
}
