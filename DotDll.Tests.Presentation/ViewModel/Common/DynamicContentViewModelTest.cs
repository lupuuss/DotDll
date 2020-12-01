using DotDll.Presentation.Navigation;
using DotDll.Presentation.ViewModel.Common;
using Moq;
using NUnit.Framework;

namespace DotDll.Tests.Presentation.ViewModel.Common
{
    [TestFixture]
    public class DynamicContentViewModelTest
    {

        private DynamicContentViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _viewModel = new DynamicContentViewModel(new Mock<INavigator>().Object);
        }
        
        [TestCase(true)]
        [TestCase(false)]
        public void IsLoading_Always_ProperlyPropagatesItsChanges(bool isLoadingValue)
        {
            var handlerTriggered = false;

            _viewModel.IsLoading = !isLoadingValue;

            _viewModel.PropertyChanged += (sender, args) =>
            {
                Assert.IsInstanceOf<DynamicContentViewModel>(sender);
                Assert.AreEqual(isLoadingValue, ((DynamicContentViewModel) sender).IsLoading);
                handlerTriggered = true;
            };

            _viewModel.IsLoading = isLoadingValue;
            
            Assert.True(handlerTriggered);
        }
     
        [TestCase(true)]
        [TestCase(false)]
        public void IsContentShown_Always_ProperlyPropagatesItsChanges(bool isContentShownValue)
        {
            var handlerTriggered = false;

            _viewModel.IsContentShown = !isContentShownValue;

            _viewModel.PropertyChanged += (sender, args) =>
            {
                Assert.IsInstanceOf<DynamicContentViewModel>(sender);
                Assert.AreEqual(isContentShownValue, ((DynamicContentViewModel) sender).IsContentShown);
                handlerTriggered = true;
            };

            _viewModel.IsContentShown = isContentShownValue;
            
            Assert.True(handlerTriggered);
        }
        
        [TestCase(true)]
        [TestCase(false)]
        public void ErrorOccured_Always_ProperlyPropagatesItsChanges(bool errorOccuredValue)
        {
            var handlerTriggered = false;

            _viewModel.ErrorOccured = !errorOccuredValue;

            _viewModel.PropertyChanged += (sender, args) =>
            {
                Assert.IsInstanceOf<DynamicContentViewModel>(sender);
                Assert.AreEqual(errorOccuredValue, ((DynamicContentViewModel) sender).ErrorOccured);
                handlerTriggered = true;
            };

            _viewModel.ErrorOccured = errorOccuredValue;
            
            Assert.True(handlerTriggered);
        }

    }
}