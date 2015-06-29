using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;


namespace TranslateApp
{
    public static class FullScreenHelper
    {
        private static Window _fullWindow;
        private static WindowState _windowState;
        private static WindowStyle _windowStyle;
        private static bool _windowTopMost;
        private static ResizeMode _windowResizeMode;
        private static Rect _windowRect;

        /// <summary>
        /// ����ȫ��
        /// </summary>
        /// <param name="window"></param>
        public static void GoFullscreen(this Window window)
        {
            //�Ѿ���ȫ��
            if(window.IsFullscreen()) return;

            //�洢������Ϣ
            _windowState = window.WindowState;
            _windowStyle = window.WindowStyle;
            _windowTopMost = window.Topmost;
            _windowResizeMode = window.ResizeMode;
            _windowRect.X = window.Left;
            _windowRect.Y = window.Top;
            _windowRect.Height = window.Width;
            _windowRect.Height = window.Height;


            //����ޱߴ���
            window.WindowState = WindowState.Normal;//�����Ѿ���Maximized���Ͳ��ܽ���ȫ�������������ȵ���״̬
            window.WindowStyle = WindowStyle.None;
            window.ResizeMode = ResizeMode.NoResize;
            window.Topmost = true;//��󻯺�������������

            //��ȡ���ھ�� 
            var handle = new WindowInteropHelper(window).Handle;

            //��ȡ��ǰ��ʾ����Ļ
            Screen screen = Screen.FromHandle(handle);

            //�����������,ȫ���Ĺؼ������������3��
            window.MaxWidth = screen.Bounds.Width;
            window.MaxHeight = screen.Bounds.Height;
            window.WindowState = WindowState.Maximized;
           

            //����л�Ӧ�ó��������
            window.Activated += new EventHandler(window_Activated);
            window.Deactivated += new EventHandler(window_Deactivated);

            //��ס�ɹ���󻯵Ĵ���
            _fullWindow = window;
        }

        static void window_Deactivated(object sender, EventArgs e)
        {
            var window = sender as Window;
            window.Topmost = false;
        }

        static void window_Activated(object sender, EventArgs e)
        {
            var window = sender as Window;
            window.Topmost = true;
        }

        /// <summary>
        /// �˳�ȫ��
        /// </summary>
        /// <param name="window"></param>
        public static void ExitFullscreen(this Window window)
        {
            //�Ѿ�����ȫ���޲���
            if (!window.IsFullscreen()) return;

            //�ָ�������ǰ��Ϣ���������˳���ȫ��
            window.Topmost = _windowTopMost;
            window.WindowStyle = _windowStyle;

            window.ResizeMode = ResizeMode.CanResize;//����Ϊ�ɵ��������С
            window.Left = _windowRect.Left;
            window.Width = _windowRect.Width;
            window.Top = _windowRect.Top;
            window.Height = _windowRect.Height;

            window.WindowState = _windowState;//�ָ�����״̬��Ϣ

            window.ResizeMode = _windowResizeMode;//�ָ����ڿɵ�����Ϣ

            //�Ƴ�����Ҫ���¼�
            window.Activated -= window_Activated;
            window.Deactivated -= window_Deactivated;

            _fullWindow = null;
        }

        /// <summary>
        /// �����Ƿ���ȫ��״̬
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static bool IsFullscreen(this Window window)
        {
            if(window==null)
            {
                throw new ArgumentNullException("window");
            }
            return _fullWindow == window;
        }
    }
}