using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Village
{
    public class VillageTile
    {
        public Vector2Int coordinate;

        public Building objectOnTile;

        public VillageTile(int x, int y)
        {
            coordinate = new Vector2Int(x, y);
        }
    }
}
