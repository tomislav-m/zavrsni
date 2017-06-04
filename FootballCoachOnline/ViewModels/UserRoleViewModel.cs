﻿using Microsoft.AspNetCore.Mvc.Rendering;

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
