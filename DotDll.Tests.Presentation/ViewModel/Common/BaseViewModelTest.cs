﻿using System.ComponentModel;
using DotDll.Presentation.ViewModel.Common;
using NUnit.Framework;

namespace DotDll.Tests.Presentation.ViewModel.Common
{
    class TestBaseViewModel : BaseViewModel
    {
        private string _testField;
        
        public string TestField
        {
            get => _testField;
            set
            {
                if (_testField == value) return;

                _testField = value;
                OnPropertyChangedAuto();
            }
        }
    }

    [TestFixture]
    public class BaseViewModelTest
    {
        private TestBaseViewModel _baseViewModel;
        
        [SetUp]
        public void SetUp() {
            _baseViewModel = new TestBaseViewModel();
        }

        [Test]
        public void BaseViewModel_Always_ProperlyNotifiesAboutFieldChanges()
        {
            var handlerCalledProperly = false;
            
            _baseViewModel.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
            {
                var actual = args.PropertyName;
                
                Assert.AreEqual("TestField", actual);
                Assert.IsInstanceOf<TestBaseViewModel>(sender);
                Assert.AreEqual("Test value", ((TestBaseViewModel) sender).TestField);
                
                handlerCalledProperly = true;
            };

            _baseViewModel.TestField = "Test value";
            
            Assert.True(handlerCalledProperly);
        }
    }
}