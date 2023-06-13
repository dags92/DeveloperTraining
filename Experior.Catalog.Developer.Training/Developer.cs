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

            //Add(Common.Icon.Get("MyAssembly"), "MyAssembly", "", Experior.Core.Environment.Simulation.Events | Experior.Core.Environment.Simulation.Physics, Create.MyAssembly);
        }

        #endregion

        #region Public Properties

        public override ImageSource Logo => Common.Icon.Get("Logo");

        #endregion
    }
}