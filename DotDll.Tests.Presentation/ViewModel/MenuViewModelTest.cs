﻿using System.Threading.Tasks;
using DotDll.Logic.MetaData;
using DotDll.Logic.MetaData.Sources;
using DotDll.Presentation.Navigation;
using DotDll.Presentation.ViewModel;
using Moq;
using NUnit.Framework;

namespace DotDll.Tests.Presentation.ViewModel
{
    [TestFixture]
    public class MenuViewModelTest
    {
        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IMetaDataService>();
            _navigatorMock = new Mock<INavigator>();
            _userInputService = new Mock<IUserInputService>();

            _userInputService
                .Setup(service => service.PickFilePath())
                .Returns(Task.FromResult(ExpectedPickedPath));

            InitViewModel();
        }

        private Mock<IMetaDataService> _serviceMock;

        private Mock<INavigator> _navigatorMock;

        private Mock<IUserInputService> _userInputService;

        private const string ExpectedPickedPath = "picked/path";

        private MenuViewModel _viewModel;

        private void InitViewModel()
        {
            _viewModel = new MenuViewModel(
                _navigatorMock.Object,
                _serviceMock.Object,
                _userInputService.Object
            );
        }

        [Test]
        public void PickFileCommand_InvalidPath_SetsPathErrorMessageShown()
        {
            // setup

            _serviceMock
                .Setup(service => service.IsValidFileSourcePath(It.IsAny<string>()))
                .Returns(false);

            InitViewModel();

            _viewModel.PathErrorMessageShown = false;

            // run

            _viewModel.PickFileCommand.Execute(null);

            // verification

            _serviceMock.Verify(
                service => service.IsValidFileSourcePath(It.IsAny<string>()),
                Times.Once
            );

            Assert.True(_viewModel.PathErrorMessageShown);
        }

        [Test]
        public void PickedFilePath_ValidPath_ResetsPathErrorMessageAndNavigatesToMetaDataView()
        {
            // setup

            var pickedSource = new Mock<Source>().Object;
            _serviceMock
                .Setup(service => service.CreateFileSource(It.IsAny<string>()))
                .Returns(pickedSource);

            _serviceMock
                .Setup(service => service.IsValidFileSourcePath(It.IsAny<string>()))
                .Returns(true);

            InitViewModel();

            _viewModel.PathErrorMessageShown = true;

            // run

            _viewModel.PickFileCommand.Execute(null);

            // verification

            _serviceMock.Verify(
                service => service.IsValidFileSourcePath(It.Is<string>(p => p == ExpectedPickedPath)),
                Times.Once
            );

            _serviceMock.Verify(
                service => service.CreateFileSource(It.Is<string>(p => p == ExpectedPickedPath)),
                Times.Once
            );

            _navigatorMock.Verify(
                navigator => navigator.NavigateTo(
                    TargetView.MetaData,
                    It.Is<Source>(s => s == pickedSource)
                ),
                Times.Once
            );

            Assert.AreEqual(pickedSource, _viewModel.Source);
            Assert.False(_viewModel.PathErrorMessageShown);
        }
    }
}