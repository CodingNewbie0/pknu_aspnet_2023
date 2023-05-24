using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace aspnet02_boardapp.Models
{
    public class Board
    {
        // 게시판을 위한 보드 매핑 정보
        [Key] // PK
        public int Id { get; set; }
        [Required(ErrorMessage = "아이디를 입력하세요")] // Not Null
        [DisplayName("아이디")]
        public string UserId { get; set; }
        [DisplayName("이름")]
        public string UserName { get; set; } // Null 허용
        [Required] // Not Null
        [MaxLength(200)] // == Varchar(200)
        [DisplayName("제목")]
        public string Title { get; set; }
        [DisplayName("조회")]
        public int ReadCount { get; set; }
        [DisplayName("날짜")]
        public DateTime PostDate { get; set; }
        [DisplayName("게시글")]
        public string Contents { get; set; }
    }
}
