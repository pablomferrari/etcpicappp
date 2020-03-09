using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EtcPicApp.Extensions;
using EtcPicApp.Models.Captions;
using EtcPicApp.Models.Jobs;
using EtcPicApp.Models.PhotoCaption;
using EtcPicApp.Models.Sql;
using SQLite;
using Xamarin.Forms;

namespace EtcPicApp.Data
{
    public class AsbestosSurveyDatabase
    {
        readonly SQLiteAsyncConnection database;

        public AsbestosSurveyDatabase()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTablesAsync(CreateFlags.None,
                new Type[]
                {
                    new JobsTable().GetType(),
                    new PhotoCaptionTable().GetType(),
                    new Captions().GetType(),
                }
            ).Wait();

        }

        public async Task<IEnumerable<Jobs>> GetJobsAsync()
        {
            var jobs = await database.Table<JobsTable>().ToListAsync().ConfigureAwait(false);
            return jobs.Select(JobsTable.ToModel).ToList();
        }
        
        public async Task<int> InsertAllAsync<T>(List<T> jobs)
        {
            return await database.InsertAllAsync(jobs, true);
        }

        public async Task<int> InsertOrReplaceAllAsync<T>(List<T> jobs)
        {
            var updated = 0;
            foreach(var job in jobs)
            {
                await database.InsertOrReplaceAsync(job);
                updated++;
            }
            return updated;
        }

        public async Task<int> DeleteAsync<T>(int value)
        {
            return await database.DeleteAsync<T>(value);
        }

        public async Task<int> InsertAsync<T>(T value) where T : BaseTable
        {
            await database.InsertAsync(value);
            return value.Id;

        }

        public async Task<int> UpdateAsync<T>(T value) where T : BaseTable
        {
            return await database.UpdateAsync(value);
        }

        //public async Task<IEnumerable<Materials>> GetMaterialsAsync(int jobId)
        //{
        //    var materialsTable = await database.Table<MaterialsTable>().Where(x => x.JobId == jobId).ToListAsync();
        //    return materialsTable.ToMaterials();
        //}



        public async Task<IEnumerable<Captions>> GetAllCaptionsAsync()
        {
            var captions = await database.Table<CaptionsTable>().ToListAsync();
            return captions.Select(CaptionsTable.ToModel).ToList();
        }

        //public async Task<IEnumerable<Materials>> GetPostMaterialsAsync()
        //{
        //    var result = database.Table<MaterialsTable>().Where(x => x.IsNew).ToListAsync().Result;
        //    var materials = result.ToMaterials().ToList();
        //    foreach (var material in materials)
        //    {
        //        var st = await database.Table<SamplesTable>().Where(x => x.MaterialId == material.Id).ToListAsync();
                    
        //        material.Samples = st.ToSamples().ToList();
        //    }
        //    return materials;
        //}

        //public async Task<IEnumerable<Materials>> GetPutMaterialsAsync()
        //{
        //    var result = await database.Table<MaterialsTable>().Where(x => x.IsNew == false && x.IsLocal)
        //        .ToListAsync()
        //        .ConfigureAwait(false);
        //    return result.ToMaterials();
        //}


        public async Task<int> ExecuteScalar(string query)
        {
            return await database.ExecuteScalarAsync<int>(query);
        }

        public void DropAndRecreateJobs()
        {
            database.DropTableAsync<JobsTable>().Wait();
            database.CreateTableAsync<JobsTable>().Wait();
        }

        public void DropAndRecreateCaptions()
        {
            database.DropTableAsync<CaptionsTable>().Wait();
            database.CreateTableAsync<CaptionsTable>().Wait();
        }

        public async Task<IEnumerable<PhotoCaption>> GetAllPhotoCaptionsAsync()
        {
            var photoCaptions = await database.Table<PhotoCaptionTable>().ToListAsync().ConfigureAwait(false);
            return photoCaptions.Select(x => x.ToModel()).ToList();
        }

        public async Task<IEnumerable<PhotoCaption>> GetPhotoCaptionsAsync(int jobId)
        {
            var photoCaptions = await database.Table<PhotoCaptionTable>().Where(x => x.JobId == jobId).ToListAsync();
            return photoCaptions.Select(x => x.ToModel()).ToList();
        }

        public async Task<PhotoCaption> GetPhotoCaptionAsync(int id)
        {
            var photoCaption = await database.Table<PhotoCaptionTable>().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            return photoCaption.ToModel();
        }
    }
}
