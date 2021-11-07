namespace DSIO.Filters.Api.Sdk.Types.V1
{
    public class OmegaFilterImageParam
    {
        public enum TaskNames
        {
            General,
            Endodontic,
            Periodontic,
            Restorative,
        }

        public TaskNames Task { get; set; }

        public int Sharpness { get; set; }
    }
}
