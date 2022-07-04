using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;
using System;
using System.Linq;

namespace AdminClientApp.ViewModels.Essential.Maps
{
    internal class MapDTOFactory : IFactory<MapViewModel, MapDTO>
    {
        private IFactory<MapItemBase, MapItemDTO> MapItemDTOFactory { get; }

        public MapDTOFactory(IFactory<MapItemBase, MapItemDTO> mapItemDTOFactory)
        {
            MapItemDTOFactory = mapItemDTOFactory;
        }

        public MapDTO Create(MapViewModel param)
        {
            var result = new MapDTO();
            result.Id = param.Id;
            result.Name = param.Name;
            result.Items = param.Items.Select(x => MapItemDTOFactory.Create(x)).ToList();
            return result;
        }
    }

    internal class MapItemDTOFactory : IFactory<MapItemBase, MapItemDTO>
    {
        public MapItemDTO Create(MapItemBase param)
        {
            var result = new MapItemDTO();
            result.RelativeTop = param.RelativeTop;
            result.RelativeLeft = param.RelativeLeft;
            result.RelativeWidth = param.RelativeWidth;
            result.RelativeHeight = param.RelativeHeight;
            result.Layer = param.Layer;
            if (param is ImageItem imageItem)
            {
                result.File = imageItem.Bytes;
                result.Type = MapItemType.Image;
            }
            else if (param is WorkstationItem workstationItem)
            {
                result.WorkstationId = workstationItem.WorkstationId;
                result.Type = MapItemType.Workstation;
            }
            else
            {
                throw new NotImplementedException();
            }
            return result;
        }
    }
}
