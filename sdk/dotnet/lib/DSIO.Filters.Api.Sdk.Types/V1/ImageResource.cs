using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSIO.Filters.Api.Sdk.Types.V1
{
    public class ImageResource
    {
        public string Id { get; set; }
        public string MediaType { get; set; }
        public string Url { get; set; }
        public ModalitySession ModalitySession { get; set; }
    }
    public class ModalitySession
    {
        public string SessionId { get; set; }
        public string ImageId { get; set; }
    }
}
