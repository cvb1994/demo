using EnvironmentSurvey.WebAPI.ClientSide.Models;
using EnvironmentSurvey.WebAPI.DataAccess;
using EnvironmentSurvey.WebAPI.DataAccess.Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.BusinessLogic
{
    public interface ISupportInfoService
    {
        public Task<SupportInfoModel> GetSuportInfo();
        public Task<bool> Create(SupportInfoModel model);
        public Task<bool> Update(SupportInfoModel model);
        public Task<bool> Delete(int Id);
    }
    public class SupportInfoService : ISupportInfoService
    {
        private readonly ESContext _context;
        public SupportInfoService(ESContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(SupportInfoModel model)
        {
            var si = new SupportInformation
            {
                Company = model.Company,
                CompanyTel = model.CompanyTel,
                Address = model.Address,
                Supporter = model.Supporter,
                SupporterTel = model.SupporterTel,
                CreatedDate = DateTime.UtcNow
            };
            _context.SupportInformations.Add(si);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;
            return false;
        }

        public async Task<bool> Delete(int Id)
        {
            SupportInformation si = await _context.SupportInformations.FindAsync(Id);
            if (si == null)
                throw new Exception("Support Information not found");
            else
            {
                si.DeletedDate = DateTime.UtcNow;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return true;
                return false;
            }
        }

        public async Task<SupportInfoModel> GetSuportInfo()
        {
            var listSI = await _context.SupportInformations.ToListAsync();
            var siModel = listSI.Where(x => !x.DeletedDate.HasValue).Select(x => new SupportInfoModel
            {
                Id = x.Id,
                Company = x.Company,
                CompanyTel = x.CompanyTel,
                Address = x.Address,
                Supporter = x.Supporter,
                SupporterTel = x.SupporterTel
            }).First();
            return siModel;
        }

        public async Task<bool> Update(SupportInfoModel model)
        {
            SupportInformation si = await _context.SupportInformations.FindAsync(model.Id);
            if (si == null)
                throw new Exception("Support Information not found");
            else
            {
                si.Company = model.Company;
                si.CompanyTel = model.CompanyTel;
                si.Address = model.Address;
                si.Supporter = model.Supporter;
                si.SupporterTel = model.SupporterTel;
                si.ModifiedDate = DateTime.UtcNow;
                _context.Entry(si).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return true;
                return false;
            }
        }
    }
}
