using DataTransfer.Interfaces;
using System.Collections.Generic;

namespace Common.DataTransfer.DataPackets.AdminClient
{
    public class SaveMapsPacket : IData
    {
        public List<MapDTO> Maps { get; set; }
    }

    public class GetMapsPacket : IData
    {
        public List<MapDTO> Maps { get; set; }
    }

    public class MapDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<MapItemDTO> Items { get; set; }
    }

    public class MapItemDTO
    {
        public double RelativeTop { get; set; }
        public double RelativeLeft { get; set; }
        public double RelativeWidth { get; set; }
        public double RelativeHeight { get; set; }
        public int Layer { get; set; }
        public byte[] File { get; set; }
        public string WorkstationId { get; set; }
        public MapItemType Type { get; set; }
    }

    public enum MapItemType
    {
        Image,
        Workstation
    }
}
