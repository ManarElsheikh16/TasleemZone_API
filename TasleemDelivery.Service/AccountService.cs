﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasleemDelivery.DTO;
using TasleemDelivery.Models;
using TasleemDelivery.Repository.UnitOfWork;

namespace TasleemDelivery.Service
{
    public class AccountService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public AccountService(IMapper mapper,IUnitOfWork unitOfWork) 
        {
            _unitOfWork= unitOfWork;
            _mapper= mapper;
        }

        public RegisterDTO AddDelivery(RegisterDTO registerDTO)
        {
            //By using Mapper he does it mapping auto because the same name and dataType
            // delivery.Id = registerDTO.Id;
            Delivery delivery = _mapper.Map<Delivery>(registerDTO);
            delivery.Points = 100;

            _unitOfWork.DeliveryRepository.Add(delivery);
            _unitOfWork.SaveChanges();

            return registerDTO;
        }
        public RegisterDTO AddClient(RegisterDTO registerDTO) 
        {
            Client client=_mapper.Map<Client>(registerDTO);

            _unitOfWork.ClientRepository.Add(client);
            _unitOfWork.SaveChanges();

            return registerDTO;
        }

        public RegisterDTO AddAdmin(RegisterDTO registerDTO)
        {
           Admin admin =_mapper.Map<Admin>(registerDTO);

            _unitOfWork.AdminRepository.Add(admin);
            _unitOfWork.SaveChanges();

            return registerDTO;
        }

        public RegisterDTO AddSubAdmin(RegisterDTO registerDTO)
        {
            SubAdmin subAdmin = _mapper.Map<SubAdmin>(registerDTO);

            _unitOfWork.SubAdminRepository.Add(subAdmin);
            _unitOfWork.SaveChanges();

            return registerDTO;
        }


        public List<RegisterDTO> AllSubAdmin()
        {
            IQueryable<SubAdmin> subAdmins = _unitOfWork.SubAdminRepository.GetByExpression(s => !s.IsDeleted).Include(e => e.ApplicationUser);

             List<RegisterDTO> dTOs = _mapper.ProjectTo<RegisterDTO>((IQueryable)subAdmins).ToList();

            return dTOs;
        }

      /*  public EditSubAdminDataDTO EditSubAdminById(EditSubAdminDataDTO dto, params string[] properties)
        {

        }*/
        public RegisterDTO GetsubAdminByID(string id)
        {
            SubAdmin subAdmin = _unitOfWork.SubAdminRepository.GetByExpression(e=>e.Id==id).Include(e=>e.ApplicationUser).FirstOrDefault();

            RegisterDTO registerDTO = _mapper.Map<RegisterDTO>(subAdmin);
            return registerDTO;


        }
        public string GetQuestionByUserName(string userName)

        {
            string Question=_unitOfWork.AccountRepository.GetQuestionByUserName(userName);

            return Question;
        }
        public async Task<string> ForgetPassword(ForgetPasswordDTO forgetPasswordDTO)
        {

            string Message = await _unitOfWork.AccountRepository.ForgetPassAsync(forgetPasswordDTO);

            _unitOfWork.SaveChanges();

            return Message;
        }

    }
}
