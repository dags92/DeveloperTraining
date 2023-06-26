using System.ComponentModel;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using Experior.Core.Parts;
using Experior.Interfaces;

namespace Experior.Catalog.Developer.Training.Assemblies.Beginner
{
    public class Texture : Experior.Core.Assemblies.Assembly
    {
        #region Fields

        private readonly TextureInfo _info;

        #endregion
        
        #region Constructor

        public Texture(TextureInfo info) : base(info)
        {
            _info = info;

            var imageName = "Cubemap.jpg";
            var bitmap = Common.Icon.Get(imageName) as BitmapSource;
            Add(new TextureBox(imageName, bitmap, 0.5f, 0.5f, 0.5f, false));
        }

        #endregion

        #region Public Properties

        public override string Category => "Intermediate";

        public override ImageSource Image => Common.Icon.Get("Texture");

        #endregion

        #region Public Methods

        public override void Inserted()
        {
            base.Inserted();

            var message = "--------------------------------------------------------------------------------------------" +
                          "\n Texture" +
                          "\n Objective: use of System.Windows.Media.Imaging.BitMapSource type in Rigid Parts" +
                          "\n --------------------------------------------------------------------------------------------";

            Log.Write(message, Colors.LimeGreen, LogFilter.Information);
        }

        #endregion
    }

    [Serializable]
    [TypeConverter(typeof(TextureInfo))]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.TextureInfo")]
    public class TextureInfo : Experior.Core.Assemblies.AssemblyInfo
    {

    }
}
