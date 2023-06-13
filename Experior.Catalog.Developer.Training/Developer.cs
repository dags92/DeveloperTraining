using System.Windows.Media;

namespace Experior.Catalog.Developer.Training
{
    public class Developer : Experior.Core.Catalog
    {
        public Developer()
            : base("MyCatalog")
        {
            Simulation = Experior.Core.Environment.Simulation.Events | Experior.Core.Environment.Simulation.Physics;

            Common.EmbeddedResourceLoader = new Experior.Core.Resources.EmbeddedResourceLoader(System.Reflection.Assembly.GetExecutingAssembly());
            Common.EmbeddedImageLoader = new Experior.Core.Resources.EmbeddedImageLoader(System.Reflection.Assembly.GetExecutingAssembly());

            Add(Common.EmbeddedImageLoader.Get("MyAssembly"), "MyAssembly", "", Experior.Core.Environment.Simulation.Events | Experior.Core.Environment.Simulation.Physics, Create.MyAssembly);
        }

        public override ImageSource Logo
        {
            get { return Common.EmbeddedImageLoader.Get("Logo"); }
        }
    }
}