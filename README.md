# Ansys-builder



- 通过WPF应用创建apdl模型脚本

- 2017/11/14 22:08:59 完成CADCtrl控件动态链接库方式引用

###### 遇到的问题：
1.  我们在写WPF控件的时候，经常会需要在xaml文件添加一些引用, 如System.Windows.Forms引用，但是在xaml文件没有提供类似于 using System.Windows.Forms; 的语法， 因此我们需要通过 xmlns关键字来添加引用。具体实现如下：
 - xmlns:wf="clr-namespace:System.Windows.Forms; assembly=System.Windows.Forms"
 - 其中：
   wf为引用别名，在xaml文件中，可以用wf来代替该引用；	clr-namespace:引用名字; assembly=引用对应的DLL名字