using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared25.Models
{
    public class MstListResponse
    {
        public bool status { get; set; }
        public string? Message { get; set; }

        public string? Message1 { get; set; }


        public List<MstDetailsEntity>? mstDetailsEntities = new List<MstDetailsEntity>();

        public List<MstNotificationEntity>? mstNotificationEntities = new List<MstNotificationEntity>();

        public List<MstRoleDetailsEntity>? mstRoleDetailsEntities = new List<MstRoleDetailsEntity>();

        public List<MstMappingEntity>? MstMappingEntities = new List<MstMappingEntity>();

        public List<MstMappingEntity> mstMappingEntity = new List<MstMappingEntity>();

    }
}
