﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interface
{
    public interface IItem
    {
        ItemData itemData { get; set; }

        bool OnUse(int qty);
    }
}
