using System;
using System.IO;
using System.Threading.Tasks;
using DSIO.Filters.Api.Sdk.Types.V1;
using DSIO.Filters.Api.Sdk.Client.V1;

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
                    Console.Write("Enter the Image Id: ");
                    var imageId = Console.ReadLine();
                    var imageResource = await service.GetImage(imageId);
                    Console.WriteLine($"Filters Api V1 service Image Resource: {imageResource.Url}");

                    // Test Select Filter
                    var selectFilterParam = new SelectFilterImageParam
                    {
                        EnhancementMode = EnhancementMode.EdgePro,
                        LutInfo = new LutInfo
                        {
                            Gamma = 2.3,
                            Slope = 65535,
                            Offset = 0,
                            TotalGrays = 4096,
                            MinimumGray = 3612,
                            MaximumGray = 418
                        }
                    };

                    // Test Supreme Filter
                    var supremeFilterParam = new SupremeFilterImageParam
                    {
                        TaskName = TaskName.General,
                        BinningMode = BinningMode.Binned2X2,
                        Sharpness = 70,
                        LutInfo = new LutInfo
                        {
                            Gamma = 2.3,
                            Slope = 65535,
                            Offset = 0,
                            TotalGrays = 4096,
                            MinimumGray = 3612,
                            MaximumGray = 418
                        }
                    };

                    // Test AE Filter
                    var omegaFilterParam = new OmegaFilterImageParam
                    {
                        TaskName = TaskName.General,
                        Sharpness = 70,
                        LutInfo = new LutInfo
                        {
                            Gamma = 2.3,
                            Slope = 65535,
                            Offset = 0,
                            TotalGrays = 4096,
                            MinimumGray = 3612,
                            MaximumGray = 418
                        }
                    };

                    // Test Unmap Filter
                    var lutInfo = new LutInfo
                    {
                        Gamma = 2.3,
                        Slope = 65535,
                        Offset = 0,
                        TotalGrays = 4096,
                        MinimumGray = 3612,
                        MaximumGray = 418
                    };

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
