﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Employer
{
    public class EUserListDTO
    {
        public int CompanyId { get; set; }

        public int EUserId { get; set; }
        
        public string Name { get; set; } 

        public string Email { get; set; }
        
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

        public bool IsActive { get; set; }
        
    }
}