using Experior.Core.Assemblies;

namespace Experior.Catalog.Developer.Training
{
    internal class Common
    {
        public static Experior.Core.Resources.EmbeddedImageLoader EmbeddedImageLoader;
        public static Experior.Core.Resources.EmbeddedResourceLoader EmbeddedResourceLoader;
    }

    public class Create
    {
        public static Assembly MyAssembly(string title, string subtitle, object properties)
        {
            var info = new Experior.Catalog.Developer.Training.Assemblies.MyAssemblyInfo { name = Experior.Core.Assemblies.Assembly.GetValidName("MyAssembly") };
            var assembly = new Experior.Catalog.Developer.Training.Assemblies.MyAssembly(info);
            return assembly;
        }
    }
}