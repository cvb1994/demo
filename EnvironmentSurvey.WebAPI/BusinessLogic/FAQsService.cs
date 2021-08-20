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
    public interface IFAQsService
    {
        public Task<List<FAQsModel>> GetAll();
        public Task<FAQsModel> GetByID(int id);
        public Task<bool> Create(FAQsModel model);
        public Task<bool> Update(FAQsModel model);
        public Task<bool> Delete(int Id);
    }
    public class FAQsService : IFAQsService
    {
        private readonly ESContext _context;
        public FAQsService(ESContext context)
        {
            _context = context;
        }
        public async Task<bool> Create(FAQsModel model)
        {
            var faq = new Faq
            {
                Issue = model.Issue,
                Solution = model.Solution,
                CreatedDate = DateTime.UtcNow
            };
            _context.Faqs.Add(faq);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;
            return false;
        }

        public async Task<bool> Delete(int Id)
        {
            Faq faq = await _context.Faqs.FindAsync(Id);
            if (faq == null)
                throw new Exception("FAQ not found");
            else
            {
                faq.DeletedDate = DateTime.UtcNow;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return true;
                return false;
            }
        }

        public async Task<List<FAQsModel>> GetAll()
        {
            var listFAQs = await _context.Faqs.ToListAsync();
            var faqModel = listFAQs.Where(x => !x.DeletedDate.HasValue).Select(x => new FAQsModel
            {
                ID = x.Id,
                Issue = x.Issue,
                Solution = x.Solution
            });
            return faqModel.ToList();
        }

        public async Task<FAQsModel> GetByID(int id)
        {
            Faq faq = await _context.Faqs.FindAsync(id);
            if (faq == null)
                throw new Exception("FAQ not found");
            else
            {
                if (faq.DeletedDate.HasValue)
                {
                    throw new Exception("FAQ not found");
                }
                else
                {
                    var faqModel = new FAQsModel
                    {
                        ID = faq.Id,
                        Issue = faq.Issue,
                        Solution = faq.Solution
                    };
                    return faqModel;
                }
            }
        }

        public async Task<bool> Update(FAQsModel model)
        {
            Faq faq = await _context.Faqs.FindAsync(model.ID);
            if (faq == null)
                throw new Exception("FAQ not found");
            else
            {
                faq.Issue = model.Issue;
                faq.Solution = model.Solution;
                faq.ModifiedDate = DateTime.UtcNow;
                _context.Entry(faq).State = EntityState.Modified;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return true;
                return false;
            }
        }
    }
}
