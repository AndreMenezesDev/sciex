using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class MenuTest
    {
        private readonly IMenuBll _menuBll;

        public MenuTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _menuBll = CrossCutting.DependenceInjetion.Initialize.Instance<IMenuBll>(typeof(MenuBll));
        }

        [TestMethod]
        public void ListarMenus()
        {
            var menus = _menuBll.Listar();
        }
    }
}