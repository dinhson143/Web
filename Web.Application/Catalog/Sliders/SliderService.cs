using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using Web.Data.Enums;
using Web.Utilities.Exceptions;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Sliders;

namespace Web.Application.Catalog.Sliders
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _env;
        private static string ApiKey = "AIzaSyDsIhxUtoEuX-GsYhTCd3T6tSUr2VA2MiA";
        private static string Bucket = "bds-asp-mvc.appspot.com";
        private static string AuthEmail = "dinhson14399@gmail.com";
        private static string AuthPassword = "tranthingocyen";

        public SliderService(AppDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<ResultApi<string>> CreateSlider(SliderCreate request)
        {
            var slider = new Slider()
            {
                Description = request.Description,
                Name = request.Name,
                Url = request.Url,
                DateCreated = DateTime.Now
            };

            string folderName = "firebaseFiles";
            string path = Path.Combine(_env.WebRootPath, $"images/{folderName}");
            // firebase uploading
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            FileStream fs = null;
            // upload file to firebase
            if (Directory.Exists(path))
            {
                using (fs = new FileStream(Path.Combine(path, request.Image.FileName), FileMode.Create))
                {
                    await request.Image.CopyToAsync(fs);
                }

                fs = new FileStream(Path.Combine(path, request.Image.FileName), FileMode.Open);
            }
            else
            {
                Directory.CreateDirectory(path);
            }

            // Cacellation token
            var cancellation = new CancellationTokenSource();
            var upload = await new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    }
                )
                .Child("assets")
                .Child($"{request.Image.FileName}.{Path.GetExtension(request.Image.FileName).Substring(1)}")
                .PutAsync(fs, cancellation.Token);

            try
            {
                var linkImg = upload;
                slider.Image = linkImg;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            await _context.Sliders.AddAsync(slider);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<string>("Thêm Slider thành công");
            }
            return new ResultErrorApi<string>("Thêm Slider thất bại");
        }

        public async Task<int> Delete(int id)
        {
            var category = await _context.Sliders.FindAsync(id);
            if (category == null) throw new WebException($"Cannot find a Slider with id: {id}");

            category.Status = Status.InActive;
            return await _context.SaveChangesAsync();
        }

        public async Task<List<SliderViewModel>> GetAll()
        {
            var query = from sl in _context.Sliders
                        where sl.Status == Status.Active
                        orderby sl.DateCreated descending
                        select new { sl };
            return await query.Select(x => new SliderViewModel()
            {
                Id = x.sl.Id,
                Name = x.sl.Name,
                Description = x.sl.Description,
                Image = x.sl.Image,
                Url = x.sl.Url,
                Status = x.sl.Status,
                DateCreated = x.sl.DateCreated
            }).ToListAsync();
        }

        public async Task<int> UpdateSlider(SliderUpdateRequest request)
        {
            var slider = await _context.Sliders.FindAsync(request.Id);
            if (slider == null) throw new WebException($"Cannot find a slider with id: {request.Id}");

            slider.Name = request.Name;
            slider.Url = request.Url;
            slider.Description = request.Description;

            if (request.Image != null)
            {
                string folderName = "firebaseFiles";
                string path = Path.Combine(_env.WebRootPath, $"images/{folderName}");
                // firebase uploading
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                FileStream fs = null;
                // upload file to firebase
                if (Directory.Exists(path))
                {
                    using (fs = new FileStream(Path.Combine(path, request.Image.FileName), FileMode.Create))
                    {
                        await request.Image.CopyToAsync(fs);
                    }

                    fs = new FileStream(Path.Combine(path, request.Image.FileName), FileMode.Open);
                }
                else
                {
                    Directory.CreateDirectory(path);
                }

                // Cacellation token
                var cancellation = new CancellationTokenSource();
                var upload = await new FirebaseStorage(
                        Bucket,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                            ThrowOnCancel = true
                        }
                    )
                    .Child("assets")
                    .Child($"{request.Image.FileName}.{Path.GetExtension(request.Image.FileName).Substring(1)}")
                    .PutAsync(fs, cancellation.Token);

                try
                {
                    var linkImg = upload;
                    slider.Image = linkImg;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<ResultApi<SliderViewModel>> GetSliderById(int sliderId)
        {
            var slider = await _context.Sliders.FindAsync(sliderId);
            var data = new SliderViewModel()
            {
                Id = slider.Id,
                Description = slider.Description,
                Image = slider.Image,
                Name = slider.Name,
                Url = slider.Url,
                Status = slider.Status
            };
            return new ResultSuccessApi<SliderViewModel>(data);
        }
    }
}