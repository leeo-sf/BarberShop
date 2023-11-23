using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime;

namespace barber_shop_tests
{
    [TestClass]
    public class AccessControllerTest
    {
        [TestMethod]
        [Owner("Leonardo")]
        [Description("Verifica se usuário está tendo acesso com dados válidos.")]
        public void LoginWithValidUser()
        {
        }

        [TestMethod]
        [Owner("Leonardo")]
        [Description("Verificar se usuário obtem erro ao logar com os dados inválidos.")]
        public void LoginWithInvalidUser()
        {
        }
    }
}
