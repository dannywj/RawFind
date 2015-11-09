using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RawFind
{
    class Program
    {
        static string JPG_PATH = ConfigurationSettings.AppSettings["JPG_PATH"].ToString();
        static string SEARCH_RAW_PATH = ConfigurationSettings.AppSettings["SEARCH_RAW_PATH"].ToString();
        static string FINAL_RESULT_PATH = ConfigurationSettings.AppSettings["FINAL_RESULT_PATH"].ToString();
        static string RAW_FILE_EXTENSIOM = ConfigurationSettings.AppSettings["RAW_FILE_EXTENSIOM"].ToString();
        static int PROCESS_COUNT = 0;
        static int JPG_COUNT = 0;
        static List<string> COPY_LIST = new List<string>();
        static void Main(string[] args)
        {
            Console.WriteLine("======Begin process======");
            //获取搜索文件列表
            var list = GetList();
            JPG_COUNT = list.Count;
            Console.WriteLine("JPG Count:" + JPG_COUNT);
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i].ToString());
                //find file
                //路径名
                string DirName = SEARCH_RAW_PATH;
                //文件中包含名
                string FileName = list[i].ToString();
                //处理程序(遍历查找+文件复制)
                ProcessFiles(FileName);
                //Console.WriteLine("PROCESS_COUNT:" + PROCESS_COUNT + "  JPG_COUNT:" + JPG_COUNT);

                //计算百分比
                double percent = (double)PROCESS_COUNT / JPG_COUNT;
                string percentText = percent.ToString("0.0%");//最后percentText的值为10.0%
                Console.WriteLine("Process:" + percentText);
            }

            Console.WriteLine("PROCESS_COUNT:" + PROCESS_COUNT);
            CopyJPGFiles(list);
            Console.WriteLine("======End process======");
            Console.ReadLine();
        }

        static List<string> GetList()
        {
            Console.WriteLine("Begin getlist");
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
                    Console.WriteLine("Find! " + finfo[i].FullName);
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
                Console.WriteLine("File Exists! " + destPath);
            }
            else
            {
                System.IO.File.Copy(fileFullName, destPath);
                Console.WriteLine("Copy File Success! " + destPath);
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
            Console.WriteLine("-------");
            List<string> jpg_need_copy_list = new List<string>();
            for (int i = 0; i < COPY_LIST.Count; i++)
            {
                Console.WriteLine("COPY_LIST:" + COPY_LIST[i].ToString());
                if (searchList.Contains(COPY_LIST[i].ToString()))
                {
                    searchList.Remove(COPY_LIST[i].ToString());
                }
            }


            for (int i = 0; i < searchList.Count; i++)
            {
                Console.WriteLine("jpg_need_copy_list:" + searchList[i].ToString());
                CopyFile(JPG_PATH + @"\" + searchList[i].ToString().Replace(RAW_FILE_EXTENSIOM, "JPG"), searchList[i].ToString().Replace(RAW_FILE_EXTENSIOM, "JPG"));
            }

            Console.WriteLine("-------");
        }
    }
}
