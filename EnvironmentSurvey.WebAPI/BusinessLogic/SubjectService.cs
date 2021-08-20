using EnvironmentSurvey.WebAPI.ClientSide.Models;
using EnvironmentSurvey.WebAPI.DataAccess;
using EnvironmentSurvey.WebAPI.DataAccess.Domains;
using EnvironmentSurvey.WebAPI.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentSurvey.WebAPI.BusinessLogic
{
    public interface ISubjectService
    {
        SubjectModel GetById(int id);
        List<SubjectModel> getAll();

        SubjectModel Create(SubjectModel model);

        SubjectModel Update(SubjectModel model);
        bool Delete(int id);

        bool Search(SubjectModel model);
    }
    public class SubjectService : ISubjectService
    {
        private readonly ESContext _context;
        private readonly IRepository<Subject> _subjectRepository;
        public SubjectService(IRepository<Subject> subjectRepository, ESContext context)
        {
            _subjectRepository = subjectRepository;
            _context = context;
        }

        public SubjectModel Create(SubjectModel model)
        {
            Subject subject = new Subject
            {
                Subject1 = model.Subject
            };
            _subjectRepository.Insert(subject);
            model.Id = subject.Id;
            return model;
        }

        public bool Delete(int id)
        {
            var subject = _subjectRepository.Get(id);
            if(subject == null) throw new Exception("There is no subject existed");
            _subjectRepository.Delete(subject);
            return true;
        }

        public  List<SubjectModel> getAll()
        {
            var listSubject =  _subjectRepository.GetAll().ToList();
            List<SubjectModel> listSubectModel = new List<SubjectModel>();
            if(listSubject.Count > 0)
            {
                listSubectModel = listSubject.Select(s => new SubjectModel
                {
                    Id = s.Id,
                    Subject = s.Subject1
            }).ToList();
            }
            return listSubectModel;
            
        }

        public SubjectModel GetById(int id)
        {
            Subject subject = _subjectRepository.Get(id);
            SubjectModel subjectModel = new SubjectModel();
            if(subject != null)
            {
                subjectModel.Id = subject.Id;
                subjectModel.Subject = subject.Subject1;
            }
            return subjectModel;
        }

        public bool Search(SubjectModel model)
        {
            var subject = _context.Subjects.Where(s => s.Subject1.Equals(model.Subject)).FirstOrDefault();
            if (subject == null) return true;
            if (subject.Id == model.Id) return true;
            return false;
        }

        public SubjectModel Update(SubjectModel model)
        {
            var subject = _subjectRepository.Get(model.Id);
            if (subject == null) throw new Exception("Subject Not Found");
            subject.Subject1 = model.Subject;
            _subjectRepository.Update(subject);
            return model;
        }
    }
}
