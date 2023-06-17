using Experior.Catalog.Developer.Training.Assemblies.Beginner;
using Experior.Catalog.Developer.Training.Assemblies.Intermediate;
using Experior.Core.Assemblies;
using Experior.Core.Resources;

namespace Experior.Catalog.Developer.Training
{
    internal class Common
    {
        public static EmbeddedImageLoader Icon;
        public static EmbeddedResourceLoader Mesh;
    }

    public class Create
    {
        #region Beginner

        public static Assembly Dimensions(string title, string subtitle, object properties)
        {
            var info = new DimensionsInfo
            {
                name = Assembly.GetValidName("Dimensions Sample "),
                length = 0.5f,
                height = 0.5f,
                width = 0.5f    
            };
            return new Dimensions(info);
        }

        public static Assembly ContextMenu(string title, string subtitle, object properties)
        {
            var info = new ContextMenuInfo
            {
                name = Assembly.GetValidName("Context Menu Sample "),
            };
            return new ContextMenu(info);
        }

        public static Assembly PositionAndOrientation(string title, string subtitle, object properties)
        {
            var info = new PositionAndOrientationInfo
            {
                name = Assembly.GetValidName("Position and Orientation Sample "),
            };
            return new PositionAndOrientation(info);
        }

        public static Assembly PlcSignal(string title, string subtitle, object properties)
        {
            var info = new PlcSignalsInfo
            {
                name = Assembly.GetValidName("PLC Signal Sample "),
                length = 1.5f,
                width = 0.5f
            };
            return new PlcSignals(info);
        }

        public static Assembly FixPoints(string title, string subtitle, object properties)
        {
            var info = new FixPointsInfo
            {
                name = Assembly.GetValidName("Fix Points Sample "),
                length = 1f
            };
            return new FixPoints(info);
        }

        public static Assembly Magnet(string title, string subtitle, object properties)
        {
            var info = new MagnetInfo
            {
                name = Assembly.GetValidName("Magnet Sample "),
            };
            return new Magnet(info);
        }

        #endregion

        #region Intermediate

        public static Assembly StraightConveyorBelt(string title, string subtitle, object properties)
        {
            var info = new StraightConveyorBeltInfo
            {
                name = Assembly.GetValidName("Straight Conveyor Belt Sample "),
                length = 1.5f,
                width = 0.5f
            };
            return new StraightConveyorBelt(info);
        }

        public static Assembly CurveConveyorBelt(string title, string subtitle, object properties)
        {
            var info = new CurveConveyorBeltInfo
            {
                name = Assembly.GetValidName("Curve Conveyor Belt Sample "),
                Radius = 0.6f,
                width = 0.5f,
                Angle = 90f
            };
            return new CurveConveyorBelt(info);
        }

        public static Assembly CadMesh(string title, string subtitle, object properties)
        {
            var info = new CadMeshInfo
            {
                name = Assembly.GetValidName("Cad Mesh Sample "),
                length = 1f
            };
            return new CadMesh(info);
        }

        public static Assembly Printer(string title, string subtitle, object properties)
        {
            var info = new PrinterInfo
            {
                name = Assembly.GetValidName("Printer Sample "),
            };
            return new Printer(info);
        }

        public static Assembly CoordinateSystems(string title, string subtitle, object properties)
        {
            var info = new CoordinateSystemsInfo()
            {
                name = Assembly.GetValidName("Coordinate System Sample "),
            };
            return new CoordinateSystems(info);
        }

        public static Assembly StraightTransportSection(string title, string subtitle, object properties)
        {
            var info = new StraightTransportSectionInfo()
            {
                name = Assembly.GetValidName("Straight Transport Section Sample "),
            };
            return new StraightTransportSection(info);
        }

        public static Assembly CurvedTransportSection(string title, string subtitle, object properties)
        {
            var info = new CurvedTransportSectionInfo()
            {
                name = Assembly.GetValidName("Curved Transport Section Sample "),
            };
            return new CurvedTransportSection(info);
        }

        #endregion

        #region Advanced



        #endregion

        #region Demo



        #endregion
    }
}