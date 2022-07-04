using AdminClientApp.ViewModels.Essential.Workstations;

namespace AdminClientApp.ViewModels.Essential.Maps
{
    public class WorkstationItem : MapItemBase
    {
        public string WorkstationId { get; }
        public WorkstationViewModel Workstation { get; }

        internal WorkstationItem(WorkstationViewModel workstation)
               : this(true, workstation.Id, workstation, 0.4, 0.4, 0.2, 0.1, 0)
        {
        }

        internal WorkstationItem(bool editMode, string workstationId, WorkstationViewModel workstation, double relativeTop, double relativeLeft, double relativeWidth, double relativeHeight, int layer)
              : base(relativeTop, relativeLeft, relativeWidth, relativeHeight, layer, editMode)
        {
            Workstation = workstation;
            WorkstationId = workstationId;
        }

    }
}
