using BL;
using DAL.Dto;
using MVVM_Core;

namespace Main.ViewModels
{
    public class OrderResultViewModel : BasePageViewModel
    {
        private readonly RegisterService registerService;
        private readonly OrderService orderService;
        private readonly UserService userService;

        public OrderResultViewModel(PageService pageservice, RegisterService registerService, 
            OrderService orderService, UserService userService) : base(pageservice)
        {
            this.registerService = registerService;
            this.orderService = orderService;
            this.userService = userService;
            Init();
        }

        public string Message { get; set; }

        public bool IsAnimVisible { get; set; }

        public OrderDto OrderDto { get; set; }

        public TourDto TourDto { get; set; }

        void Init()
        {
            OrderDto = orderService.GetOrder();
            TourDto = orderService.GetTour();
        }


        protected async override void Next()
        {
            IsAnimVisible = true;

            bool res = await orderService.ApplyOrder();
            Message = res ? "Оформлено!" : orderService.ErrorMessage;
            orderService.Clear();
            pageservice.ClearHistoryByPool(PoolIndex);
            IsAnimVisible = false;

            pageservice.ChangePage<Pages.HomePage>(PoolIndex, DisappearAnimation.Default);
        }

        public override int PoolIndex => Rules.Pages.MainPool;
    }
}