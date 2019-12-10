namespace VesselTrackApi.Data.Entities
{
    public interface IPosition
    {
        decimal Lat { get; set; }

        decimal Lon { get; set; }
    }
}