using NetTopologySuite.Geometries;

namespace TripPoints.Entities
{
    public class Poly
    {
        public int Id { get; set; }
        public Geometry Geometry { get; set; }
        // Other properties
    }
}
