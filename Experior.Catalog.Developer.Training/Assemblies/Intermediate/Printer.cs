using System;
using System.ComponentModel;
using System.Numerics;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core.Assemblies;
using Experior.Core.Loads;
using Experior.Core.Parts.Sensors;
using static System.Windows.Media.ColorConverter;
using Box = Experior.Core.Parts.Sensors.Box;

namespace Experior.Catalog.Developer.Training.Assemblies.Intermediate
{
    public class Printer : Assembly
    {
        #region Fields

        private readonly PrinterInfo _info;

        private Box _sensor;

        #endregion

        #region Constructor

        public Printer(PrinterInfo info) : base(info)
        {
            _info = info;

            _sensor = new Box(Colors.DodgerBlue, 0.2f, 0.2f, 0.2f);
            Add(_sensor);

            _sensor.OnEnter += SensorOnEnter;
        }

        #endregion

        #region Public Properties

        public override string Category => "Intermediate";

        public override ImageSource Image => Common.Icon.Get("Printer");

        #endregion

        #region Public Methods

        public override void Dispose()
        {
            _sensor.OnEnter -= SensorOnEnter;

            base.Dispose();
        }

        #endregion

        #region Private Methods

        private void SensorOnEnter(Sensor sensor, object trigger)
        {
            if (!(trigger is Load load))
            {
                return;
            }

            Mesh stamp = new Mesh(Common.Mesh.Get("SchneiderLogo"));
            stamp.Color = (Color)ConvertFromString("#FF3DCD58");
            
            load.Color = Colors.Wheat;
            load.Group(stamp, new Vector3(0, load.Height / 2 - stamp.Height / 2 + 0.0001f, 0), Matrix4x4.CreateFromYawPitchRoll(Yaw + load.Yaw, 0, 0));
        }

        #endregion
    }

    [TypeConverter(typeof(PrinterInfo))]
    [Serializable]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Intermediate.PrinterInfo")]
    public class PrinterInfo : AssemblyInfo
    {

    }
}
