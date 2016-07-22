
namespace LayerPlugin.Data
{
    public class Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Coordinate ()
        {

        }

        public Coordinate (double x, double y)
        {
            X = x;
            Y = y;
        }
        /*
        public Coordinate(Point3d point3D)
        {
            X = point3D.X;
            Y = point3D.Y;
        }*/
    }
}
