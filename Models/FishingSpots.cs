
namespace FishingAppBackend.Models;
public class FishingSpot
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public List<string> AvailableFish { get; set; }
    public List<string> RecommendedEquipment { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}