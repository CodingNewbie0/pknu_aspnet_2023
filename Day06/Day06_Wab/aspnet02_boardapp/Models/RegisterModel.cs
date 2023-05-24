using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace aspnet02_boardapp.Models
{
    // 회원가입 할 때 데이터 받는부분
    public class RegisterModel
    {
        [Required(ErrorMessage = "아이디는 필수입니다.")]
        [EmailAddress]
        [DisplayName("이메일주소")]
        public string Email { get; set; }

        [DisplayName("핸드폰 번호")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "패스워드는 필수입니다.")]
        [DataType(DataType.Password)]
        [DisplayName("패스워드")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("패스워드 확인")]
        [Compare("Password", ErrorMessage = "패스워드가 일치하지 않습니다. 다시 입력해주세요")]
        public string ConfirmPassword { get; set; }
    }
}
