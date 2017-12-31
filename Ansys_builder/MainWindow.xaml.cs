using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using Microsoft.Win32;
//using Ansys_builder.CAD;
using CADCtrl;
using log4net;

namespace Ansys_builder
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int H01 = 0, H02 = 0, H03 = 0;
        public int B01 = 0, B02 = 0, B03 = 0, B04 = 0, B05 = 0, B06 = 0;
        public int H11 = 0, H12 = 0, H21 = 0, H22 = 0, H31 = 0, H32 = 0, H41 = 0;

        private void comboBox_base_material_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selected = ((ComboBox)sender).SelectedIndex;
            switch (selected)
            {
                case 0:
                    { }break;
                case 1:
                    { }
                    break;
                case 2:
                    { }
                    break;
                case 3:
                    { }
                    break;
                case 4:
                    { }
                    break;
                case 5:
                    { }
                    break;
                case 6:
                    { }
                    break;
                default:
                    break;
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            CADctrl_frame.Test();
        }

        private void comboBox_beam_material_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void checkBox_rect_base_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox_rect_base.IsChecked == true)
            {
                checkBox_cir_base.IsChecked = false;
                text_cir_base_size.IsEnabled = false;
                text_rect_base_size.IsEnabled = true;

            }
        }

        private void checkBox_cir_base_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox_cir_base.IsChecked == true)
            {
                checkBox_rect_base.IsChecked = false;
                text_rect_base_size.IsEnabled = false;
                text_cir_base_size.IsEnabled = true;
            }
                

        }

        public int h11 = 0, h12 = 0, h21 = 0, h22 = 0, h31 = 0, h32 = 0, h41 = 0, h42 = 0;
        public int b11 = 0, b12 = 0, b21 = 0, b22 = 0, b31 = 0, b32 = 0, b41 = 0, b42 = 0;
        public bool section_form_closed = true;
        public bool section_selected_flag = false;
        public SectionForm section_form = null;
        public MainWindow()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
            log.Info("mian start up");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.text_base_len
            comboBox_base_style.SelectedIndex = -1;
            this.text_base_dist.IsEnabled = false;
        }


        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CADctrl_frame.ReactToESC();
                return;
            }

            if (e.Key == Key.L)
            {
                CADctrl_frame.ReactToL();
                return;
            }

            if (e.Key == Key.Delete)
            {
                CADctrl_frame.ReactToDel();
                return;
            }
            if (e.Key == Key.LeftShift)
            {
                CADctrl_frame.ShiftDwon();
                return;
            }
        }

        
        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
           
            if (e.Key == Key.LeftShift)
            {
                CADctrl_frame.ShiftUp();
                return;
            }
        }

        private void btn_section_Click(object sender, RoutedEventArgs e)
        {
            section_form = new SectionForm(this);
            section_form.Show();
        }

        private void comboBox_base_style_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_base_style.SelectedIndex <= 0)
            {
                this.text_base_dist.IsEnabled = false;
            }
            else
            {
                this.text_base_dist.IsEnabled = true; 
            }
        }
    }
}
