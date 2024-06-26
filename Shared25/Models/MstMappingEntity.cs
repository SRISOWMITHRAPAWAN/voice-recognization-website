namespace Shared25.Models
{
    public class MstMappingEntity
    {

        public int MappingID { get; set; }

        public int PSId { get; set; }

        public int NotificationID { get; set; }

        public int RoleID { get; set; }

        public string? DisplayName { get; set; }

        public string? DisplayNameShort { get; set; }

        public int MenuOrder { get; set; }

        public string? NotificationName { get; set; }

        public string? Roles { get; set; }
    }
}