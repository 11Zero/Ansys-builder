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
        public int h11 = 0, h12 = 0, h21 = 0, h22 = 0, h31 = 0, h32 = 0, h41 = 0, h42 = 0;


        public int b11 = 0, b12 = 0, b21 = 0, b22 = 0, b31 = 0, b32 = 0, b41 = 0, b42 = 0;
        public int section_id = -1;
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
            //CADctrl_frame.UserDelAllCubes();
            //CADctrl_frame.UserDelAllCylinders();
        }

        private void btn_draw_view_Click(object sender, RoutedEventArgs e)
        {
            //CADctrl_frame.Test();
            //return;
            //if (section_selected_flag == false || section_id == -1)
            //    return;
            CADctrl_frame.UserDelAllCubes();
            CADctrl_frame.UserDelAllCylinders();
            string bri_span = text_span_step.Text;
            bri_span.Replace(" ", "");
            if (bri_span == "")
                return;
            string[] str_arr = bri_span.Split(',');
            double[] span = new double[str_arr.Length];
            double bridge_length = 0;
            for (int i = 0; i < str_arr.Length; i++)
            {
                span[i] = double.Parse(str_arr[i]);
                bridge_length = bridge_length + span[i];
            }
            #region 画桥墩
            double base_to_zero = 0;
            for (int i = 0; i < span.Length-1; i++)
            {
                base_to_zero = base_to_zero + span[i];
                if (comboBox_base_style.SelectedIndex == 0)
                {
                    if ((bool)checkBox_rect_base.IsChecked)
                    {
                       CADView.CADCube bri_base = null;
                       string bri_base_txt = text_rect_base_size.Text;
                        bri_base_txt.Replace(" ", "");
                        if (bri_base_txt == "")
                            return;
                        str_arr = bri_base_txt.Split(',');
                        if (str_arr.Length != 3)
                            return;
                        double x_len = double.Parse(str_arr[0]);
                        double y_len = double.Parse(str_arr[1]);
                        double z_len = double.Parse(str_arr[2]);
                        bri_base = new CADView.CADCube();
                        CADView.CADPoint p1 = new CADView.CADPoint(base_to_zero-x_len/2,-y_len/2,0);
                        CADView.CADPoint p2 = new CADView.CADPoint(base_to_zero + x_len / 2, -y_len / 2, 0);
                        CADView.CADPoint p3 = new CADView.CADPoint(base_to_zero + x_len / 2, y_len / 2, 0);
                        CADView.CADPoint p4 = new CADView.CADPoint(base_to_zero - x_len / 2, y_len / 2, 0);
                        bri_base.m_surfs[0] = new CADView.Rect3D(p1,p2,p3,p4);
                        bri_base.m_surfs[1] = bri_base.m_surfs[0] + new CADView.CADPoint(0,0,z_len);

                        p1 = new CADView.CADPoint(base_to_zero - x_len / 2, -y_len / 2, z_len);
                        p2 = new CADView.CADPoint(base_to_zero + x_len / 2, -y_len / 2, z_len);
                        p3 = new CADView.CADPoint(base_to_zero + x_len / 2, -y_len / 2, 0);
                        p4 = new CADView.CADPoint(base_to_zero - x_len / 2, -y_len / 2, 0);
                        bri_base.m_surfs[2] = new CADView.Rect3D(p1, p2, p3, p4);
                        bri_base.m_surfs[3] = bri_base.m_surfs[2] + new CADView.CADPoint(0, y_len, 0);

                        p1 = new CADView.CADPoint(base_to_zero - x_len / 2, -y_len / 2, z_len);
                        p2 = new CADView.CADPoint(base_to_zero - x_len / 2, y_len / 2, z_len);
                        p3 = new CADView.CADPoint(base_to_zero - x_len / 2, y_len / 2, 0);
                        p4 = new CADView.CADPoint(base_to_zero - x_len / 2, -y_len / 2, 0);
                        bri_base.m_surfs[4] = new CADView.Rect3D(p1, p2, p3, p4);
                        bri_base.m_surfs[5] = bri_base.m_surfs[4] + new CADView.CADPoint(x_len, 0, 0);

                        //bri_base.updateCenter();
                        CADctrl_frame.UserDrawCube(bri_base);

                    }
                    if ((bool)checkBox_cir_base.IsChecked)
                    {
                        string bri_base_txt = text_cir_base_size.Text;
                        bri_base_txt.Replace(" ", "");
                        if (bri_base_txt == "")
                            return;
                        str_arr = bri_base_txt.Split(',');
                        if (str_arr.Length != 2)
                            return;
                        double r = double.Parse(str_arr[0]);
                        double z_len = double.Parse(str_arr[1]);
                        CADView.CADCylinder bri_base = new CADView.CADCylinder();
                        CADView.CADPoint p1 = new CADView.CADPoint(base_to_zero,  0, 0);
                        CADView.CADPoint p2 = new CADView.CADPoint(base_to_zero , 0, -1);
                        CADView.CADPoint p3 = new CADView.CADPoint(base_to_zero , 0, z_len);
                        CADView.CADPoint p4 = new CADView.CADPoint(base_to_zero , 0, z_len+1);

                        bri_base.m_surfs_cir[0] = new CADView.CADCircle(p1, p2, r);
                        bri_base.m_surfs_cir[1] = new CADView.CADCircle(p3, p4, r);
                        //bri_base.updateCenter();

                        CADctrl_frame.UserDrawCylinder(bri_base);
                    }

                }

                if (comboBox_base_style.SelectedIndex == 1)
                {
                    string dist_str = text_base_dist.Text;
                    dist_str.Replace(" ", "");
                    if (dist_str == "")
                        return;
                    double dist = double.Parse(dist_str);
                    CADView.CADPoint dist_vector = new CADView.CADPoint(dist, 0, 0);
                    if ((bool)checkBox_rect_base.IsChecked)
                    {
                        CADView.CADCube bri_base = null;
                        string bri_base_txt = text_rect_base_size.Text;
                        bri_base_txt.Replace(" ", "");
                        if (bri_base_txt == "")
                            return;
                        str_arr = bri_base_txt.Split(',');
                        if (str_arr.Length != 3)
                            return;
                        double x_len = double.Parse(str_arr[0]);
                        double y_len = double.Parse(str_arr[1]);
                        double z_len = double.Parse(str_arr[2]);
                        bri_base = new CADView.CADCube();
                        CADView.CADPoint p1 = new CADView.CADPoint(base_to_zero-dist/2 - x_len / 2, -y_len / 2, 0);
                        CADView.CADPoint p2 = new CADView.CADPoint(base_to_zero-dist/2 + x_len / 2, -y_len / 2, 0);
                        CADView.CADPoint p3 = new CADView.CADPoint(base_to_zero-dist/2 + x_len / 2, y_len / 2, 0);
                        CADView.CADPoint p4 = new CADView.CADPoint(base_to_zero-dist/2 - x_len / 2, y_len / 2, 0);
                        bri_base.m_surfs[0] = new CADView.Rect3D(p1, p2, p3, p4);
                        bri_base.m_surfs[1] = bri_base.m_surfs[0] + new CADView.CADPoint(0, 0, z_len);

                        p1 = new CADView.CADPoint(base_to_zero-dist/2 - x_len / 2, -y_len / 2, z_len);
                        p2 = new CADView.CADPoint(base_to_zero-dist/2 + x_len / 2, -y_len / 2, z_len);
                        p3 = new CADView.CADPoint(base_to_zero-dist/2 + x_len / 2, -y_len / 2, 0);
                        p4 = new CADView.CADPoint(base_to_zero-dist/2 - x_len / 2, -y_len / 2, 0);
                        bri_base.m_surfs[2] = new CADView.Rect3D(p1, p2, p3, p4);
                        bri_base.m_surfs[3] = bri_base.m_surfs[2] + new CADView.CADPoint(0, y_len, 0);

                        p1 = new CADView.CADPoint(base_to_zero-dist/2 - x_len / 2, -y_len / 2, z_len);
                        p2 = new CADView.CADPoint(base_to_zero-dist/2 - x_len / 2, y_len / 2, z_len);
                        p3 = new CADView.CADPoint(base_to_zero-dist/2 - x_len / 2, y_len / 2, 0);
                        p4 = new CADView.CADPoint(base_to_zero-dist/2 - x_len / 2, -y_len / 2, 0);
                        bri_base.m_surfs[4] = new CADView.Rect3D(p1, p2, p3, p4);
                        bri_base.m_surfs[5] = bri_base.m_surfs[4] + new CADView.CADPoint(x_len, 0, 0);

                        CADctrl_frame.UserDrawCube(bri_base);

                        CADctrl_frame.UserDrawCube(bri_base + dist_vector);

                    }
                    if ((bool)checkBox_cir_base.IsChecked)
                    {
                        string bri_base_txt = text_cir_base_size.Text;
                        bri_base_txt.Replace(" ", "");
                        if (bri_base_txt == "")
                            return;
                        str_arr = bri_base_txt.Split(',');
                        if (str_arr.Length != 2)
                            return;
                        double r = double.Parse(str_arr[0]);
                        double z_len = double.Parse(str_arr[1]);
                        CADView.CADCylinder bri_base = new CADView.CADCylinder();
                        CADView.CADPoint p1 = new CADView.CADPoint(base_to_zero-dist/2, 0, 0);
                        CADView.CADPoint p2 = new CADView.CADPoint(base_to_zero-dist/2, 0, -1);
                        CADView.CADPoint p3 = new CADView.CADPoint(base_to_zero-dist/2, 0, z_len);
                        CADView.CADPoint p4 = new CADView.CADPoint(base_to_zero - dist / 2, 0, z_len + 1);

                        bri_base.m_surfs_cir[0] = new CADView.CADCircle(p1, p2, r);
                        bri_base.m_surfs_cir[1] = new CADView.CADCircle(p3, p4, r);
                        CADctrl_frame.UserDrawCylinder(bri_base);

                        CADctrl_frame.UserDrawCylinder(bri_base + dist_vector);

                    }

                }
                if (comboBox_base_style.SelectedIndex == 2)
                {
                    string dist_str = text_base_dist.Text;
                    dist_str.Replace(" ", "");
                    if (dist_str == "")
                        return;
                    double dist = double.Parse(dist_str);
                    CADView.CADPoint dist_vector = new CADView.CADPoint(0,dist, 0);
                    if ((bool)checkBox_rect_base.IsChecked)
                    {
                        CADView.CADCube bri_base = null;
                        string bri_base_txt = text_rect_base_size.Text;
                        bri_base_txt.Replace(" ", "");
                        if (bri_base_txt == "")
                            return;
                        str_arr = bri_base_txt.Split(',');
                        if (str_arr.Length != 3)
                            return;
                        double x_len = double.Parse(str_arr[0]);
                        double y_len = double.Parse(str_arr[1]);
                        double z_len = double.Parse(str_arr[2]);
                        bri_base = new CADView.CADCube();
                        CADView.CADPoint p1 = new CADView.CADPoint(base_to_zero -  x_len / 2, -y_len / 2 - dist / 2 , 0);
                        CADView.CADPoint p2 = new CADView.CADPoint(base_to_zero + x_len / 2, -y_len / 2- dist / 2 , 0);
                        CADView.CADPoint p3 = new CADView.CADPoint(base_to_zero + x_len / 2,  y_len / 2- dist / 2 , 0);
                        CADView.CADPoint p4 = new CADView.CADPoint(base_to_zero -  x_len / 2,  y_len / 2 - dist / 2, 0);
                        bri_base.m_surfs[0] = new CADView.Rect3D(p1, p2, p3, p4);
                        bri_base.m_surfs[1] = bri_base.m_surfs[0] + new CADView.CADPoint(0, 0, z_len);

                        p1 = new CADView.CADPoint(base_to_zero - x_len / 2, -y_len / 2 - dist / 2, z_len);
                        p2 = new CADView.CADPoint(base_to_zero + x_len / 2, -y_len / 2 - dist / 2, z_len);
                        p3 = new CADView.CADPoint(base_to_zero + x_len / 2, -y_len / 2 - dist / 2, 0);
                        p4 = new CADView.CADPoint(base_to_zero - x_len / 2, -y_len / 2 - dist / 2, 0);
                        bri_base.m_surfs[2] = new CADView.Rect3D(p1, p2, p3, p4);
                        bri_base.m_surfs[3] = bri_base.m_surfs[2] + new CADView.CADPoint(0, y_len, 0);

                        p1 = new CADView.CADPoint(base_to_zero - x_len / 2, -y_len / 2 - dist / 2, z_len);
                        p2 = new CADView.CADPoint(base_to_zero - x_len / 2, y_len / 2 - dist / 2, z_len);
                        p3 = new CADView.CADPoint(base_to_zero - x_len / 2, y_len / 2 - dist / 2, 0);
                        p4 = new CADView.CADPoint(base_to_zero - x_len / 2, -y_len / 2 - dist / 2, 0);
                        bri_base.m_surfs[4] = new CADView.Rect3D(p1, p2, p3, p4);
                        bri_base.m_surfs[5] = bri_base.m_surfs[4] + new CADView.CADPoint(x_len, 0, 0);

                        CADctrl_frame.UserDrawCube(bri_base);

                        CADctrl_frame.UserDrawCube(bri_base + dist_vector);

                    }
                    if ((bool)checkBox_cir_base.IsChecked)
                    {
                        string bri_base_txt = text_cir_base_size.Text;
                        bri_base_txt.Replace(" ", "");
                        if (bri_base_txt == "")
                            return;
                        str_arr = bri_base_txt.Split(',');
                        if (str_arr.Length != 2)
                            return;
                        double r = double.Parse(str_arr[0]);
                        double z_len = double.Parse(str_arr[1]);
                        CADView.CADCylinder bri_base = new CADView.CADCylinder();
                        CADView.CADPoint p1 = new CADView.CADPoint(base_to_zero , 0- dist / 2, 0);
                        CADView.CADPoint p2 = new CADView.CADPoint(base_to_zero , 0- dist / 2, -1);
                        CADView.CADPoint p3 = new CADView.CADPoint(base_to_zero , 0- dist / 2, z_len);
                        CADView.CADPoint p4 = new CADView.CADPoint(base_to_zero , 0 - dist / 2, z_len + 1);

                        bri_base.m_surfs_cir[0] = new CADView.CADCircle(p1, p2, r);
                        bri_base.m_surfs_cir[1] = new CADView.CADCircle(p3, p4, r);
                        CADctrl_frame.UserDrawCylinder(bri_base);

                        CADctrl_frame.UserDrawCylinder(bri_base + dist_vector);

                    }
                }
            }
            #endregion

            #region 画主梁
            CADView.CADPoint p_bridge_len = new CADView.CADPoint(bridge_length, 0, 0);
           if (this.section_id == 0)
            {
                CADView.CADPoint ptemp = new CADView.CADPoint(0, 2 * B01 + 2 * B02 + B03, 0) / 100;

                CADView.CADPoint pl01 = new CADView.CADPoint(0, 0, 0)/100;
                CADView.CADPoint pl02 = new CADView.CADPoint(0, 0, -H01) / 100;
                CADView.CADPoint pl03 = new CADView.CADPoint(0, B01, -H01-H02) / 100;
                CADView.CADPoint pl04 = new CADView.CADPoint(0, B01, -H01 - H02-H03) / 100;

                CADView.CADPoint pl05 = new CADView.CADPoint(0, B01 + B02 + b11,-H11) / 100;
                CADView.CADPoint pl06 = new CADView.CADPoint(0, B01 + B02, -H11 - h11) / 100;
                CADView.CADPoint pl07 = new CADView.CADPoint(0, B01 + B02, -H01 - H02 - H03 + H12 + h12) / 100;
                CADView.CADPoint pl08 = new CADView.CADPoint(0, B01 + B02 + b12, -H01 - H02 - H03 + H12) / 100;

                CADView.CADPoint pr01 = ptemp - pl01;
                CADView.CADPoint pr02 = ptemp - pl02;
                CADView.CADPoint pr03 = ptemp - pl03;
                CADView.CADPoint pr04 = ptemp - pl04;

                CADView.CADPoint pr05 = ptemp - pl05;
                CADView.CADPoint pr06 = ptemp - pl06;
                CADView.CADPoint pr07 = ptemp - pl07;
                CADView.CADPoint pr08 = ptemp - pl08;


                CADView.CADCube section_temp = new CADView.CADCube(pl01 , pr01 , pr05 , pl05 ,  pl01 + p_bridge_len, pr01  + p_bridge_len, pr05 + p_bridge_len, pl05 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl04, pr04, pr08, pl08, pl04 + p_bridge_len, pr04 + p_bridge_len, pr08 + p_bridge_len, pl08 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);

                section_temp = new CADView.CADCube(pl01, pl02, pl06, pl05, pl01 + p_bridge_len, pl02 + p_bridge_len, pl06 + p_bridge_len, pl05 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl03, pl04, pl07, pl06, pl03 + p_bridge_len, pl04 + p_bridge_len, pl07 + p_bridge_len, pl06 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                CADView.CADPrism section_tri = new CADView.CADPrism(pl04,pl07,pl08, pl04 + p_bridge_len, pl07 + p_bridge_len, pl08 + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);
            }
            #endregion


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
