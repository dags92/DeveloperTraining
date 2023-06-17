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
    public class FixPoints : Assembly
    {
        #region Fields

        private readonly FixPointsInfo _info;

        private readonly Box _box;
        private readonly FixPoint _start, _end;

        #endregion

        #region Constructor

        public FixPoints(FixPointsInfo info) : base(info)
        {
            _info = info;

            _start = new FixPoint(Colors.Red, FixPoint.Types.Start, this, 0.1f, 0.1f, 0.1f);
            Add(_start);
            _start.OnBeforeSnapping += StartOnBeforeSnapping;

            _end = new FixPoint(Colors.Blue, FixPoint.Types.End, this, 0.1f, 0.1f, 0.1f);
            Add(_end);
            _end.OnBeforeSnapping += EndOnBeforeSnapping;

            _box = new Box(Colors.Wheat, 1f, 0.05f, 0.5f);
            Add(_box);

            Refresh();
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
                    return;
                }

                _info.length = value;
                InvokeRefresh();
            }
        }

        public override string Category => "Intermediate";

        public override ImageSource Image => Common.Icon.Get("FixPoints");

        #endregion

        #region Public Methods

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

        public override void Dispose()
        {
            _start.OnBeforeSnapping -= StartOnBeforeSnapping;
            _end.OnBeforeSnapping -= EndOnBeforeSnapping;

            base.Dispose();
        }

        #endregion

        #region Private Methods

        private void StartOnBeforeSnapping(FixPoint sender, FixPoint stranger, FixPoint.SnapEventArgs e)
        {
            if (stranger.Type == FixPoint.Types.End)
            {
                return;
            }

            Log.Write("Only End Fix Point type is allowed to snap !", Colors.DarkOrange, LogFilter.Information);
            e.Cancel = true;
        }

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

    [TypeConverter(typeof(FixPointsInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.FixPointsInfo")]
    public class FixPointsInfo : AssemblyInfo
    {

    }
}
