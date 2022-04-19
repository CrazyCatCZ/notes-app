using ArchivPoznamek.Data;
using ArchivPoznamek.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ArchivPoznamek.Controllers
{
    public class PoznamkyController : Controller
    {
        private ArchivPoznamekData _databaze;

        public PoznamkyController(ArchivPoznamekData databaze)
        {
            _databaze = databaze;
        }

        [HttpGet]
        public IActionResult Pridat()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Pridat(string nadpis, string obsah, string dulezity)
        {
            if (nadpis == null || nadpis.Trim().Length == 0)
                return RedirectToAction("Pridat");

            Uzivatel? prihlasenyUzivatel = KdoJePrihlasen();

            Poznamka poznamka = new Poznamka()
            {
                Dulezitost = (dulezity == "ano"),
                DatumVlozeni = DateTime.Now,
                Nadpis = nadpis,
                Obsah = obsah,
                Autor = prihlasenyUzivatel,
            };

            _databaze.Add(poznamka);
            _databaze.SaveChanges();

            return RedirectToAction("Vypsat", "Poznamky");
        }


        [HttpGet]
        public IActionResult Vypsat()
        {
            Uzivatel? prihlasenyUzivatel = KdoJePrihlasen();

            List<Poznamka> vsechnyPoznamky = _databaze.Poznamky
                .Where(u => u.Autor == prihlasenyUzivatel)
                .ToList();

            return View(vsechnyPoznamky);
        }

        private  Uzivatel? KdoJePrihlasen()
        {
            string? jmenoPrihlasenehoUzivatele = HttpContext.Session.GetString("Prihlaseny");

            if (jmenoPrihlasenehoUzivatele == null)
                return null;

            Uzivatel? prihlasenyUzivatel
                = _databaze.Uzivatele
                .Where(u => u.Jmeno == jmenoPrihlasenehoUzivatele)
                .FirstOrDefault();

            return prihlasenyUzivatel;
        }
    }
}
