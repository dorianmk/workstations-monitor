using Database.Interfaces.Workstation;
using Database.MongoDB.Common;
using MongoDB.Driver;

namespace Database.MongoDB.Workstation
{
    internal class Workstations : DbCollectionBase<IWorkstation, Workstation>, IWorkstations
    {
        internal Workstations(IMongoDatabase db)
            : base(db, "workstations")
        {
        }

        public IWorkstation AddWorkstation(string name)
        {
            var item = new Workstation(name);
            Add(item);
            return item;
        }
        
    }
}
