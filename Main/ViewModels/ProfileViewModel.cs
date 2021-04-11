using BL;
using DAL.Dto;
using MVVM_Core;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class ProfileViewModel : BasePageViewModel
    {
        private readonly UserService userService;
        private readonly OrderService orderService;
        private OrderDto selectedOrder;

        public bool DetailsVis { get; set; }

        public OrderDto SelectedOrder 
        { 
            get => selectedOrder; 
            set 
            {
                DetailsVis = value != null;

                if (DetailsVis)
                {
                    selectedOrder = value;                  
                    OnPropertyChanged();
                    SetupService(value.TourId);
                }
            }
        }

        async void SetupService(int serviceId)
        {
        }

        public TourDto Tour { get; set; }



        public ProfileViewModel(PageService pageservice, UserService userService, OrderService orderService) : base(pageservice)
        {
            this.userService = userService;
            this.orderService = orderService;
            Init();
        }

        async void Init()
        {
            Orders = new ObservableCollection<OrderDto>(await orderService.GetAllOrders(userService.CurrentUser.Id));
        }

        public ICommand CancelOrder => new CommandAsync(async x =>
        {
            if (MessageBox.Show("Удалить заказ?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await orderService.CancelOrder(SelectedOrder.Id);
                Orders.Remove(SelectedOrder);
            }
        });

        public bool IsLoadingVisible { get; set; }


        protected override void Back()
        {
            pageservice.ChangePage<Pages.HomePage>(DisappearAnimation.Default);
        }

        public ObservableCollection<OrderDto> Orders { get; set; }

        public override int PoolIndex => Rules.Pages.MainPool;
    }
}