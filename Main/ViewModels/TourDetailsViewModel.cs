using BL;
using DAL.Dto;
using DAL.Models;
using MVVM_Core;
using System;

namespace Main.ViewModels
{
    public class TourDetailsViewModel : BasePageViewModel
    {
        private readonly OrderService orderService;

        public override int PoolIndex => Rules.Pages.MainPool;

        public TourDto Tour { get; private set; }

        public TourDetailsViewModel(PageService pageservice, OrderService orderService) : base(pageservice)
        {
            this.orderService = orderService;
            Init();
        }

        protected override void Next()
        {
            
            pageservice.ChangePage<Pages.OrderDataPage>(PoolIndex, DisappearAnimation.Default);
        }

        private void Init()
        {
            Tour = orderService.GetTour();
        }
    }
}