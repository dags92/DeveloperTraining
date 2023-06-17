using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core.Assemblies;
using Experior.Core.Loads;
using Experior.Core.Parts.Sensors;
using Box = Experior.Core.Parts.Sensors.Box;

namespace Experior.Catalog.Developer.Training.Assemblies.Beginner
{
    public class Magnet : Assembly
    {
        #region Fields

        private readonly MagnetInfo _info;

        private readonly Box _sensor;
        private bool _attach;

        #endregion

        #region Constructor

        public Magnet(MagnetInfo info) : base(info)
        {
            _info = info;

            _sensor = new Box(Colors.DodgerBlue, 0.5f, 0.5f, 0.5f);
            Add(_sensor);

            _sensor.OnEnter += SensorOnEnter;
        }

        #endregion

        #region Public Properties

        public override string Category => "Intermediate";

        public override ImageSource Image => Common.Icon.Get("Magnet");

        #endregion

        #region Public Methods

        public override void KeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                _attach = !_attach;

                if (_attach)
                {
                    if (_sensor.Loads.Count > 0)
                    {
                        AttachLoads(_sensor.Loads);
                    }
                }
                else
                {
                    UnAttachAll();
                }
            }
        }

        public override void Dispose()
        {
            _sensor.OnEnter -= SensorOnEnter;

            base.Dispose();
        }

        #endregion

        #region Private Methods

        private void AttachLoads(List<Load> loads)
        {
            _sensor.Attach(loads);
        }

        private void UnAttachAll()
        {
            _sensor.UnAttach();
        }

        private void SensorOnEnter(Sensor sensor, object trigger)
        {
            if (!(trigger is Load load) || !_attach)
            {
                return;
            }

            AttachLoads(new List<Load> {load});
        }

        #endregion
    }

    [TypeConverter(typeof(MagnetInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.MagnetInfo")]
    public class MagnetInfo : AssemblyInfo
    {

    }  
}
