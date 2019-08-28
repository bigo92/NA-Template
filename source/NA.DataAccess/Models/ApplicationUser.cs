using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using NA.Common.Models;

namespace NA.DataAccess.Models
{

    public class ApplicationUser : IdentityUser<long>
    {
        public virtual DataJson data { get; set; }
        public virtual DataDbModel data_db { get; set; }
        public virtual IEnumerable<DataFilesModel> files { get; set; }

        public class DataJson
        {
            public virtual string qrCode { get; set; }

            public virtual int? accessFailedCount { get; set; }

            public virtual DateTime? lockoutEnd { get; set; }
        }
    }
}
