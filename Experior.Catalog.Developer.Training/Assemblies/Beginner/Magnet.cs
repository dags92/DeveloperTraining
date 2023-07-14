using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Core.Assemblies;
using Experior.Core.Loads;
using Experior.Core.Parts.Sensors;
using Experior.Interfaces;
using Box = Experior.Core.Parts.Sensors.Box;

namespace Experior.Catalog.Developer.Training.Assemblies.Beginner
{
    /// <summary>
    /// Class <c>Magnet</c> exemplifies the use of Experior.Core.Parts.Sensors.Box to attach a Load to the sensor.
    /// </summary>
    public class Magnet : Assembly
    {
        #region Fields

        private readonly MagnetInfo _info;

        private readonly Box _sensor;
        private bool _attach;

        #endregion

        #region Constructor

        // Note:
        // The constructor of an Assembly always contains an object deriving from the AssemblyInfo class as an argument.
        // It is used to support the mechanism for Save/Load a model.
        public Magnet(MagnetInfo info) : base(info)
        {
            _info = info;

            // Note:
            // Create a new instance of type Experior.Core.Parts.Sensors.Box
            // Sensors are able to detect Loads and Equipment (RigidParts which has the property Rigid true).
            _sensor = new Box(Colors.DodgerBlue, 0.5f, 0.5f, 0.5f)
            {
                Collision = Collisions.Loads
            };

            // Note:
            // Every Experior.Core.Parts.Sensors.Box must be added to the Assembly !
            Add(_sensor);

            //Note:
            // Experior.Core.Parts.Sensors notify when an object of the same type specified through the property Collision is detected.
            _sensor.OnEnter += SensorOnEnter;
        }

        #endregion

        #region Public Properties

        // Note:
        // Category is used by the Solution Explorer
        public override string Category => "Intermediate";

        // Note:
        // Image is used by the Solution Explorer
        public override ImageSource Image => Common.Icon.Get("Magnet");

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is called only once by Experior when the component is dropped into the Scene.
        /// </summary>
        public override void Inserted()
        {
            base.Inserted();

            var message = "--------------------------------------------------------------------------------------------" +
                          "\n Sample: Magnet" +
                          "\n" +

                          "\n Description: " +
                          "\n 1) Use of Experior.Core.Part.Sensors.Box" +
                          "\n 2) Use of Attach() and UnAttach()" +
                          "\n" +

                          "\n Usage: " +
                          "\n 1) Create a load by pressing the key F" +
                          "\n 2) Move the load to collide it with the sensor" +
                          "\n 3) Press the key A to attach the load to the sensor" +
                          "\n --------------------------------------------------------------------------------------------";

            Log.Write(message, Colors.Orange, LogFilter.Information);
        }

        /// <summary>
        /// This method is called only once by Experior when the current Assembly has been selected, and the user press a key.
        /// </summary>
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

        /// <summary>
        /// This method is called by Experior when the Assembly is deleted from the scene.
        /// It is used to unsubscribe events.
        /// </summary>
        public override void Dispose()
        {
            _sensor.OnEnter -= SensorOnEnter;

            base.Dispose();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method is used to attach loads to the sensor.
        /// </summary>
        private void AttachLoads(List<Load> loads)
        {
            // Note:
            // When a load is attached to a Sensor, it becomes kinematics. Therefore, the load does not experience forces
            // and its position will be defined by the sensor which the load has been attached to.
            _sensor.Attach(loads);
        }

        /// <summary>
        /// This method is used to release the loads attached to the sensor.
        /// </summary>
        private void UnAttachAll()
        {
            // Note:
            // When a load is unattached from the sensor, it will become dynamic again (kinematic = false).
            _sensor.UnAttach();
        }

        /// <summary>
        /// This method is called by Experior when the sensor collides with an object of the same type specified through Collision.
        /// </summary>
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

    // Note:
    // Attributes allow the developer to specify if a class is Serializable.
    // Each class must have a unique TypeName !
    [Serializable]
    [TypeConverter(typeof(MagnetInfo))]
    [XmlType(TypeName = "Experior.Catalog.Developer.Training.Assemblies.Beginner.MagnetInfo")]
    public class MagnetInfo : AssemblyInfo
    {

    }  
}
