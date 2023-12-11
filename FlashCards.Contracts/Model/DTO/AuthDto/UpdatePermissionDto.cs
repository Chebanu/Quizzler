using System.ComponentModel.DataAnnotations;

namespace FlashCard.Model.DTO.AuthDto;

public class UpdatePermissionDto
{
	[Required(ErrorMessage = "UserName is required")]
	public string UserName { get; set; }
}
