using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using Web.Data.Enums;
using Web.Utilities.Exceptions;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.LoaiPhieus;

namespace Web.Application.Catalog.LoaiPhieus
{
    public class LoaiPhieuService : ILoaiPhieuService
    {
        private readonly AppDbContext _context;

        public LoaiPhieuService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultApi<string>> CreateLoaiPhieu(LoaiPhieuCreate request)
        {
            var lp = new LoaiPhieu()
            {
                Name = request.Name,
                NgayNhap = DateTime.Now,
                Status = Status.Active
            };

            await _context.LoaiPhieus.AddAsync(lp);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<string>("Thêm loại phiếu thành công");
            }
            return new ResultErrorApi<string>("Thêm loại phiếu thất bại");
        }

        public async Task<int> Delete(int id)
        {
            var lp = await _context.LoaiPhieus.FindAsync(id);
            if (lp == null) throw new WebException($"Cannot find a loại phiếu with id: {id}");

            lp.Status = Status.InActive;
            return await _context.SaveChangesAsync();
        }

        public async Task<List<LoaiPhieuViewModel>> GetAll()
        {
            var list = await _context.LoaiPhieus.Where(x => x.Status == Status.Active).Select(x => new LoaiPhieuViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                NgayNhap = x.NgayNhap
            }).ToListAsync();
            return (list);
        }
    }
}