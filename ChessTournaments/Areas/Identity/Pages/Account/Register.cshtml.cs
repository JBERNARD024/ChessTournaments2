// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using ChessTournaments.Data;
using ChessTournaments.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace ChessTournaments.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        IFormFile imagemPessoa;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public SelectList ListaEquipas { get; set; }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "A {0} deve ter entre  {2} e {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "A password e a sua confirmação não correspondem.")]
            public string ConfirmPassword { get; set; }

            public Pessoa Pessoa { get; set; }
        }


        /// <summary>
        /// Este método para reage aos pedidos feitos em GET
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ListaEquipas = new SelectList(_context.Equipa,"Id","Nome");
        }

        /// <summary>
        /// Este método é acionado quando se enviam os dados em modo POST.
        /// E que adiciona os dados à Base de Dados.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            //variáveis auxiliares
            string nomeFoto = "";
            bool existeFoto = false;

            if (imagemPessoa == null)
            {
                // O Utilizador não fez upload de uma imagem
                // É adicionada uma imagem pré-definida à pessoa
                Input.Pessoa.ListaFotos
                        .Add(new Fotografia
                        {
                            Data = DateTime.Now,
                            Local = "SemImagem",
                            PessoaFK = 0,
                            NomeFicheiro = "ImagemDefaultPessoa.jpeg"
                        });
            }
            else
            {
                if (imagemPessoa.ContentType != "image/jpg" &&
                    imagemPessoa.ContentType != "image/png" && imagemPessoa.ContentType != "image/jpeg")
                {
                    Input.Pessoa.ListaFotos
                        .Add(new Fotografia
                        {
                            Data = DateTime.Now,
                            Local = "SemImagem",
                            PessoaFK = 0,
                            NomeFicheiro = "ImagemDefaultPessoa.jpeg"
                        });
                }
                else
                {
                    Guid g = Guid.NewGuid();
                    nomeFoto = g.ToString();
                    string extensaoNomeFoto = Path.GetExtension(imagemPessoa.FileName).ToLower();
                    nomeFoto += extensaoNomeFoto;

                    Input.Pessoa.ListaFotos
                            .Add(new Fotografia
                            {
                                Data = DateTime.Now,
                                Local = "",
                                NomeFicheiro = nomeFoto
                            });
                    existeFoto = true;
                }
            }

            //Caso os dados definidos no InputModel estejam corretos
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                //Cria o USER definido
                var result = await _userManager.CreateAsync(user, Input.Password);

                //Caso haja sucesso na sua criação
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //*********************************************************************
                    //Adicionar os Dados da Pessoa(User) à Base de Dados
                    //*********************************************************************

                    Input.Pessoa.Email = Input.Email;
                    Input.Pessoa.Username = user.Id;

                    //Adicionar os dados à Base de Dados

                    try
                    {
                        _context.Add(Input.Pessoa);
                        await _context.SaveChangesAsync();
                         if (existeFoto)
                    {
                        string nomeLocalizaoImagem = _webHostEnvironment.WebRootPath;
                        nomeLocalizaoImagem = Path.Combine(nomeLocalizaoImagem, "imagens");

                        if (!Directory.Exists(nomeLocalizaoImagem))
                        {
                            Directory.CreateDirectory(nomeLocalizaoImagem);
                        }

                        string nomeDoFicheiro = Path.Combine(nomeLocalizaoImagem, nomeFoto);
                        //guardar o ficheiro
                        using var stream = new FileStream(nomeDoFicheiro, FileMode.Create);
                        await imagemPessoa.CopyToAsync(stream);
                    }

                    return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}