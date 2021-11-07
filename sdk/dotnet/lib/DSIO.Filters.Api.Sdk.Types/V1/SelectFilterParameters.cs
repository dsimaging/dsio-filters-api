namespace DSIO.Filters.Api.Sdk.Types.V1
{
    public class SelectFilterParameters
    {
        public enum EnhancementModes
        {
            Smooth,
            EdgeLow,
            EdgeHigh,
            EdgePro
        };

        public EnhancementModes EnhancementMode { get; set; }
    }
}
