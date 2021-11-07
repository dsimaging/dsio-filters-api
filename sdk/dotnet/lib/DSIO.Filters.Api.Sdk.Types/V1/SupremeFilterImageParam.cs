namespace DSIO.Filters.Api.Sdk.Types.V1
{
    public class SupremeFilterImageParam
    {
        public enum TaskNames
        {
            General,
            Endodontic,
            Periodontic,
            Restorative,
            Hygiene
        };

        public TaskNames Task { get; set; }

        public int Sharpness { get; set; }
    }
}
