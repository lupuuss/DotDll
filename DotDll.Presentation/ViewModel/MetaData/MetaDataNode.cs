using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DotDll.Logic.MetaData.Data;
using DotDll.Logic.MetaData.Data.Base;
using Type = DotDll.Logic.MetaData.Data.Type;

namespace DotDll.Presentation.ViewModel.MetaData
{
    public class MetaDataNode : INotifyPropertyChanged
    {
        private readonly Defined _relatedDefinition;
        
        public string Name => _relatedDefinition.Definition;

        private bool _isExpanded = false;

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
        
        public MetaDataNode(Defined definition)
        {
            _relatedDefinition = definition;
        }
        
        private void PrepareLayers(bool isExpanded)
        {

            if (isExpanded)
            {
                LoadNextLayer();
            }
            else
            {
                ClearNextLayer();
            }
        }

        private void LoadNextLayer()
        {
            foreach (var node in Nodes)
            {
                node.LoadChildren();
            }
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
            if (Nodes.Count != 0)
            {
                throw new Exception("Children already loaded");
            }

            switch (_relatedDefinition)
            {
                case Member member:
                    LoadChildren(member.RelatedTypes);
                    break;
                case Namespace ns:
                    LoadChildren(ns.Types);
                    break;
                case Type type:
                    LoadChildren(type.Members);
                    break;
                default:
                    throw new ArgumentException($"Not supported children of type {_relatedDefinition.GetType().FullName}");
            }
        }

        private void LoadChildren<T>(List<T> subItems) where T : Defined
        {
            foreach (var item in subItems)
            {
                Nodes.Add(new MetaDataNode(item));
            }
        }

        public void ClearChildren()
        {
            foreach (var node in Nodes)
            {
                node.ClearChildren();
            }

            Nodes.Clear();
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler == null) return;

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }
    }
}