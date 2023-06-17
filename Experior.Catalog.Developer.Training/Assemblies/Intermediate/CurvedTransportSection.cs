using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core.Assemblies;
using Experior.Core.Mathematics;
using Experior.Core.Parts;
using Experior.Core.Properties.TypeConverter;
using Experior.Core.Properties;
using Experior.Core.TransportSections;
using Experior.Rendering.Interfaces;

namespace Experior.Catalog.Developer.Training.Assemblies.Intermediate
{
    public class CurvedTransportSection : Experior.Core.Assemblies.Assembly
    {
        #region Fields

        private readonly CurvedTransportSectionInfo _info;

        private readonly FixPoint _start, _end;
        private readonly Core.TransportSections.CurveTransportSection _section;

        #endregion

        #region Constructor

        public CurvedTransportSection(CurvedTransportSectionInfo info) : base(info)
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

            _section = new CurveTransportSection(Colors.SlateGray, Revolution.Clockwise, _info.Radius, _info.Angle, 0.5f)
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
        [DisplayName("Angle")]
        [PropertyOrder(1)]
        [TypeConverter(typeof(Degrees))]
        public float Angle
        {
            get => _info.Angle;
            set
            {
                if (value <= 0)
                {
                    return;
                }

                _info.Angle = value;
                InvokeRefresh();
            }
        }

        [Category("Size")]
        [DisplayName("Radius")]
        [PropertyOrder(2)]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        public float Radius
        {
            get => _info.Radius;
            set
            {
                if (value <= 0)
                {
                    return;
                }

                _info.Radius = value;
                InvokeRefresh();
            }
        }

        public override string Category => "Intermediate";

        public override ImageSource Image => Common.Icon.Get("CurvedTransportSection");

        #endregion

        #region Public Methods

        public override void Refresh()
        {
            if (_info == null)
            {
                return;
            }

            base.Refresh();

            _section.Angle = Angle;
            _section.Radius = Radius;
            
            _start.LocalPosition = new Vector3(0, 0, Radius);
            _end.LocalPosition = Trigonometry.RotatePoint(new Vector3(0, 0, 0), Angle.ToRadians(), Radius);
            _end.LocalYaw = 180f.ToRadians();
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

    [TypeConverter(typeof(CurvedTransportSectionInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Intermediate.CurvedTransportSectionInfo")]
    public class CurvedTransportSectionInfo : Experior.Core.Assemblies.AssemblyInfo
    {
        public float Angle { get; set; } = 180f;

        public float Radius { get; set; } = 0.6f;
    }
}
