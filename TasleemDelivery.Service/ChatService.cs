using AutoMapper;
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
    public class ChatService
    {

        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public ChatService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public ChatMessageClientDTO AddClientMsg(ChatMessageClientDTO chatDTO)
        {

            ClientChat chat = _mapper.Map<ClientChat>(chatDTO);

            _unitOfWork.ClientChatRepository.Add(chat);
            _unitOfWork.SaveChanges();

            return chatDTO;
        }

		public List<DeliveryChat> GetAllDeliveriesChatWithSpecificClient(string ClientId)
		{
			List<DeliveryChat> deliveryChats = _unitOfWork.DeliveryChatRepository
			  .GetByExpression(del => del.ClientId == ClientId)
			  .Include(d => d.Delivery)
			  .GroupBy(del => del.DeliveryId)
			  .Select(group => group.OrderByDescending(delMsgTime => delMsgTime.DeliveryMsgTime).FirstOrDefault())
			  .ToList();

			foreach (var item in deliveryChats)
			{
				ClientChat clientChat = _unitOfWork.ClientChatRepository
				.GetByExpression(del => del.DeliveryId == item.DeliveryId && del.ClientId == ClientId)
				.Include(d => d.Delivery)
				.OrderByDescending(cliMsgTime => cliMsgTime.ClientMsgTime)
				.FirstOrDefault();


				if (clientChat.ClientMsgTime > item.DeliveryMsgTime)
				{
					item.DeliveryMsg = clientChat.ClientMsg;
					item.DeliveryMsgTime = clientChat.ClientMsgTime;
				}
			}

			return deliveryChats;
		}

		public List<ClientChat> GetSpecificClientChatWithSpecificDelivery(string ClientId, string DeliveryId)
		{
			List<ClientChat> SpecificCliChatWithSpecificDeli = _unitOfWork.ClientChatRepository.GetByExpression(cli => cli.ClientId == ClientId && cli.DeliveryId == DeliveryId).ToList();

			return SpecificCliChatWithSpecificDeli;
		}
		public List<DeliveryChat> GetSpecificDeliverieChatWithSpecificClient(string ClientId, string DeliveryId)
		{
			List<DeliveryChat> SpecificDeliChatWithSpecificCli = _unitOfWork.DeliveryChatRepository.GetByExpression(del => del.ClientId == ClientId && del.DeliveryId == DeliveryId).ToList();

			return SpecificDeliChatWithSpecificCli;
		}
	}
}
