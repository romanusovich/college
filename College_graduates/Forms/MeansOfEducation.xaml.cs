﻿using College_graduates.Class;
using College_graduates.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace College_graduates.Forms
{
    /// <summary>
    /// Логика взаимодействия для MeansOfEducation.xaml
    /// </summary>
    public partial class MeansOfEducation : Window
    {
        public MeansOfEducation(EntityContext _db)
        {
            InitializeComponent();

            DataContext = new MeanViewModel(_db);
        }
    }
}
