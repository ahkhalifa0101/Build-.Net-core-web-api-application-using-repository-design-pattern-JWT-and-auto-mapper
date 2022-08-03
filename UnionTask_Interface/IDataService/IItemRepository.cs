using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnionTask_Context.DBContext;
using UnionTask_Model.DTOModel;

namespace UnionTask_Interface.IDataService
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        DTOItem AddItem(DTOItem Item);
        DTOItem EditItem(DTOItem Item);
        void DeleteItems(List<DTOItem> Items);
        void DeleteItems(DTOItem Items);
        void DeleteItems(List<int> ItemIDs);
        void DeleteItems(int itemId);
        List<DTOItem> SelectAll(bool? isAdmin);
        DTOItem SelectById(int id);
    }
}
