using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UnionTask_Interface.IDataService;
using UnionTask_Model.DTOModel;
using UnionTask_Model.Settings;

namespace UnionTask.Controllers
{

    public class UserController : ControllerBase
    {
        #region Declar IServices InterFaces


        private static byte[] keyAndIvBytes;

        private readonly IUserRepository _UserRepository;
        private int clientId;
        private readonly IHttpContextAccessor _accessor;
        private readonly DTOConfig _appSettings;
        #endregion

        #region Inject IServices into Constructor
        public UserController(


        IUserRepository _User,

            IHttpContextAccessor _HttpContextAccessor,

            IOptions<DTOConfig> appSettings
            )
        {

            keyAndIvBytes = UTF8Encoding.UTF8.GetBytes("X2JU253B59M7M4EV");
            _UserRepository = _User;
            _accessor = _HttpContextAccessor;
            _appSettings = appSettings.Value;


        }

        #endregion



        #region User
       
      
        [HttpPost]
        public IActionResult Login(DTOUser Client)
        {
            var result1 = _UserRepository.Login(Client.Email, Client.Password);
            var result = _UserRepository.Authenticate(Client);
            if (result != null)
            {
                var obj = new
                {
                    access_token = result.FcmToken,
                    Id = result.Id,
                    UserName = result.UserName,
                    Gender = result.Gender,
                    Phone = result.Phone,
                    Email = result.Email,
                    CreationDate = result.CreationDate,
                    Otp = result.Otp,
                    FcmToken = result.FcmToken,
                    password = result.Password

                };
                return Ok(obj);
            }

            if (result != null)
                return Ok(result);
            else
                return Ok(new { status = false, message = "Invalid User" });
        }

        [Authorize]
        [HttpPost]
        public IActionResult ClientDeleteAccount()
        {
            clientId = Convert.ToInt32(_accessor.HttpContext.User.FindFirst(c => c.Type == "userId").Value);
            string userId = User.FindFirst(ClaimTypes.Email)?.Value;
            _UserRepository.DeleteItems(clientId);
            return Ok(new { message = "Clientt Has Been Deleted" });
        }

        [HttpPost]
        public IActionResult ClientAuthenticate(DTOUser Client)
        {
            var result = _UserRepository.Authenticate(Client);
            if (result != null)
            {
                var obj = new
                {
                    access_token = result.FcmToken,
                    Id = result.Id,
                    UserName = result.UserName,
                    Gender = result.Gender,
                    Phone = result.Phone,
                    Email = result.Email,
                    CreationDate = result.CreationDate,
                    Otp = result.Otp,
                    FcmToken = result.FcmToken,
                    password = result.Password
                };
                return Ok(obj);
            }
            else
                return Ok(new { message = "Invalid User" });
        }

        [HttpPost]
        public IActionResult Register(DTOUser Client)
        {

            if (_UserRepository.CheckEmailIsExist(Client.Email))
            {
                return Ok(new { status = false, message = "User Already Exists" });
            }
            else
            {
                var _Item = _UserRepository.AddItem(Client);
                var result = _UserRepository.Authenticate(_Item);
                if (result != null)
                {
                    var obj = new
                    {
                        access_token = result.FcmToken,
                        Id = result.Id,
                        UserName = result.UserName,
                        Gender = result.Gender,
                        Phone = result.Phone,
                        Email = result.Email,
                        CreationDate = result.CreationDate,
                        Otp = result.Otp,
                        FcmToken = result.FcmToken,
                        password = result.Password
                    };
                    return Ok(obj);
                }
                return Ok(_Item);
            }
        }

    
        #endregion
    }
}
