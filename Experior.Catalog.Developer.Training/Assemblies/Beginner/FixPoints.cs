using System;
using System.ComponentModel;
using System.Numerics;
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
    /// Class <c>FixPoints</c> exemplifies the use of the Fix Points to snap Assemblies.
    /// </summary>
    public class FixPoints : Assembly
    {
        #region Fields

        private readonly FixPointsInfo _info;

        private readonly Box _box;
        private readonly FixPoint _start, _end;

        #endregion

        #region Constructor

        // Note:
        // The constructor of an Assembly always contains an object deriving from the AssemblyInfo class as an argument.
        // It is used to support the mechanism for Save/Load a model.   
        public FixPoints(FixPointsInfo info) : base(info)
        {
            _info = info;

            // Note:
            // Create a new instance of type Experior.Core.Parts.FixPoint
            // FixPoint are used to snap Assemblies. Snap process occurs when the user moves the Assembly and holds the CTRL key.
            // The Assembly which owns the Fix Point must move close enough to another Fix Point, so that Experior is capable to
            // detect the collision between them.
            _start = new FixPoint(Colors.Red, FixPoint.Types.Start, this, 0.1f, 0.1f, 0.1f);

            // Note:
            // Every FixPoint must be added to the Assembly !
            Add(_start);

            // Note:
            // Multiple Events are provided in order to notify the developer if the Fix Point is about to start the snapping process,
            // if it has been snapped or un-snapped.
            _start.OnBeforeSnapping += StartOnBeforeSnapping;

            _end = new FixPoint(Colors.Blue, FixPoint.Types.End, this, 0.1f, 0.1f, 0.1f);
            Add(_end);
            _end.OnBeforeSnapping += EndOnBeforeSnapping;

            // Note:
            // Create a new instance of type Experior.Core.Parts.Box
            // Primitive Shapes inside the namespace Experior.Core.Parts are not rigid by default.
            _box = new Box(Colors.Wheat, 1f, 0.05f, 0.5f);

            // Note:
            // Every RigidPart must be added to the Assembly !
            Add(_box);

            Refresh();
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
                    // Note:
                    // Log class allows the developer to display custom messages in the Log Window.
                    Log.Warning("Length cannot be less than 0 mm");
                    return;
                }

                // Note:
                //  Every property value changed from the Property Window (UI) is handled by the Main Thread.
                //  On the other hand, changes regarding the visualization, position, creation or deletion
                //  of RigidParts/Assemblies must be handled by the Engine Thread. Therefore it is required to invoke it. 
                //  Invoke(Refresh) Invokes the Engine Thread to execute the method <c>Refresh</c> !

                _info.length = value;
                InvokeRefresh();
            }
        }

        // Note:
        // Category is used by the Solution Explorer
        public override string Category => "Beginner";

        // Note:
        // Image is used by the Solution Explorer
        public override ImageSource Image => Common.Icon.Get("FixPoints");

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is called only once by Experior when the component is dropped into the Scene.
        /// </summary>
        public override void Inserted()
        {
            base.Inserted();

            var message = "--------------------------------------------------------------------------------------------" +
                          "\n Sample: Fix Points" +
                          "\n" +

                          "\n Description: " +
                          "\n 1) Use of Experior.Core.Parts.FixPoint" +
                          "\n 2) Snapping Assemblies" +
                          "\n" +

                          "\n Usage: " +
                          "\n 1) Drop another Fix Point assembly into the scene" +
                          "\n 2) Move the assembly to overlap the Red Fix Point with the Blue Fix Point" +
                          "\n 3) Press the CTRL key" +
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

            _start.LocalPosition = new Vector3(-Length / 2, 0f, 0f);
            _end.LocalPosition = new Vector3(Length / 2, 0f, 0f);
            _box.LocalPosition = new Vector3(0f, -_box.Height / 2, 0f);
        }

        /// <summary>
        /// This method is called by Experior when the Assembly is deleted from the scene. It is used to unsubscribe events.
        /// </summary>
        public override void Dispose()
        {
            _start.OnBeforeSnapping -= StartOnBeforeSnapping;
            _end.OnBeforeSnapping -= EndOnBeforeSnapping;

            base.Dispose();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method approves or rejects the snapping process based on the Fix Point types.
        /// </summary>
        private void StartOnBeforeSnapping(FixPoint sender, FixPoint stranger, FixPoint.SnapEventArgs e)
        {
            if (stranger.Type == FixPoint.Types.End)
            {
                return;
            }

            Log.Write("Only End Fix Point type is allowed to snap !", Colors.DarkOrange, LogFilter.Information);
            e.Cancel = true;
        }

        /// <summary>
        /// This method approves or rejects the snapping process based on the Fix Point types.
        /// </summary>
        private void EndOnBeforeSnapping(FixPoint sender, FixPoint stranger, FixPoint.SnapEventArgs e)
        {
            if (stranger.Type == FixPoint.Types.Start)
            {
                return;
            }

            Log.Write("Only Start Fix Point type is allowed to snap !", Colors.DarkOrange, LogFilter.Information);
            e.Cancel = true;
        }

        #endregion
    }

    // Note:
    // Attributes allow the developer to specify if a class is Serializable.
    // Each class must have a unique TypeName !
    [Serializable]
    [TypeConverter(typeof(FixPointsInfo))]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.FixPointsInfo")]
    public class FixPointsInfo : AssemblyInfo
    {

    }
}
