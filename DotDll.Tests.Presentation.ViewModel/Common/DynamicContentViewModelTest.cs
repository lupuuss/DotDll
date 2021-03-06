﻿using System;
using DotDll.Presentation.Model.Navigation;
using DotDll.Presentation.ViewModel.Common;
using Moq;
using NUnit.Framework;

namespace DotDll.Tests.Presentation.ViewModel.Common
{
    [TestFixture]
    public class DynamicContentViewModelTest
    {
        [SetUp]
        public void SetUp()
        {
            var mock = new Mock<RelayCommandFactory>();
            
            mock.Setup(f => f.CreateCommand(
                    It.IsAny<Action<Object>>(),
                    It.IsAny<Predicate<Object>>()
                )
            ).Returns<Action<object>,Predicate<object>?>(
                (action, predicate) => new TestRelayCommand(action, predicate)
            );
            
            _viewModel = new DynamicContentViewModel(new Mock<INavigator>().Object, mock.Object);
        }

        private DynamicContentViewModel _viewModel;

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