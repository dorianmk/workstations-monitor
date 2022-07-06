using AdminClientApp.Entry;
using AdminClientApp.ViewModels.Common;
using AdminClientApp.ViewModels.Essential.Maps;
using AdminClientApp.ViewModels.Essential.Workstations;
using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;
using DataTransfer.Interfaces;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AdminClientApp.ViewModels.Essential
{
    public class MapsViewModel : BindableBase
    {
        private IClientConnection Connection { get; }
        private IFactory<MapViewModel, MapDTO> MapDTOFactory { get; }
        private IFactory<MapDTO, MapViewModel> MapViewModelFactory { get; }
        private  IProvider<WorkstationViewModel> WorkstationsProvider { get; }
        private IDialogCoordinator DialogCoordinator { get; }
        public ObservableCollection<MapViewModel> Maps { get; }

        private MapViewModel selectedMap;
        public MapViewModel SelectedMap
        {
            get => selectedMap;
            set
            {
                SetProperty(ref selectedMap, value);
                RemoveMapCommand.RaiseCanExecuteChanged();
            }
        }

        public List<WorkstationViewModel> Workstations { get; } 

        private bool editMode;
        public bool EditMode
        {
            get => editMode;
            set
            {
                SetProperty(ref editMode, value);
                foreach (var map in Maps)
                    foreach (var item in map.Items)
                        item.EditMode = EditMode;
                if (!EditMode)
                    SaveChanges();
            }
        }

        private void SaveChanges()
        {
            var packet = new SaveMapsPacket();
            packet.Maps = Maps.Select(x => MapDTOFactory.Create(x)).ToArray();
            Connection.Server.Write(packet);
        }

        public RelayCommand AddMapCommand { get; }
        public RelayCommand RemoveMapCommand { get; }
        public RelayCommand SizeChangedCommand { get; }
        public RelayCommand LoadedCommand { get; }
        public RelayCommand DeleteKeyDownCommand { get; }

        internal MapsViewModel(
            IClientConnection connection,
            IFactory<MapViewModel, MapDTO> mapDTOFactory,
            IFactory<MapDTO, MapViewModel> mapViewModelFactory,
            IProvider<WorkstationViewModel> workstationsProvider,
            IDialogCoordinator dialogCoordinator)
        {
            Connection = connection;
            MapDTOFactory = mapDTOFactory;
            MapViewModelFactory = mapViewModelFactory;
            WorkstationsProvider = workstationsProvider;
            DialogCoordinator = dialogCoordinator;
            Maps = new ObservableCollection<MapViewModel>();
            Workstations = new List<WorkstationViewModel>(WorkstationsProvider.Items);
            AddMapCommand = new RelayCommand(AddMap);
            RemoveMapCommand = new RelayCommand(RemoveMap, CanRemoveMap);
            SizeChangedCommand = new RelayCommand(SizeChanged);
            LoadedCommand = new RelayCommand(Loaded);
            DeleteKeyDownCommand = new RelayCommand(DeleteKeyDown);
            Connection.Server.OnRead += OnDataRead;
            WorkstationsProvider.ItemAdded += OnWorkstationAdded;
        }

        private void OnDataRead(object sender, IData readData)
        {
            if (readData is GetMapsPacket getMaps)
            {
                App.Current.Dispatcher.Invoke(() => Maps.Clear());
                foreach (var item in getMaps.Maps)
                {
                    var mapVM = MapViewModelFactory.Create(item);
                    App.Current.Dispatcher.Invoke(() => Maps.Add(mapVM));
                }
            }
        }

        private async void AddMap(object obj)
        {
            var dialogSettings = new MetroDialogSettings() { DefaultText = "New map", AnimateShow = false, AnimateHide = false };
            var mapName = await DialogCoordinator.ShowInputAsync(this, string.Empty, string.Empty, dialogSettings);

            if (!string.IsNullOrEmpty(mapName))
            {
                var map = new MapViewModel(mapName);
                Maps.Add(map);
                SelectedMap = map;
            }
        }

        private void OnWorkstationAdded(object sender, WorkstationViewModel workstation)
        {
            Workstations.Add(workstation);
        }

        private bool CanRemoveMap(object obj) => SelectedMap != null;

        private void RemoveMap(object obj)
        {
            Maps.Remove(SelectedMap);
        }

        private void SizeChanged(object obj)
        {
            var args = obj as SizeChangedEventArgs;
            OnNewSize(args.NewSize.Width, args.NewSize.Height);
        }

        private void Loaded(object obj)
        {
            var args = obj as RoutedEventArgs;
            var fe = args.OriginalSource as FrameworkElement;
            OnNewSize(fe.ActualWidth, fe.ActualHeight);
        }

        private void OnNewSize(double areaWidth, double areaHeight)
        {
            foreach (var map in Maps)
                map.Rescale(areaWidth, areaHeight);
        }

        private void DeleteKeyDown(object obj)
        {
            if (EditMode)
                SelectedMap?.RemoveSelectedItem();
        }

    }
}
