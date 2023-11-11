using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TasleemDelivery.DTO;
using TasleemDelivery.Models;
using TasleemDelivery.Repository.UnitOfWork;
using TasleemDelivery.Service;

namespace TasleemDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        AdminService _adminService;
        public AdminController(

            IMapper mapper,
            IUnitOfWork unitOfWork,
			AdminService adminService
			)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _adminService = adminService;
        }

        [HttpDelete("DeleteSubAdmin")]
        public async Task<IActionResult> DeleteSubAdmin(string subAdminId)
        {
            string Result = await _adminService.DeleteSubAdmin(subAdminId);
            _unitOfWork.CommitChanges();

            if (Result == "Deleted")
            {
				return Ok(new { Message = "Success" });
			}
			else
				return BadRequest(new { Message = "Not Success" });

		}


	}
}