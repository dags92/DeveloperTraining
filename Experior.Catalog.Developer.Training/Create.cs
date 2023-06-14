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

        #endregion

        #region Advanced



        #endregion

        #region Demo



        #endregion
    }
}