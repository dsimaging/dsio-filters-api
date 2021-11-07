using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using DSIO.Filters.Api.Sdk.Types.V1;

namespace DSIO.Filters.Api.Sdk.Client.V1
{
    /// <summary>
    /// A class that wraps <see cref="HttpClient" /> and provides async
    /// methods for sending API requests and receiving responses.
    /// </summary>
    public class ServiceProxy
    {
        public ServiceProxy()
        {
            Client = new HttpClient()
            {
                // Initialize BaseAddress to known endpoint for Filters Api V1 in IO Sensor Software
                BaseAddress = new Uri(@"https://localhost:43809/api/dsio/filters/v1/")
            };
        }

        /// <summary>
        /// Gets or sets the HttpClient used for API requests
        /// </summary>
        /// <value>HttpClient</value>
        public HttpClient Client { get; set; }

        /// <summary>
        /// Creates the proper Authorization header for use with API requests
        /// and adds the header to the <see cref="Client" />
        /// </summary>
        /// <param name="username">The API username</param>
        /// <param name="password">The API Key</param>
        public void SetBasicAuthenticationHeader(string username, string password)
        {
            // encode basic auth credentials as Base64
            var auth = $"{username}:{password}";
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));

            // Add default header to HttpClient
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64);
        }

        /// <summary>
        /// Sends a request to the API Endpoint to check if the service is up an running
        /// </summary>
        /// <returns>true if the service is available, false otherwise</returns>
        public async Task<bool> IsServiceAvailable()
        {
            // This method is intended to simply check if the service is running.
            // When not running, an HTTP Request will generate a Socket exception
            // with the error status ConnectionRefused. Handle this exception and
            // return false, but rethrow all other exceptions.
            try
            {
                var response = await Client.GetAsync("");
                response.EnsureSuccessStatusCode();
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Check for expected exception when service not running (connection refused)
                // iterate through all InnerExceptions looking for SocketException
                var innerException = ex;
                while (innerException != null)
                {
                    // Check for SocketException with ConnectionRefused error
                    if (innerException is SocketException socketException &&
                        socketException.SocketErrorCode == SocketError.ConnectionRefused)
                    {
                        // Expected error code when service is not running, just return false
                        return false;
                    }
                    innerException = innerException.InnerException;
                }

                // rethrow exception if we land here
                throw;
            }
        }

        #region Images

        /// <summary>
        /// Create a new image by uploading a 16-bit grayscale PNG image file.
        /// </summary>
        /// <returns>An ImageResource of <see cref="ImageResource" /> object</returns>
        public async Task<ImageResource> UploadImage(ImageInfo imageInfo, StreamContent imageContent, string ContentMediaType)
        {
            var content = new MultipartFormDataContent();
            imageContent.Headers.ContentType = new MediaTypeHeaderValue(ContentMediaType);
            content.Add(imageContent, "imageMedia");
            var imageInfoJson = JsonConvert.SerializeObject(imageInfo);
            content.Add( new StringContent(imageInfoJson), "ImageInfo");
            var response = await Client.PostAsync("images", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<ImageResource>();
        }

        /// <summary>
        /// Create a new image resource by referencing an image captured in a current modality session.
        /// </summary>
        /// <returns>An ImageResource of <see cref="ImageResource" /> object</returns>
        public async Task<ImageResource> CreateImage(ModalitySession modalitySession)
        {
            var response = await Client.PostAsJsonAsync("images/modality", modalitySession);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<ImageResource>();
        }

        /// <summary>
        /// Gets an image resource
        /// </summary>
        /// <returns>An ImageResource of <see cref="ImageResource" /> object</returns>
        public async Task<ImageResource> GetImage(string id)
        {
            var response = await Client.GetAsync("images/" + id);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<ImageResource>();
            return result;
        }

        /// <summary>
        /// Deletes an image resource from the filters service. This image will no longer be available for processing.
        /// </summary>
        /// <returns>The Http Status Code of the result</returns>
        public async Task<HttpStatusCode> DeleteImage(string id)
        {
            var response = await Client.DeleteAsync("images/" + id);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        /// <summary>
        /// Gets the media associated with an image resource
        /// </summary>
        /// <returns>A stream representing the media</returns>
        public async Task<Stream> GetImageMedia(string id)
        {
            var response = await Client.GetAsync("images/" + id + "/media");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStreamAsync();
            return result;
        }

        #endregion

        #region Filters

        /// <summary>
        /// Applies the Select Filter to an image resource
        /// </summary>
        /// <param name="imageId">The Id of the <see cref="ImageResource"/></param>
        /// <param name="selectFilterParameters">The <see cref="SelectFilterParameters"/> parameters used to process the image</param>
        /// <returns>A stream representing the filtered image</returns>
        public async Task<Stream> SelectFilter(string imageId, SelectFilterParameters selectFilterParameters)
        {
            var response = await Client.PostAsJsonAsync("images/" + imageId + "/filters/select", selectFilterParameters);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            return stream;
        }

        /// <summary>
        /// Applies the Supreme Filter to an image resource
        /// </summary>
        /// <param name="imageId">The Id of the <see cref="ImageResource"/></param>
        /// <param name="supremeFilterParameters">The <see cref="SupremeFilterParameters"/> parameters used to process the image</param>
        /// <returns>A Stream representing the filtered image</returns>
        public async Task<Stream> SupremeFilter(string imageId, SupremeFilterParameters supremeFilterParameters)
        {
            var response = await Client.PostAsJsonAsync("images/" + imageId + "/filters/supreme", supremeFilterParameters);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        /// <summary>
        /// Applies the AE Filter to an image resource 
        /// </summary>
        /// <param name="imageId">The Id of the <see cref="ImageResource"/></param>
        /// <param name="aeFilterParameters">The <see cref="AEFilterParameters"/> parameters used to process the image</param>        
        /// <returns>A Stream representing the filtered image</returns>
        public async Task<Stream> AeFilter(string imageId, AEFilterParameters aeFilterParameters)
        {
            var response = await Client.PostAsJsonAsync("images/" + imageId + "/filters/ae", aeFilterParameters);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        /// <summary>
        /// Removes the mapping applied to an image resource and returns the original image
        /// </summary>
        /// <param name="imageId">The Id of the <see cref="ImageResource"/></param>
        /// <returns>A stream representing the unmapped image</returns>
        public async Task<Stream> UnmapFilter(string imageId)
        {
            var response = await Client.PostAsync("images/" + imageId + "/filters/unmap", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        #endregion

    }
}
