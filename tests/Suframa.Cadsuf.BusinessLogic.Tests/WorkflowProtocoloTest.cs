using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class WorkflowProtocoloTest
    {
        private readonly IWorkflowProtocoloBll _workflowProtocoloBll;

        public WorkflowProtocoloTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();
            _workflowProtocoloBll = CrossCutting.DependenceInjetion.Initialize.Instance<WorkflowProtocoloBll>(typeof(WorkflowProtocoloBll));
        }

        [TestMethod]
        public void CalcularDiasRestantesPrimeiraRegra()
        {
            var workflowProtocoloDto = new WorkflowProtocoloDto
            {
                DataAguardandoReanalise = new DateTime(2017, 11, 10),
                DataAtual = new DateTime(2018, 01, 16),
                DataComPendencia = new DateTime(2017, 11, 6),
                DataConferenciaAdministrativa = new DateTime(2017, 11, 21),
                DataDesignacao = new DateTime(2017, 11, 1),
                DataEmAnalise = new DateTime(2017, 11, 6),
                QuantidadeDiasAnalise = 5
            };

            var diasRestantes = _workflowProtocoloBll.CalcularDiasRestantes(workflowProtocoloDto);
        }

        [TestMethod]
        public void CalcularDiasRestantesSegundaRegra()
        {
            var WorkflowProtocoloDto = new WorkflowProtocoloDto
            {
                DataAtual = new DateTime(2018, 01, 16),
                DataComPendencia = new DateTime(2018, 01, 16),
                DataDesignacao = new DateTime(2018, 01, 16),
                QuantidadeDiasAnalise = 5
            };

            var diasRestantes = _workflowProtocoloBll.CalcularDiasRestantes(WorkflowProtocoloDto);
        }
    }
}