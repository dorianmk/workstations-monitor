using AdminClientApp.ViewModels.Essential.Workstations;
using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;
using System;
using System.Linq;

namespace AdminClientApp.ViewModels.Essential.Maps
{
    internal class MapViewModelFactory : IFactory<MapDTO, MapViewModel>
    {
        private IFactory<MapItemDTO, MapItemBase> MapItemViewModelFactory { get; }

        public MapViewModelFactory(IFactory<MapItemDTO, MapItemBase> mapItemViewModelFactory)
        {
            MapItemViewModelFactory = mapItemViewModelFactory;
        }

        public MapViewModel Create(MapDTO param)
        {
            var result = new MapViewModel(param.Name, param.Id);
            foreach (var item in param.Items)
            {
                var itemVM = MapItemViewModelFactory.Create(item);
                result.AddItem(itemVM);
            }
            return result;
        }
    }

    internal class MapItemViewModelFactory : IFactory<MapItemDTO, MapItemBase>
    {
        private IProvider<WorkstationViewModel> WorkstationsProvider { get; }

        public MapItemViewModelFactory(IProvider<WorkstationViewModel> workstationsProvider)
        {
            WorkstationsProvider = workstationsProvider;
        }

        public MapItemBase Create(MapItemDTO param)
        {
            if (param.Type == MapItemType.Image)
                return new ImageItem(param.File, param.RelativeTop, param.RelativeLeft, param.RelativeWidth, param.RelativeHeight, param.Layer);
            else if (param.Type == MapItemType.Workstation)
            {
                var workstation = WorkstationsProvider.Items.FirstOrDefault(x => x.Id.Equals(param.WorkstationId));
                return new WorkstationItem(false, param.WorkstationId, workstation, param.RelativeTop, param.RelativeLeft, param.RelativeWidth, param.RelativeHeight, param.Layer);
            }
            throw new NotImplementedException();
        }
    }
}
