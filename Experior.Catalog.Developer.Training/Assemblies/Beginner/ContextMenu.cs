using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core.Assemblies;
using Experior.Core.Parts;
using Colors = System.Windows.Media.Colors;
using Environment = Experior.Core.Environment;

namespace Experior.Catalog.Developer.Training.Assemblies.Beginner
{
    public class ContextMenu : Assembly
    {
        #region Fields

        private readonly ContextMenuInfo _info;

        private readonly Box _box;
        private bool _isActive;

        #endregion

        #region Constructor

        public ContextMenu(ContextMenuInfo info) : base(info)
        {
            _info = info;

            _box = new Box(Colors.Wheat, 0.5f, 0.5f, 0.5f);
            Add(_box);
        }

        #endregion

        #region Public Properties

        public override string Category => "Beginner";

        public override ImageSource Image => Common.Icon.Get("ContextMenu");

        #endregion

        #region Protected Properties

        protected bool IsActive
        {
            get => _isActive;
            private set
            {
                _isActive = value;
                Invoke(ModifyColor);
            }
        }

        #endregion

        #region Public Methods

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

        private void ModifyColor()
        {
            _box.Color = IsActive ? Colors.LimeGreen : Colors.Wheat;
            Deselect();
        }

        #endregion
    }

    [TypeConverter(typeof(ContextMenuInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.ContextMenuInfo")]
    public class ContextMenuInfo : AssemblyInfo
    {

    }
}
