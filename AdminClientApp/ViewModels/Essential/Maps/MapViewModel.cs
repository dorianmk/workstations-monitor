using AdminClientApp.ViewModels.Common;
using AdminClientApp.ViewModels.Essential.Workstations;
using System.Collections.ObjectModel;

namespace AdminClientApp.ViewModels.Essential.Maps
{
    public class MapViewModel : BindableBase
    {
        private double lastAreaWidth;
        private double lastAreaHeight;
        private string name;
        private MapItemBase selectedItem;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public ObservableCollection<MapItemBase> Items { get; }
        public string Id { get; }
        public MapItemBase SelectedItem
        {
            get => selectedItem;
            private set => SetProperty(ref selectedItem, value);
        }
        public RelayCommand AddImageCommand { get; }
        public RelayCommand AddWorkstationCommand { get; }

        internal MapViewModel(string name, string id = null)
        {
            Name = name;
            Id = id;
            Items = new ObservableCollection<MapItemBase>();
            AddImageCommand = new RelayCommand(AddImage);
            AddWorkstationCommand = new RelayCommand(AddWorkstation);
        }

        internal void Rescale(double areaWidth, double areaHeight)
        {
            lastAreaWidth = areaWidth;
            lastAreaHeight = areaHeight;
            foreach (var item in Items)
                item.Rescale(areaWidth, areaHeight);
        }

        private void AddImage(object obj)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "Image files (*.png,*.jpeg,*.jpg)|*.png;*.jpeg;*.jpg";
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                var item = new ImageItem(dlg.FileName);
                item.Rescale(lastAreaWidth, lastAreaHeight);
                AddItem(item);
            }
        }

        private void AddWorkstation(object obj)
        {
            var workstation = obj as WorkstationViewModel;
            var item = new WorkstationItem(workstation);
            item.Rescale(lastAreaWidth, lastAreaHeight);
            AddItem(item);
        }

        internal void AddItem(MapItemBase item)
        {
            Items.Add(item);
            item.PropertyChanged += Item_PropertyChanged;
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(MapItemBase.IsSelected)))
            {
                var sourceItem = (sender as MapItemBase);
                if (sourceItem.IsSelected)
                {
                    if (SelectedItem != null)
                        SelectedItem.IsSelected = false;
                    SelectedItem = sourceItem;
                }
                else
                {
                    SelectedItem = null;
                }
            }
        }

        internal void RemoveSelectedItem()
        {
            if (SelectedItem != null)
            {
                Items.Remove(SelectedItem);
                SelectedItem = null;
            }
        }
    }
}
