using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using System.Linq;
using Web.Data.Enums;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Congtys;
using Microsoft.EntityFrameworkCore;
using Web.Utilities.Exceptions;

namespace Web.Application.Catalog.Congtys
{
    public class CongtyService : ICongtyService
    {
        private readonly AppDbContext _context;

        public CongtyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultApi<string>> CreateCongty(CongtyCreate request)
        {
            var congty = new CongTy()
            {
                Name = request.Name,
                Diachi = request.Diachi,
                Masothue = request.Masothue,
                Sdt = request.Sdt,
                Status = Status.Active
            };

            await _context.CongTys.AddAsync(congty);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<string>("Thêm công ty thành công");
            }
            return new ResultErrorApi<string>("Thêm công ty thất bại");
        }

        public async Task<int> Delete(int id)
        {
            var congty = await _context.CongTys.FindAsync(id);
            if (congty == null) throw new WebException($"Cannot find a công ty with id: {id}");

            congty.Status = Status.InActive;
            return await _context.SaveChangesAsync();
        }

        public async Task<List<CongtyViewModel>> GetAll()
        {
            var query = from c in _context.CongTys
                        where c.Status == Status.Active
                        select new { c };
            return await query.Select(x => new CongtyViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                Diachi = x.c.Diachi,
                Masothue = x.c.Masothue,
                Status = x.c.Status,
                Sdt = x.c.Sdt
            }).ToListAsync();
        }

        public async Task<CongtyViewModel> GetCongtytById(int id)
        {
            var congty = await _context.CongTys.FindAsync(id);

            var data = new CongtyViewModel()
            {
                Id = congty.Id,
                Name = congty.Name,
                Diachi = congty.Diachi,
                Masothue = congty.Masothue,
                Sdt = congty.Sdt
            };
            return data;
        }

        public async Task<int> UpdateCongty(CongtyViewModel request)
        {
            var congty = await _context.CongTys.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (congty == null) throw new WebException($"Cannot find a công ty with id: {request.Id}");

            congty.Name = request.Name;
            congty.Masothue = request.Masothue;
            congty.Sdt = request.Sdt;
            congty.Diachi = request.Diachi;
            return await _context.SaveChangesAsync();
        }
    }
}