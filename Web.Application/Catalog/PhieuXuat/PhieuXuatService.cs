using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using Web.Data.Enums;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.LoaiPhieus;
using Web.ViewModels.Catalog.PhieuNhaps;
using Web.ViewModels.Catalog.PhieuXuats;

namespace Web.Application.Catalog.PhieuXuat
{
    public class PhieuXuatService : IPhieuXuatService
    {
        private readonly AppDbContext _context;

        public PhieuXuatService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateCTPhieXuat(CTPhieuXuatCreate request)
        {
            var pnCT = await _context.PhieuNXchitiets.FirstOrDefaultAsync(x => x.ProductId == request.ProductId
            && x.SizeId == request.SizeId && x.PhieuNXId == request.PhieuNXId);

            if (pnCT != null)
            {
                return false;
            }

            var ctpx = new PhieuNhap_Xuatchitiet()
            {
                Giaban = request.Giaban,
                Dongia = request.Dongia,
                PhieuNXId = request.PhieuNXId,
                ProductId = request.ProductId,
                SizeId = request.SizeId,
                Soluong = request.Soluong
            };
            await _context.PhieuNXchitiets.AddAsync(ctpx);

            // cập nhật PCS
            var pcs = await _context.PCSs.FirstOrDefaultAsync(x => x.ProductId == request.ProductId
           && x.SizeId == request.SizeId);

            pcs.Stock -= request.Soluong;

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<ResultApi<string>> CreatePhieuXuat(PhieuXuatCreate request)
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
                if (String.Compare(item.Name, "Xuất") == 0)
                {
                    loaiphieu = item;
                    break;
                }
            }
            var px = new PhieuNhap_Xuat()
            {
                CongTyId = request.CongTyId,
                LoaiPhieuId = loaiphieu.Id,
                NgayNhap = DateTime.Now,
                Status = Status.Active
            };

            await _context.PhieuNXs.AddAsync(px);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<string>("Thêm Phiếu xuất thành công");
            }
            return new ResultErrorApi<string>("Thêm Phiếu xuất thất bại");
        }

        public async Task<List<PhieuXuatViewModel>> GetAll()
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
                if (String.Compare(item.Name, "Xuất") == 0)
                {
                    loaiphieu = item;
                    break;
                }
            }

            var query = from p in _context.PhieuNXs
                        where p.Status == Status.Active && p.LoaiPhieuId == loaiphieu.Id
                        select new { p };
            var data = await query.Select(x => new PhieuXuatViewModel()
            {
                Id = x.p.Id,
                TenCongTy = x.p.CongTy.Name,
                TenLoaiphieu = x.p.LoaiPhieu.Name,
                NgayNhap = x.p.NgayNhap
            }).ToListAsync();
            return data;
        }

        public async Task<List<PhieuNXchitietViewModel>> GetDetailPXById(int id, string languageId)
        {
            var result = from ct in _context.PhieuNXchitiets
                         where ct.PhieuNXId == id
                         select new { ct };

            var listCTPX = await result.Select(x => new PhieuNXchitietViewModel()
            {
                Id = x.ct.Id,
                Dongia = x.ct.Dongia,
                Giaban = x.ct.Giaban,
                Soluong = x.ct.Soluong,
                ProductId = x.ct.ProductId,
                SizeId = x.ct.SizeId,
                PhieuNXId = x.ct.Id
            }).ToListAsync();

            foreach (var ctpn in listCTPX)
            {
                var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == ctpn.ProductId
                && x.LanguageId == languageId);
                var size = await _context.Sizes.FindAsync(ctpn.SizeId);
                ctpn.TenSize = size.Name;
                ctpn.TenSP = productTranslation.Name;
            }
            return listCTPX;
        }
    }
}