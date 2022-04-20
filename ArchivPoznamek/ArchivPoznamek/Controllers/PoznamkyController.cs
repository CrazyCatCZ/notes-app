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
                .OrderByDescending(u => u.DatumVlozeni)
                .ToList();

            return View(vsechnyPoznamky);
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            Uzivatel? prihlasenyUzivatel = KdoJePrihlasen();

            Poznamka? poznamka = _databaze.Poznamky
                .Where(u => u.Id == id)
                .FirstOrDefault();

            if (prihlasenyUzivatel == null)
                return RedirectToAction("Prihlasit", "Uzivatel");

            if (prihlasenyUzivatel != poznamka.Autor)
            {
                return RedirectToAction("Vypsat");
            }

            return View(poznamka);
        }

        [HttpGet]
        public IActionResult Smazat(int id)
        {
            Uzivatel? prihlasenyUzivatel = KdoJePrihlasen();

            if (prihlasenyUzivatel == null)
                return RedirectToAction("Prihlasit", "Uzivatel");

            Poznamka? poznamka = _databaze.Poznamky
                .Where(u => u.Id == id)
                .FirstOrDefault();

            if (poznamka != null && poznamka.Autor == prihlasenyUzivatel)
            {
                _databaze.Poznamky.Remove(poznamka);
                _databaze.SaveChanges();
            }

            return RedirectToAction("Vypsat");
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
