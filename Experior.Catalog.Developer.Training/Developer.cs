using System.Windows.Media;

namespace Experior.Catalog.Developer.Training
{
    public class Developer : Experior.Core.Catalog
    {
        #region Constructor

        public Developer()
            : base("Developer")
        {
            Simulation = Experior.Core.Environment.Simulation.Events | Experior.Core.Environment.Simulation.Physics;

            Common.Mesh = new Experior.Core.Resources.EmbeddedResourceLoader(System.Reflection.Assembly.GetExecutingAssembly());
            Common.Icon = new Experior.Core.Resources.EmbeddedImageLoader(System.Reflection.Assembly.GetExecutingAssembly());

            AddIntermediateSamples();
        }

        #endregion

        #region Public Properties

        public override ImageSource Logo => Common.Icon.Get("Logo");

        #endregion

        #region Private Methods

        private void AddIntermediateSamples()
        {
            Add(Common.Icon.Get("StraightConveyorBelt"), "Intermediate", "Straight Conveyor Belt", Simulation, Create.StraightConveyorBelt);
            Add(Common.Icon.Get("CurveConveyorBelt"), "Intermediate", "Curve Conveyor Belt", Simulation, Create.CurveConveyorBelt);
        }

        #endregion
    }
}