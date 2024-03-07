﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.DTOs.Order
{
    public class SingleOrder
    {
        public object BasketItems { get; set; }
        public string Address { get; set; } 
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public string OrderCode { get; set; }
        public string Id { get; set; }
        public bool Completed { get; set; }
    }
}
