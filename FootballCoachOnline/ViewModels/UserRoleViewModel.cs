﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballCoachOnline.ViewModels
{
    public class UserRoleViewModel
    {
        public SelectList UserList { get; set; }
        public string UserId { get; set; }
        public SelectList RoleList { get; set; }
        public string RoleId { get; set; }
    }
}
