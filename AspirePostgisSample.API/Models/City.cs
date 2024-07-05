using NetTopologySuite.Geometries;

namespace AspirePostgisSample.API.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Point Location { get; set; }
    }
}
