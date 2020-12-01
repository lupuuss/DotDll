using System;
using DotDll.Logic.MetaData.Sources;
using DotDll.Presentation.Navigation;
using DotDll.Presentation.ViewModel.Common;
using Moq;
using NUnit.Framework;

namespace DotDll.Tests.Presentation.ViewModel.Common
{
    [TestFixture]
    public class NavigationViewModelTest
    {
        [SetUp]
        public void SetUp()
        {
            _navigatorMock = new Mock<INavigator>();

            _navigatorMock.Setup(
                navigator => navigator.NavigateTo(TargetView.MetaData)
            ).Throws(new ArgumentException("Argument expected"));

            _viewModel = new NavigationViewModel(_navigatorMock.Object);
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

            _viewModel = new NavigationViewModel(_navigatorMock.Object);

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

            _viewModel = new NavigationViewModel(_navigatorMock.Object);

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

            _viewModel = new NavigationViewModel(_navigatorMock.Object);

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

            _viewModel = new NavigationViewModel(_navigatorMock.Object);

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
            _viewModel.Source = new Mock<Source>().Object;
            var actual = _viewModel.NavigateToMetaDataCommand.CanExecute(null);

            Assert.True(actual);
        }

        [Test]
        public void NavigateToMetaDataCommand_SourceNotNull_ExecuteDelegatesToNavigator()
        {
            _viewModel.Source = new Mock<Source>().Object;

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