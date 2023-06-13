using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Experior.Core.Communication.PLC;
using Experior.Core.Properties;

namespace Experior.Catalog.Developer.Training.Motors.Parts
{
    [TypeConverter(typeof(ObjectConverter))]
    [Serializable, XmlInclude(typeof(MechanicalSwitch)), XmlType(TypeName = "Experior.Catalog.Developer.Training.Motors.Parts.MechanicalSwitch")]
    public class MechanicalSwitch
    {
        #region Public Properties

        public Output State { get; set; }

        public bool Enabled { get; set; }

        [XmlIgnore]
        public bool Warning => State.Warning;

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return "Mechanical Switch";
        }

        #endregion
    }
}
