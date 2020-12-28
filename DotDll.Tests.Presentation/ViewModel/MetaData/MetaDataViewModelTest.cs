using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotDll.Logic.Metadata;
using DotDll.Logic.Metadata.Data;
using DotDll.Logic.Metadata.Sources;
using DotDll.Presentation.Navigation;
using DotDll.Presentation.ViewModel.Metadata;
using Moq;
using NUnit.Framework;

namespace DotDll.Tests.Presentation.ViewModel.MetaData
{
    [TestFixture]
    public class MetaDataViewModelTest
    {
        [SetUp]
        public void SetUp()
        {
            _navigatorMock = new Mock<INavigator>();
            _serviceMock = new Mock<IMetadataService>();

            _metadata = new MetadataDeclarations("Project", _namespaces);

            _serviceMock
                .Setup(service => service.LoadMetadata(It.IsAny<Source>()))
                .Returns(Task.FromResult(_metadata));

            _serviceMock
                .Setup(service => service.SaveMetadata(It.IsAny<Source>()))
                .Returns(Task.FromResult(true));
        }

        private Mock<INavigator> _navigatorMock;
        private Mock<IMetadataService> _serviceMock;

        private MetadataViewModel _viewModel;

        private Source _targetSource;

        private readonly List<DNamespace> _namespaces = new List<DNamespace>
        {
            new DNamespace("Namespace1", new List<DType>()),
            new DNamespace("Namespace2", new List<DType>()),
            new DNamespace("Namespace3", new List<DType>())
        };

        private MetadataDeclarations _metadata;

        private void InitViewModel()
        {
            _viewModel = new MetadataViewModel(
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
                service => service.LoadMetadata(_targetSource),
                Times.Once
            );
        }

        [Test]
        public void Constructor_FileSource_LoadsData()
        {
            _targetSource = new FileSource("path/to/file");
            InitViewModel();

            _serviceMock.Verify(
                service => service.LoadMetadata(_targetSource),
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
            Assert.AreEqual(_metadata.Name, _viewModel.MetaDataName);
            Assert.AreEqual(_namespaces.Count, _viewModel.Nodes.Count);
        }

        [Test]
        public void Constructor_BadMetaDataLoad_ShowsError()
        {
            _serviceMock
                .Setup(service => service.LoadMetadata(It.IsAny<Source>()))
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
                .Setup(service => service.LoadMetadata(It.IsAny<Source>()))
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
                .Setup(service => service.SaveMetadata(It.IsAny<Source>()))
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