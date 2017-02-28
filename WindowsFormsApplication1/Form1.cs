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
    public partial class RawFind : Form
    {
        static string JPG_PATH;
        static string RAW_FILE_EXTENSIOM;
        static int JPG_COUNT;
        static string SEARCH_RAW_PATH;
        static string FINAL_RESULT_PATH;
        static int PROCESS_COUNT;
        static List<string> COPY_LIST = new List<string>();
        static StringBuilder LOG_INFO = new StringBuilder();

        public RawFind()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
            AppendLogInfo("======Begin process======");
            JPG_PATH = textBox1.Text.ToString();
            SEARCH_RAW_PATH = textBox2.Text.ToString();
            FINAL_RESULT_PATH = textBox3.Text.ToString();
            RAW_FILE_EXTENSIOM = comboBox1.Text.ToString();
            PROCESS_COUNT = 0;
            JPG_COUNT = 0;

            var list = GetList();
            JPG_COUNT = list.Count;
            AppendLogInfo(JPG_COUNT.ToString());


            AppendLogInfo("JPG Count:" + JPG_COUNT);
            for (int i = 0; i < list.Count; i++)
            {
                AppendLogInfo(list[i].ToString());
                //find file
                //路径名
                string DirName = SEARCH_RAW_PATH;
                //文件中包含名
                string FileName = list[i].ToString();
                //处理程序(遍历查找+文件复制)
                ProcessFiles(FileName);
                AppendLogInfo("PROCESS_COUNT:" + PROCESS_COUNT + "  JPG_COUNT:" + JPG_COUNT);

                //计算百分比
                double percent = (double)PROCESS_COUNT / JPG_COUNT;
                string percentText = percent.ToString("0.0%");//最后percentText的值为10.0%
                AppendLogInfo("Process:" + percentText);
            }

            AppendLogInfo("PROCESS_COUNT:" + PROCESS_COUNT);
            CopyJPGFiles(list);
            AppendLogInfo("======End process======");
            AppendLogInfo(LOG_INFO.ToString());



        }

        private void AppendLogInfo(String info)
        {
            LOG_INFO.Append(info + "\r\n");
            rtbLog.ForeColor = Color.Green;
            rtbLog.Text = LOG_INFO.ToString();
        }

        private List<string> GetList()
        {
            AppendLogInfo("Begin getlist");
            List<string> list = new List<string>();
            string path = JPG_PATH;
            DirectoryInfo folder = new DirectoryInfo(path);

            foreach (FileInfo file in folder.GetFiles())
            {
                list.Add(file.Name.ToUpper().Replace("JPG", RAW_FILE_EXTENSIOM));
            }
            return list;
        }


        private void GetFileName(string DirName, string FileName)
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
                //if (fname.IndexOf(FileName) > -1)
                if (fname == FileName)
                {
                    AppendLogInfo("Find! " + finfo[i].FullName);
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
        private void CopyFile(string fileFullName, string fileName)
        {
            string destPath = FINAL_RESULT_PATH + @"\" + fileName;
            if (File.Exists(destPath))
            {
                AppendLogInfo("File Exists! " + destPath);
            }
            else
            {
                System.IO.File.Copy(fileFullName, destPath);
                AppendLogInfo("Copy File Success! " + destPath);
            }
            MarkFile(fileName);
        }
        /// <summary>
        /// 执行遍历处理程序并计数
        /// </summary>
        /// <param name="FileName"></param>
        private void ProcessFiles(string FileName)
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
        private void CopyJPGFiles(List<string> searchList)
        {
            AppendLogInfo("-------");
            List<string> jpg_need_copy_list = new List<string>();
            for (int i = 0; i < COPY_LIST.Count; i++)
            {
                AppendLogInfo("COPY_LIST:" + COPY_LIST[i].ToString());
                if (searchList.Contains(COPY_LIST[i].ToString().ToUpper()))
                {
                    searchList.Remove(COPY_LIST[i].ToString().ToUpper());
                }
            }


            for (int i = 0; i < searchList.Count; i++)
            {
                AppendLogInfo("jpg_need_copy_list:" + searchList[i].ToString());
                CopyFile(JPG_PATH + @"\" + searchList[i].ToString().Replace(RAW_FILE_EXTENSIOM, "JPG"), searchList[i].ToString().Replace(RAW_FILE_EXTENSIOM, "JPG"));
            }

            AppendLogInfo("-------");
        }

        private void rtbLog_TextChanged(object sender, EventArgs e)
        {
            //将光标位置设置到当前内容的末尾
            rtbLog.SelectionStart = rtbLog.Text.Length;
            //滚动到光标位置
            rtbLog.ScrollToCaret();
        }
        

        // 文件路径选择Begin

        private void btnSelect1_Click(object sender, EventArgs e)
        {
            textBox1.Text = GetSelectedPath();
        }

        private void btnSelect2_Click(object sender, EventArgs e)
        {
            textBox2.Text = GetSelectedPath();
        }

        private void btnSelect3_Click(object sender, EventArgs e)
        {
            textBox3.Text = GetSelectedPath();
        }

        private String GetSelectedPath()
        {
            string defaultPath = "";
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            //打开的文件夹浏览对话框上的描述  
            dialog.Description = "请选择一个文件夹";
            //是否显示对话框左下角 新建文件夹 按钮，默认为 true  
            dialog.ShowNewFolderButton = false;
            //首次defaultPath为空，按FolderBrowserDialog默认设置（即桌面）选择  
            if (defaultPath != "")
            {
                //设置此次默认目录为上一次选中目录  
                dialog.SelectedPath = defaultPath;
            }
            //按下确定选择的按钮  
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //记录选中的目录  
                defaultPath = dialog.SelectedPath;
            }
            return defaultPath;
        }

        // 文件路径选择End
    }
}
