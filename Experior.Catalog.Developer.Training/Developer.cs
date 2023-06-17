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

            AddBeginnerSamples();
            AddIntermediateSamples();
            AddAdvancedSamples();
        }

        #endregion

        #region Public Properties

        public override ImageSource Logo => Common.Icon.Get("Logo");

        #endregion

        #region Private Methods

        private void AddBeginnerSamples()
        {
            Add(Common.Icon.Get("Dimensions"), "Beginner", "Dimensions", Simulation, Create.Dimensions);
            Add(Common.Icon.Get("ContextMenu"), "Beginner", "Context Menu", Simulation, Create.ContextMenu);
            Add(Common.Icon.Get("PositionAndOrientation"), "Beginner", "Position and Orientation", Simulation, Create.PositionAndOrientation);
            Add(Common.Icon.Get("Magnet"), "Intermediate", "Magnet", Simulation, Create.Magnet);
            Add(Common.Icon.Get("FixPoints"), "Intermediate", "Fix Points", Simulation, Create.FixPoints);
            Add(Common.Icon.Get("PlcSignals"), "Beginner", "PLC Signal", Simulation, Create.PlcSignal);
        }

        private void AddIntermediateSamples()
        {
            Add(Common.Icon.Get("StraightConveyorBelt"), "Intermediate", "Straight Conveyor Belt", Simulation, Create.StraightConveyorBelt);
            Add(Common.Icon.Get("CurveConveyorBelt"), "Intermediate", "Curve Conveyor Belt", Simulation, Create.CurveConveyorBelt);
            Add(Common.Icon.Get("CadMesh"), "Intermediate", "CAD Mesh", Simulation, Create.CadMesh);
            Add(Common.Icon.Get("Printer"), "Intermediate", "Printer", Simulation, Create.Printer);
            Add(Common.Icon.Get("CoordinateSystems"), "Intermediate", "Coordinate System", Simulation, Create.CoordinateSystems);
            Add(Common.Icon.Get("StraightTransportSection"), "Intermediate", "Straight Transport Section", Simulation, Create.StraightTransportSection);
            Add(Common.Icon.Get("CurvedTransportSection"), "Intermediate", "Curved Transport Section", Simulation, Create.CurvedTransportSection);
        }

        private void AddAdvancedSamples()
        {
        }

        #endregion
    }
}