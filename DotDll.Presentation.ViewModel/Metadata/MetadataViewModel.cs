using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DotDll.Logic.Metadata;
using DotDll.Logic.Metadata.Data;
using DotDll.Logic.Metadata.Sources;
using DotDll.Presentation.Model.Navigation;
using DotDll.Presentation.ViewModel.Common;

namespace DotDll.Presentation.ViewModel.Metadata
{
    public class MetadataViewModel : DynamicContentViewModel
    {
        private readonly IMetadataService _service;
        private readonly Source _source;

        private bool _alreadySerialized;

        private MetadataDeclarations? _metadata;

        private string _metadataName = "...";

        private RelayCommand? _serializeCommand;

        public MetadataViewModel(
            INavigator navigator, 
            IMetadataService service, 
            Source source, 
            RelayCommandFactory factory
            ) : base(navigator, factory)
        {
            _service = service;
            _source = source;

            _alreadySerialized = _source is SerializedSource;

            LoadData();
        }

        public string MetaDataSource => _source.Identifier;

        public string MetaDataName
        {
            get => _metadataName;
            set
            {
                if (_metadataName == value) return;

                _metadataName = value;
                OnPropertyChangedAuto();
            }
        }

        public ObservableCollection<MetadataNode> Nodes { get; } = new ObservableCollection<MetadataNode>();

        public ICommand SerializeCommand =>
            _serializeCommand ??= CommandFactory.CreateCommand(
                o => SaveData(),
                o => !_alreadySerialized &&
                     !IsLoading &&
                     IsContentShown &&
                     _metadata != null
            );

        private async void LoadData()
        {
            IsLoading = true;
            ErrorOccured = false;
            IsContentShown = false;

            try
            {
                _metadata = await _service.LoadMetadata(_source);
                LoadFirstLayer(_metadata);
                IsContentShown = true;
                MetaDataName = _metadata.Name;
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

            _alreadySerialized = await _service.SaveMetadata(_source);

            ErrorOccured = !_alreadySerialized;
            IsLoading = false;
            _serializeCommand?.RaiseCanExecuteChanged();
        }

        private void LoadFirstLayer(MetadataDeclarations metadata)
        {
            foreach (var node in metadata.Namespaces.Select(nSpace => new MetadataNode(nSpace)))
            {
                Nodes.Add(node);

                node.LoadChildren();
            }
        }
    }
}