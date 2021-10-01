using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Category
    {
        public int id { set; get; }
        public int SortOrder{ set; get; }
        public bool isShowOnHome{ set; get; }
        public int ParentId{ set; get; }
        public Status stauts{ set; get; }

}
}
