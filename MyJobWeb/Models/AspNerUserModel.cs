﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobWeb.Models
{
    public class AspNerUserModel
    {
        public class AspNetUserModel
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public bool EmailConfirmed { get; set; }
            public string PasswordHash { get; set; }
            public string SecurityStamp { get; set; }
            public string PhoneNumber { get; set; }
            public bool PhoneNumberConfirmed { get; set; }
            public bool TwoFactorEnabled { get; set; }
            public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
            public bool LockoutEnabled { get; set; }
            public int AccessFailedCount { get; set; }
            public string UserName { get; set; }
            public string RoleType { get; set; }

        }
    }
}