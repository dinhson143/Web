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
using Web.ViewModels.Catalog.PhieuNhaps;

namespace Web.Application.Catalog.PhieuNhap
{
    public class PhieuNhapService : IPhieuNhapService
    {
        private readonly AppDbContext _context;

        public PhieuNhapService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateCTPhieuNhap(CTPhieuNhapCreate request)
        {
            var pnCT = await _context.PhieuNXchitiets.FirstOrDefaultAsync(x => x.ProductId == request.ProductId
           && x.SizeId == request.SizeId && x.PhieuNXId == request.PhieuNXId);

            if (pnCT != null)
            {
                return false;
            }

            var ctpn = new PhieuNhap_Xuatchitiet()
            {
                Giaban = request.Giaban,
                Dongia = request.Dongia,
                PhieuNXId = request.PhieuNXId,
                ProductId = request.ProductId,
                SizeId = request.SizeId,
                Soluong = request.Soluong
            };
            await _context.PhieuNXchitiets.AddAsync(ctpn);

            // cập nhật PCS
            var pcs = await _context.PCSs.FirstOrDefaultAsync(x => x.ProductId == request.ProductId
           && x.SizeId == request.SizeId);

            pcs.Stock += request.Soluong;
            pcs.OriginalPrice = request.Giaban;

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<string> CreatePhieuNhap(PhieuNhapCreate request)
        {
            var list = await _context.LoaiPhieus.Where(x => x.Status == Status.Active).Select(x => new LoaiPhieuViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                NgayNhap = x.NgayNhap
            }).ToListAsync();

            var loaiphieu = new LoaiPhieuViewModel();
            foreach (var item in list)
            {
                if (String.Compare(item.Name, "Nhập") == 0)
                {
                    loaiphieu = item;
                    break;
                }
            }
            var pn = new PhieuNhap_Xuat()
            {
                CongTyId = request.CongTyId,
                LoaiPhieuId = loaiphieu.Id,
                NgayNhap = DateTime.Now,
                Status = Status.Active
            };

            await _context.PhieuNXs.AddAsync(pn);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return pn.Id.ToString();
            }
            return "Tạo mới phiếu nhập thất bại";
        }

        public async Task<List<PhieuNhapViewModel>> GetAll()
        {
            var list = await _context.LoaiPhieus.Where(x => x.Status == Status.Active).Select(x => new LoaiPhieuViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                NgayNhap = x.NgayNhap
            }).ToListAsync();

            var loaiphieu = new LoaiPhieuViewModel();
            foreach (var item in list)
            {
                if (String.Compare(item.Name, "Nhập") == 0)
                {
                    loaiphieu = item;
                    break;
                }
            }
            var query = from p in _context.PhieuNXs
                        where p.Status == Status.Active && p.LoaiPhieuId == loaiphieu.Id
                        select new { p };
            var data = await query.Select(x => new PhieuNhapViewModel()
            {
                Id = x.p.Id,
                TenCongTy = x.p.CongTy.Name,
                TenLoaiphieu = x.p.LoaiPhieu.Name,
                NgayNhap = x.p.NgayNhap
            }).ToListAsync();
            return data;
        }

        public async Task<List<PhieuNXchitietViewModel>> GetDetailPNById(int id, string languageId)
        {
            var result = from ct in _context.PhieuNXchitiets
                         where ct.PhieuNXId == id
                         select new { ct };

            var listCTPN = await result.Select(x => new PhieuNXchitietViewModel()
            {
                Id = x.ct.Id,
                Dongia = x.ct.Dongia,
                Giaban = x.ct.Giaban,
                Soluong = x.ct.Soluong,
                ProductId = x.ct.ProductId,
                SizeId = x.ct.SizeId,
                PhieuNXId = x.ct.Id
            }).ToListAsync();

            foreach (var ctpn in listCTPN)
            {
                var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == ctpn.ProductId
                && x.LanguageId == languageId);
                var size = await _context.Sizes.FindAsync(ctpn.SizeId);
                ctpn.TenSize = size.Name;
                ctpn.TenSP = productTranslation.Name;
            }
            return listCTPN;
        }
        public async Task<int> Delete(int id)
        {
            var pn = await _context.PhieuNXs.FindAsync(id);
            if (pn == null) throw new WebException($"Cannot find a bình luận with id: {id}");

            pn.Status = Status.InActive;
            return await _context.SaveChangesAsync();
        }
    }
}