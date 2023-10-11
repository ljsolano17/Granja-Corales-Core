using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using data = Solution.DO.Objects;

namespace Solution.API.Mapping
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {


            CreateMap<data.Articulos, DataModels.Articulos>().ReverseMap();

            CreateMap<data.ArticulosSolicitud, DataModels.ArticulosSolicitud>().ReverseMap();

            CreateMap<data.Solicitudes, DataModels.Solicitudes>().ReverseMap();

            CreateMap<data.Categorias, DataModels.Categorias>().ReverseMap();

        }

    }
}
