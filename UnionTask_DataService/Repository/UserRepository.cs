using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using UnionTask_Context.DBContext;
using UnionTask_Interface.IDataService;
using UnionTask_Model.DTOModel;
using UnionTask_Model.Settings;

namespace UnionTask_DataService.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        private int? UserId = null;
        private string UserType = null;
        private readonly DTOConfig _appSettings;

        public UserRepository(UnionContext c, IMapper m, IHttpContextAccessor accessor, IOptions<DTOConfig> appSettings) : base(c)
        {

            _mapper = m;
            _accessor = accessor;
            _appSettings = appSettings.Value;

            if (_accessor.HttpContext.User.FindFirst(c => c.Type == "userId") != null)
            {
                UserId = Convert.ToInt32(_accessor.HttpContext.User.FindFirst(c => c.Type == "userId").Value);
            }

            if (_accessor.HttpContext.User.FindFirst(c => c.Type == "userType") != null)
            {
                UserType = _accessor.HttpContext.User.FindFirst(c => c.Type == "userType").Value;
            }
        }
        public DTOUser AddItem(DTOUser Item)
        {
            var result = _mapper.Map<User>(Item);
            result.UserName = Item.UserName;
            result.CreationDate = DateTime.Now;
            Add(result);
            Save();
            Item.Id = result.Id;
            List<int> ids = new List<int>();
            ids.Add(Item.Id);

     
            return Item;

        }


        public DTOUser EditItem(DTOUser Item)
        {
            UserId = Convert.ToInt32(_accessor.HttpContext.User.FindFirst(c => c.Type == "userId").Value);
            var result = _mapper.Map<User>(Item);
            Edit(result);
            Save();
            List<int> ids = new List<int>();
            ids.Add(Item.Id);
            return Item;
        }
        public void DeleteItems(List<DTOUser> Items)
        {
            foreach (DTOUser Item in Items)
            {
                var i = FindBy(x => x.Id == Item.Id).FirstOrDefault();
                if (i != null)
                {
                    Delete(i);
                    Save();
                }
            }
        }
 
        public void DeleteItems(DTOUser Item)
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
        public List<DTOUser> SelectAll()
        {
            var list = new List<DTOUser>();
            list = (from q in GetAll()
                    select new DTOUser

                    {
                        Id = q.Id,

                        UserName = q.UserName,
                        Phone = q.Phone,
                        Email = q.Email,
                        CreationDate = q.CreationDate,
                        Otp = q.Otp,
                        FcmToken = q.FcmToken,
                        Gender = q.Gender,
                        BirthDate = q.BirthDate,
                        Password = q.Password,


                    }).OrderByDescending(on => on.Id).ToList();
            return list;
        }
        public List<DTOUser> SelectAll(int pageIndex, int pageSize)
        {
            var list = new List<DTOUser>();
            list = (from q in GetAll()
                    select new DTOUser

                    {
                        Id = q.Id,
                        UserName = q.UserName,
                        Phone = q.Phone,
                        Email = q.Email,
                        CreationDate = q.CreationDate,
                        Otp = q.Otp,
                        FcmToken = q.FcmToken,
                        Gender = q.Gender,
                        BirthDate = q.BirthDate,
                        Password = q.Password,


                    }).OrderByDescending(on => on.Id).Skip((pageIndex) * pageSize).Take(pageSize).ToList();
            return list;
        }
        public DTOUser SelectById(int id)
        {
            var list = new DTOUser();
            list = (from q in GetAll()
                    where q.Id == id
                    select new DTOUser

                    {
                        Id = q.Id,
                        UserName = q.UserName,
                        Phone = q.Phone,
                        Email = q.Email,
                        CreationDate = q.CreationDate,
                        Otp = q.Otp,
                        FcmToken = q.FcmToken,
                        Gender = q.Gender,
                        BirthDate = q.BirthDate,
                        Password = q.Password,


                    }).FirstOrDefault();
            return list;
        }


        private string GetToken(DTOUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecurityKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim("userId", user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMonths(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public DTOUser Authenticate(DTOUser Item)
        {
            var user = FindBy(x => x.Email == Item.Email && x.Password == Item.Password).FirstOrDefault();
            if (user != null)
            {
                if (user.Otp == Item.Otp)
                {
                    user.FcmToken = Item.FcmToken;
                    Edit(user);
                    Save();
                    var item = SelectById(user.Id);
                    item.FcmToken = GetToken(item);

                    return item;
                }
                else
                {

                    return null;
                }
            }
            else
            {
                return null;
            }

        }
     
        public DTOUser Login(string Email, string Password)
        {
            var Item = FindBy(x => x.Email == Email && x.Password == Password).FirstOrDefault();
            if (Item == null)
                return null;
            else

            {
                var otp = new Random().Next(1000, 9999);
                Item.Otp = otp.ToString();
                Edit(Item);
                Save();
                string msgText = "task App OTP : " + otp.ToString();
                SendMail(Email, msgText);
                return SelectById(Item.Id);
            }
        }

        public void ChangeFCMToken(string FcmToken)
        {
            var result = Context.Users.Find(UserId);
            if (result != null)
            {
                result.FcmToken = FcmToken;
                Edit(result);
                Save();
            }
        }
        public void ChangePassWord(string Email, string OldPass, string NewPass)
        {
            var result = FindBy(x => x.Email == Email && x.Password == OldPass ).FirstOrDefault();
            if (result != null)
            {
                result.Otp = NewPass;
                Edit(result);
                Save();
            }
        }

        public bool CheckEmailIsExist(string Email)
        {
            var result = FindBy(s => s.Email == Email).FirstOrDefault();
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public void SendMail(string Email, string otp)
        {
            var result = FindBy(s => s.Email == Email).FirstOrDefault();
            if (result != null)
            {
                string to = result.Email;
                string from = "******@gmail.com";
                MailMessage message = new MailMessage(from, to);
                string password = "Your mail app key ";
                string mailbody = "Your Code : " + otp;

                message.Subject = "send At " + DateTime.Now;
                message.Body = mailbody;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("******@gmail.com", password)
                };
                using (var mess = new MailMessage(from, to)
                {

                    Subject = message.Subject,
                    Body = message.Body
                })
                {

                    mess.IsBodyHtml = true;
                    smtp.Send(mess);
                }

            } 
       

               
            }

        }
    }
