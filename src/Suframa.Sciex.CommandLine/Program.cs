using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DependenceInjetion;
using System;

namespace Suframa.Sciex.CommandLine
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            Initialize.InitSingleton();
           
        }
    }
}