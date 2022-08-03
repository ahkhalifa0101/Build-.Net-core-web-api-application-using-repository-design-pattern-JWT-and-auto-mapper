using System;
using System.Collections.Generic;
using System.Text;
using UnionTask_Context.DBContext;
using UnionTask_Model.DTOModel;

namespace UnionTask_Interface.IDataService
{
    public interface IUserRepository : IGenericRepository<User>
    {
        DTOUser AddItem(DTOUser Item);
        DTOUser EditItem(DTOUser Item);
        void DeleteItems(List<DTOUser> Items);
        void DeleteItems(DTOUser Items);
        void DeleteItems(List<int> ItemIDs);
        void DeleteItems(int itemId);
   
        DTOUser SelectById(int id);
        DTOUser Authenticate(DTOUser Item);
        DTOUser Login(string Email, string Passwoed);

        bool CheckEmailIsExist(string Email);
        void SendMail(string Email,string otp);
    }
}
