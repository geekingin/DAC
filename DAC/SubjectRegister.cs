﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Collections;
using ADOX;
using System.IO;

namespace DAC
{
    public partial class SubjectRegister : Form
    {
        public bool isAdmin=false;//是否注册的是管理员
        public SubjectRegister()
        {
            InitializeComponent();
        }
        private void subjectRegister_Load(object sender, EventArgs e)
        {
            if (getIsAdmin == true)
            {
                this.userWaring.Text = "管理员帐号";
                userWaring.Visible = true;
                this.user.Text = "Admin";
                this.user.ReadOnly = true;
            }
        }
         
        public bool getIsAdmin
        {
            set
            {
                isAdmin = value;
            }
            get
            {
                return isAdmin;
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 登记并注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enter_Click(object sender, EventArgs e)
        {
            //检查账号及密码
            if (user.Text.Length == 0 || pwd.Text.Length == 0 || rpwd.Text.Length == 0)
            {
                MessageBox.Show("账号及密码不能为空，注册失败");
                return;
            }
            if (IsUserNameUsed(user.Text) == true ||IsUserNameUsed1(user.Text) == true)//已被注册,返回
            {
                MessageBox.Show("账号已被注册或将被申请通过,注册失败！");
                return;
            }
            if (string.Equals(pwd.Text, rpwd.Text) == false)
            {
                MessageBox.Show("两次输入的密码不一致，注册失败！");
                pwdWarning.Visible = true;
                return;
            }
            if (user.Text.ToLower().CompareTo("admin") == 0)
            {
                if (getIsAdmin == false)
                {
                   MessageBox.Show( "账号名非法,注册失败");
                   return;
                }
            }
            if (pwd.Text.Length < 6)//密码长度过小
            {

                if (MessageBox.Show(this, "密码长度过小，是否重新设置密码", "提醒", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    return;
                }
            }

            //将注册信息插入到注册表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();           
        
            OleDbCommand conn = new OleDbCommand();
            conn.Connection = oleDB;
            if (isAdmin == true)//如果是管理员进行注册
            {
                conn.CommandText = "insert into  [主体信息表](主体名称,密码,注册时间) values('" + user.Text.ToLower() + "','" + pwd.Text.GetHashCode() + "','" + DateTime.Now.ToString() + "')";
            }
            else//普通用户
            {
                conn.CommandText = "insert into  [注册管理表](主体名称,注册时间,密码) values('" + user.Text.ToLower() + "','" + DateTime.Now.ToString() + "','" + pwd.Text.GetHashCode() + "')";
            }
            conn.ExecuteNonQuery();//输出受影响的行数
            oleDB.Close();
            if (isAdmin == true)//如果是管理员进行注册
            {
                MessageBox.Show("注册成功！，按确定返回");
            }
            else
            {
                MessageBox.Show("注册成功，等待管理员通过！，按确定返回");
            }
            this.Close();
        }

        private void user_TextChanged(object sender, EventArgs e)
        {
            if (user.Text.ToLower().CompareTo("admin") == 0)
            {
                if (getIsAdmin == false)
                {
                    userWaring.Visible = true;
                    userWaring.Text = "账号名非法";
                    return;
                }
            }
         
            if (IsUserNameUsed(user.Text) == true|| IsUserNameUsed1(user.Text) == true)//已被注册,返回
            {
                userWaring.Visible = true;
                userWaring.Text = "账号已被注册";
                return;
            }
            else
            {
                userWaring.Visible = false;
            }

        }
        //检查是否已经注册
        public static bool IsUserNameUsed(string useName)
        {
            bool isUsed = false;    
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand conn = new OleDbCommand();
            conn.Connection = oleDB;
            conn.CommandText = "select *  from  [主体信息表] where 主体名称=" + "'" + useName.ToLower() + "'";//查找账号是否已经被注册
            OleDbDataReader dr = conn.ExecuteReader();
            isUsed = dr.HasRows;
            dr.Close();
            oleDB.Close();
            return isUsed;
        }
        //检查是否已经申请注册
        public static bool IsUserNameUsed1(string useName)
        {
            bool isUsed = false;
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand conn = new OleDbCommand();
            conn.Connection = oleDB;
            conn.CommandText = "select *  from  [注册管理表] where 主体名称=" + "'" + useName.ToLower() + "'";//查找账号是否已经被注册
            OleDbDataReader dr = conn.ExecuteReader();
            isUsed = dr.HasRows;
            dr.Close();
            oleDB.Close();
            return isUsed;

        }
        private void pwd_TextChanged(object sender, EventArgs e)
        {
            CheckPwd();
        }

        private void rpwd_TextChanged(object sender, EventArgs e)
        {
            CheckPwd();
        }
        /// <summary>
        /// 检查密码是否符合要求
        /// </summary>
        private void CheckPwd()
        {
            if (rpwd.Text.Length != 0 && pwd.Text.Length != 0 && pwd.Capture==false && rpwd.Capture==false)//若输入密码非空，且当前文框不是鼠标焦点
            {
                if (string.Equals(rpwd.Text, pwd.Text) == false)//比对两次的密码
                {
                    pwdWarning.Visible = true;//两次的密码不一致，则提示
                }
                else
                {
                    pwdWarning.Visible = false;
                }
            }
            else
            {
                pwdWarning.Visible = false;
            }
        }
    }
}
