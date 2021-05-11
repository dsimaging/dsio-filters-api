namespace DSIO.Filters.Api.Sdk.Types.V1
{
    public class LutInfo
    {
        /// <summary>
        /// Gamma value used in map
        /// </summary>
        public double Gamma { get; set; }

        /// <summary>
        /// Slope value used in map
        /// </summary>
        public double Slope { get; set; }

        /// <summary>
        /// Offset value used in map
        /// </summary>
        public double Offset { get; set; }

        /// <summary>
        /// Total number of possibe gray levels (full range) in original unmapped image
        /// </summary>
        public int TotalGrays { get; set; }

        /// <summary>
        /// Minimum value of gray level present in original unmapped image
        /// </summary>
        public int MinimumGray { get; set; }

        /// <summary>
        /// Maximum value of gray level present in original unmapped image
        /// </summary>
        public int MaximumGray { get; set; }
    }
}
