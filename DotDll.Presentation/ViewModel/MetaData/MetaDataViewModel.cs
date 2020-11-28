﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DotDll.Logic.MetaData;
using DotDll.Logic.MetaData.Data;
using DotDll.Logic.MetaData.Data.Base;
using DotDll.Logic.MetaData.Sources;
using DotDll.Presentation.Navigation;
using DotDll.Presentation.ViewModel.Common;

namespace DotDll.Presentation.ViewModel.MetaData
{
    public class MetaDataViewModel : DynamicContentViewModel
    {
        private readonly IMetaDataService _service;
        private readonly Source _source;

        private MetaDataObject _metaData;
        public ObservableCollection<MetaDataNode> Nodes { get; } = new ObservableCollection<MetaDataNode>();
        public string MetaDataSource => _source.Identifier;

        private string _metaDataName = "...";

        public string MetaDataName
        {
            get => _metaDataName;
            set
            {
                if (_metaDataName == value) return;

                _metaDataName = value;
                OnPropertyChanged("MetaDataName");
            }
        }
        
        public MetaDataViewModel(INavigator navigator, IMetaDataService service, Source source) : base(navigator)
        {
            _service = service;
            _source = source;

            LoadData();
        }

        private async void LoadData()
        {
            IsLoading = true;
            ErrorOccured = false;
            IsContentShown = false;
            
            try
            {
                _metaData = await _service.LoadMetaData(_source);
                LoadFirstLayer();
                IsContentShown = true;
                MetaDataName = _metaData.Name;
            }
            catch (Exception e)
            {
                ErrorOccured = true;
            }

            IsLoading = false;
        }

        private void LoadFirstLayer()
        {
            foreach (var nSpace in _metaData.Namespaces)
            {
                var node = new MetaDataNode(nSpace);
                Nodes.Add(node);

                node.LoadChildren();
            }
        }
    }
}