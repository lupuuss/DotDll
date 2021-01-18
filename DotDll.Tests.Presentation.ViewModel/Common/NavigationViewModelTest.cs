using System;
using DotDll.Logic.Metadata.Sources;
using DotDll.Presentation.Model.Navigation;
using DotDll.Presentation.ViewModel.Common;
using Moq;
using NUnit.Framework;

namespace DotDll.Tests.Presentation.ViewModel.Common
{
    [TestFixture]
    public class NavigationViewModelTest
    {
        private Mock<RelayCommandFactory> _factoryMock;
        
        private RelayCommandFactory _factory;
        
        [SetUp]
        public void SetUp()
        {
            _navigatorMock = new Mock<INavigator>();
            _factoryMock = new Mock<RelayCommandFactory>();

            _factoryMock.Setup(f => f.CreateCommand(
                    It.IsAny<Action<Object>>(),
                    It.IsAny<Predicate<Object>>()
                )
            ).Returns<Action<object>,Predicate<object>?>(
                (action, predicate) => new TestRelayCommand(action, predicate)
            );

            _factory = _factoryMock.Object;
            
            _navigatorMock.Setup(
                navigator => navigator.NavigateTo(TargetView.MetaData)
            ).Throws(new ArgumentException("Argument expected"));

            _viewModel = new NavigationViewModel(_navigatorMock.Object, _factory);
        }

        private Mock<INavigator> _navigatorMock;
        private NavigationViewModel _viewModel;

        [TestCase(TargetView.Menu)]
        [TestCase(TargetView.DeserializeList)]
        public void NavigateToCommand_NoArgTarget_DelegatesToINavigatorWithPassedArgument(TargetView target)
        {
            _viewModel.NavigateToCommand.Execute(target);

            _navigatorMock.Verify(
                navigator => navigator.NavigateTo(
                    It.Is<TargetView>(param => param == target)
                ),
                Times.Once
            );
        }

        [TestCase(TargetView.MetaData)]
        public void NavigateToCommand_ArgTarget_Failed(TargetView targetView)
        {
            Assert.Catch<ArgumentException>(
                () => _viewModel.NavigateToCommand.Execute(targetView)
            );

            _navigatorMock.Verify(
                navigator => navigator.NavigateTo(
                    It.Is<TargetView>(param => param == targetView)
                ),
                Times.Once
            );
        }

        [TestCase(TargetView.Menu)]
        [TestCase(TargetView.DeserializeList)]
        [TestCase(TargetView.MetaData)]
        public void NavigateToCommand_Always_CanExecuteReturnsTrue(TargetView targetView)
        {
            var actual = _viewModel.NavigateToCommand.CanExecute(targetView);
            Assert.True(actual);
        }

        [Test]
        public void NavigateBackwardsCommand_NavigatorCannotGoBackwards_CanExecuteReturnsFalse()
        {
            _navigatorMock
                .Setup(navigator => navigator.CanGoBackwards())
                .Returns(false);

            _viewModel = new NavigationViewModel(_navigatorMock.Object, _factory);

            var actual = _viewModel.NavigateBackwardsCommand.CanExecute(null);

            _navigatorMock.Verify(
                navigator => navigator.CanGoBackwards(),
                Times.Once
            );
            Assert.False(actual);
        }

        [Test]
        public void NavigateBackwardsCommand_NavigatorCanGoBackwards_CanExecuteReturnsFalse()
        {
            _navigatorMock
                .Setup(navigator => navigator.CanGoBackwards())
                .Returns(true);

            _viewModel = new NavigationViewModel(_navigatorMock.Object, _factory);

            var actual = _viewModel.NavigateBackwardsCommand.CanExecute(null);

            _navigatorMock.Verify(
                navigator => navigator.CanGoBackwards(),
                Times.Once
            );

            Assert.True(actual);
        }

        [Test]
        public void NavigateForwardsCommand_NavigatorCannotGoForward_CanExecuteReturnsTrue()
        {
            _navigatorMock
                .Setup(navigator => navigator.CanGoForwards())
                .Returns(false);

            _viewModel = new NavigationViewModel(_navigatorMock.Object, _factory);

            var actual = _viewModel.NavigateForwardsCommand.CanExecute(null);

            _navigatorMock.Verify(
                navigator => navigator.CanGoForwards(),
                Times.Once
            );
            Assert.False(actual);
        }

        [Test]
        public void NavigateForwardsCommand_NavigatorCanGoForwards_CanExecuteReturnsTrue()
        {
            _navigatorMock
                .Setup(navigator => navigator.CanGoForwards())
                .Returns(true);

            _viewModel = new NavigationViewModel(_navigatorMock.Object, _factory);

            var actual = _viewModel.NavigateForwardsCommand.CanExecute(null);

            _navigatorMock.Verify(
                navigator => navigator.CanGoForwards(),
                Times.Once
            );

            Assert.True(actual);
        }

        [Test]
        public void NavigateToMetaDataCommand_SourceNull_CanExecuteReturnsFalse()
        {
            var actual = _viewModel.NavigateToMetaDataCommand.CanExecute(null);

            Assert.False(actual);
        }

        [Test]
        public void NavigateToMetaDataCommand_SourceNotNull_CanExecuteReturnsTrue()
        {
            _viewModel.Source = new SerializedSource("");
            var actual = _viewModel.NavigateToMetaDataCommand.CanExecute(null);

            Assert.True(actual);
        }

        [Test]
        public void NavigateToMetaDataCommand_SourceNotNull_ExecuteDelegatesToNavigator()
        {
            _viewModel.Source = new SerializedSource("");

            _viewModel.NavigateToMetaDataCommand.Execute(null);

            _navigatorMock.Verify(
                navigator => navigator.NavigateTo(
                    It.Is<TargetView>(target => target == TargetView.MetaData),
                    It.IsNotNull<Source>()
                ),
                Times.Once
            );
        }
    }
}