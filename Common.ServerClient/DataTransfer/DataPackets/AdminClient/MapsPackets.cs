using DataTransfer.Interfaces;

namespace Common.DataTransfer.DataPackets.AdminClient
{
    public class SaveMapsPacket : IData
    {
        public MapDTO[] Maps { get; set; }
    }

    public class GetMapsPacket : IData
    {
        public MapDTO[] Maps { get; set; }
    }

    public class MapDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public MapItemDTO[] Items { get; set; }
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
