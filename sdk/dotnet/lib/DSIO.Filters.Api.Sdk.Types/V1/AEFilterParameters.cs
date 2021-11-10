namespace DSIO.Filters.Api.Sdk.Types.V1
{
    public class AEFilterParameters
    {
        public enum TaskNames
        {
            General,
            Endodontic,
            Periodontic,
            Restorative,
        }

        public TaskNames Task { get; set; }

        public float Sharpness { get; set; }
    }
}
