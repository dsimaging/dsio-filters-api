using System;
using System.IO;
using System.Threading.Tasks;
using DSIO.Filters.Api.Sdk.Types.V1;
using DSIO.Filters.Api.Sdk.Client.V1;
using System.Net;
using System.Net.Http;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // API Keys are long, modify Console to get around 254 char limit
            Console.SetIn(new StreamReader(Console.OpenStandardInput(8192)));

            Console.WriteLine("Sample console app for Filters Api");

            // Read credentials
            Console.Write("Enter your API Username: ");
            var username = Console.ReadLine();
            Console.Write("Enter your API Key: ");
            var apikey = Console.ReadLine();

            // Create service proxy and set credentials
            var service = new ServiceProxy();
            service.SetBasicAuthenticationHeader(username, apikey);

            // ServiceProxy HttpClient calls will throw exceptions when
            // unsuccessful. Handle exceptions and show errors
            try
            {
                // Test availability
                Console.WriteLine("Checking availability of service...");
                var isAvailable = await service.IsServiceAvailable();
                Console.WriteLine($"Filters Api V1 service isAvailable: {isAvailable}");

                if (isAvailable)
                {
                    ImageResource imageResource = null;

                    // Test Upload Image
                    Console.Write("Enter the Image File URL Input Parameter: ");
                    string uploadImageFileName = Console.ReadLine();
                    var fileStream = new FileStream(uploadImageFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    var imageContent = new StreamContent(fileStream);
                    // Upload a new image
                    service.UploadImage(imageContent, "image/png").ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine(task.Exception?.Message);
                        }
                        else if (task.IsCompleted)
                        {
                            imageResource = task.Result;
                            Console.WriteLine($"Filters Api V1 service Image Resource: {imageResource.Url}");
                        }
                    });

                    // Test Create Image From a Modality Session
                    Console.Write("Enter the Modality Session Input Parameters: ");
                    string modalitySessionParam = Console.ReadLine();
                    ModalitySession modalitySession = Newtonsoft.Json.JsonConvert.DeserializeObject<ModalitySession>(modalitySessionParam);
                    // Create image from Modality Session
                    service.CreateImage(modalitySession).ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine(task.Exception?.Message);
                        }
                        else if (task.IsCompleted)
                        {
                            imageResource = task.Result;
                        }
                    });
                    if (imageResource != null)
                    {
                        Console.WriteLine($"Filters Api V1 service Image Resource: {imageResource.Url}");
                    }
                    else
                    {
                        Console.WriteLine($"Filters Api V1 service Image Resource Not Found");
                    }

                    // Test Get Image
                    Console.Write("Enter the Image Id: ");
                    var imageId = Console.ReadLine();
                    imageResource = await service.GetImage(imageId);
                    if (imageResource != null)
                    {
                        Console.WriteLine($"Filters Api V1 service Image Resource: {imageResource.Url}");
                    }
                    else
                    {
                        Console.WriteLine($"Filters Api V1 service Image Resource Not Found");
                    }

                    // Test Delete Image
                    Console.Write("Enter the Image Id: ");
                    imageId = Console.ReadLine();
                    HttpStatusCode httpStatus = await service.DeleteImage(imageId);
                    if (httpStatus == HttpStatusCode.OK)
                    {
                        Console.WriteLine($"Filters Api V1 service Image Resource: {imageResource.Url}");
                    }
                    else
                    {
                        Console.WriteLine($"Filters Api V1 service Image Resource Not Found");
                    }

                    // Test Select Filter
                    string selectFilteredImageFileName = string.Empty;
                    Console.Write("Enter the Image Id: ");
                    imageId = Console.ReadLine();
                    Console.Write("Enter the Select Filter Input Parameters: ");
                    string selectFilterParam = Console.ReadLine();
                    SelectFilterImageParam selectFilterImageParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SelectFilterImageParam>(selectFilterParam);
                    // Apply Select Filter
                    service.SelectFilter(imageId, selectFilterImageParam).ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine(task.Exception?.Message);
                        }
                        else if (task.IsCompleted)
                        {
                            selectFilteredImageFileName = System.Environment.CurrentDirectory + @"\SelectFilteredImage.png";
                            task.Result.WriteToFile(selectFilteredImageFileName);
                        }
                    });

                    // Test Supreme Filter
                    string supremeFilteredImageFileName = string.Empty;
                    Console.Write("Enter the Image Id: ");
                    imageId = Console.ReadLine();
                    Console.Write("Enter the Supreme Filter Input Parameters: ");
                    string supremeFilterParam = Console.ReadLine();
                    SupremeFilterImageParam supremeFilterImageParam = Newtonsoft.Json.JsonConvert.DeserializeObject<SupremeFilterImageParam>(supremeFilteredImageFileName);
                    // Apply Select Filter
                    service.SupremeFilter(imageId, supremeFilterImageParam).ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine(task.Exception?.Message);
                        }
                        else if (task.IsCompleted)
                        {
                            supremeFilteredImageFileName = System.Environment.CurrentDirectory + @"\SupremeFilteredImage.png";
                            task.Result.WriteToFile(supremeFilteredImageFileName);
                        }
                    });

                    // Test AE Filter
                    string aeFilteredImageFileName = string.Empty;
                    Console.Write("Enter the Image Id: ");
                    imageId = Console.ReadLine();
                    Console.Write("Enter the Ae Filter Input Parameters: ");
                    string omegaFilterParam = Console.ReadLine();
                    OmegaFilterImageParam omegaFilterImageParam = Newtonsoft.Json.JsonConvert.DeserializeObject<OmegaFilterImageParam>(omegaFilterParam);
                    // Apply ae Filter
                    service.AeFilter(imageId, omegaFilterImageParam).ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine(task.Exception?.Message);
                        }
                        else if (task.IsCompleted)
                        {
                            aeFilteredImageFileName = System.Environment.CurrentDirectory + @"\AEFilteredImage.png";
                            task.Result.WriteToFile(aeFilteredImageFileName);
                        }
                    });

                    // Test Unmap Filter
                    string UnmapFilteredImageFileName = string.Empty;
                    Console.Write("Enter the Image Id: ");
                    imageId = Console.ReadLine();
                    Console.Write("Enter the Unmap Filter Input Parameters: ");
                    string unmapFilterParam = Console.ReadLine();
                    LutInfo lutInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<LutInfo>(unmapFilterParam);
                    // Apply Unmap Filter
                    service.UnmapFilter(imageId, lutInfo).ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine(task.Exception?.Message);
                        }
                        else if (task.IsCompleted)
                        {
                            UnmapFilteredImageFileName = System.Environment.CurrentDirectory + @"\UnmapFilteredImage.png";
                            task.Result.WriteToFile(UnmapFilteredImageFileName);
                        }
                    });

                    // We are now listening for changes in the Device list. Try
                    // changing the connected sensor of the Simulator to see examples
                    // of event data sent to this client.
                    Console.WriteLine("Press Enter to stop filters api test...");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception encountered: {ex.Message}");
            }

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
