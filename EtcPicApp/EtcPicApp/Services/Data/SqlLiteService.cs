using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EtcPicApp.Contracts.Services.Data;
using EtcPicApp.Models.Captions;
using EtcPicApp.Models.Jobs;
using EtcPicApp.Models.PhotoCaption;
using EtcPicApp.Models.Sql;
using static EtcPicApp.App;

namespace EtcPicApp.Services.Data
{
    public class SqlLiteService : ISqlLiteService
    {
        public async Task<IEnumerable<Jobs>> GetJobsAsync()
        {
#if DEBUG
            var list = new List<Jobs>();
            list.Add(new Jobs
            {
                Client = new Models.Clients.Client
                {
                    Id = 1,
                    Name = "Test Client"
                },
                FacilityAddress = "123 Main St",
                FacilityName = "Test facility",
                JobId = 1234
            });
            return list;
#endif
            return await Database.GetJobsAsync().ConfigureAwait(false);
        }


        public async Task<IEnumerable<PhotoCaption>> GetAllPhotoCaptionsAsync()
        {
            return await Database.GetAllPhotoCaptionsAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<PhotoCaption>> GetPhotoCaptionsAsync(int jobId)
        {
            return await Database.GetPhotoCaptionsAsync(jobId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Captions>> GetAllCaptionsAsync()
        {
#if DEBUG
            var caption = new Captions { Caption = "Test", Id = 1, IsRequired = true, ServiceId = 1 };
            return new List<Captions> { caption };
#endif
            return await Database.GetAllCaptionsAsync().ConfigureAwait(false);
        }




        public async Task<int> DeleteAsync<T>(int id)
        {
            return await Database.DeleteAsync<T>(id);
        }

        public Task<IEnumerable<PhotoStreamCaption>> GetPutPhotoStreamCaptions()
        {
            throw new System.NotImplementedException();
        }


        public Task<IEnumerable<PhotoStreamCaption>> GetPhotoStreamCaptions()
        {
            throw new System.NotImplementedException();
        }



        public async Task<int> GetLastJobIdAsync()
        {
            return await Database.ExecuteScalar("SELECT JobId FROM JobsTable order by JobId desc LIMIT 1 ");
        }

        public async Task<int> GetLastMaterialIdAsync()
        {
            return await Database.ExecuteScalar("SELECT Id FROM MaterialsTable order by Id desc  LIMIT 1 ");
        }

        public async Task<int> GetLastSampleIdAsync()
        {
            return await Database.ExecuteScalar("SELECT Id FROM SamplesTable order by Id desc  LIMIT 1 ");
        }




        public void DropAndRecreateJobs()
        {
            Database.DropAndRecreateJobs();
        }

        public void DropAndRecreateCaptions()
        {
            Database.DropAndRecreateCaptions();
        }

        
        public async Task<int> InsertOrUpdateAllAsync<T>(List<T> values) where T : BaseTable
        {
            return await Database.InsertOrReplaceAllAsync<T>(values);
        }

        public async Task<int> InsertAllAsync<T>(List<T> values)
        {
            return await Database.InsertAllAsync<T>(values);
        }


        public async Task<int> UpdateAsync<T>(T value) where T : BaseTable
        {
            return await Database.UpdateAsync<T>(value);
        }

        public async Task<int> InsertAsync<T>(T value) where T : BaseTable
        {
            return await Database.InsertAsync<T>(value);
        }

        public async Task<IEnumerable<PhotoStreamCaption>> GetPhotoStreamCaptionsAsync(int jobId)
        {
            var photoCaptions = (await GetPhotoCaptionsAsync(jobId)).ToList();
            var result = new List<PhotoStreamCaption>();
            foreach (var pc in photoCaptions)
            {
                var stream = File.OpenRead(pc.Path);
                result.Add(new PhotoStreamCaption
                {
                    Caption = pc.Caption,
                    Stream = stream,
                    Id = pc.Id,
                    JobId = pc.JobId,
                    Path = pc.Path
                });
                //stream.Close();
                //stream.Dispose();
            }
            return result;
        }

        public Task<IEnumerable<PhotoStreamCaption>> GetPostPhotoStreamCaptions()
        {
            throw new System.NotImplementedException();
        }

        public async Task<PhotoStreamCaption> GetPhotoCaptionAsync(int id)
        {
            var pc = await Database.GetPhotoCaptionAsync(id);
            var stream = File.OpenRead(pc.Path);
            return new PhotoStreamCaption
            {
                Caption = pc.Caption,
                Stream = stream,
                Id = pc.Id,
                JobId = pc.JobId,
                Path = pc.Path
            };
        }
    }
}
