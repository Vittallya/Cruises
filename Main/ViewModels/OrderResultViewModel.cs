using BL;
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

        async void Init()
        {
            IsAnimVisible = true;

            bool res = await orderService.ApplyOrder();
            Message = res ? "Оформлено!" : orderService.ErrorMessage;
            orderService.Clear();
            pageservice.ClearHistoryByPool(PoolIndex);
            IsAnimVisible = false;
        }


        protected override void Next()
        {
            pageservice.ChangePage<Pages.HomePage>(PoolIndex, DisappearAnimation.Default);
        }

        public override int PoolIndex => Rules.Pages.MainPool;
    }
}