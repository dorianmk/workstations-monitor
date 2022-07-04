using Database.Interfaces.Map.Item;

namespace Database.MongoDB.Map.Item
{
    internal class WorkstationItem : MapItem, IWorkstationItem
    {
        public string WorkstationId { get; private set; }

        internal WorkstationItem(double relativeTop, double relativeLeft, double relativeWidth, double relativeHeight, int layer, string workstationId)
              : base(relativeTop, relativeLeft, relativeWidth, relativeHeight, layer)
        {
            WorkstationId = workstationId;
        }

    }
}
