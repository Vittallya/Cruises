using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Models;
using DAL.Dto;
using System.Data.Entity;
using AutoMapper;

namespace BL
{
    public class ToursService
    {
        private readonly AllDbContext dbContext;
        private readonly MapperService mapper;
        private IEnumerable<Tour> allServices;
        private IEnumerable<TourDto> allServicesDto;


        public ToursService(AllDbContext dbContext, MapperService mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task ReloadAsync(Func<string, string> pathGetter)
        {
            await dbContext.Tours.LoadAsync();

            allServices = dbContext.Tours.AsNoTracking().ToList();

            allServicesDto = allServices.Select(x => 
            {
                var t = mapper.MapTo<Tour, TourDto>(x);
                t.ImagePath = pathGetter?.Invoke(t.ImageName);
                return t;
            });
        }

        public async Task<IEnumerable<TourDto>> GetAllServices(string name = null)
        {            

            if(name != null)
            {
                return allServicesDto.Where(x => x.Name.Contains(name));
            }

            return allServicesDto;
        }

    }
}
