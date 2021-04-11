using DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BL
{
    public class SourceService
    {
        public SourceService(MapperService mapper)
        {
            this.mapper = mapper;
        }
        public IEnumerable<dynamic> Resources { get; private set; }

        public bool IsSetted => Resources != null;

        public string Header { get; set; } = "Наши сертификаты";

        public void SetWorkLaynersImages(Window _curWindow)
        {
            Image image = new Image();

            
            Resources = new List<dynamic>
            {
                new
                {
                    Source = _curWindow.FindResource("l1"),
                    Header = "Caribbean Princess 5*lux",
                    Descr = "Лайнер был спущен на воду в апреле 2004 года. Caribbean Princess 5*lux стал первым из лайнеров компании Princess, на котором появился впечатляющий кинотеатр под открытым небом."
                },

                new
                {
                    Source = _curWindow.FindResource("l2"),
                    Header = "Coral Princess",
                    Descr = "Лайнер был спущен на воду в январе 2003 года и обновлен в январе 2019 года. Coral Princess, построенный специально для круизов через Панамский Канал - уютный лайнер, сочетает в себе роскошную красоту и домашний комфорт.",
                },
                new
                {
                    Source = _curWindow.FindResource("l3"),
                    Header = "ФЛАГМАН - Sky Princess 5* lux",
                    Descr = "Sky Princess 5* lux. сохранит все черты, которые стали визитной карточкой компании: грандиозный атриум с огромным разнообразием развлечений и уютных ресторанов, \"кино под звездами\" и \"персональный кусочек моря\" - все внешние каюты оборудованы балконом. Компания продолжит свой курс на инновации, добавив на новом лайнере еще больше новых возможностей."
                },
            };
        }

        private readonly MapperService mapper;

       
    }
}