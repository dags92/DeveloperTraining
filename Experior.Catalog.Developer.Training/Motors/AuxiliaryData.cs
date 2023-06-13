using System;
using System.Xml.Serialization;

namespace Experior.Catalog.Developer.Training.Motors
{
    [Serializable, XmlInclude(typeof(AuxiliaryData)), XmlType(TypeName = "Experior.Catalog.Developer.Training.Motors.AuxiliaryData")]
    public static class AuxiliaryData
    {
        [XmlType(TypeName = "Experior.Catalog.Developer.Training.Motors.MotorTypes")]
        public enum MotorTypes
        {
            Surface,
            Vector,
            Rotation
        }

        [XmlType(TypeName = "Experior.Catalog.Developer.Training.Motors.Commands")]
        public enum Commands
        {
            Forward = 1,
            Backward = -1,
            Stop = 0
        }

        [XmlType(TypeName = "Experior.Catalog.Developer.Training.Motors.DefaultVectorPositions")]
        public enum DefaultVectorPositions
        {
            Down,
            Middle,
            Up
        }

        [XmlType(TypeName = "Experior.Catalog.Developer.Training.Motors.VectorMovementLimits")]
        public enum VectorMovementLimits
        {
            Stop,
            Eccentric
        }
    }
}
