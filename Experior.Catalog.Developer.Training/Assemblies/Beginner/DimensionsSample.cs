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

        private readonly DimensionsSampleInfo _info;

        private readonly Box _box;

        #endregion

        #region Constructor

        // The constructor of an Assembly always contains an object deriving from the AssemblyInfo class as an argument.
        // It is used to support the mechanism for Save/Load a model.
        public DimensionsSample(DimensionsSampleInfo sampleInfo) : base(sampleInfo)
        {
            _info = sampleInfo;

            
            _box = new Box(Colors.Wheat, _info.length, _info.height, _info.width); // Create a new instance of type Experior.Core.Parts.Box
            Add(_box); // Every Rigid Part must be added to the Assembly !
        }

        #endregion

        #region Public Properties

        // Every public property is displayed in the Property Window !

        // Attributes enhance the visualization of the properties.

        // [Category("Size")] -> Allocates the property inside a category
        // [DisplayName("Length")] -> Displays the name specified instead of the property name
        // [PropertyOrder(1)] -> Defines the property order in the category
        // [TypeConverter(typeof(FloatMeterToMillimeter))] -> Displays units in the property window

        [Category("Size")]
        [DisplayName("Length")] 
        [PropertyOrder(1)] 
        [TypeConverter(typeof(FloatMeterToMillimeter))] 
        public float Length
        {
            get => _info.length;
            set
            {
                if (value <= 0)
                {
                    Log.Write("Length cannot be less than 0 mm", Colors.Orange, LogFilter.Information); // Writes in the Log window
                    return;
                }

                //  Every property value changed from the Property Window (UI) is handled by the Main Thread.
                //  On the other hand, changes regarding the visualization, position, creation or deletion
                //  of RigidParts/Assemblies must be handled by the Engine Thread. Therefore it is required to invoke it. 
                //  Invoke(Refresh) Invokes the Engine Thread to execute the method <c>Refresh</c> !

                _info.length = value;
                Invoke(Refresh);
            }
        }

        [Category("Size")]
        [DisplayName("Height")]
        [PropertyOrder(2)]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        public float Height
        {
            get => _info.height;
            set
            {
                if (value <= 0)
                {
                    Log.Write("Height cannot be less than 0 mm", Colors.Orange, LogFilter.Information); // Writes in the Log window
                    return;
                }

                _info.height = value;
                Invoke(Refresh); // Invokes the Engine Thread to execute the method Refresh !
            }
        }

        [Category("Size")]
        [DisplayName("Width")]
        [PropertyOrder(1)]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        public float Width
        {
            get => _info.width;
            set
            {
                if (value <= 0)
                {
                    Log.Write("Width cannot be less than 0 mm", Colors.Orange, LogFilter.Information); // Writes in the Log window
                    return;
                }

                _info.width = value;
                Invoke(Refresh); // Invokes the Engine Thread to execute the method Refresh !
            }
        }

        public override string Category => "Beginner"; // Category used by the Solution Explorer

        public override ImageSource Image => Common.Icon.Get("DimensionsSample"); // Image/Icon used by the Solution Explorer

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is called only once by Experior when the component is dropped into the Scene.
        /// </summary>
        public override void Inserted()
        {
            base.Inserted();

            var message = "--------------------------------------------------------------------------------------------" +
                          "\n Sample: Dimensions" +
                          "\n" +

                          "\n Description: " +
                          "\n 1) Use of Experior.Core.Parts.Box" +
                          "\n 2) Modification of dimensions through the Property Window" +
                          "\n 3) Use of Invoke() to invoke the Engine Thread" +
                          "\n" +

                          "\n Usage: " +
                          "\n 1) Select the box" +
                          "\n 2) Modify the dimensions of the box through the Property Window" +
                          "\n --------------------------------------------------------------------------------------------";

            Log.Write(message, Colors.Orange, LogFilter.Information);
        }

        /// <summary>
        /// This method is called to update the internals of the Assembly when some of its properties have changed. It can be used to create/delete sub-parts.
        /// The method is only called by the developer. Therefore, the developer must take care of invoking the Engine Thread if required.
        /// </summary>
        public override void Refresh()
        {
            if (_info == null)
            {
                return;
            }

            _box.Length = Length;
            _box.Width = Width;
            _box.Height = Height;
        }

        #endregion
    }

    [TypeConverter(typeof(DimensionsSampleInfo))] // -> Attributes to specify the class is Serializable
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.DimensionsSampleInfo")] // -> TypeName must be unique !
    public class DimensionsSampleInfo : AssemblyInfo
    {

    }
}
