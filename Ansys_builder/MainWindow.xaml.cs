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

            text_span_step.Text = "30,60,30";
            comboBox_base_style.SelectedIndex = 0;
            checkBox_cir_base.IsChecked = true;
            text_cir_base_size.Text = "1,20";
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
            CADctrl_frame.UserDelAllPrisms();
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
            double base_height = 0;
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
                        base_height = z_len;
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
                        base_height = z_len;

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
                        base_height = z_len;

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
                        base_height = z_len;

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
                        base_height = z_len;

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
                        base_height = z_len;

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
                //CADView.CADPoint ptemp = new CADView.CADPoint(0, 2 * B01 + 2 * B02 + B03, 0) / 100;
                CADView.CADPoint p_to_center = new CADView.CADPoint(0, -B01 -B02 - B03/2, H01+H02+H03+base_height*100) / 100;


                CADView.CADPoint pl01 = new CADView.CADPoint(0, 0, 0)/100;
                CADView.CADPoint pl02 = new CADView.CADPoint(0, 0, -H01) / 100 ;
                CADView.CADPoint pl03 = new CADView.CADPoint(0, B01, -H01-H02) / 100 ;
                CADView.CADPoint pl04 = new CADView.CADPoint(0, B01, -H01 - H02-H03) / 100 ;
                CADView.CADPoint pl05 = new CADView.CADPoint(0, B01 + B02 + b11,-H11) / 100 ;
                CADView.CADPoint pl06 = new CADView.CADPoint(0, B01 + B02, -H11 - h11) / 100 ;
                CADView.CADPoint pl07 = new CADView.CADPoint(0, B01 + B02, -H01 - H02 - H03 + H12 + h12) / 100 ;
                CADView.CADPoint pl08 = new CADView.CADPoint(0, B01 + B02 + b12, -H01 - H02 - H03 + H12) / 100 ;

                CADView.CADPoint pr01 = new CADView.CADPoint(0, 2 * B01 + 2 * B02 + B03-0, 0)/100;
                CADView.CADPoint pr02 = new CADView.CADPoint(0, 2 * B01 + 2 * B02 + B03-0, -H01) / 100 ;
                CADView.CADPoint pr03 = new CADView.CADPoint(0, 2 * B01 + 2 * B02 + B03-B01, -H01-H02) / 100 ;
                CADView.CADPoint pr04 = new CADView.CADPoint(0, 2 * B01 + 2 * B02 + B03-B01, -H01 - H02-H03) / 100 ;
                CADView.CADPoint pr05 = new CADView.CADPoint(0, 2 * B01 + 2 * B02 + B03-(B01 + B02 + b11),-H11) / 100 ;
                CADView.CADPoint pr06 = new CADView.CADPoint(0, 2 * B01 + 2 * B02 + B03-(B01 + B02), -H11 - h11) / 100 ;
                CADView.CADPoint pr07 = new CADView.CADPoint(0, 2 * B01 + 2 * B02 + B03-(B01 + B02), -H01 - H02 - H03 + H12 + h12) / 100 ;
                CADView.CADPoint pr08 = new CADView.CADPoint(0, 2 * B01 + 2 * B02 + B03-(B01 + B02 + b12), -H01 - H02 - H03 + H12) / 100;

                pl01 = pl01 + p_to_center;
                pl02 = pl02 + p_to_center;
                pl03 = pl03 + p_to_center;
                pl04 = pl04 + p_to_center;
                pl05 = pl05 + p_to_center;
                pl06 = pl06 + p_to_center;
                pl07 = pl07 + p_to_center;
                pl08 = pl08 + p_to_center;

                pr01 = pr01 + p_to_center;
                pr02 = pr02 + p_to_center;
                pr03 = pr03 + p_to_center;
                pr04 = pr04 + p_to_center;
                pr05 = pr05 + p_to_center;
                pr06 = pr06 + p_to_center;
                pr07 = pr07 + p_to_center;
                pr08 = pr08 + p_to_center;


                CADView.CADCube section_temp = new CADView.CADCube(pl01 , pr01 , pr05 , pl05 ,  pl01 + p_bridge_len, pr01  + p_bridge_len, pr05 + p_bridge_len, pl05 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl04, pr04, pr08, pl08, pl04 + p_bridge_len, pr04 + p_bridge_len, pr08 + p_bridge_len, pl08 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);

                section_temp = new CADView.CADCube(pl01, pl02, pl03, pl05, pl01 + p_bridge_len, pl02 + p_bridge_len, pl03 + p_bridge_len, pl05 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl03, pl04, pl07, pl06, pl03 + p_bridge_len, pl04 + p_bridge_len, pl07 + p_bridge_len, pl06 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                CADView.CADPrism section_tri = new CADView.CADPrism(pl04, pl07, pl08, pl04 + p_bridge_len, pl07 + p_bridge_len, pl08 + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);
                section_tri = new CADView.CADPrism(pl03, pl06, pl05, pl03 + p_bridge_len, pl06 + p_bridge_len, pl05 + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);

                section_temp = new CADView.CADCube(pr01, pr02, pr03, pr05, pr01 + p_bridge_len, pr02 + p_bridge_len, pr03 + p_bridge_len, pr05 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr03, pr04, pr07, pr06, pr03 + p_bridge_len, pr04 + p_bridge_len, pr07 + p_bridge_len, pr06 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_tri = new CADView.CADPrism(pr04, pr07, pr08, pr04 + p_bridge_len, pr07 + p_bridge_len, pr08 + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);
                section_tri = new CADView.CADPrism(pr03, pr06, pr05, pr03 + p_bridge_len, pr06 + p_bridge_len, pr05 + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);

            }
            if (this.section_id == 1)
            {
                CADView.CADPoint p_to_center = new CADView.CADPoint(0, -B01 - B02 - B03-B04 / 2, H01 + H02 + H03 + base_height * 100) / 100;

                double beam_width = 2 * B01 + 2 * B02 + 2 * B03 + B04;
                CADView.CADPoint pl01 = new CADView.CADPoint(0, 0, 0) / 100;
                CADView.CADPoint pl02 = new CADView.CADPoint(0, 0, -H01) / 100;
                CADView.CADPoint pl03 = new CADView.CADPoint(0, B01, -H01 - H02) / 100;
                CADView.CADPoint pl04 = new CADView.CADPoint(0, B01, -H01 - H02 - H03) / 100;
                CADView.CADPoint pl05 = new CADView.CADPoint(0, B01 + B02 + b11, -H11) / 100;
                CADView.CADPoint pl06 = new CADView.CADPoint(0, B01 + B02, -H11 - h11) / 100;
                CADView.CADPoint pl07 = new CADView.CADPoint(0, B01 + B02, -H01 - H02 - H03 + H12 + h12) / 100;
                CADView.CADPoint pl08 = new CADView.CADPoint(0, B01 + B02 + b12, -H01 - H02 - H03 + H12) / 100;
                CADView.CADPoint pl09 = new CADView.CADPoint(0, B01 + B02 + B03 - b21, -H11) / 100;
                CADView.CADPoint pl10 = new CADView.CADPoint(0, B01 + B02+B03, -H11 - h21) / 100;
                CADView.CADPoint pl11 = new CADView.CADPoint(0, B01 + B02+B03, -H01 - H02 - H03 + H12 + h22) / 100;
                CADView.CADPoint pl12 = new CADView.CADPoint(0, B01 + B02 +B03- b22, -H01 - H02 - H03 + H12) / 100;

                CADView.CADPoint pr01 = new CADView.CADPoint(0, beam_width-0, 0) / 100;
                CADView.CADPoint pr02 = new CADView.CADPoint(0, beam_width-0, -H01) / 100;
                CADView.CADPoint pr03 = new CADView.CADPoint(0, beam_width-(B01), -H01 - H02) / 100;
                CADView.CADPoint pr04 = new CADView.CADPoint(0, beam_width-(B01), -H01 - H02 - H03) / 100;
                CADView.CADPoint pr05 = new CADView.CADPoint(0, beam_width-(B01 + B02 + b11), -H11) / 100;
                CADView.CADPoint pr06 = new CADView.CADPoint(0, beam_width-(B01 + B02), -H11 - h11) / 100;
                CADView.CADPoint pr07 = new CADView.CADPoint(0, beam_width-(B01 + B02), -H01 - H02 - H03 + H12 + h12) / 100;
                CADView.CADPoint pr08 = new CADView.CADPoint(0, beam_width-(B01 + B02 + b12), -H01 - H02 - H03 + H12) / 100;
                CADView.CADPoint pr09 = new CADView.CADPoint(0, beam_width-(B01 + B02 + B03 - b21), -H11) / 100;
                CADView.CADPoint pr10 = new CADView.CADPoint(0, beam_width-(B01 + B02 + B03), -H11 - h21) / 100;
                CADView.CADPoint pr11 = new CADView.CADPoint(0, beam_width-(B01 + B02 + B03), -H01 - H02 - H03 + H12 + h22) / 100;
                CADView.CADPoint pr12 = new CADView.CADPoint(0, beam_width-(B01 + B02 + B03 - b22), -H01 - H02 - H03 + H12) / 100;


                pl01 = pl01 + p_to_center;
                pl02 = pl02 + p_to_center;
                pl03 = pl03 + p_to_center;
                pl04 = pl04 + p_to_center;
                pl05 = pl05 + p_to_center;
                pl06 = pl06 + p_to_center;
                pl07 = pl07 + p_to_center;
                pl08 = pl08 + p_to_center;
                pl09 = pl09 + p_to_center;
                pl10 = pl10 + p_to_center;
                pl11 = pl11 + p_to_center;
                pl12 = pl12 + p_to_center;

                pr01 = pr01 + p_to_center;
                pr02 = pr02 + p_to_center;
                pr03 = pr03 + p_to_center;
                pr04 = pr04 + p_to_center;
                pr05 = pr05 + p_to_center;
                pr06 = pr06 + p_to_center;
                pr07 = pr07 + p_to_center;
                pr08 = pr08 + p_to_center;
                pr09 = pr09 + p_to_center;
                pr10 = pr10 + p_to_center;
                pr11 = pr11 + p_to_center;
                pr12 = pr12 + p_to_center;



                CADView.CADCube section_temp = new CADView.CADCube(pl01, pr01, pr05, pl05, pl01 + p_bridge_len, pr01 + p_bridge_len, pr05 + p_bridge_len, pl05 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl04, pr04, pr08, pl08, pl04 + p_bridge_len, pr04 + p_bridge_len, pr08 + p_bridge_len, pl08 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl09, pr09, pr10, pl10, pl09 + p_bridge_len, pr09 + p_bridge_len, pr10 + p_bridge_len, pl10 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl10, pr10, pr11, pl11, pl10 + p_bridge_len, pr10 + p_bridge_len, pr11 + p_bridge_len, pl11 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl11, pr11, pr12, pl12, pl11 + p_bridge_len, pr11 + p_bridge_len, pr12 + p_bridge_len, pl12 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);

                section_temp = new CADView.CADCube(pl01, pl02, pl03, pl05, pl01 + p_bridge_len, pl02 + p_bridge_len, pl03 + p_bridge_len, pl05 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl03, pl04, pl07, pl06, pl03 + p_bridge_len, pl04 + p_bridge_len, pl07 + p_bridge_len, pl06 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                CADView.CADPrism section_tri = new CADView.CADPrism(pl04, pl07, pl08, pl04 + p_bridge_len, pl07 + p_bridge_len, pl08 + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);
                section_tri = new CADView.CADPrism(pl03, pl06, pl05, pl03 + p_bridge_len, pl06 + p_bridge_len, pl05 + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);

                section_temp = new CADView.CADCube(pr01, pr02, pr03, pr05, pr01 + p_bridge_len, pr02 + p_bridge_len, pr03 + p_bridge_len, pr05 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr03, pr04, pr07, pr06, pr03 + p_bridge_len, pr04 + p_bridge_len, pr07 + p_bridge_len, pr06 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_tri = new CADView.CADPrism(pr04, pr07, pr08, pr04 + p_bridge_len, pr07 + p_bridge_len, pr08 + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);
                section_tri = new CADView.CADPrism(pr03, pr06, pr05, pr03 + p_bridge_len, pr06 + p_bridge_len, pr05 + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);

            }
            if (this.section_id == 2)
            {
                CADView.CADPoint p_to_center = new CADView.CADPoint(0, -B01 - B02 - B03 - B04-B05 / 2, H01 + H02 + H03 + base_height * 100) / 100;
                CADView.CADPoint p_top_center = new CADView.CADPoint(0, B01 + B02 + B03 + B04 + B05 / 2, 0) / 100+ p_to_center;
                CADView.CADPoint p_bottom_center = new CADView.CADPoint(0, B01 + B02 + B03 + B04 + B05 / 2, -H01 - H02 - H03) / 100 + p_to_center;

                double beam_width = 2 * B01 + 2 * B02 + 2 * B03 + 2*B04+B05;
                CADView.CADPoint pl01 = new CADView.CADPoint(0, 0, 0) / 100;
                CADView.CADPoint pl02 = new CADView.CADPoint(0, 0, -H01) / 100;
                CADView.CADPoint pl03 = new CADView.CADPoint(0, B01, -H01 - H02) / 100;
                CADView.CADPoint pl04 = new CADView.CADPoint(0, B01, -H01 - H02 - H03) / 100;
                CADView.CADPoint pl05 = new CADView.CADPoint(0, B01 + B02 + b11, -H11) / 100;
                CADView.CADPoint pl06 = new CADView.CADPoint(0, B01 + B02, -H11 - h11) / 100;
                CADView.CADPoint pl07 = new CADView.CADPoint(0, B01 + B02, -H01 - H02 - H03 + H12 + h12) / 100;
                CADView.CADPoint pl08 = new CADView.CADPoint(0, B01 + B02 + b12, -H01 - H02 - H03 + H12) / 100;
                CADView.CADPoint pl09 = new CADView.CADPoint(0, B01 + B02 + B03 - b21, -H11) / 100;
                CADView.CADPoint pl10 = new CADView.CADPoint(0, B01 + B02 + B03, -H11 - h21) / 100;
                CADView.CADPoint pl11 = new CADView.CADPoint(0, B01 + B02 + B03, -H01 - H02 - H03 + H12 + h22) / 100;
                CADView.CADPoint pl12 = new CADView.CADPoint(0, B01 + B02 + B03 - b22, -H01 - H02 - H03 + H12) / 100;
                CADView.CADPoint pl13 = new CADView.CADPoint(0, B01 + B02 + B03+B04 + b31, -H21) / 100;
                CADView.CADPoint pl14 = new CADView.CADPoint(0, B01 + B02 + B03+B04, -H21 - h31) / 100;
                CADView.CADPoint pl15 = new CADView.CADPoint(0, B01 + B02 + B03+B04, -H01 - H02 - H03 + H22 + h32) / 100;
                CADView.CADPoint pl16 = new CADView.CADPoint(0, B01 + B02 + B03+B04 + b32, -H01 - H02 - H03 + H22) / 100;

                CADView.CADPoint pr01 = new CADView.CADPoint(0, beam_width - 0, 0) / 100;
                CADView.CADPoint pr02 = new CADView.CADPoint(0, beam_width - 0, -H01) / 100;
                CADView.CADPoint pr03 = new CADView.CADPoint(0, beam_width - B01, -H01 - H02) / 100;
                CADView.CADPoint pr04 = new CADView.CADPoint(0, beam_width - B01, -H01 - H02 - H03) / 100;
                CADView.CADPoint pr05 = new CADView.CADPoint(0, beam_width - (B01 + B02 + b11), -H11) / 100;
                CADView.CADPoint pr06 = new CADView.CADPoint(0, beam_width - (B01 + B02), -H11 - h11) / 100;
                CADView.CADPoint pr07 = new CADView.CADPoint(0, beam_width - (B01 + B02), -H01 - H02 - H03 + H12 + h12) / 100;
                CADView.CADPoint pr08 = new CADView.CADPoint(0, beam_width - (B01 + B02 + b12), -H01 - H02 - H03 + H12) / 100;
                CADView.CADPoint pr09 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 - b21), -H11) / 100;
                CADView.CADPoint pr10 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03), -H11 - h21) / 100;
                CADView.CADPoint pr11 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03), -H01 - H02 - H03 + H12 + h22) / 100;
                CADView.CADPoint pr12 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 - b22), -H01 - H02 - H03 + H12) / 100;
                CADView.CADPoint pr13 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 + B04 + b31), -H21) / 100;
                CADView.CADPoint pr14 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 + B04), -H21 - h31) / 100;
                CADView.CADPoint pr15 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 + B04), -H01 - H02 - H03 + H22 + h32) / 100;
                CADView.CADPoint pr16 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 + B04 + b32), -H01 - H02 - H03 + H22) / 100;


                pl01 = pl01 + p_to_center;
                pl02 = pl02 + p_to_center;
                pl03 = pl03 + p_to_center;
                pl04 = pl04 + p_to_center;
                pl05 = pl05 + p_to_center;
                pl06 = pl06 + p_to_center;
                pl07 = pl07 + p_to_center;
                pl08 = pl08 + p_to_center;
                pl09 = pl09 + p_to_center;
                pl10 = pl10 + p_to_center;
                pl11 = pl11 + p_to_center;
                pl12 = pl12 + p_to_center;
                pl13 = pl13 + p_to_center;
                pl14 = pl14 + p_to_center;
                pl15 = pl15 + p_to_center;
                pl16 = pl16 + p_to_center;

                pr01 = pr01 + p_to_center;
                pr02 = pr02 + p_to_center;
                pr03 = pr03 + p_to_center;
                pr04 = pr04 + p_to_center;
                pr05 = pr05 + p_to_center;
                pr06 = pr06 + p_to_center;
                pr07 = pr07 + p_to_center;
                pr08 = pr08 + p_to_center;
                pr09 = pr09 + p_to_center;
                pr10 = pr10 + p_to_center;
                pr11 = pr11 + p_to_center;
                pr12 = pr12 + p_to_center;
                pr13 = pr13 + p_to_center;
                pr14 = pr14 + p_to_center;
                pr15 = pr15 + p_to_center;
                pr16 = pr16 + p_to_center;



                CADView.CADCube section_temp = new CADView.CADCube(pl01, pl02, pl05, pl09, pl01 + p_bridge_len, pl02 + p_bridge_len, pl05 + p_bridge_len, pl09 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl02, pl03, pl06, pl05, pl02 + p_bridge_len, pl03 + p_bridge_len, pl06 + p_bridge_len, pl05 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl03, pl04, pl07, pl06, pl03 + p_bridge_len, pl04 + p_bridge_len, pl07 + p_bridge_len, pl06 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl04, pl07, pl08, pl12, pl04 + p_bridge_len, pl07 + p_bridge_len, pl08 + p_bridge_len, pl12 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl09, pl13, pl14, pl10, pl09 + p_bridge_len, pl13 + p_bridge_len, pl14 + p_bridge_len, pl10 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl10, pl14, pl15, pl11, pl10 + p_bridge_len, pl14 + p_bridge_len, pl15 + p_bridge_len, pl11 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl11, pl15, pl16, pl12, pl11 + p_bridge_len, pl15 + p_bridge_len, pl16 + p_bridge_len, pl12 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl01, pl09, pl13, p_top_center, pl01 + p_bridge_len, pl09 + p_bridge_len, pl13 + p_bridge_len, p_top_center + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl04, pl12, pl16, p_bottom_center, pl04 + p_bridge_len, pl12 + p_bridge_len, pl16 + p_bridge_len, p_bottom_center + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);

                CADView.CADPrism section_tri = new CADView.CADPrism(pl13, pr13, p_top_center, pl13 + p_bridge_len, pr13 + p_bridge_len, p_top_center + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);
                section_tri = new CADView.CADPrism(pl16, pr16, p_bottom_center, pl16 + p_bridge_len, pr16 + p_bridge_len, p_bottom_center + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);

                section_temp = new CADView.CADCube(pr01, pr02, pr05, pr09, pr01 + p_bridge_len, pr02 + p_bridge_len, pr05 + p_bridge_len, pr09 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr02, pr03, pr06, pr05, pr02 + p_bridge_len, pr03 + p_bridge_len, pr06 + p_bridge_len, pr05 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr03, pr04, pr07, pr06, pr03 + p_bridge_len, pr04 + p_bridge_len, pr07 + p_bridge_len, pr06 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr04, pr07, pr08, pr12, pr04 + p_bridge_len, pr07 + p_bridge_len, pr08 + p_bridge_len, pr12 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr09, pr13, pr14, pr10, pr09 + p_bridge_len, pr13 + p_bridge_len, pr14 + p_bridge_len, pr10 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr10, pr14, pr15, pr11, pr10 + p_bridge_len, pr14 + p_bridge_len, pr15 + p_bridge_len, pr11 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr11, pr15, pr16, pr12, pr11 + p_bridge_len, pr15 + p_bridge_len, pr16 + p_bridge_len, pr12 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr01, pr09, pr13, p_top_center, pr01 + p_bridge_len, pr09 + p_bridge_len, pr13 + p_bridge_len, p_top_center + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr04, pr12, pr16, p_bottom_center, pr04 + p_bridge_len, pr12 + p_bridge_len, pr16 + p_bridge_len, p_bottom_center + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
            }

            if (this.section_id == 3)
            {
                CADView.CADPoint p_to_center = new CADView.CADPoint(0, -B01 - B02 - B03 - B04 - B05 -B06/ 2, H01 + H02 + H03 + base_height * 100) / 100;
                CADView.CADPoint p_top_center = new CADView.CADPoint(0, B01 + B02 + B03 + B04 + B05 + B06 / 2, 0) / 100 + p_to_center;
                CADView.CADPoint p_bottom_center = new CADView.CADPoint(0, B01 + B02 + B03 + B04 + B05 + B06 / 2, -H01 - H02 - H03) / 100 + p_to_center;

                double beam_width = 2 * B01 + 2 * B02 + 2 * B03 + 2 * B04 + 2*B05+B06;
                CADView.CADPoint pl01 = new CADView.CADPoint(0, 0, 0) / 100;
                CADView.CADPoint pl02 = new CADView.CADPoint(0, 0, -H01) / 100;
                CADView.CADPoint pl03 = new CADView.CADPoint(0, B01, -H01 - H02) / 100;
                CADView.CADPoint pl04 = new CADView.CADPoint(0, B01, -H01 - H02 - H03) / 100;
                CADView.CADPoint pl05 = new CADView.CADPoint(0, B01 + B02 + b11, -H11) / 100;
                CADView.CADPoint pl06 = new CADView.CADPoint(0, B01 + B02, -H11 - h11) / 100;
                CADView.CADPoint pl07 = new CADView.CADPoint(0, B01 + B02, -H01 - H02 - H03 + H12 + h12) / 100;
                CADView.CADPoint pl08 = new CADView.CADPoint(0, B01 + B02 + b12, -H01 - H02 - H03 + H12) / 100;
                CADView.CADPoint pl09 = new CADView.CADPoint(0, B01 + B02 + B03 - b21, -H11) / 100;
                CADView.CADPoint pl10 = new CADView.CADPoint(0, B01 + B02 + B03, -H11 - h21) / 100;
                CADView.CADPoint pl11 = new CADView.CADPoint(0, B01 + B02 + B03, -H01 - H02 - H03 + H12 + h22) / 100;
                CADView.CADPoint pl12 = new CADView.CADPoint(0, B01 + B02 + B03 - b22, -H01 - H02 - H03 + H12) / 100;
                CADView.CADPoint pl13 = new CADView.CADPoint(0, B01 + B02 + B03 + B04 + b31, -H21) / 100;
                CADView.CADPoint pl14 = new CADView.CADPoint(0, B01 + B02 + B03 + B04, -H21 - h31) / 100;
                CADView.CADPoint pl15 = new CADView.CADPoint(0, B01 + B02 + B03 + B04, -H01 - H02 - H03 + H22 + h32) / 100;
                CADView.CADPoint pl16 = new CADView.CADPoint(0, B01 + B02 + B03 + B04 + b32, -H01 - H02 - H03 + H22) / 100;
                CADView.CADPoint pl17 = new CADView.CADPoint(0, B01 + B02 + B03 + B04 +B05- b41, -H21) / 100;
                CADView.CADPoint pl18 = new CADView.CADPoint(0, B01 + B02 + B03 + B04+B05, -H21 - h41) / 100;
                CADView.CADPoint pl19 = new CADView.CADPoint(0, B01 + B02 + B03 + B04 + B05 , -H01 - H02 - H03 + H22 + h42) / 100;
                CADView.CADPoint pl20 = new CADView.CADPoint(0, B01 + B02 + B03 + B04 + B05 - b42, -H01 - H02 - H03 + H22) / 100;

                CADView.CADPoint pr01 = new CADView.CADPoint(0, beam_width - 0, 0) / 100;
                CADView.CADPoint pr02 = new CADView.CADPoint(0, beam_width - 0, -H01) / 100;
                CADView.CADPoint pr03 = new CADView.CADPoint(0, beam_width - B01, -H01 - H02) / 100;
                CADView.CADPoint pr04 = new CADView.CADPoint(0, beam_width - B01, -H01 - H02 - H03) / 100;
                CADView.CADPoint pr05 = new CADView.CADPoint(0, beam_width - (B01 + B02 + b11), -H11) / 100;
                CADView.CADPoint pr06 = new CADView.CADPoint(0, beam_width - (B01 + B02), -H11 - h11) / 100;
                CADView.CADPoint pr07 = new CADView.CADPoint(0, beam_width - (B01 + B02), -H01 - H02 - H03 + H12 + h12) / 100;
                CADView.CADPoint pr08 = new CADView.CADPoint(0, beam_width - (B01 + B02 + b12), -H01 - H02 - H03 + H12) / 100;
                CADView.CADPoint pr09 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 - b21), -H11) / 100;
                CADView.CADPoint pr10 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03), -H11 - h21) / 100;
                CADView.CADPoint pr11 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03), -H01 - H02 - H03 + H12 + h22) / 100;
                CADView.CADPoint pr12 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 - b22), -H01 - H02 - H03 + H12) / 100;
                CADView.CADPoint pr13 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 + B04 + b31), -H21) / 100;
                CADView.CADPoint pr14 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 + B04), -H21 - h31) / 100;
                CADView.CADPoint pr15 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 + B04), -H01 - H02 - H03 + H22 + h32) / 100;
                CADView.CADPoint pr16 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 + B04 + b32), -H01 - H02 - H03 + H22) / 100;
                CADView.CADPoint pr17 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 + B04 + B05 - b41), -H21) / 100;
                CADView.CADPoint pr18 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 + B04 + B05), -H21 - h41) / 100;
                CADView.CADPoint pr19 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 + B04 + B05), -H01 - H02 - H03 + H22 + h42) / 100;
                CADView.CADPoint pr20 = new CADView.CADPoint(0, beam_width - (B01 + B02 + B03 + B04 + B05 - b42), -H01 - H02 - H03 + H22) / 100;


                pl01 = pl01 + p_to_center;
                pl02 = pl02 + p_to_center;
                pl03 = pl03 + p_to_center;
                pl04 = pl04 + p_to_center;
                pl05 = pl05 + p_to_center;
                pl06 = pl06 + p_to_center;
                pl07 = pl07 + p_to_center;
                pl08 = pl08 + p_to_center;
                pl09 = pl09 + p_to_center;
                pl10 = pl10 + p_to_center;
                pl11 = pl11 + p_to_center;
                pl12 = pl12 + p_to_center;
                pl13 = pl13 + p_to_center;
                pl14 = pl14 + p_to_center;
                pl15 = pl15 + p_to_center;
                pl16 = pl16 + p_to_center;
                pl17 = pl17 + p_to_center;
                pl18 = pl18 + p_to_center;
                pl19 = pl19 + p_to_center;
                pl20 = pl20 + p_to_center;

                pr01 = pr01 + p_to_center;
                pr02 = pr02 + p_to_center;
                pr03 = pr03 + p_to_center;
                pr04 = pr04 + p_to_center;
                pr05 = pr05 + p_to_center;
                pr06 = pr06 + p_to_center;
                pr07 = pr07 + p_to_center;
                pr08 = pr08 + p_to_center;
                pr09 = pr09 + p_to_center;
                pr10 = pr10 + p_to_center;
                pr11 = pr11 + p_to_center;
                pr12 = pr12 + p_to_center;
                pr13 = pr13 + p_to_center;
                pr14 = pr14 + p_to_center;
                pr15 = pr15 + p_to_center;
                pr16 = pr16 + p_to_center;
                pr17 = pr17 + p_to_center;
                pr18 = pr18 + p_to_center;
                pr19 = pr19 + p_to_center;
                pr20 = pr20 + p_to_center;



                CADView.CADCube section_temp = new CADView.CADCube(pl01, pl02, pl05, pl09, pl01 + p_bridge_len, pl02 + p_bridge_len, pl05 + p_bridge_len, pl09 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl02, pl03, pl06, pl05, pl02 + p_bridge_len, pl03 + p_bridge_len, pl06 + p_bridge_len, pl05 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl03, pl04, pl07, pl06, pl03 + p_bridge_len, pl04 + p_bridge_len, pl07 + p_bridge_len, pl06 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl04, pl07, pl08, pl12, pl04 + p_bridge_len, pl07 + p_bridge_len, pl08 + p_bridge_len, pl12 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl09, pl13, pl14, pl10, pl09 + p_bridge_len, pl13 + p_bridge_len, pl14 + p_bridge_len, pl10 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl10, pl14, pl15, pl11, pl10 + p_bridge_len, pl14 + p_bridge_len, pl15 + p_bridge_len, pl11 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl11, pl15, pl16, pl12, pl11 + p_bridge_len, pl15 + p_bridge_len, pl16 + p_bridge_len, pl12 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl18, pl19, pr19, pr18, pl18 + p_bridge_len, pl19 + p_bridge_len, pr19 + p_bridge_len, pr18 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl01, pl09, pl13, p_top_center, pl01 + p_bridge_len, pl09 + p_bridge_len, pl13 + p_bridge_len, p_top_center + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pl04, pl12, pl16, p_bottom_center, pl04 + p_bridge_len, pl12 + p_bridge_len, pl16 + p_bridge_len, p_bottom_center + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                CADView.CADPrism section_tri = new CADView.CADPrism(pl13, pl17, p_top_center, pl13 + p_bridge_len, pl17 + p_bridge_len, p_top_center + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);
                section_tri = new CADView.CADPrism(pl17, pl18, p_top_center, pl17 + p_bridge_len, pl18 + p_bridge_len, p_top_center + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);
                section_tri = new CADView.CADPrism(pl16, pl20, p_bottom_center, pl16 + p_bridge_len, pl20 + p_bridge_len, p_bottom_center + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);
                section_tri = new CADView.CADPrism(pl19, pl20, p_bottom_center, pl19 + p_bridge_len, pl20 + p_bridge_len, p_bottom_center + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);


                ///中间对称部位
                section_temp = new CADView.CADCube(pl18, pl19, pr19, pr18, pl18 + p_bridge_len, pl19 + p_bridge_len, pr19 + p_bridge_len, pr18 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_tri = new CADView.CADPrism(pl18, pr18, p_top_center, pl18 + p_bridge_len, pr18 + p_bridge_len, p_top_center + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);
                section_tri = new CADView.CADPrism(pl19, pr19, p_bottom_center, pl19 + p_bridge_len, pr19 + p_bridge_len, p_bottom_center + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);

                section_temp = new CADView.CADCube(pr01, pr02, pr05, pr09, pr01 + p_bridge_len, pr02 + p_bridge_len, pr05 + p_bridge_len, pr09 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr02, pr03, pr06, pr05, pr02 + p_bridge_len, pr03 + p_bridge_len, pr06 + p_bridge_len, pr05 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr03, pr04, pr07, pr06, pr03 + p_bridge_len, pr04 + p_bridge_len, pr07 + p_bridge_len, pr06 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr04, pr07, pr08, pr12, pr04 + p_bridge_len, pr07 + p_bridge_len, pr08 + p_bridge_len, pr12 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr09, pr13, pr14, pr10, pr09 + p_bridge_len, pr13 + p_bridge_len, pr14 + p_bridge_len, pr10 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr10, pr14, pr15, pr11, pr10 + p_bridge_len, pr14 + p_bridge_len, pr15 + p_bridge_len, pr11 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr11, pr15, pr16, pr12, pr11 + p_bridge_len, pr15 + p_bridge_len, pr16 + p_bridge_len, pr12 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr18, pr19, pr19, pr18, pr18 + p_bridge_len, pr19 + p_bridge_len, pr19 + p_bridge_len, pr18 + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr01, pr09, pr13, p_top_center, pr01 + p_bridge_len, pr09 + p_bridge_len, pr13 + p_bridge_len, p_top_center + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_temp = new CADView.CADCube(pr04, pr12, pr16, p_bottom_center, pr04 + p_bridge_len, pr12 + p_bridge_len, pr16 + p_bridge_len, p_bottom_center + p_bridge_len);
                CADctrl_frame.UserDrawCube(section_temp);
                section_tri = new CADView.CADPrism(pr13, pr17, p_top_center, pr13 + p_bridge_len, pr17 + p_bridge_len, p_top_center + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);
                section_tri = new CADView.CADPrism(pr17, pr18, p_top_center, pr17 + p_bridge_len, pr18 + p_bridge_len, p_top_center + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);
                section_tri = new CADView.CADPrism(pr16, pr20, p_bottom_center, pr16 + p_bridge_len, pr20 + p_bridge_len, p_bottom_center + p_bridge_len);
                CADctrl_frame.UserDrawPrism(section_tri);
                section_tri = new CADView.CADPrism(pr19, pr20, p_bottom_center, pr19 + p_bridge_len, pr20 + p_bridge_len, p_bottom_center + p_bridge_len);
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
