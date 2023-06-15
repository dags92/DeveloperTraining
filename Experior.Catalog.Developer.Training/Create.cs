using Experior.Core.Assemblies;
using Experior.Core.Mathematics;

namespace Experior.Catalog.Developer.Training
{
    internal class Common
    {
        public static Experior.Core.Resources.EmbeddedImageLoader Icon;
        public static Experior.Core.Resources.EmbeddedResourceLoader Mesh;
    }

    public class Create
    {
        #region Beginner

        public static Assembly PositionAndOrientation(string title, string subtitle, object properties)
        {
            var info = new Assemblies.Beginner.PositionAndOrientationInfo()
            {
                name = Experior.Core.Assemblies.Assembly.GetValidName("Position and Orientation "),
            };
            return new Assemblies.Beginner.PositionAndOrientation(info);
        }

        public static Assembly PlcSignal(string title, string subtitle, object properties)
        {
            var info = new Assemblies.Beginner.PlcSignalsInfo()
            {
                name = Experior.Core.Assemblies.Assembly.GetValidName("PLC Signal "),
                length = 1.5f,
                width = 0.5f
            };
            return new Assemblies.Beginner.PlcSignals(info);
        }

        #endregion

        #region Intermediate

        public static Assembly StraightConveyorBelt(string title, string subtitle, object properties)
        {
            var info = new Assemblies.Intermediate.StraightConveyorBeltInfo()
            {
                name = Experior.Core.Assemblies.Assembly.GetValidName("Straight Conveyor Belt "),
                length = 1.5f,
                width = 0.5f
            };
            return new Assemblies.Intermediate.StraightConveyorBelt(info);
        }

        public static Assembly CurveConveyorBelt(string title, string subtitle, object properties)
        {
            var info = new Assemblies.Intermediate.CurveConveyorBeltInfo()
            {
                name = Experior.Core.Assemblies.Assembly.GetValidName("Curve Conveyor Belt "),
                Radius = 0.6f,
                width = 0.5f,
                Angle = 90f
            };
            return new Assemblies.Intermediate.CurveConveyorBelt(info);
        }

        public static Assembly FixPoints(string title, string subtitle, object properties)
        {
            var info = new Assemblies.Intermediate.FixPointsInfo()
            {
                name = Experior.Core.Assemblies.Assembly.GetValidName("Fix Points "),
                length = 1f
            };
            return new Assemblies.Intermediate.FixPoints(info);
        }

        public static Assembly CadMesh(string title, string subtitle, object properties)
        {
            var info = new Assemblies.Intermediate.CadMeshInfo()
            {
                name = Experior.Core.Assemblies.Assembly.GetValidName("Cad Mesh "),
                length = 1f
            };
            return new Assemblies.Intermediate.CadMesh(info);
        }

        public static Assembly Printer(string title, string subtitle, object properties)
        {
            var info = new Assemblies.Intermediate.PrinterInfo()
            {
                name = Experior.Core.Assemblies.Assembly.GetValidName("Printer "),
            };
            return new Assemblies.Intermediate.Printer(info);
        }

        public static Assembly Magnet(string title, string subtitle, object properties)
        {
            var info = new Assemblies.Intermediate.MagnetInfo()
            {
                name = Experior.Core.Assemblies.Assembly.GetValidName("Magnet "),
            };
            return new Assemblies.Intermediate.Magnet(info);
        }

        #endregion

        #region Advanced



        #endregion

        #region Demo



        #endregion
    }
}