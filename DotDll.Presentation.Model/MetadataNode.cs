using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using DotDll.Model.Data;
using DotDll.Model.Data.Base;

namespace DotDll.Presentation.Model
{
    public abstract class MetadataNode : INotifyPropertyChanged
    {

        private bool _isExpanded;

        public abstract string Name { get; }

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

        public abstract void LoadChildren();

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
        
        protected string GetAccessString(Access typeAccess)
        {
            return typeAccess switch
            {
                Access.Public => "public",
                Access.Internal => "internal",
                Access.Protected => "protected",
                Access.InternalProtected => "internal protected",
                Access.Private => "private",
                Access.Inner => "",
                _ => throw new System.ArgumentOutOfRangeException(nameof(typeAccess), typeAccess, null)
            };
        }

        protected string GetAttributesString(Member member)
        {
            return member.Attributes.Any() ? $"[{string.Join(", ", member.Attributes.Select(a => a.Name))}]\n" : "";
        }
        
        protected string MapParameters(IEnumerable<Parameter> parameters)
        {
            return "(" + string.Join(
                ", ",
                parameters.Select(param =>
                {
                    var attrs = param.Attributes.Any()
                        ? "[" + string.Join(", ", param.Attributes.Select(a => a.Name)) + "] "
                        : "";

                    return $"{attrs}{param.ParameterType.FullName()} {param.Name}";
                })
            ) + ")";
        }

        protected string MapGenericArguments(IEnumerable<Type> methodGenericArguments)
        {
            var args = string.Join(", ", methodGenericArguments.Select(arg => arg.FullName()));

            return $"<{args}>";
        }
    }
}