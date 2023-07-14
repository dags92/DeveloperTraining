using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core.Assemblies;
using Experior.Core.Parts;
using Experior.Interfaces;
using Colors = System.Windows.Media.Colors;
using Environment = Experior.Core.Environment;

namespace Experior.Catalog.Developer.Training.Assemblies.Beginner
{
    /// <summary>
    /// Class <c>ContextMenuSampleSample</c> exemplifies the use of the Context Menu of the Assembly to modify the color of a Box.
    /// </summary>
    public class ContextMenuSample : Assembly
    {
        #region Fields

        private readonly ContextMenuSampleInfo _info;

        private readonly Box _box;
        private bool _isActive;

        #endregion

        #region Constructor

        // Note:
        // The constructor of an Assembly always contains an object deriving from the AssemblyInfo class as an argument.
        // It is used to support the mechanism for Save/Load a model.
        public ContextMenuSample(ContextMenuSampleInfo info) : base(info)
        {
            _info = info;

            // Note:
            // Create a new instance of type Experior.Core.Parts.Box
            // Primitive Shapes inside the namespace Experior.Core.Parts are not rigid by default.
            _box = new Box(Colors.Wheat, 0.5f, 0.5f, 0.5f);

            // Note:
            // Every RigidPart must be added to the Assembly !
            Add(_box);
        }

        #endregion

        #region Public Properties

        // Note:
        // Category is used by the Solution Explorer
        public override string Category => "Beginner"; 

        // Note:
        // Image is used by the Solution Explorer
        public override ImageSource Image => Common.Icon.Get("ContextMenuSample"); 

        #endregion

        #region Protected Properties

        protected bool IsActive
        {
            get => _isActive;
            private set
            {
                //  Every property value changed from the Property Window (UI) is handled by the Main Thread.
                //  On the other hand, changes regarding the visualization, position, creation or deletion
                //  of RigidParts/Assemblies must be handled by the Engine Thread. Therefore it is required to invoke it. 
                //  Invoke(ModifyColor) Invokes the Engine Thread to execute the method <c>ModifyColor</c> !

                _isActive = value;     
                Invoke(ModifyColor);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is called only once by Experior when the component is dropped into the Scene.
        /// </summary>
        public override void Inserted()
        {
            base.Inserted();

            var message = "--------------------------------------------------------------------------------------------" +
                          "\n Sample: Context Menu" +
                          "\n" +

                          "\n Description: " +
                          "\n 1) Use of ShowContextMenu()" +
                          "\n 2) Modification of the Box color" +
                          "\n" +

                          "\n Usage: " +
                          "\n 1) Select the box" +
                          "\n 2) Use right click on the box to display the Context Menu" +
                          "\n --------------------------------------------------------------------------------------------";

            Log.Write(message, Colors.Orange, LogFilter.Information);
        }

        /// <summary>
        /// This method is called by Experior when the user uses right click on the selected Assembly.
        /// </summary>
        public override List<Environment.UI.Toolbar.BarItem> ShowContextMenu()
        {
            var menu = new List<Environment.UI.Toolbar.BarItem>();
            
            if (IsActive)
            {
                menu.Add(new Environment.UI.Toolbar.Button("De-activate", Common.Icon.Get("Wheat.png"))
                {
                    OnClick = (sender, args) => IsActive = false
                });
            }
            else
            {
                menu.Add(new Environment.UI.Toolbar.Button("Activate", Common.Icon.Get("Green.png"))
                {
                    OnClick = (sender, args) => IsActive = true
                });
            }
            
            return menu;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method created to modify the color of the Box based on the property <c>IsActive</c>.
        /// </summary>
        private void ModifyColor()
        {
            _box.Color = IsActive ? Colors.LimeGreen : Colors.Wheat;
            Deselect();
        }

        #endregion
    }

    // Note:
    // Attributes allow the developer to specify if a class is Serializable.
    // Each class must have a unique TypeName !
    [Serializable]
    [TypeConverter(typeof(ContextMenuSampleInfo))]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.ContextMenuSampleInfo")] 
    public class ContextMenuSampleInfo : AssemblyInfo
    {

    }
}
