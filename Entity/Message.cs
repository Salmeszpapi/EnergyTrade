﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csaba.Entity
{
    public class Message : BaseEntity
    {
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }
}