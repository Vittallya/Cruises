using DAL.Models;
using MVVM_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BL;
using DAL.Dto;
using System.Windows.Input;

namespace Main.ViewModels
{
    public class ToursViewModel : BasePageViewModel
    {
        private readonly ToursService toursService;
        private readonly OrderService orderService;

        public override int PoolIndex => Rules.Pages.MainPool;
        public ToursViewModel(PageService pageservice, ToursService toursService, OrderService orderService) : base(pageservice)
        {
            this.toursService = toursService;
            this.orderService = orderService;
            Init();
        }

        async void Init()
        {
            await toursService.ReloadAsync(x => $"{Environment.CurrentDirectory}\\Images\\{x}.jpg");
            Tours = new ObservableCollection<TourDto>(await toursService.GetAllServices());
        }

        public ObservableCollection<TourDto> Tours { get; set; }

        public ICommand SelectTour => new CommandAsync(async x =>
        {
            if (x is TourDto tour) 
            {
                await orderService.SetupTour(tour);
                pageservice.ChangePage<Pages.TourDetails>(PoolIndex, DisappearAnimation.Default);
            }
        });
    }
}
