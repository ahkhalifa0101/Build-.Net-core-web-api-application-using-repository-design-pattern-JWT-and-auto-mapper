using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UnionTask_Context.DBContext;
using UnionTask_Model.DTOModel;

namespace UnionTask_DataService.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {

            CreateMap<User, DTOUser>().ReverseMap();
            CreateMap<Item, DTOItem>().ReverseMap();


        }
    }
}
