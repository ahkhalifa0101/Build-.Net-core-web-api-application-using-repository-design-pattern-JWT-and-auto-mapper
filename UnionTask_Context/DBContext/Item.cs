using System;
using System.Collections.Generic;
using System.Text;

namespace UnionTask_Context.DBContext
{
    public class Item
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string ItemImage { get; set; }
    }
}