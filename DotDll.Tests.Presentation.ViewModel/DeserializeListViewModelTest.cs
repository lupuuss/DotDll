using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotDll.Logic.Metadata;
using DotDll.Logic.Metadata.Sources;
using DotDll.Logic.Navigation;
using DotDll.Presentation.ViewModel;
using DotDll.Presentation.ViewModel.Common;
using Moq;
using NUnit.Framework;

namespace DotDll.Tests.Presentation.ViewModel
{
    [TestFixture]
    public class DeserializeListViewModelTest
    {

        private Mock<INavigator> _navigatorMock;

        private Mock<IMetadataService> _serviceMock;

        private Mock<RelayCommandFactory> _factory;
        
        private DeserializeListViewModel _viewModel;

        private readonly List<Source> _sources = new List<Source>
        {
            new SerializedSource("Example1"),
            new SerializedSource("Example2")
        };
        
        [SetUp]
        public void SetUp()
        {
            _navigatorMock = new Mock<INavigator>();
            _serviceMock = new Mock<IMetadataService>();
            _factory = new Mock<RelayCommandFactory>();
            
            _serviceMock
                .Setup(service => service.GetSerializedSources())
                .Returns(Task.FromResult(_sources));
            
            _factory.Setup(f => f.CreateCommand(
                It.IsAny<Action<Object>>(),
                It.IsAny<Predicate<Object>>()
                )
            ).Returns<Action<object>,Predicate<object>?>(
                (action, predicate) => new TestRelayCommand(action, predicate)
                );
        }

        private void InitViewModel()
        {

            
            _viewModel = new DeserializeListViewModel(_navigatorMock.Object, _serviceMock.Object, _factory.Object);
        }

        [Test]
        public void Constructor_DataOk_LoadsSourcesDataFromService()
        {
            InitViewModel();

            _serviceMock.Verify(
                service => service.GetSerializedSources(),
                Times.Once
            );

            var actualSourcesSize = _viewModel.Sources.Count;

            Assert.AreEqual(_sources.Count, actualSourcesSize);

            foreach (var source in _viewModel.Sources) Assert.True(_sources.Contains(source));

            Assert.False(_viewModel.ErrorOccured);
            Assert.False(_viewModel.IsLoading);
            Assert.True(_viewModel.IsContentShown);
        }

        [Test]
        public void Constructor_DataBad_LoadsSourcesDataFromService()
        {
            _serviceMock
                .Setup(service => service.GetSerializedSources())
                .Throws(new Exception("Data Failed"));

            InitViewModel();

            _serviceMock.Verify(
                service => service.GetSerializedSources(),
                Times.Once
            );

            var actualSourcesSize = _viewModel.Sources.Count;

            Assert.AreEqual(0, actualSourcesSize);

            Assert.True(_viewModel.ErrorOccured);
            Assert.False(_viewModel.IsLoading);
            Assert.False(_viewModel.IsContentShown);
        }
    }
}