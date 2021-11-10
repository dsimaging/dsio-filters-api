namespace DSIO.Filters.Api.Sdk.Types.V1
{
    public class SupremeFilterParameters
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

        public float Sharpness { get; set; }
    }
}
