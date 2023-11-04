namespace ASPDotNetCrud.Models
{
    public class Community
    {
        public Community(uint _id, string _name, string _description, uint _owner)
        {
            id = _id;
            name = _name;
            description = _description;
            owner = _owner;
        }

        public uint   id { get; private set; }
        public string name { get; private set; }
        public string description { get; private set; }
        public uint   owner { get; private set; }
    }
}
