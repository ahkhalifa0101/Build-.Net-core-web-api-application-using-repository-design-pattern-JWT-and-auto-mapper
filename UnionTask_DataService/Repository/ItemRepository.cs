using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnionTask_Context.DBContext;
using UnionTask_Interface.IDataService;
using UnionTask_Model.DTOModel;
using UnionTask_Model.Settings;

namespace UnionTask_DataService.Repository
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        private IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        private int? UserId = null;
        
        private readonly DTOConfig _appSettings;

        public ItemRepository(UnionContext c, IMapper m, IHttpContextAccessor accessor, IOptions<DTOConfig> appSettings) : base(c)
        {

            _mapper = m;
            _accessor = accessor;
            _appSettings = appSettings.Value;

            if (_accessor.HttpContext.User.FindFirst(c => c.Type == "userId") != null)
            {
                UserId = Convert.ToInt32(_accessor.HttpContext.User.FindFirst(c => c.Type == "userId").Value);
            }

          
        }
        public DTOItem AddItem(DTOItem Item)
        {
            UserId = Convert.ToInt32(_accessor.HttpContext.User.FindFirst(c => c.Type == "userId").Value);
            var result = _mapper.Map<Item>(Item);
            Add(result);
            Save();
            Item.Id = result.Id;
            List<int> ids = new List<int>();

            return Item;

        }


        public DTOItem EditItem(DTOItem Item)
        {
            UserId = Convert.ToInt32(_accessor.HttpContext.User.FindFirst(c => c.Type == "userId").Value);
            var result = _mapper.Map<Item>(Item);
            Edit(result);
            Save();
            List<int> ids = new List<int>();
            ids.Add(Item.Id);
            return Item;
        }
        public void DeleteItems(List<DTOItem> Items)
        {
            foreach (DTOItem Item in Items)
            {
                var i = FindBy(x => x.Id == Item.Id).FirstOrDefault();
                if (i != null)
                {
                    Delete(i);
                    Save();
                }
            }
        }
        public List<DTOItem> SelectAll(bool? isAdmin)
        {
            var list = new List<DTOItem>();
            list = (from q in Context.Items
                    select new DTOItem
                    {
                        Id = q.Id,
                        ItemDescription = q.ItemDescription,
                        ItemImage = q.ItemImage,
                        ItemName = q.ItemName

                    }).ToList();


            return list;
        }
        public void DeleteItems(DTOItem Item)
        {
            var i = FindBy(x => x.Id == Item.Id).FirstOrDefault();
            if (i != null)
            {
                Delete(i);
                Save();
            }
        }
        public void DeleteItems(List<int> ItemIDs)
        {
            foreach (int ItemId in ItemIDs)
            {
                var i = FindBy(x => x.Id == ItemId).FirstOrDefault();
                if (i != null)
                {
                    Delete(i);
                    Save();
                }
            }
        }
        public void DeleteItems(int ItemId)
        {
            var i = FindBy(x => x.Id == ItemId).FirstOrDefault();
            if (i != null)
            {
                Delete(i);
                Save();
            }
        }
        public List<DTOItem> SelectAll()
        {
            var list = new List<DTOItem>();
            list = (from q in GetAll()
                    select new DTOItem

                    {
                        Id = q.Id,
                        ItemDescription = q.ItemDescription,
                        ItemImage = q.ItemImage,
                        ItemName = q.ItemName


                    }).OrderByDescending(on => on.Id).ToList();
            return list;
        }
        public List<DTOItem> SelectAll(int pageIndex, int pageSize)
        {
            var list = new List<DTOItem>();
            list = (from q in GetAll()
                    select new DTOItem

                    {
                        Id = q.Id,
                        ItemDescription = q.ItemDescription,
                        ItemImage = q.ItemImage,
                        ItemName = q.ItemName


                    }).OrderByDescending(on => on.Id).Skip((pageIndex) * pageSize).Take(pageSize).ToList();
            return list;
        }
        public DTOItem SelectById(int id)
        {
            var list = new DTOItem();
            list = (from q in GetAll()
                    where q.Id == id
                    select new DTOItem

                    {
                        Id = q.Id,
                        ItemDescription = q.ItemDescription,
                        ItemImage = q.ItemImage,
                        ItemName = q.ItemName


                    }).FirstOrDefault();
            return list;
        }
    }
}
