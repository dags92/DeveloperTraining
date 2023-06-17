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
    public class Dimensions : Assembly
    {
        #region Fields

        private readonly DimensionsInfo _info;

        private readonly Box _box;

        #endregion

        #region Constructor

        public Dimensions(DimensionsInfo info) : base(info)
        {
            _info = info;

            _box = new Box(Colors.Wheat, _info.length, _info.height, _info.width);
            Add(_box);
        }

        #endregion

        #region Public Properties

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
                    Log.Write("Length cannot be less than 0 mm", Colors.Orange, LogFilter.Information);
                    return;
                }

                _info.length = value;
                InvokeRefresh();
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
                    Log.Write("Height cannot be less than 0 mm", Colors.Orange, LogFilter.Information);
                    return;
                }

                _info.height = value;
                InvokeRefresh();
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
                    Log.Write("Width cannot be less than 0 mm", Colors.Orange, LogFilter.Information);
                    return;
                }

                _info.width = value;
                InvokeRefresh();
            }
        }

        public override string Category => "Beginner";

        public override ImageSource Image => Common.Icon.Get("Dimensions");

        #endregion

        #region Public Methods

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

    [TypeConverter(typeof(DimensionsInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.DimensionsInfo")]
    public class DimensionsInfo : AssemblyInfo
    {

    }
}
