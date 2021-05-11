using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
        /// Upload a new image resource for processing. 
        /// Create a new image by uploading a 16-bit grayscale PNG image file.
        /// </summary>
        /// <returns>An ImageResource of <see cref="ImageResource" /> object</returns>
        public async Task<ImageResource> UploadImage(StreamContent imageContent, string ContentMediaType)
        {
            imageContent.Headers.ContentType = new MediaTypeHeaderValue(ContentMediaType);
            var response = await Client.PostAsync("images", imageContent);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<ImageResource>();
        }

        /// <summary>
        /// Create an image resource from a Modality session.
        /// Create a new image resource by referencing an image captured in a current modality session.
        /// </summary>
        /// <returns>An ModalitySession of <see cref="ModalitySession" /> object</returns>
        public async Task<ImageResource> CreateImage(ModalitySession modalitySession)
        {
            var response = await Client.PostAsJsonAsync("images/modality", modalitySession);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<ImageResource>();
        }

        /// <summary>
        /// Gets an image resource
        /// Retrieves an image resource
        /// </summary>
        /// <returns>An id of <see cref="id" /> object</returns>
        public async Task<ImageResource> GetImage(string id)
        {
            var response = await Client.GetAsync("images/" + id);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<ImageResource>();
            return result;
        }

        /// <summary>
        /// Delete an image resource
        /// Deletes an image resource from the filters service. This image will no longer be available for processing.
        /// </summary>
        /// <returns>An id of <see cref="id" /> object</returns>
        public async Task<HttpStatusCode> DeleteImage(string id)
        {
            var response = await Client.DeleteAsync("images/" + id);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        #endregion

        #region Filters

        /// <summary>
        /// Retrieves the image stream by Applying the Select Filter
        /// </summary>
        /// <param name="imageId">Image Id</param>
        /// <param name="selectFilterImageParam">The parameters used to process the image</param>
        /// <returns>An StreamContent of <see cref="StreamContent" /> object</returns>
        public async Task<Stream> SelectFilter(string imageId, SelectFilterImageParam selectFilterImageParam)
        {
            var response = await Client.PostAsJsonAsync("images/" + imageId + "/filters/select", selectFilterImageParam);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            return stream;
        }

        /// <summary>
        /// Retrieves the image stream by Applying the Supreme Filter
        /// </summary>
        /// <param name="imageId">Image Id</param>
        /// <param name="supremeFilterImageParam">The parameters used to process the image</param>
        /// <returns>An StreamContent of <see cref="StreamContent" /> object</returns>
        public async Task<Stream> SupremeFilter(string imageId, SupremeFilterImageParam supremeFilterImageParam)
        {
            var response = await Client.PostAsJsonAsync("images/" + imageId + "/filters/supreme", supremeFilterImageParam);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        /// <summary>
        /// Retrieves the image stream by Applying the Ae Filter
        /// </summary>
        /// <param name="imageId">Image Id</param>
        /// <param name="omegaFilterImageParam">The parameters used to process the image</param>        
        /// <returns>An StreamContent of <see cref="StreamContent" /> object</returns>
        public async Task<Stream> AeFilter(string imageId, OmegaFilterImageParam omegaFilterImageParam)
        {
            var response = await Client.PostAsJsonAsync("images/" + imageId + "/filters/ae", omegaFilterImageParam);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        /// <summary>
        /// Retrieves the image stream by Applying the Unmap Filter
        /// </summary>
        /// <param name="imageId">Image Id</param>
        /// <param name="lutInfo">The parameters used to process the image</param>
        /// <returns>An StreamContent of <see cref="StreamContent" /> object</returns>
        public async Task<Stream> UnmapFilter(string imageId, LutInfo lutInfo)
        {
            var response = await Client.PostAsJsonAsync("images/" + imageId + "/filters/unmap", lutInfo);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }

        #endregion

    }
}
