using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DotDll.Logic.Metadata.Data;
using DotDll.Logic.Metadata.Data.Base;

namespace DotDll.Presentation.ViewModel.Metadata
{
    public sealed class MetadataNode : INotifyPropertyChanged
    {
        private readonly Declared _relatedDefinition;

        private bool _isExpanded;

        public MetadataNode(Declared definition)
        {
            _relatedDefinition = definition;
        }

        public string Name => _relatedDefinition.Declaration;

        public ObservableCollection<MetadataNode> Nodes { get; } = new ObservableCollection<MetadataNode>();


        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value) return;

                _isExpanded = value;

                OnPropertyChanged("IsExpanded");

                PrepareLayers(_isExpanded);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void PrepareLayers(bool isExpanded)
        {
            if (isExpanded)
                LoadNextLayer();
            else
                ClearNextLayer();
        }

        private void LoadNextLayer()
        {
            foreach (var node in Nodes) node.LoadChildren();
        }

        private void ClearNextLayer()
        {
            foreach (var node in Nodes)
            {
                node.ClearChildren();
                node.IsExpanded = false;
            }
        }

        public void LoadChildren()
        {
            if (Nodes.Count != 0) throw new Exception("Children already loaded");

            switch (_relatedDefinition)
            {
                case DMember member:
                    LoadChildren(member.RelatedTypes);
                    break;
                case DNamespace ns:
                    LoadChildren(ns.Types);
                    break;
                case DType type:
                    LoadChildren(type.Members);
                    break;
                default:
                    throw new ArgumentException(
                        $"Not supported children of type {_relatedDefinition.GetType().FullName}"
                    );
            }
        }

        private void LoadChildren<T>(IEnumerable<T> subItems) where T : Declared
        {
            foreach (var item in subItems) Nodes.Add(new MetadataNode(item));
        }

        public void ClearChildren()
        {
            foreach (var node in Nodes) node.ClearChildren();

            Nodes.Clear();
        }

        private void OnPropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
    }
}