using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;
using Database.Interfaces.Map;
using Database.Interfaces.Map.Item;
using System;
using System.Linq;

namespace ServerService.Essential.AdminClient
{
    internal class MapsEntityToDtoConverter : IFactory<IMap, MapDTO>
    {
        private IFactory<IMapItem, MapItemDTO> MapItemsEntityToDtoConverter { get; }

        public MapsEntityToDtoConverter(IFactory<IMapItem, MapItemDTO> mapItemsEntityToDtoConverter)
        {
            MapItemsEntityToDtoConverter = mapItemsEntityToDtoConverter;
        }

        public MapDTO Create(IMap param)
        {
            var result = new MapDTO();
            result.Id = param.GetId();
            result.Name = param.Name;
            result.Items = param.Items.Select(x => MapItemsEntityToDtoConverter.Create(x)).ToArray();
            return result;
        }
    }

    internal class MapItemsEntityToDtoConverter : IFactory<IMapItem, MapItemDTO>
    {
        public MapItemDTO Create(IMapItem param)
        {
            var result = new MapItemDTO();
            result.RelativeTop = param.RelativeTop;
            result.RelativeLeft = param.RelativeLeft;
            result.RelativeWidth = param.RelativeWidth;
            result.RelativeHeight = param.RelativeHeight;
            result.Layer = param.Layer;
            if (param is IImageItem image)
            {
                result.Type = MapItemType.Image;
                result.File = image.File;
            }
            else if (param is IWorkstationItem workstation)
            {
                result.Type = MapItemType.Workstation; 
                result.WorkstationId = workstation.WorkstationId;
            }
            else
            {
                throw new NotImplementedException();
            }
            return result;
        }
    }
}
