using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotDll.Logic.MetaData;
using DotDll.Logic.MetaData.Data;
using DotDll.Logic.MetaData.Sources;
using DotDll.Presentation.Navigation;
using DotDll.Presentation.ViewModel.MetaData;
using Moq;
using NUnit.Framework;
using Type = DotDll.Logic.MetaData.Data.Type;

namespace DotDll.Tests.Presentation.ViewModel.MetaData
{
    [TestFixture]
    public class MetaDataViewModelTest
    {
        [SetUp]
        public void SetUp()
        {
            _navigatorMock = new Mock<INavigator>();
            _serviceMock = new Mock<IMetaDataService>();

            _metaDataObject = new MetaDataObject("Project", _namespaces);

            _serviceMock
                .Setup(service => service.LoadMetaData(It.IsAny<Source>()))
                .Returns(Task.FromResult(_metaDataObject));

            _serviceMock
                .Setup(service => service.SaveMetaData(It.IsAny<Source>()))
                .Returns(Task.FromResult(true));
        }

        private Mock<INavigator> _navigatorMock;
        private Mock<IMetaDataService> _serviceMock;

        private MetaDataViewModel _viewModel;

        private Source _targetSource;

        private readonly List<Namespace> _namespaces = new List<Namespace>
        {
            new Namespace("Namespace1", new List<Type>()),
            new Namespace("Namespace2", new List<Type>()),
            new Namespace("Namespace3", new List<Type>())
        };

        private MetaDataObject _metaDataObject;

        private void InitViewModel()
        {
            _viewModel = new MetaDataViewModel(
                _navigatorMock.Object,
                _serviceMock.Object,
                _targetSource
            );
        }

        [Test]
        public void Constructor_SerializedSource_LoadsData()
        {
            _targetSource = new SerializedSource("Identifier");
            InitViewModel();

            _serviceMock.Verify(
                service => service.LoadMetaData(_targetSource),
                Times.Once
            );
        }

        [Test]
        public void Constructor_FileSource_LoadsData()
        {
            _targetSource = new FileSource("path/to/file");
            InitViewModel();

            _serviceMock.Verify(
                service => service.LoadMetaData(_targetSource),
                Times.Once
            );
        }

        [Test]
        public void Constructor_SuccessfulMetaDataLoad_ShowsResults()
        {
            _targetSource = new SerializedSource("Example1");
            InitViewModel();

            Assert.False(_viewModel.ErrorOccured);
            Assert.False(_viewModel.IsLoading);
            Assert.True(_viewModel.IsContentShown);

            Assert.AreEqual(_targetSource.Identifier, _viewModel.MetaDataSource);
            Assert.AreEqual(_metaDataObject.Name, _viewModel.MetaDataName);
            Assert.AreEqual(_namespaces.Count, _viewModel.Nodes.Count);
        }

        [Test]
        public void Constructor_BadMetaDataLoad_ShowsError()
        {
            _serviceMock
                .Setup(service => service.LoadMetaData(It.IsAny<Source>()))
                .Throws(new Exception("Data failed!"));

            _targetSource = new SerializedSource("Example1");
            InitViewModel();

            Assert.True(_viewModel.ErrorOccured);
            Assert.False(_viewModel.IsLoading);
            Assert.False(_viewModel.IsContentShown);

            Assert.AreEqual(0, _viewModel.Nodes.Count);
        }

        [Test]
        public void SerializeCommand_MetaDataBad_CanExecuteReturnsFalse()
        {
            _serviceMock
                .Setup(service => service.LoadMetaData(It.IsAny<Source>()))
                .Throws(new Exception("Data failed!"));

            _targetSource = new FileSource("path/to/file");
            InitViewModel();

            Assert.False(_viewModel.SerializeCommand.CanExecute(null));
        }

        [Test]
        public void SerializeCommand_MetaDataOkAndSourceIsSerializedSource_CanExecuteReturnsFalse()
        {
            _targetSource = new SerializedSource("Identifier");
            InitViewModel();

            Assert.False(_viewModel.SerializeCommand.CanExecute(null));
        }

        [Test]
        public void SerializeCommand_MetaDataOkAndSourceIsFileSource_CanExecuteReturnsTrue()
        {
            _targetSource = new FileSource("path/to/file");
            InitViewModel();

            Assert.True(_viewModel.SerializeCommand.CanExecute(null));
        }

        [Test]
        public void SerializeCommand_MetDataOkAndAfterSuccessfulSerialization_CanExecuteReturnsFalse()
        {
            _targetSource = new FileSource("path/to/file");
            InitViewModel();

            _viewModel.SerializeCommand.Execute(null);
            Assert.False(_viewModel.SerializeCommand.CanExecute(null));
        }

        [Test]
        public void SerializeCommand_MetaDataOkAndAfterFailedSerialization_CanExecuteReturnsTrue()
        {
            _serviceMock
                .Setup(service => service.SaveMetaData(It.IsAny<Source>()))
                .Returns(Task.FromResult(false));

            _targetSource = new FileSource("path/to/file");
            InitViewModel();

            _viewModel.SerializeCommand.Execute(null);
            Assert.True(_viewModel.SerializeCommand.CanExecute(null));

            _viewModel.SerializeCommand.Execute(null);
            Assert.True(_viewModel.SerializeCommand.CanExecute(null));
        }
    }
}