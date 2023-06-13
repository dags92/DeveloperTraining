using System;
using System.Windows.Media;
using System.Xml.Serialization;
using Experior.Catalog.Developer.Training.Motors.Interfaces;

namespace Experior.Catalog.Developer.Training.Motors.Basic
{
    /// <summary>
    /// Class <c>Surface</c> designed to be used with conveyor belts.
    /// </summary>
    public class Surface : Base, IElectricSurfaceMotor
    {
        #region Fields

        private readonly SurfaceInfo _info;

        #endregion

        #region Constructor

        public Surface(SurfaceInfo info) : base(info)
        {
            _info = info;
        }

        #endregion

        #region Public Properties

        public override ImageSource Image => Common.Icon.Get("SurfaceMotor_Icon");

        #endregion

        #region Protected Methods

        protected override string GetMotorName() => GetValidName("Basic Surface Motor ");

        #endregion
    }

    [Serializable, XmlInclude(typeof(SurfaceInfo)), XmlType(TypeName = "Experior.Catalog.Developer.Training.Motors.Basic.Surface")]
    public class SurfaceInfo : BaseInfo
    {

    }
}
