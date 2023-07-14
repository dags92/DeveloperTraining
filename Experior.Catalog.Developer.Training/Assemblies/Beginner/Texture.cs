using System.ComponentModel;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using Experior.Core.Parts;
using Experior.Interfaces;

namespace Experior.Catalog.Developer.Training.Assemblies.Beginner
{
    /// <summary>
    /// Class <c>DimensionsSample</c> exemplifies the creation of a Experior.Core.Parts.TextureBox
    /// </summary>
    public class Texture : Experior.Core.Assemblies.Assembly
    {
        #region Fields

        private readonly TextureInfo _info;

        #endregion

        #region Constructor

        // Note:
        // The constructor of an Assembly always contains an object deriving from the AssemblyInfo class as an argument.
        // It is used to support the mechanism for Save/Load a model.
        public Texture(TextureInfo info) : base(info)
        {
            _info = info;

            // Note:
            // The image allocated in the Icon folder must have the property Build Action equal to Embedded Resource
            var imageName = "Cubemap.jpg";
            var bitmap = Common.Icon.Get(imageName) as BitmapSource;

            // Note:
            // Every RigidPart must be added to the Assembly !
            Add(new TextureBox(imageName, bitmap, 0.5f, 0.5f, 0.5f, false));
        }

        #endregion

        #region Public Properties

        // Note:
        // Category is used by the Solution Explorer
        public override string Category => "Beginner";

        // Note:
        // Image is used by the Solution Explorer
        public override ImageSource Image => Common.Icon.Get("Texture");

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is called only once by Experior when the component is dropped into the Scene.
        /// </summary>
        public override void Inserted()
        {
            base.Inserted();

            var message = "--------------------------------------------------------------------------------------------" +
                          "\n Sample: Texture Box" +
                          "\n" +

                          "\n Description: " +
                          "\n 1) Use of Experior.Core.Parts.TextureBox" +
                          "\n" +
                          "\n --------------------------------------------------------------------------------------------";

            Log.Write(message, Colors.Orange, LogFilter.Information);
        }

        #endregion
    }

    // Note:
    // Attributes allow the developer to specify if a class is Serializable.
    // Each class must have a unique TypeName !
    [Serializable]
    [TypeConverter(typeof(TextureInfo))]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.TextureInfo")]
    public class TextureInfo : Experior.Core.Assemblies.AssemblyInfo
    {

    }
}
