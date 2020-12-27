using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DotDll.Logic.MetaData.Data;
using DotDll.Logic.MetaData.Data.Base;

namespace DotDll.Presentation.ViewModel.MetaData
{
    public class MetaDataNode : INotifyPropertyChanged
    {
        private readonly Declared _relatedDefinition;

        private bool _isExpanded;

        public MetaDataNode(Declared definition)
        {
            _relatedDefinition = definition;
        }

        public string Name => _relatedDefinition.Declaration;

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

        public ObservableCollection<MetaDataNode> Nodes { get; } = new ObservableCollection<MetaDataNode>();

        public event PropertyChangedEventHandler PropertyChanged;

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
            foreach (var item in subItems) Nodes.Add(new MetaDataNode(item));
        }

        public void ClearChildren()
        {
            foreach (var node in Nodes) node.ClearChildren();

            Nodes.Clear();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler == null) return;

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }
    }
}