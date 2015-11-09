using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        static string JPG_PATH;
        static string RAW_FILE_EXTENSIOM;
        static int JPG_COUNT;
        static string SEARCH_RAW_PATH;
        static string FINAL_RESULT_PATH;
        static int PROCESS_COUNT;
        static List<string> COPY_LIST = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string txt = textBox1.Text.ToString();
            ////MessageBox.Show(txt);

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btn_process_Click(object sender, EventArgs e)
        {
            JPG_PATH = textBox1.Text.ToString();
            SEARCH_RAW_PATH = textBox2.Text.ToString();
            FINAL_RESULT_PATH = textBox3.Text.ToString();
            RAW_FILE_EXTENSIOM = comboBox1.Text.ToString();
            PROCESS_COUNT = 0;
            JPG_COUNT = 0;



            var list = GetList();
            JPG_COUNT = list.Count;
            //MessageBox.Show(JPG_COUNT.ToString());


            //MessageBox.Show("JPG Count:" + JPG_COUNT);
            for (int i = 0; i < list.Count; i++)
            {
                //MessageBox.Show(list[i].ToString());
                //find file
                //路径名
                string DirName = SEARCH_RAW_PATH;
                //文件中包含名
                string FileName = list[i].ToString();
                //处理程序(遍历查找+文件复制)
                ProcessFiles(FileName);
                ////MessageBox.Show("PROCESS_COUNT:" + PROCESS_COUNT + "  JPG_COUNT:" + JPG_COUNT);

                //计算百分比
                double percent = (double)PROCESS_COUNT / JPG_COUNT;
                string percentText = percent.ToString("0.0%");//最后percentText的值为10.0%
                //MessageBox.Show("Process:" + percentText);
            }

            //MessageBox.Show("PROCESS_COUNT:" + PROCESS_COUNT);
            CopyJPGFiles(list);
            //MessageBox.Show("======End process======");




        }

        private List<string> GetList()
        {
            ////MessageBox.Show("Begin getlist");
            List<string> list = new List<string>();
            string path = JPG_PATH;
            DirectoryInfo folder = new DirectoryInfo(path);

            foreach (FileInfo file in folder.GetFiles())
            {
                list.Add(file.Name.ToUpper().Replace("JPG", RAW_FILE_EXTENSIOM));
            }
            return list;
        }


        static void GetFileName(string DirName, string FileName)
        {
            //文件夹信息
            DirectoryInfo dir = new DirectoryInfo(DirName);
            //如果非根路径且是系统文件夹则跳过
            if (null != dir.Parent && dir.Attributes.ToString().IndexOf("System") > -1)
            {
                return;
            }
            //取得所有文件
            FileInfo[] finfo = dir.GetFiles();
            string fname = string.Empty;
            for (int i = 0; i < finfo.Length; i++)
            {
                fname = finfo[i].Name.ToUpper();
                //判断文件是否包含查询名
                if (fname.IndexOf(FileName) > -1)
                {
                    //MessageBox.Show("Find! " + finfo[i].FullName);
                    CopyFile(finfo[i].FullName, finfo[i].Name);
                }
            }
            //取得所有子文件夹
            DirectoryInfo[] dinfo = dir.GetDirectories();
            for (int i = 0; i < dinfo.Length; i++)
            {
                //查找子文件夹中是否有符合要求的文件
                GetFileName(dinfo[i].FullName, FileName);
            }


        }

        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <param name="fileName"></param>
        static void CopyFile(string fileFullName, string fileName)
        {
            string destPath = FINAL_RESULT_PATH + @"\" + fileName;
            if (File.Exists(destPath))
            {
                //MessageBox.Show("File Exists! " + destPath);
            }
            else
            {
                System.IO.File.Copy(fileFullName, destPath);
                //MessageBox.Show("Copy File Success! " + destPath);
            }
            MarkFile(fileName);
        }
        /// <summary>
        /// 执行遍历处理程序并计数
        /// </summary>
        /// <param name="FileName"></param>
        static void ProcessFiles(string FileName)
        {
            PROCESS_COUNT++;
            string DirName = SEARCH_RAW_PATH;
            GetFileName(DirName, FileName);
        }

        /// <summary>
        /// 标记已经搜索到的文件
        /// </summary>
        /// <param name="name"></param>
        static void MarkFile(string name)
        {
            COPY_LIST.Add(name);
        }

        /// <summary>
        /// 将未找到raw的jpg复制到结果集中
        /// </summary>
        static void CopyJPGFiles(List<string> searchList)
        {
            //MessageBox.Show("-------");
            List<string> jpg_need_copy_list = new List<string>();
            for (int i = 0; i < COPY_LIST.Count; i++)
            {
                //MessageBox.Show("COPY_LIST:" + COPY_LIST[i].ToString());
                if (searchList.Contains(COPY_LIST[i].ToString()))
                {
                    searchList.Remove(COPY_LIST[i].ToString());
                }
            }


            for (int i = 0; i < searchList.Count; i++)
            {
                //MessageBox.Show("jpg_need_copy_list:" + searchList[i].ToString());
                CopyFile(JPG_PATH + @"\" + searchList[i].ToString().Replace(RAW_FILE_EXTENSIOM, "JPG"), searchList[i].ToString().Replace(RAW_FILE_EXTENSIOM, "JPG"));
            }

            //MessageBox.Show("-------");
        }

    }
}
