using DotDll.Presentation.ViewModel.Common;
using NUnit.Framework;

namespace DotDll.Tests.Presentation.ViewModel
{
    [TestFixture]
    public class RelayCommandTest
    {
        [Test]
        public void RelayCommand_Always_RunPassedActionOnExecute()
        {
            var delegated = false;
            var relayParam = "Some Param";
            
            var command = new RelayCommand((param =>
            {
                Assert.IsInstanceOf<string>(param);
                Assert.AreEqual(relayParam, (string) param);
                delegated = true;
            }));
            
            command.Execute(relayParam);
            
            Assert.True(delegated);
        }
        
        [Test]
        public void RelayCommand_NoPredicate_CanExecuteReturnsTrue()
        {
            var actual = new RelayCommand((o => { })).CanExecute(null);
            Assert.True(actual);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void RelayCommand_PredicatePassed_CanExecuteReturnsPredicateReturnValue(bool predicateReturn)
        {
            var delegated = false;
            var relayParam = "Some param";
            
            var relayCommand = new RelayCommand(
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