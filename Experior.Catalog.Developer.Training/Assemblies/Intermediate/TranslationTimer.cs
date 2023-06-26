using System;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core;
using Experior.Core.Assemblies;
using Experior.Core.Parts;
using Experior.Core.Properties;
using Experior.Core.Properties.TypeConverter;
using Colors = System.Windows.Media.Colors;

namespace Experior.Catalog.Developer.Training.Assemblies.Intermediate
{
    public class TranslationTimer : Assembly
    {
        #region Fields

        private readonly TranslationTimerInfo _info;

        private readonly Model _back, _body, _front;
        private readonly Cylinder _piston;
        private readonly Box _pusher;

        private readonly Timer.Translate _timer;
        private bool _ejected;

        #endregion

        #region Constructor

        public TranslationTimer(TranslationTimerInfo info) : base(info)
        {
            _info = info;

            _back = new Model(Common.Mesh.Get("BackBase_Cylinder.stl")) { Color = Colors.Silver };
            _body = new Model(Common.Mesh.Get("Body_Cylinder.stl")) { Color = Colors.Silver };
            _front = new Model(Common.Mesh.Get("FrontBase_Cylinder.stl")) { Color = Colors.Silver };

            Add(_back);
            Add(_body);
            Add(_front);

            _piston = new Cylinder(Colors.DimGray, 0.5f, 0.01f, 10);
            Add(_piston);

            _pusher = new Box(Colors.DarkOrange, 0.02f, 0.1f, 0.2f) { Rigid = true };
            Add(_pusher);

            _timer = new Timer.Translate();
            _timer.Add(_piston);
            _timer.Add(_pusher);

            Refresh();
        }

        #endregion

        #region Public Properties

        [Category("Motion")]
        [DisplayName("Distance")]
        [PropertyOrder(0)]
        [TypeConverter(typeof(FloatMeterToMillimeter))]
        public float Distance
        {
            get => _info.Distance;
            set
            {
                if (value <= 0 || _timer.Started)
                {
                    return;
                }

                _info.Distance = value;
                InvokeRefresh();
            }
        }

        [Category("Motion")]
        [DisplayName("Time")]
        [PropertyOrder(1)]
        [TypeConverter(typeof(Seconds))]
        public float Time
        {
            get => _info.Time;
            set
            {
                if (value <= 0 || _timer.Started)
                {
                    return;
                }

                _info.Time = value;
                InvokeRefresh();
            }
        }

        public override string Category => "Intermediate";

        public override ImageSource Image => Common.Icon.Get("TranslationTimer");

        #endregion

        #region Public Methods

        public override void Refresh()
        {
            if (_info == null)
            {
                return;
            }

            _body.Length = Distance;
            _body.LocalPosition = new Vector3(-0.048f, 0, 0);

            _front.LocalPosition = new Vector3(0, 0, 0);
            _back.LocalPosition = new Vector3(-_body.Length + 0.048f, 0, 0);

            _piston.LocalYaw = (float)Math.PI / 2;
            _piston.Length = Distance * 1.05f;
            _piston.LocalPosition = new Vector3(-_piston.Length / 2 + 0.01f, 0, 0);
            _pusher.LocalPosition = _piston.LocalPosition + new Vector3(_piston.Length / 2 + _pusher.Length / 2, 0, 0);
        }

        public override void KeyDown(KeyEventArgs e)
        {
            if (_timer.Started)
            {
                return;
            }

            if (e.Key == Key.D)
            {
                Invoke(Eject);
            }
            else if (e.Key == Key.A)
            {
                Invoke(Inject);
            }
        }

        #endregion

        #region Private Methods

        private void Eject()
        {
            if (_ejected)
            {
                return;
            }

            _timer.Start(() =>
            {
                _ejected = true;
            }, new Vector3(Distance, 0, 0), Time);
        }

        private void Inject()
        {
            if (!_ejected)
            {
                return;
            }

            _timer.Start(() =>
            {
                _ejected = false;
            }, new Vector3(-Distance, 0, 0), Time);
        }

        #endregion
    }

    [TypeConverter(typeof(TranslationTimerInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Intermediate.TranslationTimerInfo")]
    public class TranslationTimerInfo : AssemblyInfo
    {
        public float Distance { get; set; } = 0.3f;
        public float Time { get; set; } = 1f;
    }
}
