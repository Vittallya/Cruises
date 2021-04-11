using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DAL;
using DAL.Dto;
using DAL.Models;
using AutoMapper;
using System.Data.Entity;

namespace BL
{
    public class OrderService
    {
        private readonly AllDbContext dbContext;
        private readonly MapperService mapper;

        private Order _currentOrder;
        private int _clientId;
        private int _tourId;
        private Tour _tour;
        private TourDto _tourDto;
        private double _fullCost;

        public bool IsEdit { get; private set; }
        public bool IsStarted { get; private set; }

        public void Clear()
        {
            IsEdit = false;
            IsStarted = false;
            _currentOrder = null;
        }

        public OrderService(AllDbContext dbContext, MapperService mapperService)
        {
            this.dbContext = dbContext;
            this.mapper = mapperService;
        }

        public void SetupClient(int clientId)
        {
            _clientId = clientId;
        }

        public async Task StartEditOrder(int orderId)
        {
            IsEdit = true;

            await dbContext.Orders.LoadAsync();
            _currentOrder = await dbContext.Orders.FindAsync(orderId);
        }

        public async Task SetupTour(TourDto tour)
        {

            _tourId = tour.Id;
            _tour = await dbContext.Tours.FindAsync(tour.Id);
            _tourDto = tour;
        }

        public OrderDto GetOrder()
        {
            return _currentOrder != null ? mapper.MapTo<Order, OrderDto>(_currentOrder) : 
                new OrderDto() { StartDate = DateTime.Now.AddDays(1), OrderStatus = OrderStatus.Active, EndDate = DateTime.Now.AddDays(5)};
        }

        public TourDto GetTour()
        {
            return _tourDto;
        }

        public void SetupFilledOrder(OrderDto order)
        {
            _currentOrder = mapper.MapTo<OrderDto, Order>(order);
        }


        public async Task<bool> ApplyOrder()
        {
            await dbContext.Tours.LoadAsync();
            _currentOrder.TourId = _tourId;
            
            if (!IsEdit)
            {
                _currentOrder.CreationDate = DateTimeOffset.Now;
                _currentOrder.OrderStatus = OrderStatus.Active;
                _currentOrder.ClientId = _clientId;
                dbContext.Orders.Add(_currentOrder);
            }
            else
            {
                dbContext.Entry(_currentOrder).State = EntityState.Modified;
            }

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }

            return true;
        }



        public string ErrorMessage { get; private set; }

        public async Task<IEnumerable<OrderDto>> GetAllOrders(int clientId)
        {
            await dbContext.Orders.Include(x => x.Tour).LoadAsync();

            var list = await dbContext.
                Orders.
                AsNoTracking().
                Where(x => x.ClientId == clientId && (x.OrderStatus == OrderStatus.Active || x.OrderStatus == OrderStatus.Completed)).
                OrderBy(x => x.CreationDate).
                ToListAsync();

            return list.Select(y => 
            { 
                var inst = mapper.MapTo<Order, OrderDto>(y);
                return inst;
            });
        }


        public async Task<OrderDto> CancelOrder(int orderId)
        {
            await dbContext.Orders.LoadAsync();
            var order = await dbContext.Orders.FindAsync(orderId);

            order.OrderStatus = OrderStatus.Canceled;
            dbContext.Entry(order).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return mapper.MapTo<Order, OrderDto>(order);
        }
    }
}
