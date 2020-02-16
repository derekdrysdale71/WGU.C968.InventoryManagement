using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WGU.C968.InventoryManagement.Helpers
{
    public static class HelperExtensions
    {
        public static int GetNextId(this List<int> ids)
        {
            return ids.Max() + 1;
        }
    }
}
