using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MuseuAMSG3.InfraStructure.Data.Context;
using MuseuAMSG3.Models;
using SQLitePCL;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace MuseuAMSG3.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDataContext _context;
        private readonly PasswordHasher<Login> _passwordHasher;

        public LoginController(ApplicationDataContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Login>();
        }
        



        public async Task<IActionResult> Index()
        {
            return View(await _context.Login.ToListAsync());
        }

        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var login = await _context.Login
                .FirstOrDefaultAsync(m => m.Id == id);
            if (login == null)
            {
                return NotFound();
            }

            return View(login);
        }
    
        [HttpPost]
        public IActionResult Verificar(Login login)
        {
            // Validar se os campos estão preenchidos
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.PassWord))
            {
                ViewBag.Message = "Por favor, preencha todos os campos!";
                return View();
            }

            // Verificar se o email existe no banco
         
           

            var senha = login.PassWord;

            bool minLength = senha.Length >= 8;
            bool hasUpperCase = senha.Any(char.IsUpper);
            bool hasLowerCase = senha.Any(char.IsLower);
            bool hasNumber = senha.Any(char.IsDigit);
            bool hasSpecialChar = senha.Any(ch => "!@#$%^&*(),.?\":{}|<>".Contains(ch));

            // Verificar todos os critérios de senha
            if (!minLength || !hasUpperCase || !hasLowerCase || !hasNumber || !hasSpecialChar)
            {
                ViewBag.Message = "<ul>" +
                                  "<li>Pelo menos 8 caracteres</li>" +
                                  "<li>Pelo menos uma letra maiúscula</li>" +
                                  "<li>Pelo menos uma letra minúscula</li>" +
                                  "<li>Pelo menos um número</li>" +
                                  "<li>Pelo menos um caractere especial</li>" +
                                  "</ul>";

            }
            var cadastro = _context.Cadastro.AsQueryable();
            // Verificar se o email existe no banco

            if (login == null || !cadastro.Any(o => o.Senha == login.PassWord))
            {
                ViewBag.Message = "Email ou senha inválidos.";
                return View();
            }

            ViewBag.Message = "Login realizado com sucesso!";
            return RedirectToAction("Index", "Login"); 
        }


    


    public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(string Email, string PassWord)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(PassWord))
            {
                return Json(new { success = false, message = "Email e senha são obrigatórios." });
            }

            using (var db = new ApplicationDataContext())
            {
                var login = db.Login.FirstOrDefault(u => u.Email == Email);

                if (login != null)
                {
                    // Se as senhas forem armazenadas como texto simples
                    if (login.PassWord == PassWord)
                    {
                        return Json(new { success = true, message = "Login realizado com sucesso!" });
                    }

                    // Mensagem de erro para senha incorreta
                    return Json(new { success = false, message = "Senha incorreta." });
                }
            }

            return Json(new { success = false, message = "Email não encontrado." });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Password,Id")] Login login)
        {
            if (ModelState.IsValid)
            {
                login.Id = Guid.NewGuid();
                _context.Add(login);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(login);
        }

        // GET: Login/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var login = await _context.Login.FindAsync(id);
            if (login == null)
            {
                return NotFound();
            }
            return View(login);
        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Email,Password,Id")] Login login)
        {
            if (id != login.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(login);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoginExists(login.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(login);
        }

        // GET: Login/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var login = await _context.Login
                .FirstOrDefaultAsync(m => m.Id == id);
            if (login == null)
            {
                return NotFound();
            }

            return View(login);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var login = await _context.Login.FindAsync(id);
            if (login != null)
            {
                _context.Login.Remove(login);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoginExists(Guid id)
        {
            return _context.Login.Any(e => e.Id == id);
        }
    }
}
