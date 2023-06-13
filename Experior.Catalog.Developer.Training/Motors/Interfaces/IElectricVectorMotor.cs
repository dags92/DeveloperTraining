using Experior.Catalog.Developer.Training.Motors.Collections;
using Experior.Core.Communication.PLC;
using Experior.Interfaces;


namespace Experior.Catalog.Developer.Training.Motors.Interfaces
{
    public interface IElectricVectorMotor : IElectricMotor
    {
        VectorPartCollection Parts { get; }

        VectorAssemblyCollection Assemblies { get; }

        AuxiliaryData.VectorMovementLimits VectorMovementLimit { get; set; }

        AuxiliaryData.DefaultVectorPositions DefaultPosition { get; set; }

        Output OutputMaxLimit { get; set; }

        Output OutputMidLimit { get; set; }

        Output OutputMinLimit { get; set; }

        void Calibrate();
    }
}
