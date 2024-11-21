using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MuseuAMSG3.InfraStructure.Data.Context;
using MuseuAMSG3.Models;

namespace MuseuAMSG3.Controllers
{
    public class BilheteriaController : Controller
    {
        private readonly ApplicationDataContext _context;

        public BilheteriaController(ApplicationDataContext context)
        {
            _context = context;
        }

        public IActionResult BilheteriaList()
        {
            var bilheteria = _context.Bilheteria.ToList();
            return View(bilheteria);
        }
        public IActionResult BilheteriaDetails(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bilheteria = _context.Bilheteria
                .FirstOrDefault(m => m.Id == id);
            if (bilheteria == null)
            {
                return NotFound();
            }

            return View(bilheteria);
        }


        public IActionResult BilheteriaConcluido()
        {
            return View();
        }
        public IActionResult AddBilheteria()
        {
            return View();

        }

        [HttpPost]

        public IActionResult AddBilheteria(Bilheteria bilheteria)
        {

                bilheteria.Id = Guid.NewGuid();
                _context.Add(bilheteria);
                _context.SaveChanges();
                return RedirectToAction("BilheteriaConcluido");

        }


        public IActionResult BilheteriaEdit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bilheteria = _context.Bilheteria.Find(id);
            if (bilheteria == null)
            {
                return NotFound();
            }
            return View(bilheteria);
        }


        [HttpPost]
        public IActionResult BilheteriaEdit(Guid id, Bilheteria bilheteria)
        {
            if (id != bilheteria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bilheteria);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BilheteriaExists(bilheteria.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("BilheteriaList");
            }
            return View(bilheteria);
        }


        public IActionResult BilheteriaDelete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bilheteria = _context.Bilheteria
                .FirstOrDefault(m => m.Id == id);
            if (bilheteria == null)
            {
                return NotFound();
            }

            return View(bilheteria);
        }

        [HttpPost, ActionName("BilheteriaDelete")]

        public IActionResult DeleteConfirmed(Guid id)
        {
            var bilheteria = _context.Bilheteria.Find(id);
            if (bilheteria != null)
            {
                _context.Bilheteria.Remove(bilheteria);
            }

           _context.SaveChanges();
            return RedirectToAction("BilheteriaList");
        }

        private bool BilheteriaExists(Guid id)
        {
            return _context.Bilheteria.Any(e => e.Id == id);
        }

    }
}
