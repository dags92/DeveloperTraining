using System;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core.Assemblies;
using Experior.Core.Mathematics;
using Experior.Core.Parts;
using Experior.Core.Properties.TypeConverter;
using Experior.Core.Properties;

namespace Experior.Catalog.Developer.Training.Assemblies.Intermediate
{
    public class StraightTransportSection : Assembly
    {
        #region Fields

        private readonly StraightTransportSectionInfo _info;

        private readonly FixPoint _start, _end;
        private readonly Core.TransportSections.StraightTransportSection _section;

        #endregion

        #region Constructor

        public StraightTransportSection(StraightTransportSectionInfo info) : base(info)
        {
            _info = info;

            _start = new FixPoint(Colors.Red, FixPoint.Types.Start, this);
            Add(_start);

            _start.OnBeforeSnapping += StartOnBeforeSnapping;
            _start.OnSnapped += StartOnSnapped;
            _start.OnUnSnapped += StartOnUnSnapped;

            _end = new FixPoint(Colors.Blue, FixPoint.Types.End, this);
            Add(_end);

            _end.OnBeforeSnapping += EndOnBeforeSnapping;
            _end.OnSnapped += EndOnSnapped;
            _end.OnUnSnapped += EndOnUnSnapped;

            _section = new Core.TransportSections.StraightTransportSection(Colors.LightGray, 1f, 0.5f)
            {
                Visible = true
            };
            Add(_section);
            
            _start.Route = _section.Route;
            _end.Route = _section.Route;

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

        public override ImageSource Image => Common.Icon.Get("StraightTransportSection");

        #endregion

        #region Public Methods

        public override void Refresh()
        {
            if (_info == null)
            {
                return;
            }

            base.Refresh();

            _section.Length = Length;
            _section.LocalYaw = 180f.ToRadians();
            _start.LocalPosition = new Vector3(-_section.Length / 2, 0, 0);
            _end.LocalPosition = new Vector3(_section.Length / 2, 0, 0);
        }

        public override void DoubleClick()
        {
            Invoke(CreateLoad);
        }

        public override void Dispose()
        {
            _start.OnBeforeSnapping -= StartOnBeforeSnapping;
            _start.OnSnapped -= StartOnSnapped;
            _start.OnUnSnapped -= StartOnUnSnapped;

            _end.OnBeforeSnapping -= EndOnBeforeSnapping;
            _end.OnSnapped -= EndOnSnapped;
            _end.OnUnSnapped -= EndOnUnSnapped;

            base.Dispose();
        }

        #endregion

        #region Private Methods

        private void CreateLoad()
        {
            var load = Experior.Core.Loads.Load.CreateBox(0.2f, 0.2f, 0.2f, Colors.SandyBrown);
            load.Switch(_section.Route);
        }

        private void StartOnBeforeSnapping(FixPoint sender, FixPoint stranger, FixPoint.SnapEventArgs e)
        {
            if (stranger.Type != FixPoint.Types.End)
            {
                e.Cancel = true;
            }
        }

        private void StartOnSnapped(FixPoint stranger, FixPoint.SnapEventArgs e)
        {
            _section.Route.PreviousRoute = stranger.Route;
        }

        private void StartOnUnSnapped(FixPoint stranger)
        {
            _section.Route.PreviousRoute = null;
        }

        private void EndOnBeforeSnapping(FixPoint sender, FixPoint stranger, FixPoint.SnapEventArgs e)
        {
            if (stranger.Type != FixPoint.Types.Start)
            {
                e.Cancel = true;
            }
        }

        private void EndOnSnapped(FixPoint stranger, FixPoint.SnapEventArgs e)
        {
            _section.Route.NextRoute = stranger.Route;
        }

        private void EndOnUnSnapped(FixPoint stranger)
        {
            _section.Route.NextRoute = null;
        }

        #endregion
    }

    [TypeConverter(typeof(StraightTransportSectionInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Intermediate.StraightTransportSectionInfo")]
    public class StraightTransportSectionInfo : AssemblyInfo
    {

    }
}
