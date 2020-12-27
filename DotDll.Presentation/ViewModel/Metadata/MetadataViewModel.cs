using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DotDll.Logic.Metadata;
using DotDll.Logic.Metadata.Data;
using DotDll.Logic.Metadata.Sources;
using DotDll.Presentation.Navigation;
using DotDll.Presentation.ViewModel.Common;

namespace DotDll.Presentation.ViewModel.Metadata
{
    public class MetadataViewModel : DynamicContentViewModel
    {
        private readonly IMetadataService _service;
        private readonly Source _source;

        private bool _alreadySerialized;

        private MetaDataDeclarations _metaData;

        private string _metaDataName = "...";

        private RelayCommand _serializeCommand;

        public MetadataViewModel(INavigator navigator, IMetadataService service, Source source) : base(navigator)
        {
            _service = service;
            _source = source;

            _alreadySerialized = _source is SerializedSource;

            LoadData();
        }

        public string MetaDataSource => _source.Identifier;

        public string MetaDataName
        {
            get => _metaDataName;
            set
            {
                if (_metaDataName == value) return;

                _metaDataName = value;
                OnPropertyChangedAuto();
            }
        }

        public ObservableCollection<MetadataNode> Nodes { get; } = new ObservableCollection<MetadataNode>();

        public ICommand SerializeCommand =>
            _serializeCommand ?? (_serializeCommand = new RelayCommand(
                o => SaveData(),
                o => !_alreadySerialized &&
                     !IsLoading &&
                     IsContentShown &&
                     _metaData != null
            ));

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
            catch (Exception)
            {
                ErrorOccured = true;
            }

            IsLoading = false;
            _serializeCommand?.RaiseCanExecuteChanged();
        }

        private async void SaveData()
        {
            IsLoading = true;
            ErrorOccured = false;

            _alreadySerialized = await _service.SaveMetaData(_source);

            ErrorOccured = !_alreadySerialized;
            IsLoading = false;
            _serializeCommand?.RaiseCanExecuteChanged();
        }

        private void LoadFirstLayer()
        {
            foreach (var node in _metaData.Namespaces.Select(nSpace => new MetadataNode(nSpace)))
            {
                Nodes.Add(node);

                node.LoadChildren();
            }
        }
    }
}