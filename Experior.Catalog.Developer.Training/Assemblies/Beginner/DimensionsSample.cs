using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core.Assemblies;
using Experior.Core.Parts;
using Experior.Core.Properties;
using Experior.Core.Properties.TypeConverter;
using Experior.Interfaces;

namespace Experior.Catalog.Developer.Training.Assemblies.Beginner
{
    /// <summary>
    /// Class <c>DimensionsSample</c> exemplifies the creation of a Box, and the modification of its dimensions through the Property Window.
    /// </summary>
    public class DimensionsSample : Assembly
    {
        #region Fields

        private readonly DimensionsSampleInfo _sampleInfo;

        private readonly Box _box;

        #endregion

        #region Constructor

        //
        public DimensionsSample(DimensionsSampleInfo sampleInfo) : base(sampleInfo)
        {
            _sampleInfo = sampleInfo;

            
            _box = new Box(Colors.Wheat, _sampleInfo.length, _sampleInfo.height, _sampleInfo.width); // Create a new instance of type Experior.Core.Parts.Box
            Add(_box); // Every Rigid Part must be added to the Assembly !
        }

        #endregion

        #region Public Properties

        // Every public property is displayed in the Property Window !
        // Attributes enhances the visualization of the properties.
        // [Category("Size")] -> Allocates the property inside a category
        // [DisplayName("Length")] -> Displays the name specified instead of the property name
        // [PropertyOrder(1)] -> Defines the property order in the category
        // [TypeConverter(typeof(FloatMeterToMillimeter))] -> Displays units in the property window

        [Category("Size")] // Allocates the property inside a category
        [DisplayName("Length")] // Displays the name specified instead of the property name
        [PropertyOrder(1)] // Defines the property order in the category
        [TypeConverter(typeof(FloatMeterToMillimeter))] // Displays units in the property window
        public float Length
        {
            get => _sampleInfo.length;
            set
            {
                if (value <= 0)
                {
                    Log.Write("Length cannot be less than 0 mm", Colors.Orange, LogFilter.Information); // Writes in the Log window
                    return;
                }

                _sampleInfo.length = value;
                Invoke(Refresh); // Invokes the Engine Thread to execute the method Refresh
            }
        }

        [Category("Size")]
        [DisplayName("Height")]
        [PropertyOrder(2)]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        public float Height
        {
            get => _sampleInfo.height;
            set
            {
                if (value <= 0)
                {
                    Log.Write("Height cannot be less than 0 mm", Colors.Orange, LogFilter.Information); // Writes in the Log window
                    return;
                }

                _sampleInfo.height = value;
                Invoke(Refresh); // Invokes the Engine Thread to execute the method Refresh
            }
        }

        [Category("Size")]
        [DisplayName("Width")]
        [PropertyOrder(1)]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        public float Width
        {
            get => _sampleInfo.width;
            set
            {
                if (value <= 0)
                {
                    Log.Write("Width cannot be less than 0 mm", Colors.Orange, LogFilter.Information); // Writes in the Log window
                    return;
                }

                _sampleInfo.width = value;
                Invoke(Refresh); // Invokes the Engine Thread to execute the method Refresh
            }
        }

        public override string Category => "Beginner";

        public override ImageSource Image => Common.Icon.Get("DimensionsSample");

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is called to update the internals of the Assembly when some of its properties have changed. It can be used to create/delete sub-parts.
        /// The method is only called by the developer. Therefore, the developer must take care of invoking the Engine Thread if required.
        /// </summary>
        public override void Refresh()
        {
            if (_sampleInfo == null)
            {
                return;
            }

            _box.Length = Length;
            _box.Width = Width;
            _box.Height = Height;
        }

        #endregion
    }

    [TypeConverter(typeof(DimensionsSampleInfo))] // Attributes to specify the class is Serializable
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.DimensionsSampleInfo")] // TypeName must be unique !
    public class DimensionsSampleInfo : AssemblyInfo
    {

    }
}
