using DotDll.Presentation.View;
using DotDll.Presentation.ViewModel.Common;
using NUnit.Framework;

namespace DotDll.Tests.Presentation.ViewModel.Common
{
    [TestFixture]
    public class RelayCommandTest
    {
        
        private readonly RelayCommandFactory _factory = new WpfRelayCommandFactory();
        
        [Test]
        public void RelayCommand_Always_RunPassedActionOnExecute()
        {
            var delegated = false;
            const string relayParam = "Some Param";

            var command = _factory.CreateCommand(param =>
            {
                Assert.IsInstanceOf<string>(param);
                Assert.AreEqual(relayParam, (string) param);
                delegated = true;
            });

            command.Execute(relayParam);

            Assert.True(delegated);
        }

        [Test]
        public void RelayCommand_NoPredicate_CanExecuteReturnsTrue()
        {
            var actual = _factory.CreateCommand(o => { }).CanExecute(null);
            Assert.True(actual);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void RelayCommand_PredicatePassed_CanExecuteReturnsPredicateReturnValue(bool predicateReturn)
        {
            var delegated = false;
            const string relayParam = "Some param";

            var relayCommand = _factory.CreateCommand(
                o => { },
                param =>
                {
                    Assert.IsInstanceOf<string>(param);
                    Assert.AreEqual(relayParam, (string) param);
                    delegated = true;
                    return predicateReturn;
                });


            var actual = relayCommand.CanExecute(relayParam);

            Assert.True(delegated);
            Assert.AreEqual(predicateReturn, actual);
        }
    }
}