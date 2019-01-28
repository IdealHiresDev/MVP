using IdealHires.BAL.DataContext;
using IdealHires.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IdealHires.DTO.Candidate;
using IdealHires.DTO;

namespace IdealHires.BAL.Business
{
    public class PreferencesService : IDisposable
    {
        #region Private Menber
        bool disposed = false;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public PreferencesService()
        {
            _unitOfWork = new UnitOfWork(new IdealHiresDbContext());
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            disposed = true;
        }
        #endregion

        #region Preferences       

        public int InsertPreferencesDetails(CandidatePreferencesDTO preferencesCandidate)
        {
            int keywordId = 0;
            KeywordsProfile keywordProfile = new KeywordsProfile();
            try
            {
                if (preferencesCandidate != null)
                {
                    User user = _unitOfWork.Users.Get(preferencesCandidate.UserId);
                    if (user.Id > 0)
                    {
                        var profile = user.Profiles.FirstOrDefault();
                        List<JobTypeProfile> jobTypeProfiles = profile.JobTypeProfiles.ToList();
                        List<JobCategoryProfile> jobCategoryProfiles = profile.JobCategoryProfiles.ToList();
                        KeywordsProfile keywordsProfiles = profile.KeywordsProfiles.FirstOrDefault();
                        preferencesCandidate.ProfileId = profile.Id;

                        // Add/Update objective
                        profile.Objective = preferencesCandidate.Objective;
                        _unitOfWork.ProfileRepository.Update(profile);                       

                        if (jobTypeProfiles.Count > 0)
                        {
                            var jobTypeProfileList = MapJobTypeProfileData(preferencesCandidate);
                            _unitOfWork.JobTypeProfileRepository.RemoveRange(jobTypeProfiles);
                            _unitOfWork.JobTypeProfileRepository.AddRange(jobTypeProfileList);                           
                        }
                        else
                        {
                            var jobTypeProfileList = MapJobTypeProfileData(preferencesCandidate);
                            _unitOfWork.JobTypeProfileRepository.AddRange(jobTypeProfileList);                            
                        }
                        if (jobCategoryProfiles.Count > 0)
                        {
                            var jobCategoryProfileList = MapJobCategoryProfileData(preferencesCandidate);
                            _unitOfWork.JobCategoryProfileRepository.RemoveRange(jobCategoryProfiles);
                            _unitOfWork.JobCategoryProfileRepository.AddRange(jobCategoryProfileList);                            
                        }
                        else
                        {
                            var jobCategoryProfileList = MapJobCategoryProfileData(preferencesCandidate);
                            _unitOfWork.JobCategoryProfileRepository.AddRange(jobCategoryProfileList);                            
                        }
                        if (keywordsProfiles != null)
                        {
                            keywordsProfiles.Keywords = preferencesCandidate.Keywords;
                            keywordsProfiles.UpdatedAt = DateTime.Now;
                            keywordsProfiles.UpdatedBy = preferencesCandidate.UserId; 
                            
                            _unitOfWork.KeywordsProfileRepository.Update(keywordsProfiles);
                            keywordId = keywordsProfiles.Id;
                        }
                        else
                        {
                            keywordProfile = new KeywordsProfile()
                            {
                                ProfileId = preferencesCandidate.ProfileId,
                                Keywords = preferencesCandidate.Keywords,
                                CreatedAt = DateTime.Now,
                                CreatedBy = preferencesCandidate.UserId
                            };
                            _unitOfWork.KeywordsProfileRepository.Add(keywordProfile);
                            keywordId = keywordProfile.Id;
                        }
                    }
                    _unitOfWork.Complete();                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return keywordId;
        }

        public JobTypeDTO GetJobType(int id)
        {
            JobTypeDTO jobTypeDTO = new JobTypeDTO();
            try
            {

                JobType jobTypeList = _unitOfWork.JobTypeRepository.Get(id);
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<JobType, JobTypeDTO>()
                    .ForMember(destination => destination.Id, opts => opts.MapFrom(source => source.Id))
                    .ForMember(destination => destination.Name, opts => opts.MapFrom(source => source.Name))
                    .ForMember(destination => destination.IsActive, opts => opts.MapFrom(source => source.IsActive));
                });
                IMapper iMapper = config.CreateMapper();
                jobTypeDTO = iMapper.Map<JobType, JobTypeDTO>(jobTypeList);
                return jobTypeDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JobTypeDTO> GetJobTypeList()
        {
            List<JobTypeDTO> jobTypeDTOList = new List<JobTypeDTO>();
            try
            {

                List<JobType> jobTypeList = _unitOfWork.JobTypeRepository.GetAll().ToList();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<JobType, JobTypeDTO>()
                    .ForMember(destination => destination.Id, opts => opts.MapFrom(source => source.Id))
                    .ForMember(destination => destination.Name, opts => opts.MapFrom(source => source.Name))
                    .ForMember(destination => destination.IsActive, opts => opts.MapFrom(source => source.IsActive));
                });
                IMapper iMapper = config.CreateMapper();
                jobTypeDTOList = iMapper.Map<List<JobType>, List<JobTypeDTO>>(jobTypeList);
                return jobTypeDTOList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobCategoryDTO GetJobCategory(int id)
        {
            JobCategoryDTO jobCategoryDTO = new JobCategoryDTO();
            try
            {

                JobCategory jobCategory = _unitOfWork.JobCategoryRepository.Get(id);
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<JobCategory, JobCategoryDTO>()
                    .ForMember(destination => destination.Id, opts => opts.MapFrom(source => source.Id))
                    .ForMember(destination => destination.Name, opts => opts.MapFrom(source => source.Name))
                    .ForMember(destination => destination.IsActive, opts => opts.MapFrom(source => source.IsActive));
                });
                IMapper iMapper = config.CreateMapper();
                jobCategoryDTO = iMapper.Map<JobCategory, JobCategoryDTO>(jobCategory);
                return jobCategoryDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JobCategoryDTO> GetJobCategoryList()
        {
            List<JobCategoryDTO> jobCategoryDTOList = new List<JobCategoryDTO>();
            try
            {

                List<JobCategory> jobCategoryList = _unitOfWork.JobCategoryRepository.GetAll().ToList();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<JobCategory, JobCategoryDTO>()
                    .ForMember(destination => destination.Id, opts => opts.MapFrom(source => source.Id))
                    .ForMember(destination => destination.Name, opts => opts.MapFrom(source => source.Name))
                    .ForMember(destination => destination.IsActive, opts => opts.MapFrom(source => source.IsActive));
                });
                IMapper iMapper = config.CreateMapper();
                jobCategoryDTOList = iMapper.Map<List<JobCategory>, List<JobCategoryDTO>>(jobCategoryList);
                return jobCategoryDTOList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Private
        private List<JobTypeProfile> MapJobTypeProfileData(CandidatePreferencesDTO preferencesCandidate)
        {
            List<JobTypeProfile> jobTypeProfileList = new List<JobTypeProfile>();
            for (int i = 0; i < preferencesCandidate.SelectedJobTypes.Count; i++)
            {
                var jobTypeProfileConvert = new JobTypeProfile()
                {
                    ProfileId = preferencesCandidate.ProfileId,
                    JobTypeId = int.Parse(preferencesCandidate.SelectedJobTypes[i]),
                    CreatedAt = DateTime.Now,
                    CreatedBy = preferencesCandidate.UserId
                };
                jobTypeProfileList.Add(jobTypeProfileConvert);
            }
            return jobTypeProfileList;
        }

        private List<JobCategoryProfile> MapJobCategoryProfileData(CandidatePreferencesDTO preferencesCandidate)
        {
            List<JobCategoryProfile> jobCategoryProfileList = new List<JobCategoryProfile>();
            for (int i = 0; i < preferencesCandidate.SelectedJobCategory.Count; i++)
            {
                var jobCategoryProfileConvert = new JobCategoryProfile()
                {
                    ProfileId = preferencesCandidate.ProfileId,
                    JobCategoryId = int.Parse(preferencesCandidate.SelectedJobCategory[i]),
                    CreatedAt = DateTime.Now,
                    CreatedBy = preferencesCandidate.UserId
                };
                jobCategoryProfileList.Add(jobCategoryProfileConvert);
            }
            return jobCategoryProfileList;
        }
        #endregion
    }
}
