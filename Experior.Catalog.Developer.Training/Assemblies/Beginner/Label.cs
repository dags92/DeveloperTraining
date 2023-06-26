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
    public class Label : Experior.Core.Assemblies.Assembly
    {
        #region Fields

        private readonly LabelInfo _info;

        private LabelData _labelData;
        private bool _useLabel;

        private readonly Box _box;

        #endregion

        #region Constructor

        public Label(LabelInfo info) : base(info)
        {
            _info = info;

            _box = new Box(Colors.LemonChiffon, 0.4f, 0.4f, 0.4f);
            Add(_box);

            _labelData = new LabelData();
        }

        #endregion

        #region Public Properties

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

        [Browsable(false)]
        public override bool Selected
        {
            get => base.Selected;
            set
            {
                base.Selected = value;

                //if (UseLabel)
                //    DisplayLabel1();
            }
        }

        public override string Category => "Beginner";

        public override ImageSource Image => Common.Icon.Get("Label");

        #endregion

        #region Protected Methods

        protected override void ShowLabel()
        {
            if (_labelData == null)
                return;

            _labelData.Text = $"Component: Label \n Position X: {Position.X}mm \n Position Y: {Position.Y}mm \n Position Z: {Position.Z}mm";
            _labelData.Position = _box.Position;

            Experior.Core.Environment.Scene.Label.Show(_labelData.Text, _labelData);
        }

        #endregion

        #region Nested Types

        private class LabelData : ILabel
        {
            public string Text { get; set; }

            public Vector3 Position { get; set; }
        }

        #endregion
    }

    [Serializable]
    [TypeConverter(typeof(LabelInfo))]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.LabelInfo")]

    public class LabelInfo : Experior.Core.Assemblies.AssemblyInfo
    {

    }
}
