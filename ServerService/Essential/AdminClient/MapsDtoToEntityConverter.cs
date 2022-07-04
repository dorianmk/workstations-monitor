using Common.DataTransfer.DataPackets.AdminClient;
using Common.Interfaces;
using Database.Interfaces.Map;
using Database.Interfaces.Map.Item;
using System;

namespace ServerService.Essential.AdminClient
{
    internal class MapsDtoToEntityConverter : IFactory<MapDTO, IMap>
    {
        private IFactory<MapItemDTO, IMapItem> MapItemsDtoToEntityConverter { get; }
        private IMaps Maps { get; }

        public MapsDtoToEntityConverter(IFactory<MapItemDTO, IMapItem> mapItemsDtoToEntityConverter, IMaps maps)
        {
            MapItemsDtoToEntityConverter = mapItemsDtoToEntityConverter;
            Maps = maps;
        }

        public IMap Create(MapDTO param)
        {
            var result = Maps.CreateMap(param.Name, param.Id);
            foreach (var item in param.Items)
            {
                var itemEntity = MapItemsDtoToEntityConverter.Create(item);
                result.AddItem(itemEntity);
            }
            return result;
        }
    }

    internal class MapItemsDtoToEntityConverter : IFactory<MapItemDTO, IMapItem>
    {
        private IMaps Maps { get; }

        public MapItemsDtoToEntityConverter(IMaps maps)
        {
            Maps = maps;
        }

        public IMapItem Create(MapItemDTO param)
        {
            if (param.Type == MapItemType.Image)
                return Maps.CreateImageItem(param.RelativeTop, param.RelativeLeft, param.RelativeWidth, param.RelativeHeight, param.Layer, param.File);
            else if (param.Type == MapItemType.Workstation)
                return Maps.CreateWorkstationItem(param.RelativeTop, param.RelativeLeft, param.RelativeWidth, param.RelativeHeight, param.Layer, param.WorkstationId);
            throw new NotImplementedException();
        }
    }
}
