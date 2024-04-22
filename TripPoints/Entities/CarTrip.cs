using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripPoints.Entities
{
    public class CarTrip
    {
        public int Id { get; set; }
        [Column(TypeName = "geometry")]
        public Point StartPoint { get; set; }
        [Column(TypeName = "geometry")]
        public Point EndPoint { get; set; }
        // Other properties
    }
}
