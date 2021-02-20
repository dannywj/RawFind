using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RawFindDesktop
{
    public partial class RawFind : Form
    {
        static string VERSION = "3.0";
        static string JPG_PATH;
        static string RAW_FILE_EXTENSIOM = "NEF";
        static int JPG_COUNT;
        static string SEARCH_RAW_PATH;
        static string FINAL_RESULT_PATH;
        static int PROCESS_COUNT;
        static List<string> COPY_LIST = new List<string>();
        static StringBuilder LOG_INFO = new StringBuilder();

        static string PIC_RESIZE_PATH;
        static string PIC_RESIZE_OUTPUT_PATH;
        public static ArrayList processlist = new ArrayList();

        static string SYNC_JPG_PATH;
        static string SYNC_RAW_PATH;
        //第一步：定义BackgroundWorker对象，并注册事件（执行线程主体、执行UI更新事件）
        private BackgroundWorker backgroundWorker_resizer = null;

        // ver3.0
        static string PREFIX_JPG_ALL = "jpg_all_";
        static string PREFIX_RAW_ALL = "raw_all_";
        static string PREFIX_RAW_SELECTED = "raw_selected_";
        static string PREFIX_PS = "ps_";
        static string BACKUP_NAME_CONTAIN;
        static string BACKUP_PATH;
        private BackgroundWorker bgWorker = new BackgroundWorker();

        static StringBuilder BACKUP_LOG = new StringBuilder();
        static StringBuilder BACKUP_REPORT = new StringBuilder();
        private BackgroundWorker bgBackWorker = new BackgroundWorker();

        public RawFind()
        {
            InitializeComponent();
            initForm();

            //多线程处理照片缩放
            backgroundWorker_resizer = new BackgroundWorker();
            //设置报告进度更新
            backgroundWorker_resizer.WorkerReportsProgress = true;
            //注册线程主体方法
            backgroundWorker_resizer.DoWork += new DoWorkEventHandler(backgroundWorker_resizer_DoWork);
            //注册更新UI方法
            backgroundWorker_resizer.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_resizer_ProgressChanged);
            //backgroundWorker_resizer.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker_resizer_RunWorkerCompleted);


            this.PrograssBarUseBackgroundWorker();
        }

        private void RawFind_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        #region   oldversion
        private void initForm()
        {
            SetBtnStyle(this.btn_process);
            SetBtnStyle(this.btnResize);
            SetBtnStyle(this.btnSync);
            //default selected 1500px
            comboBox2.SelectedIndex = 2;
            //改变窗体风格，使之不能用鼠标拖拽改变大小
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //禁止使用最大化按钮
            this.MaximizeBox = false;
            this.toolStripStatusLabel.Text = "Copyright © 2021 Develop by Danny Wang    Version:" + VERSION;
        }

        //线程主体方法
        public void backgroundWorker_resizer_DoWork(object sender, DoWorkEventArgs e)
        {
            //...执行线程任务
            AppendLogInfo("=====================begin resize======================");
            //btnResize.Enabled = false;// 线程任务内无法执行控件UI

            //PIC_RESIZE_PATH = "E:\\pic";
            //PIC_RESIZE_OUTPUT_PATH = "E:\\re";
            PIC_RESIZE_PATH = textBox4.Text.ToString();
            PIC_RESIZE_OUTPUT_PATH = textBox5.Text.ToString();
            if (String.IsNullOrEmpty(PIC_RESIZE_PATH) || String.IsNullOrEmpty(PIC_RESIZE_OUTPUT_PATH))
            {
                //MessageBox.Show("All Path can not empty");
                AppendLogInfo("All Path can not empty");
                //btnResize.Enabled = true;
                return;
            }
            AppendLogInfo("PIC_RESIZE_PATH:" + PIC_RESIZE_PATH);
            AppendLogInfo("Resize px:" + int.Parse(comboBox2.Text.Replace("px", "")).ToString());
            AppendLogInfo("PIC_RESIZE_OUTPUT_PATH:" + PIC_RESIZE_OUTPUT_PATH);

            ProcessFile(PIC_RESIZE_PATH, int.Parse(comboBox2.Text.Replace("px", "")), PIC_RESIZE_OUTPUT_PATH, true);

            AppendLogInfo("=====================end resize======================");
            MessageBox.Show("操作完成");
            btnResize.Enabled = true;
        }

        //UI更新方法
        public void backgroundWorker_resizer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            rtbLog.ForeColor = Color.Green;
            rtbLog.Text = LOG_INFO.ToString();
        }

        private void SetBtnStyle(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;//样式
            btn.ForeColor = Color.Transparent;//前景
            btn.BackColor = Color.Transparent;//去背景
            btn.FlatAppearance.BorderSize = 0;//去边线
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;//鼠标经过
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;//鼠标按下
        }

        #region RAW Finder
        /// <summary>
        /// 执行主入口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_process_Click(object sender, EventArgs e)
        {
            this.progressBar.Value = 0;
            //this.progressBar.Maximum = 200;
            this.progressBar.Step = 1;

            LOG_INFO = new StringBuilder();
            AppendLogInfo("======Begin process======");
            JPG_PATH = textBox1.Text.ToString();
            SEARCH_RAW_PATH = textBox2.Text.ToString();
            FINAL_RESULT_PATH = textBox3.Text.ToString();
            RAW_FILE_EXTENSIOM = comboBox1.Text.ToString();
            PROCESS_COUNT = 0;
            JPG_COUNT = 0;
            if (String.IsNullOrEmpty(JPG_PATH) || String.IsNullOrEmpty(SEARCH_RAW_PATH) || String.IsNullOrEmpty(FINAL_RESULT_PATH))
            {
                MessageBox.Show("All Path can not empty");
                AppendLogInfo("All Path can not empty");
                return;
            }

            var list = GetList();

            //MessageBox.Show("50");
            JPG_COUNT = list.Count;
            AppendLogInfo("JPG Count:" + JPG_COUNT);
            if (JPG_COUNT > 0)
            {
                this.progressBar.Maximum = list.Count;

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
                    AppendLogInfo("double Process:" + percent);
                    double tmp = percent * 100;
                    if (tmp < 1)
                    {
                        tmp = 1;
                    }
                    this.progressBar.Value = PROCESS_COUNT;

                    string percentText = percent.ToString("0.0%");//最后percentText的值为10.0%
                    AppendLogInfo("Process:" + percentText);
                }

                AppendLogInfo("PROCESS_COUNT:" + PROCESS_COUNT);
                CopyJPGFiles(list);
            }
            else
            {
                AppendLogInfo("not find valid jpg files");
            }

            AppendLogInfo("======End process======");
            MessageBox.Show("操作完成");
        }

        #region FilesOperation

        private List<string> GetList()
        {
            AppendLogInfo("Begin getlist");
            List<string> list = new List<string>();
            string path = JPG_PATH;
            DirectoryInfo folder = new DirectoryInfo(path);

            foreach (FileInfo file in folder.GetFiles())
            {
                var ext_name = Path.GetExtension(file.Name).ToUpper();
                if (ext_name == ".JPG")
                {
                    list.Add(file.Name.ToUpper().Replace("JPG", RAW_FILE_EXTENSIOM));
                }
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
                File.Delete(fileFullName);
                AppendLogInfo("Delete Raw Success! " + destPath);
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

        #endregion

        #region BindSelectButton
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
        #endregion

        #region Others
        private void rtbLog_TextChanged(object sender, EventArgs e)
        {
            //将光标位置设置到当前内容的末尾
            rtbLog.SelectionStart = rtbLog.Text.Length;
            //滚动到光标位置
            rtbLog.ScrollToCaret();
        }

        private void AppendLogInfo(String info)
        {
            LOG_INFO.Append(info + "\r\n");
            //在线程中更新UI（通过ReportProgress方法）
            backgroundWorker_resizer.ReportProgress(1, null);
            //rtbLog.ForeColor = Color.Green;
            //rtbLog.Text = LOG_INFO.ToString();
        }

        private void btn_process_MouseHover(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.FlatAppearance.BorderSize = 1;
        }

        private void btn_process_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.FlatAppearance.BorderSize = 0;
        }
        #endregion

        #endregion

        #region Resizer

        /// <summary>
        /// 照片缩放入口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResize_Click(object sender, EventArgs e)
        {

            //启动多线程
            this.backgroundWorker_resizer.RunWorkerAsync();
        }


        /// <summary>
        /// 获取待处理照片列表
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static ArrayList GetPhotoList(string dir, bool processChildFoder)
        {
            try
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    try
                    {
                        if (File.Exists(d))
                        {
                            if (Path.GetExtension(d).ToLower() == ".jpg")
                            {
                                processlist.Add(d);
                            }
                            else
                            {
                                throw new Exception("Not a JPG File.");
                            }
                        }
                        else
                        {
                            // string processChildFoder = //System.Configuration.ConfigurationManager.AppSettings["processChildFoder"].ToString();
                            if (processChildFoder == true)
                            {
                                DirectoryInfo d1 = new DirectoryInfo(d);
                                if (d1.GetFiles().Length != 0)
                                {
                                    GetPhotoList(d1.FullName, processChildFoder);//递归
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return processlist;
        }

        public void ProcessFile(string dir, int maxWidthHeight, string saveUrlPrefix, bool processChildFoder)
        {
            AppendLogInfo("-----begin ProcessFile-----");
            try
            {
                var list = GetPhotoList(dir, processChildFoder);
                if (list.Count == 0)
                {
                    AppendLogInfo("不存在要处理的照片");
                    return;
                }
                this.progressBar.Maximum = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    try
                    {
                        AppendLogInfo("--process current:" + (i + 1) + " file--");
                        string FileName = Path.GetFileName(list[i].ToString());
                        string FileURL = list[i].ToString();
                        string saveUrl = saveUrlPrefix;
                        saveUrl += ("/" + Path.GetFileNameWithoutExtension(list[i].ToString()) + "_" + maxWidthHeight.ToString() + "px_process.jpg");

                        FileInfo fi = new FileInfo(FileURL);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        {
                            fi.Attributes = FileAttributes.Normal;
                        }

                        // 获取照片的Exif信息  
                        AppendLogInfo("begin get exif info");
                        var exif = GetImageProperties(FileURL);
                        AppendLogInfo("get exif info ok");
                        AppendLogInfo("begin MakeThumbnail");
                        MakeThumbnail(FileURL, saveUrl, maxWidthHeight, maxWidthHeight, "EQU", exif);
                        AppendLogInfo("finish MakeThumbnail");
                        AppendLogInfo("--finish current:" + (i + 1) + " file--");
                        //File.Delete(re.FileURL);
                        this.progressBar.Value = i + 1;
                    }
                    catch (Exception ex)
                    {
                        AppendLogInfo("ProcessFile Exception");
                        AppendLogInfo(ex.Message);
                        AppendLogInfo(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLogInfo("ProcessFile Exception2");
                AppendLogInfo(ex.Message);
                AppendLogInfo(ex.StackTrace);
            }
            AppendLogInfo("-----end ProcessFile-----");
        }

        /// <summary>  
        /// 图片缩放  
        /// </summary>  
        /// <param name="originalImagePath">原始图片路径，如：c:\\images\\1.gif</param>  
        /// <param name="thumbnailPath">生成缩略图图片路径，如：c:\\images\\2.gif</param>  
        /// <param name="width">宽</param>  
        /// <param name="height">高</param>  
        /// <param name="mode">EQU：指定最大高宽等比例缩放；HW：//指定高宽缩放（可能变形）；W:指定宽，高按比例；H:指定高，宽按比例；Cut：指定高宽裁减（不变形）</param>  
        public void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode, List<PropertyItem> exif)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            if (mode == "EQU")//指定最大高宽，等比例缩放  
            {
                //if(height/oh>width/ow),如果高比例多，按照宽来缩放；如果宽的比例多，按照高来缩放  
                if (height * ow > width * oh)
                {
                    mode = "W";
                }
                else
                {
                    mode = "H";
                }
            }
            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）  
                    break;
                case "W"://指定宽，高按比例  
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例  
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）  
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片  
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板  
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法  
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度  
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充  
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分  
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);
            try
            {
                // 设置EXIF    
                foreach (PropertyItem pitem in exif)
                {
                    bitmap.SetPropertyItem(pitem);
                }

                //以jpg格式保存缩略图  
                if (!File.Exists(thumbnailPath))
                {
                    bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else
                {
                    AppendLogInfo("File Exists! continue");
                }


            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>  
        /// 获取照片的Exif属性，存储成二进制list  
        /// </summary>  
        /// <param name="FileName"></param>  
        /// <returns></returns>  
        public List<PropertyItem> GetImageProperties(string FileName)
        {
            if (!File.Exists(FileName)) return null;

            List<PropertyItem> rtn = new List<PropertyItem>();

            System.Drawing.Image img = null;

            try
            {
                img = System.Drawing.Image.FromFile(FileName);
                PropertyItem[] pt = img.PropertyItems;

                foreach (PropertyItem p in pt)
                {
                    rtn.Add(p);
                }
            }
            catch
            {
                rtn = null;
            }
            finally
            {
                if (img != null) img.Dispose();
            }

            return rtn;
        }

        private void btnSelect4_Click(object sender, EventArgs e)
        {
            //PIC_RESIZE_PATH = GetSelectedPath();
            textBox4.Text = GetSelectedPath();
            // lbl_resize_path.Text = "缩放路径：" + PIC_RESIZE_PATH;
        }

        private void btnSelect5_Click(object sender, EventArgs e)
        {
            //PIC_RESIZE_OUTPUT_PATH = GetSelectedPath();
            //lbl_output_path.Text = "输出路径：" + PIC_RESIZE_OUTPUT_PATH;
            textBox5.Text = GetSelectedPath();
        }

        private void btnResize_MouseHover(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.FlatAppearance.BorderSize = 1;
        }

        private void btnResize_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.FlatAppearance.BorderSize = 0;
        }
        #endregion

        #region SYNC
        private void btnSync_Click(object sender, EventArgs e)
        {
            SYNC_JPG_PATH = textBox6.Text.ToString();
            SYNC_RAW_PATH = textBox7.Text.ToString();
            if (String.IsNullOrEmpty(SYNC_JPG_PATH) || String.IsNullOrEmpty(SYNC_RAW_PATH))
            {
                MessageBox.Show("All Path can not empty");
                AppendLogInfo("All Path can not empty");
                return;
            }

            AppendLogInfo("Begin get sync file list");
            List<string> jpg_list = new List<string>();
            string path = SYNC_JPG_PATH;
            DirectoryInfo folder = new DirectoryInfo(path);

            foreach (FileInfo file in folder.GetFiles())
            {
                var ext_name = Path.GetExtension(file.Name).ToUpper();
                if (ext_name == ".JPG")
                {
                    jpg_list.Add(file.Name.ToUpper().Replace("JPG", "NEF"));
                }
            }

            List<string> del_list = new List<string>();
            DirectoryInfo folder_raw = new DirectoryInfo(SYNC_RAW_PATH);

            foreach (FileInfo file in folder_raw.GetFiles())
            {
                var ext_name = Path.GetExtension(file.Name).ToUpper();
                if (ext_name == ".NEF")
                {
                    if (!jpg_list.Contains(file.Name.ToUpper()))
                    {
                        del_list.Add(file.Name.ToUpper());
                    }

                }
            }

            foreach (var item in del_list)
            {
                string delpath = SYNC_RAW_PATH + @"\" + item.ToString();
                File.Delete(delpath);
                AppendLogInfo("deleted -> " + item.ToString());
            }
            AppendLogInfo("同步完成");
            MessageBox.Show("同步完成");
        }

        private void btnSelect6_Click(object sender, EventArgs e)
        {
            textBox6.Text = GetSelectedPath();
        }

        private void btnSelect7_Click(object sender, EventArgs e)
        {
            textBox7.Text = GetSelectedPath();
        }
        #endregion

        #endregion

        #region 自动处理功能
        private void btnAutoCreate_Click(object sender, EventArgs e)
        {
            if (bgWorker.IsBusy)
            {
                return;
            }
            bgWorker.RunWorkerAsync("hello");
        }

        private void AutoCreate(object sender, DoWorkEventArgs e)
        {
            //string picAllPath = "C:\\test\\2021-01-01大望京";
            //string picSelectedPath = "C:\\selected";

            string picAllPath = this.textBox8.Text.ToString();
            string picSelectedPath = this.textBox9.Text.ToString();

            var albumNameArr = picAllPath.Split('\\');
            // 专辑名
            var albumName = albumNameArr[albumNameArr.Length - 1];
            string rawAllPath = @picAllPath + "\\" + PREFIX_RAW_ALL + albumName;

            // raw重命名为格式化名称
            if (!Directory.Exists(picAllPath + "\\raw"))
            {
                MessageBox.Show("raw 文件夹未找到");
                return;
            }

            Directory.Move(@picAllPath + "\\raw", rawAllPath);

            // 创建jpg全集文件夹
            string jpgAllPath = picAllPath + "\\" + PREFIX_JPG_ALL + albumName;
            //MessageBox.Show(jpgAllPath);
            if (Directory.Exists(jpgAllPath))
            {
                MessageBox.Show("jpg 全集文件夹已存在");
                return;
            }
            Directory.CreateDirectory(jpgAllPath);

            // 移动当前目录下全部jpg到全集文件夹
            this.moveFiles(picAllPath, jpgAllPath);

            // 同步已删除的jpg和raw
            this.syncJPGAndRaw(jpgAllPath, rawAllPath);
            //MessageBox.Show("同步完成");



            // 创建raw精选文件夹
            string rawSelectedPath = picAllPath + "\\" + PREFIX_RAW_SELECTED + albumName;
            //MessageBox.Show(jpgAllPath);
            if (Directory.Exists(rawSelectedPath))
            {
                MessageBox.Show("raw精选文件夹已存在");
                return;
            }
            Directory.CreateDirectory(rawSelectedPath);


            // 精选jpg的拷贝

            this.selectedCopy(picSelectedPath, rawAllPath, rawSelectedPath);

            //创建精修文件夹
            // 创建raw精选文件夹
            string psPath = picAllPath + "\\" + PREFIX_PS + albumName;
            //MessageBox.Show(jpgAllPath);
            if (Directory.Exists(psPath))
            {
                MessageBox.Show("ps文件夹已存在");
                return;
            }
            Directory.CreateDirectory(psPath);

            MessageBox.Show("处理完成！");
        }

        // 移动jpg文件
        private void moveFiles(string srcFolder, string destFolder)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(srcFolder);
            FileInfo[] files = directoryInfo.GetFiles();

            foreach (FileInfo file in files)
            {
                if (file.Extension.ToUpper() == ".JPG")
                {
                    file.MoveTo(Path.Combine(destFolder, file.Name));
                }
                // will move all files without if stmt
                //file.MoveTo(Path.Combine(destFolder, file.Name));
            }
        }

        private void syncJPGAndRaw(string jpgPath, string rawPath)
        {
            SYNC_JPG_PATH = jpgPath;
            SYNC_RAW_PATH = rawPath;
            if (String.IsNullOrEmpty(SYNC_JPG_PATH) || String.IsNullOrEmpty(SYNC_RAW_PATH))
            {
                MessageBox.Show("All Path can not empty");
                return;
            }

            List<string> jpg_list = new List<string>();
            string path = SYNC_JPG_PATH;
            DirectoryInfo folder = new DirectoryInfo(path);

            foreach (FileInfo file in folder.GetFiles())
            {
                var ext_name = Path.GetExtension(file.Name).ToUpper();
                if (ext_name == ".JPG")
                {
                    jpg_list.Add(file.Name.ToUpper().Replace("JPG", "NEF"));
                }
            }

            List<string> del_list = new List<string>();
            DirectoryInfo folder_raw = new DirectoryInfo(SYNC_RAW_PATH);

            foreach (FileInfo file in folder_raw.GetFiles())
            {
                var ext_name = Path.GetExtension(file.Name).ToUpper();
                if (ext_name == "." + RAW_FILE_EXTENSIOM)
                {
                    if (!jpg_list.Contains(file.Name.ToUpper()))
                    {
                        del_list.Add(file.Name.ToUpper());
                    }
                }
            }

            foreach (var item in del_list)
            {
                string delpath = SYNC_RAW_PATH + @"\" + item.ToString();
                File.Delete(delpath);
            }
        }

        private void selectedCopy(string picSelectedPath, string rawAllPath, string rawSelectedPath)
        {
            // 精选文件列表
            List<string> selectedList = new List<string>();
            // 收集精选文件
            DirectoryInfo folder = new DirectoryInfo(picSelectedPath);
            foreach (FileInfo file in folder.GetFiles())
            {
                var ext_name = Path.GetExtension(file.Name).ToUpper();
                if (ext_name == ".JPG")
                {
                    selectedList.Add(file.Name.ToUpper().Replace("JPG", RAW_FILE_EXTENSIOM));
                }
            }

            // 移动raw all里的精选文件到raw精选目录
            DirectoryInfo directoryInfo = new DirectoryInfo(rawAllPath);
            FileInfo[] files = directoryInfo.GetFiles();


            int i = 0;
            this.progressBarAuto.Maximum = files.Length;


            foreach (FileInfo rawfile in files)
            {
                if (selectedList.Contains(rawfile.Name.ToUpper()))
                {
                    rawfile.MoveTo(Path.Combine(rawSelectedPath, rawfile.Name));
                }
                i++;
                bgWorker.ReportProgress(i, "Working");
                //System.Threading.Thread.Sleep(5000);
            }


            //for (int i = 0; i <= 100; i++)
            //{
            //    bgWorker.ReportProgress(i, "Working");
            //    System.Threading.Thread.Sleep(100);
            //}

        }


        public void PrograssBarUseBackgroundWorker()
        {
            InitializeBackgroundWorker();
        }

        private void InitializeBackgroundWorker()
        {
            bgWorker.WorkerReportsProgress = true;
            //bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += new DoWorkEventHandler(AutoCreate);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgessChanged);
            // bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_WorkerCompleted);
        }

        public void bgWorker_ProgessChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBarAuto.Value = e.ProgressPercentage;
            //this.label7.Text = "处理进度:" + Convert.ToString(e.ProgressPercentage) + "%";
        }

        #endregion


        #region 备份导出功能
        private void btnBackup_Click(object sender, EventArgs e)
        {
            initBack();
            this.bgBackWorker.RunWorkerAsync();
        }

        private void backupProcess(string path, int isChild = 0)
        {
            //文件夹信息
            DirectoryInfo dir = new DirectoryInfo(path);
            //如果非根路径且是系统文件夹则跳过
            if (null != dir.Parent && dir.Attributes.ToString().IndexOf("System") > -1)
            {
                return;
            }
            //取得所有子文件夹
            DirectoryInfo[] childDirs = dir.GetDirectories();
            for (int i = 0; i < childDirs.Length; i++)
            {
                // 子目录名称
                string childFName = childDirs[i].FullName;
                string reportBegin = "";
                AddBackLog("当前处理目录：" + childFName);
                if (isChild == 0)
                {
                    BACKUP_REPORT.Append("\r\n扫描-" + childFName);
                }
                else
                {
                    reportBegin = "发现-" + childFName;
                }
                if (childFName.Contains(BACKUP_NAME_CONTAIN))
                {
                    // 复制导出
                    var albumNameArr = childFName.Split('\\');
                    var albumName = albumNameArr[albumNameArr.Length - 2];
                    var psName = albumNameArr[albumNameArr.Length - 1];

                    string[] files = Directory.GetFiles(childFName);
                    var fileCount = 0;
                    foreach (string file in files)
                    {
                        string pForderPath = string.Empty;
                        if (childFName.Contains("精修"))
                        {
                            pForderPath = BACKUP_PATH + "\\" + albumName;
                        }
                        else
                        {
                            pForderPath = BACKUP_PATH + "\\" + psName;
                        }

                        // create forder
                        if (!Directory.Exists(pForderPath))
                        {
                            Directory.CreateDirectory(pForderPath);
                        }

                        string pFilePath = pForderPath + "\\" + Path.GetFileName(file);
                        if (File.Exists(pFilePath))
                        {
                            AddBackLog("文件已存在===>" + pFilePath);
                            continue;
                        }
                        File.Copy(file, pFilePath, true);
                        AddBackLog("拷贝文件===>" + albumName + "\\" + Path.GetFileName(file));
                        fileCount++;
                    }
                    BACKUP_REPORT.Append(reportBegin + "-已导出" + fileCount + "个文件 - ok " + "\r\n");
                }
                else
                {
                    AddBackLog("当前目录未找到");
                    if (isChild == 0)
                    {
                        BACKUP_REPORT.Append("\r\n");
                    }
                    backupProcess(childFName, 1);
                }
            }
            return;
        }

        private void AddBackLog(String info)
        {
            BACKUP_LOG.Append(info + "\r\n");
            //在线程中更新UI（通过ReportProgress方法）
            bgBackWorker.ReportProgress(1, null);
        }

        private void initBack()
        {
            bgBackWorker = new BackgroundWorker();
            bgBackWorker.WorkerReportsProgress = true;
            bgBackWorker.DoWork += new DoWorkEventHandler(bgBackWorker_DoWork);
            bgBackWorker.ProgressChanged += new ProgressChangedEventHandler(bgBackWorker_ProgressChanged);
        }

        public void bgBackWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //...执行线程任务
            AddBackLog("=====================begin backup======================");

            BACKUP_NAME_CONTAIN = this.comboBox3.Text.ToString();
            BACKUP_PATH = this.txtBackUpPath.Text.ToString();
            string picSourceDir = this.textBox10.Text.ToString();
            //picSourceDir = "C:\\test2";
            //BACKUP_PATH = "C:\\导出";
            backupProcess(picSourceDir);
            AddBackLog("=====================end backup======================");

            AddBackLog("\r\n\r\n=====================begin report======================");
            AddBackLog(BACKUP_REPORT.ToString());
            AddBackLog("=====================end report======================");
            MessageBox.Show("处理完成！");
        }

        //UI更新方法
        public void bgBackWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            rtbBackLog.ForeColor = Color.Green;
            rtbBackLog.Text = BACKUP_LOG.ToString();
        }

        private void rtbBackLog_TextChanged(object sender, EventArgs e)
        {
            //将光标位置设置到当前内容的末尾
            rtbBackLog.SelectionStart = rtbBackLog.Text.Length;
            //滚动到光标位置
            rtbBackLog.ScrollToCaret();
        }
        #endregion

    }
}
