   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  H DAnsys_builder, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Ansys_builderAnsys_builder I2clr-namespace:System.Collections;assembly=mscorlibSystem.Collections  B >CADCtrl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null2&clr-namespace:CADCtrl;assembly=CADCtrlCADCtrl    Ansys_builder.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     7core&clr-namespace:System;assembly=mscorlib     -localclr-namespace:Ansys_builder     Bcol2clr-namespace:System.Collections;assembly=mscorlib     6CAD&clr-namespace:CADCtrl;assembly=CADCtrl     -     � Title$&   ANSYS等截面梁桥建模助手��$	��800��$
��1000��.��� .+�  CADCtrl.CADView  -     Name$ CADctrl_frame��$
��Left=�$	��Top=�$	��466��$	��663��$��	315,0,0,0q��� -   $��label_bri_len��$��桥长��$��28��$
��Left=�$��	52,25,0,0q�$	��Top=��� -   $��textBox_bri_len��$��23��$
��Left=�$��	92,30,0,0q�$	��Top=�$��60���� -   $��label_bri_len_unit��$��m��$��28��$
��Left=�$��
163,25,0,0q�$	��Top=��� -   $��label_base_height��$��墩高��$��28��$
��Left=�$��
206,25,0,0q�$	��Top=��� -   $��textBox_base_height_unit��$��23��$
��Left=�$��
246,30,0,0q�$	��Top=�$��60���� -   $��label_base_height_unit��$��m��$��28��$
��Left=�$��
316,25,0,0q�$	��Top=��� -	   $��label_span_sep��$��分跨��$��28��$
��Left=�$��	52,59,0,0q�$	��Top=��� -
   $��textBox_span_sep��$��23��$
��Left=�$��	92,64,0,0q�$	��Top=�$	��214���� -   $��label_span_unit��$��m��$��28��$
��Left=�$��
315,60,0,0q�$	��Top=��� -   $��label_base_style��$��桥墩分布形式��$��28��$
��Left=�$��	52,93,0,0q�$	��Top=��� -   $��comboBox_base_style��$��23��$
��Left=�$��
148,95,0,0q�$	��Top=�$��92��.@��� $��单墩�� � DataContext �� �� $��双墩顺桥���� $��双墩横桥���� -   $��btn_section��$��定义箱梁截面��$��30��$
��Left=�$��190,338,0,0q�$	��Top=�$	��100���� -   $��label2��$��双墩间距��$��28��$
��Left=�$��
53,162,0,0q�$	��Top=��� -   $��text_base_dist��$��23��$
��Left=�$��120,164,0,0q�$	��Top=�$��79��{�. �� -   $��	groupBox1��$��桥墩尺寸��$	��100��$
��Left=�$��
53,203,0,0q�$	��Top=�$	��237��.���  � ColumnDefinitions �� $	��36* ��� $
��152* �.+��� -   $��checkBox_rect_base��$��	矩形墩��$��19��$
��Left=�$��7,19,0,0q�$	��Top=�$��64��$��2���� -   $��checkBox_cir_base��$��	圆形墩��$��18��$
��Left=�$��7,53,0,0q�$	��Top=�$��64��$��2���� 
���� K�$��0��$��1q�$��
34,16,0,35q�$
��Left=�$	��132��$��1��.�� � 	Resources    	HelpBrush j� TileMode �� Opacity j� Stretch j� 
AlignmentX	 !� Visual��%r   &	        !� $
 None=�$	 0.3Q�$
 None=�$
 Left=�	 �� $��Italic$�$!��顺桥长 , 横桥长 , 高��.������ 
 �� 
TargetType"
 �� �� Triggers P�  P� Property � �� P� Value Z� . ���  �� Property � �� �� Value# [  P�  � ��$  ��. ���  � ��# [  .��