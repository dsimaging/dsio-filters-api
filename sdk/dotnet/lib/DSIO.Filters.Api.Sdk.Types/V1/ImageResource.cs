﻿using System;

namespace DSIO.Filters.Api.Sdk.Types.V1
{
    public class ImageResource
    {
        public string Id { get; set; }

        public string MediaType { get; set; }

        public string Url { get; set; }

        public ModalitySession ModalitySession { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime Expires { get; set; }

        public ImageInfo ImageInfo { get; set; }

    }
}
