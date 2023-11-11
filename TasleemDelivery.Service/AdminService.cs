using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasleemDelivery.Models;
using TasleemDelivery.Repository.Interfaces;
using TasleemDelivery.Repository.UnitOfWork;

namespace TasleemDelivery.Service
{
	public class AdminService
	{
		private readonly IGenericRepository<SubAdmin, string> _subAdminRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;

		public AdminService(IGenericRepository<SubAdmin, string> subAdminRepository,
			IUnitOfWork unitOfWork,
			UserManager<ApplicationUser> userManager)
		{
			_subAdminRepository=subAdminRepository;
			_unitOfWork = unitOfWork;
			_userManager=userManager;
		}

		public async Task<string> DeleteSubAdmin(string subAdminId)
		{
			

			ApplicationUser user=await _userManager.FindByIdAsync(subAdminId);

			if(user!=null)
			{
				_subAdminRepository.Delete(subAdminId);
				_unitOfWork.SaveChanges();


				// Remove user's roles first
				var userRoles = await _userManager.GetRolesAsync(user);
				foreach (var role in userRoles)
				{
					await _userManager.RemoveFromRoleAsync(user, role);
				}

				IdentityResult Result =await _userManager.DeleteAsync(user);

				if(Result.Succeeded) 
				{
					return ("Deleted");
				}
				else
				{
					return ("Not Deleted");
				}
			}
	     return ("Not Deleted");

		}
	}
}
