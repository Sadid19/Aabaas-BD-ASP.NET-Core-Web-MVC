using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Collections.Generic;

namespace BLL.Services
{
    public class PackageService
    {
        PackageRepo repo;
        Mapper mapper;

        public PackageService(PackageRepo repo)
        {
            this.repo = repo;
            mapper = MapperConfig.GetMapper();
        }

        public List<HotPackageDTO> Get()
        {
            List<HotPackage> data = repo.GetActive();
            return mapper.Map<List<HotPackageDTO>>(data);
        }

        public List<HotPackageDTO> GetAll()
        {
            List<HotPackage> data = repo.Get();
            return mapper.Map<List<HotPackageDTO>>(data);
        }

        public HotPackageDTO Get(int id)
        {
            HotPackage data = repo.Get(id);
            if (data == null)
            {
                return null;
            }

            return mapper.Map<HotPackageDTO>(data);
        }

        public bool Create(HotPackageDTO dto)
        {
            HotPackage package = mapper.Map<HotPackage>(dto);
            return repo.Create(package);
        }

        public bool Update(HotPackageDTO dto)
        {
            HotPackage package = mapper.Map<HotPackage>(dto);
            package.PackageId = dto.PackageId;
            return repo.Update(package);
        }

        public bool Delete(int id)
        {
            return repo.Delete(id);
        }
    }
}
