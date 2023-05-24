using aspnet02_boardapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace aspnet02_boardapp.Controllers
{
    // 사용자 회원가입, 로그인, 로그아웃
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }



        // https://localhost:7183/Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        // public IActionResult Register(RegisterModel model)
        // 비동기가 아니면 return값은 IActionResult
        // 비동기가 되면 Task<IActionResult>
        public async Task<IActionResult> Register(RegisterModel model)
        {
            ModelState.Remove("PhoneNumber"); // PhoneNumber는 입력값 검증에서 제거
            if (ModelState.IsValid) // 데이터를 제대로 입력해서 검증 성공하면
            {
                // ASP.NET user - aspnetusers 테이블에 데이터넣기 위해 매핑되는 인스턴스를 생성
                var user = new IdentityUser(){
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                // aspnetusers 테이블에 사용자 데이터를 대입
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // 회원가입에 성공했으면 로그인한뒤 localhost:7183/Home/Index로 감
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home"); 
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model); // 회원가입을 실패하면 화면 그대로 유지
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}
