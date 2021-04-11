using BL;
using DAL.Dto;
using MVVM_Core;
using System;
using System.Windows;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class OrderDataViewModel : BasePageViewModel
    {
        private readonly OrderService orderService;
        private readonly UserService userService;
        private readonly RegisterService registerService;
        private readonly EventBus eventBus;

        public bool IsAutorized { get; set; }

        public OrderDto OrderDto { get; set; }

        public ClientDto ClientDto { get; set; } = new ClientDto();

        public int DaysCount { get; set; }

        public double FullCost { get; set; }

        public int Old { get; set; }
        public int Child { get; set; }

        double cost;
        double costChild;

        public OrderDataViewModel(PageService pageservice, 
            OrderService orderService, UserService userService, 
            RegisterService registerService, EventBus eventBus) : base(pageservice)
        {
            this.orderService = orderService;
            this.userService = userService;
            this.registerService = registerService;
            this.eventBus = eventBus;
            Init();
        }

        void Init()
        {
            cost = orderService.GetTour().Cost;
            costChild = orderService.GetTour().ChildCost;
            OrderDto = orderService.GetOrder();
            StartDate = OrderDto.StartDate.DateTime;
            IsAutorized = userService.IsAutorized;
        }

        double GetCost(double cost, double costChild, int old, int child, int daysCount)
        {
            return (cost * old + costChild * child) * daysCount;
        }

        public DateTime StartDate { get; set; }

        bool Check()
        {
            //if (!IsAutorized)
            //{
            //    if (string.IsNullOrEmpty(ClientDto.Name))
            //    {
            //        MessageBox.Show("Поле 'Имя' должно быть заполнено", "", MessageBoxButton.OK, MessageBoxImage.Error);
            //        return false;
            //    }

            //    if (string.IsNullOrEmpty(ClientDto.Phone))
            //    {
            //        MessageBox.Show("Поле 'Номер телефона' должно быть заполнено", "", MessageBoxButton.OK, MessageBoxImage.Error);
            //        return false;
            //    }
            //}


            if (StartDate < DateTime.Now)
            {
                MessageBox.Show("Занчение поля 'Дата' не должно быть раньше, чем сегодня", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public ICommand GetCostCommand => new Command(x =>
        {

            FullCost = GetCost(cost, costChild, Old, Child, DaysCount);
        });

        protected override void Next()
        {
            if (!Check())
            {
                return;
            }


            OrderDto.FullCost = GetCost(cost, costChild, Old, Child, DaysCount);
            OrderDto.PeopleCount = Child + Old;
            OrderDto.ChildCount = Child;
            OrderDto.StartDate = new DateTimeOffset(StartDate);
            OrderDto.EndDate = new DateTimeOffset(StartDate.AddDays(DaysCount));
            orderService.SetupFilledOrder(OrderDto);
            
            pageservice.ChangePage<Pages.ClientRegisterPage>(DisappearAnimation.Default);
            
        }





        public override int PoolIndex => Rules.Pages.MainPool;
    }
}