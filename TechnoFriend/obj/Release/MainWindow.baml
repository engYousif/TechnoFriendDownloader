   M S B A M L   `   `   `  ���� G  CTechnoFriend, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:TechnoFriendTechnoFriend  X TXceed.Wpf.Toolkit, Version=3.6.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4P:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.ToolkitXceed.Wpf.Toolkit     TechnoFriend.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:TechnoFriend     Ntoolkit:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.Toolkit     c _Microsoft.Expression.Drawing, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35=ed4http://schemas.microsoft.com/expression/2010/drawing  -   $��window1��  � WindowStartupLocation$  CenterScreen=� � 
ResizeMode$ NoResize=� � Title$ TechnoFriend��$	��380��#��� }� � WindowStyle$
 None=� � 	Resources    ButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType	 �� Triggers
 P� Property P� Value��%�   &	        �� " ��.���  � ��
 �������  � �� �� " ��.R��� #��z��._��� $��Center=�$��Center=�	 P� 
 �@J�IsMouseOver . . ���  � ��
 �����.��� '  !System.Windows.VisualStateManager   VisualStateGroups %  System.Windows.VisualStateGroup  -   VisualStateGroup�� � ColumnDefinitions �� .+��� $��Stretch=�$��Stretch=�.+��  ��  � RowDefinitions �� $��1* ��� $��2* ��� $��5* �.+�� $��0��
�������.+��� $��0��$��50��$
��Left=� �� Orientation$ 
Horizontal=�.+��� $��/Images/logo.png��$��10,0,0,0q� �� BitmapScalingMode$ HighQuality=�$��40��$��50���� $��TechnoFriend��$��Center=�
�������$��Calibri)�$��30(�$��10,0,0,0q��� -   $��/Images/min.png��$��30��$��120,-1,0,29.6q��� $ 
Horizontal=�$	��Top=�$
��10 0q�$��1��$��50��.+��� -   
 J� Uid$ 0��#��[  $��70��$��Status��$��50�� � Cursor$
 Hand�����	#FF2196F3��Z� ��Z� �� -   $ 1��$��70��$��Define��$��50��$
 Hand��#��[  ���	#FF2196F3��Z� ��Z� �� -   $ 2��$��70��$��Settings��#��[  $��50��$
 Hand�����	#FF2196F3��Z� ��Z� �� -   $ 3��$��70��$��	Schedular��#��[  $��50��$
 Hand�����	#FF2196F3��Z� ��Z� �� -   $��50,0,0,0q�$��/Images/power.png��$��30��$��30��$ HighQuality=�$
 Hand��� -	    �Name$ tabline��$��70��$��5��$��1�����	#FF2196F3$
��Left=�$��10,10,10,-20q��� -
   $��
tabcontrol��$��2������ *Height+ �� ElementName$ window1��  �� CornerRadius ��Name �� 
TargetName��%V  '��       �� " ��.���  � �� �� " ��.R��� $��Border��$��1,1,1,0q�
�������$ 4,4,0,0��$	��2,0q�._��� $ ContentSite��$��Center=�$��Center=�$��Header��$
��10,2q�	 P� 
 �@��
IsSelected . . ��� $ Border�� � ��
 ��·�P� 
 �@��
IsSelected .  . ��� $ Border�� � ��
 �����.@��� -   $��	statustab��$ 0��$	��Top=�.��� $��0,0,-0.4,123q� �� -    �Name$
 col0�� �� $��1* ��� -   $ lrow1������ *Height+$ window1��.+��� -   $��status_list��$��1��$��0q�$��	0,0,0,130q����� .R��� $ Vertical=����� .R��� $��_sborder��$��1q�$ 10��
���   �$	��330��._��� $ Vertical=�$��10q�.+��  �� ��  �� �� �� �� �� �� �� �� .+��� $��Server Name: ��$��0,0,0,0q�$��0��$��0���� $��Port: ��$��1��$��0��$��0,0,0,0q��� $��Server: ��$��2��$��0��$��0,0,0,0q��� $��File Name: ��$��3��$��0��$��0,0,0,0q��� $��File Size: ��$��4��$��0��$��0,0,0,0q��� $��Speed: ��$��5��$��0��$��0,0,0,0q�� $
��pbar��-   $��1q�$��10��$��6��$��0��$��2�� � Value �� *
progress+ �� Mode$ TwoWay=����� *Width+$ statustoolbar���� $��	srvr_name��$	��150��$��0��$��1��$��25��$��0,0,0,0q����� *Server_Name+$ OneWay=��� $	��prt��$��25��$��1��$��1��$	��150��$��0,0,0,0q����� *Port+$ OneWay=��� $
��srvr��$	��150��$��2��$��1��$��25��$��0,0,0,0q����� *Server+$ OneWay=��� $��file_pth��$	��150��$��25��$��3��$��1��$��0,0,0,0q����� *	File_Path+$ OneWay=��� $ 
Horizontal=�$��4��$��1��.+��� $��	file_size��$��25��$��0,0,0,0q����� *	File_Size+$ OneWay=��� $��-5,0,0,0q����� *Unit+$ OneWay=��� $��5��$ 
Horizontal=�$��1��.+��� $��
speedlabel������ *Speed+$ OneWay=��� $
��kbps��$��0,0,0,0q�c� $��statustoolbar��
���   � `� IsLocked . $��35��$��0��$��7��$��2��$��0,0,0,0q�.@��� -   $
��Stop��$��10,0,0,0q�
�������$	��Top=�
�������{��� *	isStarted+$ TwoWay=��� -   $��Start��
�������
�������$��10,0,0,0q�{��� *
!isStarted+$ TwoWay=��� -   $��Delete��
�������
�������$��10,0,0,0q��� -   $��	definetab��$ 1��.��� 
�������$��	0,0,0.4,0q� ��  �� -   $
 row1��$��2* ��� $	��29* �.+��� $��1��$
��Left=��� -   $��define_list��$��0q�$��	0,0,0,130q�$��1������ .R��� $ Vertical=����� .R��� $��borderc��$��1q�
���   �$	��340��._��� $ Vertical=�.+��� $ 
Horizontal=�.+��� $ Vertical=�$��10q�.+��� $��Server Name: ��$��0,0,0,0q�$��30���� $��Port: ��$��30��$��0,0,0,0q��� $��Server: ��$��30��$��0,0,0,0q��� $��File Path: ��$��30��$��0,0,0,0q��� $��
Username: ��$��30��$��0,0,0,0q��� $��
Password: ��$��30��$��0,0,0,0q��� $��Speed: ��$��30��$��0,0,0,0q��� $ Vertical=�$��10q�.+��� $��server_name��-   $	��150��$��20��$��0,0,0,10q����� *Server_Name+$ TwoWay=��� $
��port��-   $��20��$	��150��{�.  $��0,0,0,10q����� *Port+$ TwoWay=��� $��server��-   $	��150��$��20��$��0,0,0,10q����� *Server+$ TwoWay=��� $��	file_path��-   $	��150��$��eBackup.zip��{�.  $��20��$��0,0,0,10q��� $��username��-   $	��150��$��20��$��0,0,0,10q����� *
Username+$ TwoWay=���  ��Name$ password��-   $	��150��
������ $��20��$��0,0,0,10q����� *
Password+$ TwoWay=��� $ 
Horizontal=�.+��� $ speed��-   $	��110��$��20������ *Speed+$ TwoWay=��� $
��Kbps���� .+�c� 
���   � . $��35��$��0,0,0,0q�.@��� -   $��Delete��
�������
�������$��10,0,0,0q��� -   $	��Run��
�������
�������$��10,0,0,0q�c� $
��Left=�$��1��$��0��$��1�� . $	��Top=�$	��375������ *Height+$
 row1��.@��� -   $��25��$��25��$��0,-1,0,-1.6q�$��Bottom=�����  �� ImageSource$ /Images/add.png�� �� EdgeMode$ Aliased=�$ HighQuality=��� $��	Set Speed��$��20,0,0,0q��� -    $��speedTextBox��$��50���� -!   $��	noofitems��$��0��$��20,0,0,0q�$	��Top=��� $��	Downloads��$��Center=��� -"   $��settingstab��$ 2��.��� 
������� �� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* �.+��� $ Vertical=�$��10q�.+��� $ 
Horizontal=�$	��Top=�.+��� $��Output Folder: ��$��Center=��� $ 
Horizontal=�.+��� -#   $
��path��$	��250�� �� TextAlignment$
 Left=�$��Center=�$�� ���� -$   $��Browse..��$��25��$��50���� -%   $��schedtab��.���  �� �� �� �� ��  �� �� .+��� -&   $��	selection��$��	Selection��{�.  $��10(�$��	10,20,0,0q�.���� $
��Left=�.+��� -'   $��
schedcombo��$��10,10,10,10q�$	��120��$��17��.@��� .���� $��First���� .���� $
��Last���� .���� $
��From���� $ 
Horizontal=�$��10,0,0,0q�.+��� -(   $��fromtb��$��From ��$��10(��� -)   $��fromtx��$�� ��$��40���� -*   $
��totb��$
�� To ��$��10(��� -+   $
��totx��$�� ��$��40���� $��1��$��2��$��
10,20,10,0q�.+��� $��0��$��1������ -,   $��
overridecb��$��Override Speed��$��10(�.���� $��0,10,0,0q�$
��Left=�$ 
Horizontal=�.+��� --   $��overridespeed��$	��130��{�.  �� $�� Kbps��$��10(��� $��Timing��$��10(�$��0,6,0,0q�.��� $��0q� �� $��9* ���  �� $��1* ��� $��1* ��� $��1* �.+��� -.   $��checkpic��$��/Images/check.png��$��15��$��1��$��0��$��13��$y�Hidden=��� -/   $��datecb��$��On Datetime��$��10(�$��0,10,0,0q�$��0��$��0���� $ 
Horizontal=�$��0��$��1��$��0,10,0,0q�.+��� $
��Date��$��10(�(  "System.Windows.Controls.DatePicker  -0   $��	dateofrun��$��10(�{�.  $��Center=�$��Center=�$��	10,-3,0,0q�$	��100��$��20���� $ 
Horizontal=�$��0��$��2��$��0,5,0,0q�.+��� $
��Time��"  Xceed.Wpf.Toolkit.TimePicker  -1   $��	timeofrun��$��	10,0,0,10q�$��10(�$	��100��{�.  $��17���� $ Vertical=�$	��360��$��1��$��10q�$��2��$��0��$��2��.+��� -2   $��	selectall��$��
Select All��$��10(�$��
0,0,-0.4,0q��� -3   $��tasksselector��$	��290��$��0,10,0,0q�$��1��$��2��$��0��$��2������ .R��� $ Vertical=����� .R��� $ 
Horizontal=�.+��� ���� *Server_Name+$ OneWay=�  d� 	IsChecked  �� *	isChecked+$ TwoWay=��� $��50,0,0,0q����� ! �� Path$! progress�$ TwoWay=��� $��%���� $��
85,100,0,0q�$ 
Horizontal=�$��3��$��20��$��2��.+��� -4   $��startButton��$��Start��$	��100���� -5   $��
stopButton��$
��Stop��{�.  $��5,0,0,0q�$	��100��