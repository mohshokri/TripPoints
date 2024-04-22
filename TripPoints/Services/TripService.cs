//using NetTopologySuite.Geometries;
//using NetTopologySuite.Index.Strtree;
//using NetTopologySuite.Operation.Union;
using TripPoints.Entities;
using TripPoints.Infrastructure;

using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Union;
using NetTopologySuite.Index.Strtree;

namespace TripPoints.Services
{
    public class TripService
    {
        private readonly TripDbContext _dbContext;
        private readonly STRtree<Poly> _strTree;

        public TripService(TripDbContext dbContext)
        {
            _dbContext = dbContext;

            // Preprocess and index polygons
            var polygons = PreprocessPolygons(_dbContext.Polies.ToList());
            _strTree = BuildIndex(polygons);
        }

        public List<Poly> GetPolygonsContainingCarTrip(int carTripId)
        {
            var carTrip = _dbContext.CarTrips.Find(carTripId);
            if (carTrip == null)
            {
                // Car trip not found
                return new List<Poly>();
            }

            // Spatial filtering to quickly identify candidate polygons
            var carEnvelope = new Envelope(carTrip.StartPoint.Coordinate, carTrip.EndPoint.Coordinate);
            var candidatePolygons = _strTree.Query(carEnvelope).Cast<Poly>();

            // Query polygons containing the car trip
            var polygonsContainingCarTrip = candidatePolygons.Where(p => p.Geometry.Contains(carTrip.StartPoint) && p.Geometry.Contains(carTrip.EndPoint)).ToList();

            return polygonsContainingCarTrip;
        }

        private List<Poly> PreprocessPolygons(List<Poly> polygons)
        {
            // Create a GeometryFactory instance
            var geometryFactory = new GeometryFactory();

            // Create a GeometryCollection from input geometries
            var geometryCollection = new GeometryCollection(polygons.Select(p => p.Geometry).ToArray());

            // Perform unary union operation on the GeometryCollection
            var unionOp = new UnaryUnionOp(geometryCollection.ToList());
            var unionResult = unionOp.Union();

            // Convert the result back to polygons
            var simplifiedGeometries = (IEnumerable<Poly>)unionResult;
            var processedPolygons = simplifiedGeometries.Select((g, i) => new Poly { Id = i + 1, Geometry = g.Geometry }).ToList();

            return processedPolygons;
            //// Simplify geometries and remove redundant or overlapping polygons
            //var unionOp = new UnaryUnionOp();
            //var unionResult = unionOp.Union(polygons.Select(p => p.Geometry));
            //var simplifiedGeometries = unionResult.Geometries.Cast<Geometry>().ToList();

            //// Convert simplified geometries back to polygons
            //var processedPolygons = simplifiedGeometries.Select((g, i) => new Poly { Id = i + 1, Geometry = g }).ToList();

            //return processedPolygons;
        }

        private STRtree<Poly> BuildIndex(List<Poly> polygons)
        {
            // Initialize the STRtree with a node capacity
            var strTree = new STRtree<Poly>(10);  // Here, 10 is an example node capacity
            foreach (var polygon in polygons)
            {
                // Ensure geometry is not null and EnvelopeInternal is valid
                if (polygon.Geometry != null && !polygon.Geometry.IsEmpty)
                    strTree.Insert(polygon.Geometry.EnvelopeInternal, polygon);
            }
            strTree.Build();  // This is crucial to finalize the tree structure

            return strTree;


            //    // Build the STRTree index with appropriate parameters
            //    var strTree = new STRtree<Poly>(10, 4);
            //    foreach (var polygon in polygons)
            //    {
            //        strTree.Insert(polygon.Geometry.EnvelopeInternal, polygon);
            //    }
            //    strTree.Build();

            //    return strTree;
            //}
        }
        //public class TripService
        //{
        //    private readonly TripDbContext _dbContext;
        //    private readonly STRtree<Poly> _strTree;

        //    public TripService(TripDbContext dbContext)
        //    {
        //        _dbContext = dbContext;

        //        // Build the STRTree index from polygons
        //        var polygons = _dbContext.Polies.ToList();
        //        _strTree = new STRtree<Poly>();
        //        foreach (var polygon in polygons)
        //        {
        //            _strTree.Insert(polygon.Geometry.EnvelopeInternal, polygon);
        //        }
        //        _strTree.Build();
        //    }

        //    public List<Poly> GetPolygonsContainingCarTrip(int carTripId)
        //    {
        //        var carTrip = _dbContext.CarTrips.Find(carTripId);
        //        if (carTrip == null)
        //        {
        //            // Car trip not found
        //            return new List<Poly>();
        //        }

        //        //// Query polygons containing the car trip using spatial functions
        //        //var polygonsContainingCarTrip = _dbContext.Polies
        //        //    .Where(p => p.Geometry.Contains(carTrip.StartPoint) && p.Geometry.Contains(carTrip.EndPoint))
        //        //    .ToList();

        //        // Query polygons containing the car trip using STRTree index
        //        var carEnvelope = new Envelope(carTrip.StartPoint.Coordinate, carTrip.EndPoint.Coordinate);
        //        var candidatePolygons = _strTree.Query(carEnvelope).Cast<Poly>();
        //        var polygonsContainingCarTrip = candidatePolygons.Where(p => p.Geometry.Contains(carTrip.StartPoint) && p.Geometry.Contains(carTrip.EndPoint)).ToList();


        //        return polygonsContainingCarTrip;
        //    }
    }
}
