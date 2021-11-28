﻿using AOEMods.Essence.Chunky;
using AOEMods.Essence.SGA;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace AOEMods.Essence.Editor
{
    public class ArchiveItemViewModel : ObservableRecipient
    {
        public string? DisplayText => Node?.Name;

        public ObservableCollection<ArchiveItemViewModel>? Children
        {
            get => children;
            set => SetProperty(ref children, value);
        }

        public IArchiveNode? Node
        {
            get => node;
            set => SetProperty(ref node, value);
        }

        private IArchiveNode? node = null;

        private ObservableCollection<ArchiveItemViewModel>? children = null;
        private ArchiveItemViewModel? parentViewModel = null;

        public ICommand ExportCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand OpenCommand { get; }

        public ArchiveItemViewModel(IArchiveNode? node, ArchiveItemViewModel? parentViewModel)
        {
            Node = node;
            this.parentViewModel = parentViewModel;

            ExportCommand = new RelayCommand(Export);
            DeleteCommand = new RelayCommand(Delete);
            OpenCommand = new RelayCommand(Open);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == nameof(Node))
            {
                if (Node != null && Node is IArchiveFolderNode folderNode)
                {
                    Children = new ObservableCollection<ArchiveItemViewModel>(folderNode.Children.Select(child => new ArchiveItemViewModel(child, this)));
                }
                else
                {
                    Children = null;
                }
            }
        }

        private void Open()
        {
            if (node is IArchiveFileNode fileNode)
            {
                WeakReferenceMessenger.Default.Send(new OpenStreamMessage(
                    new MemoryStream(fileNode.GetData().ToArray()),
                    fileNode.Extension
                ));
            }
        }

        private void Export()
        {
            if (Node != null)
            {
                ExportArchiveUtil.ShowExportArchiveNodeDialog(
                    Node,
                    "Select a path to unpack to"
                );
            }
        }

        private void Delete()
        {
            if (parentViewModel?.Node != null && Node != null)
            {
                parentViewModel.Children?.Remove(this);
                if (parentViewModel.Node is IArchiveFolderNode folderNode)
                {
                    folderNode.Children.Remove(Node);
                }
            }
        }
    }
}
