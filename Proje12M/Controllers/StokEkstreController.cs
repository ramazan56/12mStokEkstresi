using Microsoft.AspNetCore.Mvc;
using Proje12M.Models;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Proje12M.Controllers
{
    public class StokEkstreController : Controller
    {
        private readonly ILogger<StokEkstreController> _logger;
        private readonly ApplicationDbContext _context;
        STI tI = new STI();
        public StokEkstreController(ILogger<StokEkstreController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

        }
        public IActionResult Index()
        {
            //mal adı seçmek için db den stk tablosunu listeliyoruz
            ViewBag.STK = _context.Set<STK>().FromSqlRaw("select * from STK");

            return View();
        }
        public JsonResult StokEktresi(DateTime basTarih, DateTime bitTarih, string malAdi)
        {
            //baslangıç tarihini db ye int olarak göndermek için convert işlemi yapılıyor
            int baslangicTarih = Convert.ToInt32((basTarih).ToOADate());
            //bitiş tarihini db ye int olarak göndermek için convert işlemi yapılıyor
            int bitisTarih = Convert.ToInt32((bitTarih).ToOADate());
            //seçilen parametrelere göre stored procedure parametre yollayıp donen listeyi alıyoruz
            var stokEkteresi = _context.Set<STI>().FromSqlRaw("execute sp_stokEkstresi @BaslagicTarih=" + baslangicTarih + ",@BitisTarih=" + bitisTarih + ",@MalAdi='" + malAdi + "'").ToList();
            var stok = 0;
            int sira = 1;
            //işlem türüne göre giriş çıkış ve stok miktarlarını belirliyoruz.
            foreach (var item in stokEkteresi)
            {
                if (item.IslemTur == "0")
                {
                    stok += item.Miktar;
                    item.IslemTur = "Giriş";
                    item.GirisMiktar = item.Miktar;
                }
                else
                {
                    item.IslemTur = "Çıkış";
                    item.CikisMiktar = item.Miktar;
                    stok -= item.Miktar;
                }
                item.StokMiktar = stok;
                item.SiraNo = sira;
                sira++;
            }
            return Json(stokEkteresi);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}