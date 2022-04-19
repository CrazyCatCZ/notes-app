using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using ArchivPoznamek.Data;
using ArchivPoznamek.Models;

namespace ArchivPoznamek.Controllers
{
    public class UzivatelController : Controller
    {
        private ArchivPoznamekData _databaze;

        public UzivatelController(ArchivPoznamekData databaze)
        {
            _databaze = databaze;
        }

        [HttpGet]
        public IActionResult Registrovat()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrovat(string jmeno, string email, string heslo, string hesloKontrola)
        {
            if (jmeno != null)
                jmeno = jmeno.Trim().ToLower();
            if (heslo != null)
                heslo = heslo.Trim();

            if (jmeno.Length == 0)
                return RedirectToAction("Registrovat");
            if (heslo.Length == 0)
                return RedirectToAction("Registrovat");
            if (heslo != hesloKontrola)
                return RedirectToAction("Registrovat");

            string zashashovaneHeslo = BCrypt.Net.BCrypt.HashPassword(heslo);
            Uzivatel? existujiciUzivatel = _databaze.Uzivatele
                .Where(u => u.Jmeno == jmeno)
                .FirstOrDefault();

            if (existujiciUzivatel != null)
                return RedirectToAction("Registrovat");

            Uzivatel novyUzivatel = new Uzivatel()
            {
                Jmeno = jmeno,
                Email = email,
                Heslo = zashashovaneHeslo,
            };

            _databaze.Add(novyUzivatel);
            _databaze.SaveChanges();

            return RedirectToAction("Prihlasit");
        }

        [HttpGet]
        public IActionResult Prihlasit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Prihlasit(string jmeno, string heslo)
        {
            if (jmeno == null || jmeno.Trim().Length == 0)
                return RedirectToAction("Prihlasit");
            if (heslo == null || heslo.Trim().Length == 0)
                return RedirectToAction("Prihlasit");

            Uzivatel? prihlasovanyUzivatel = _databaze.Uzivatele
                .Where(u => u.Jmeno == jmeno)
                .FirstOrDefault();

            if (prihlasovanyUzivatel == null)
                return RedirectToAction("Prihlasit");

            bool validPassword = BCrypt.Net.BCrypt.Verify(heslo, prihlasovanyUzivatel.Heslo);
            if (validPassword == false)
            {
                return RedirectToAction("Prihlasit");
            }

            /*
            if (prihlasovanyUzivatel.Heslo != heslo)
                return RedirectToAction("Prihlasit");
            */

            HttpContext.Session.SetString("Prihlaseny", prihlasovanyUzivatel.Jmeno);

            return RedirectToAction("", "");
        }

        [HttpGet]
        public IActionResult Odhlasit()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("", "");
        }
    }
}