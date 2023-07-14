using System;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core.Parts;
using Experior.Core.Properties;
using Experior.Interfaces;
using Colors = System.Windows.Media.Colors;

namespace Experior.Catalog.Developer.Training.Assemblies.Beginner
{
    /// <summary>
    /// Class <c>Label</c> exemplifies the content modification of the Assembly Label.
    /// </summary>
    public class Label : Experior.Core.Assemblies.Assembly
    {
        #region Fields

        private readonly LabelInfo _info;

        private LabelData _labelData;
        private bool _useLabel;

        private readonly Box _box;

        #endregion

        #region Constructor

        // Note:
        // The constructor of an Assembly always contains an object deriving from the AssemblyInfo class as an argument.
        // It is used to support the mechanism for Save/Load a model.  
        public Label(LabelInfo info) : base(info)
        {
            _info = info;

            // Note:
            // Create a new instance of type Experior.Core.Parts.Box
            // Primitive Shapes inside the namespace Experior.Core.Parts are not rigid by default.
            _box = new Box(Colors.LemonChiffon, 0.4f, 0.4f, 0.4f);

            // Note:
            // Every RigidPart must be added to the Assembly !
            Add(_box);

            _labelData = new LabelData();
        }

        #endregion

        #region Public Properties

        // Note:
        // Every public property is displayed in the Property Window.
        // If the property does not have any attributes, Experior automatically will place it inside the category Miscellaneous

        // Tips:
        // Attributes enhance the visualization of the properties.
        // [Category("Size")] -> Allocates the property inside a category
        // [DisplayName("Length")] -> Displays the name specified instead of the property name
        // [PropertyOrder(1)] -> Defines the property order in the category
        // [TypeConverter(typeof(FloatMeterToMillimeter))] -> Displays units in the property window

        [Category("Visualization")]
        [PropertyOrder(3)]
        [DisplayName("Use Label")]
        public bool UseLabel
        {
            get => _useLabel;
            set
            {
                _useLabel = value;
                if (value)
                {
                    Experior.Core.Environment.Scene.Label.Visible = true;
                    ShowLabel();
                }
                else
                {
                    Experior.Core.Environment.Scene.Label.Visible = false;
                }
            }
        }

        // Note:
        // Category is used by the Solution Explorer
        public override string Category => "Beginner";

        // Note:
        // Image is used by the Solution Explorer
        public override ImageSource Image => Common.Icon.Get("Label");

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is called only once by Experior when the component is dropped into the Scene.
        /// </summary>
        public override void Inserted()
        {
            base.Inserted();

            var message = "--------------------------------------------------------------------------------------------" +
                          "\n Sample: Assembly Label" +
                          "\n" +

                          "\n Description: " +
                          "\n 1) Use of ShowLabel() to modify the content of the Label" +
                          "\n 2) Modification of the Box color" +
                          "\n" +

                          "\n Usage: " +
                          "\n 1) Press CTRL + E to enable the visualization of Labels in the scene" +
                          "\n 2) Select the assembly Label" +
                          "\n --------------------------------------------------------------------------------------------";

            Log.Write(message, Colors.Orange, LogFilter.Information);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// This method is called by Experior when the Label tool is active and the Assembly has been selected.
        /// </summary>
        protected override void ShowLabel()
        {
            if (_labelData == null)
            {
                return;
            }

            _labelData.Text = $"Component: Label \n Position X: {Position.X}mm \n Position Y: {Position.Y}mm \n Position Z: {Position.Z}mm";
            _labelData.Position = _box.Position;

            Experior.Core.Environment.Scene.Label.Show(_labelData.Text, _labelData);
        }

        #endregion

        #region Nested Types

        // Note:
        // Custom class which implements the interface ILabel required by Experior.Core.Environment.Scene.Label.Show()
        private class LabelData : ILabel
        {
            public string Text { get; set; }

            public Vector3 Position { get; set; }
        }

        #endregion
    }

    // Note:
    // Attributes allow the developer to specify if a class is Serializable.
    // Each class must have a unique TypeName !
    [Serializable]
    [TypeConverter(typeof(LabelInfo))]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.LabelInfo")]

    public class LabelInfo : Experior.Core.Assemblies.AssemblyInfo
    {

    }
}
