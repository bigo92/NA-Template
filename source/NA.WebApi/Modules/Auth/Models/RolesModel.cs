using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static tci.repository.Models.RolesRepositoryModel;

namespace tci.server.Modules.Aut.Models
{
    public class RolesModel
    {
        public class Search_RolesModel : Search_RolesRepositoryModel
        {
        }

        public class Add_RolesModel : Data_RolesRepositoryModel
        {
            [Required(ErrorMessage = "Không được để trống")]
            public override string name { get => base.name; set => base.name = value; }

        }

        public class Edit_RolesModel : Add_RolesModel
        {

        }

        public class Patch_RolesModel : Edit_RolesModel
        {

        }

        public class Detail_RolesModel: Detail_RolesRepositoryModel
        {

        }

        public class Delete_RolesModel: Delete_RolesRepositoryModel
        {

        }

        public class Move_RolesModel : Move_RolesRepositoryModel
        {

        }

        public class Upload_RolesModel : Upload_RolesRepositoryModel
        {

        }
    }
}
