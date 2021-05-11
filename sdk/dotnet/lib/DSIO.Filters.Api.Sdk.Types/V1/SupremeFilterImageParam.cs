namespace DSIO.Filters.Api.Sdk.Types.V1
{
    public class SupremeFilterImageParam
    {
        public TaskName? TaskName { get; set; }

        public BinningMode? BinningMode { get; set; }

        public int Sharpness { get; set; }

        public LutInfo LutInfo { get; set; }
    }
}
