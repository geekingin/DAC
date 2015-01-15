using System;
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
    public partial class Dac : Form
    {
        // LinkedList<Subject> Subject;//主体的链表
        // LinkedList<Object> Object;  //客体的链表
        public string SubjectName = string.Empty;//主体名字 
        string[] ABILITY = { "读", "写", "添加", "执行" }; 
        public Dac()
        {
            InitializeComponent();
        }
        private void main_Load(object sender, EventArgs e)
        {
            //显示系统时间
            this.timer.Start();
            this.timer.Interval = 100;
            this.timer.Tick += timer_Tick;
            if (string.Equals(this.GetSubjectName, "admin") == false)//若不是管理员角色，则不能删除相应的主体，也不能加入相应的主体在系统中
            {
                AbilityTable.TabPages.Remove(tabPage6);
                AbilityTable.TabPages.Remove(tabPage7);
                //DeleteSubjectToolStripMenuItem.Visible = false;
                //RegisterToolStripMenuItem.Visible = false;
                RToolStripMenuItem1.Visible = false;
                SToolStripMenuItem1.Visible = false;
                SToolStripMenuItem2.Visible = false;
            }
            else
                if (UserRegister() == true)
                {
                    if (MessageBox.Show(this, "有新的账号需要检查通过，是否现在查看", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        AbilityTable.SelectedIndex = 7;
                    }
                }
            Refresh__AuthorityTable();
        }
        //检查是否有新账号注册需要通过
        private bool UserRegister()
        {
            bool b = false;
            //添加注册管理中的待拉用户到列表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [注册管理表] ";
            OleDbDataReader dr = cmd.ExecuteReader();

            b = dr.HasRows;

            dr.Close();
            oleDB.Close();
            return b;
        }

        public string GetSubjectName
        {
            set
            {
                this.SubjectName = value;
            }
            get
            {
                return this.SubjectName;
            }
        }


        //以下是选项卡 - 授权表显示的代码

        /// <summary>
        /// 客体的授权表
        /// </summary>

        void  Refresh__AuthorityTable()
        {
            DataGridView_AuthorityData.Rows.Clear();
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [访问控制矩阵表]";

            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)

                while (dr.Read())
                {
                    DataGridViewRow r = new DataGridViewRow();
                    r.CreateCells(this.DataGridView_AuthorityData);
                    r.Cells[0].Value = dr["主体名称"];
                    r.Cells[6].Value =   Get_Subject_Creat_Time( dr["主体名称"].ToString());
                    r.Cells[1].Value = dr["客体名称"];
                    r.Cells[7].Value = Get_Object_Creat_Time(dr["客体名称"].ToString());
                    r.Cells[2].Value =  Get_Object_Creat_Subject( dr["客体名称"].ToString());
                    r.Cells[3].Value = dr["权限"];
                    r.Cells[4].Value = dr["权限授予者"];
                    r.Cells[5].Value = dr["授予权"];

                    this.DataGridView_AuthorityData.Rows.Add(r);
                }
            dr.Close();
            oleDB.Close();
        }
        /// <summary>
        /// 获取主体的创建的时间
        /// </summary>
        /// <param name="subjectName"></param>
        /// <returns></returns>
        string Get_Subject_Creat_Time(string subjectName)
        {
            string s = string.Empty;
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [主体信息表] where 主体名称=" + "'" + subjectName.ToLower() + "'";//查找账号是否已经被注册
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    s = dr["注册时间"].ToString();
                }
            }

            dr.Close();
            oleDB.Close();
            return s;
        }
        /// <summary>
        /// 获取客体的创建者
        /// </summary>
        /// <param name="subjectName"></param>
        /// <returns></returns>
      private  static  string Get_Object_Creat_Subject(string objectName)
        {
            string s = string.Empty;
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [客体信息表] where 客体名称=" + "'" + objectName.ToLower() + "'";//查找账号是否已经被注册
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    s = dr["创建者"].ToString();
                }
            }

            dr.Close();
            oleDB.Close();
            return s;
        }
        /// <summary>
        /// 获取客体的创建时间
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        string Get_Object_Creat_Time(string objectName)
        {
            string s = string.Empty;
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select *  from  [客体信息表] where 客体名称=" + "'" + objectName.ToLower() + "'";
            OleDbDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    s = dr["创建时间"].ToString();
                }
            }

            dr.Close();
            oleDB.Close();
            return s;
        }
        void timer_Tick(object sender, EventArgs e)
        {
            this.timeLabel.Text = DateTime.Now.ToString();
        }

        private void Dac_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.timer.Stop();
        }
        /// <summary>
        ///更新 
        /// </summary>
        private void Refresh_GiveAbilitys()//##changed add a "s" at end
        {
            this.ComboBox_Subject_Inquiry.Items.Clear();
            this.ComboBox_Object_Inquiry.Items.Clear();
            //  bool IsCanBeE = false;
            //添加主体创建的客体的名称到下拉列表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [客体信息表] where 创建者=   " + "'" + this.GetSubjectName + "'";
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {
                while (dr.Read())
                {
                    this.ComboBox_Subject_Inquiry.Items.Add(dr["客体名称"]);//                
                }
            }
            dr.Close();
            if (this.ComboBox_Subject_Inquiry.Items.Count != 0)
            {
                this.ComboBox_Subject_Inquiry.SelectedIndex = 0;
            }
            //添加其他主体称到主体名称下拉列表中
            cmd.CommandText = "select * from [主体信息表]";
            dr = cmd.ExecuteReader();
            if (dr.HasRows == true)
            {
                while (dr.Read())
                {
                 //   if (string.Equals(dr["主体名称"], this.GetSubjectName) == false)//将所有其他主体添加到主体名字下拉框中
                    {
                        this.ComboBox_Object_Inquiry.Items.Add(dr["主体名称"]);//                
                    }
                }
            }

            if (this.ComboBox_Object_Inquiry.Items.Count != 0)
            {
                this.ComboBox_Object_Inquiry.SelectedIndex = 0;
            }
            dr.Close();
            oleDB.Close();
        }

        //ignore
        /// <summary>
        /// 以下是选项卡-授权查询的代码
        /// </summary>
        private void Refresh_Subject_Object_Inquiry()
        {
            this.ComboBox_Object_Inquiry.Items.Clear();
            this.ComboBox_Subject_Inquiry.Items.Clear();
            this.DataGridView_Ability_Inquiry.Rows.Clear();

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [主体信息表] ";
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {
                while (dr.Read())
                {
                    this.ComboBox_Subject_Inquiry.Items.Add(dr["主体名称"]);
                }


            }

            if (this.ComboBox_Subject_Inquiry.Items.Count != 0)
            {
                this.ComboBox_Subject_Inquiry.SelectedIndex = 0;
            }
            dr.Close();
            cmd.CommandText = "select * from [客体信息表] ";
            dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {
                while (dr.Read())
                {
                    this.ComboBox_Object_Inquiry.Items.Add(dr["客体名称"]);
                }


            }

            if (this.ComboBox_Object_Inquiry.Items.Count != 0)
            {
                this.ComboBox_Object_Inquiry.SelectedIndex = 0;
            }
            dr.Close();
            oleDB.Close();
        }

        //授权查询界面，主体名称变化回调
        private void ComboBox_Subject_Inquiry_SelectedIndexChanged(object sender, EventArgs e)
        {
            View_Inquiry();
        }

        //授权查询界面，客体名称变化回调
        private void ComboBox_Object_Inquiry_SelectedIndexChanged(object sender, EventArgs e)
        {
            View_Inquiry();
        }
        /// <summary>
        /// 显示当前查询的主客体 对应 的权限
        /// </summary>
        private void View_Inquiry()
        {
            this.DataGridView_Ability_Inquiry.Rows.Clear();
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [访问控制矩阵表] where 主体名称=" + "'" + this.ComboBox_Subject_Inquiry.Text.ToLower() + "'" + "and 客体名称=" + "'" + this.ComboBox_Object_Inquiry.Text.ToLower() + "'";

            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
                while (dr.Read())
                {
                    DataGridViewRow r = new DataGridViewRow();
                    r.CreateCells(this.DataGridView_Ability_Inquiry);
                    r.Cells[0].Value = dr["权限"];
                    r.Cells[1].Value = dr["权限授予者"];
                    if (string.Equals(dr["权限授予者"], this.ComboBox_Subject_Inquiry.Text.ToLower()) == false)
                    {
                        LinkedList<string> Link = Get_Link(dr["权限授予者"].ToString(), this.ComboBox_Object_Inquiry.Text.ToLower(), dr["权限"].ToString());

                        while (Link.Count != 0)
                        {
                            r.Cells[2].Value += Link.First.Value + "->";
                            Link.RemoveFirst();
                        }
                        r.Cells[2].Value += dr["权限授予者"] + "->";
                    }
                    r.Cells[2].Value += this.ComboBox_Subject_Inquiry.Text.ToLower();
                    r.Cells[3].Value = dr["授予权"];
                    this.DataGridView_Ability_Inquiry.Rows.Add(r);
                }
            dr.Close();
            oleDB.Close();
        }

        /// <summary>
        /// 获得权限授予者
        /// </summary>
        /// <param name="SubjectName"></param>
        /// <param name="ObjectName"></param>
        /// <param name="ability"></param>
        /// <returns></returns>
        private static string Get_Subject_Object_Deliver(string SubjectName, string ObjectName, string ability)
        {
            string Link = string.Empty;

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand conn = new OleDbCommand();
            conn.Connection = oleDB;
            conn.CommandText = "select *  from  [访问控制矩阵表] where 客体名称=" + "'" + ObjectName.ToLower() + "'" + "and 主体名称=" + "'" + SubjectName.ToLower() + "'" + "and 权限=" + "'" + ability + "'";//查找主客体对应权限
            OleDbDataReader dr = conn.ExecuteReader();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    if (string.Equals(dr["权限授予者"].ToString(), SubjectName) == false)//subjectname创建了客体
                        Link = dr["权限授予者"].ToString();
                }
            }
            dr.Close();
            oleDB.Close();

            return Link;
        }


        //以下是选项卡-权限管理的代码
        void Refresh__Abillity_Manage(int index_Subject, int index_Object)
        {
            this.ComboBox_Object_Manage.Items.Clear();
            this.ComboBox_Subject_Manage.Items.Clear();
            this.CheckedListBox_Ability_Delete.Items.Clear();
            this.CheckedListBox_Ability_Deliver_Delete.Items.Clear();
            this.CheckedListBox_Ability_Add.Items.Clear();
            this.CheckedListBox_Ability_Deliver_Add.Items.Clear();

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [主体信息表] ";

            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
                while (dr.Read())
                {
                    this.ComboBox_Subject_Manage.Items.Add(dr["主体名称"]);
                }
            dr.Close();
            cmd.CommandText = "select * from [访问控制矩阵表] ";

            dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
                while (dr.Read())
                {
                    if (string.Equals(dr["主体名称"], GetSubjectName) && string.Equals(dr["授予权"].ToString().ToLower(), "true")
                        && this.ComboBox_Object_Manage.Items.IndexOf(dr["客体名称"]) == -1)//只能添加自己有权限且有授予其他主体能力的客体
                    {
                        this.ComboBox_Object_Manage.Items.Add(dr["客体名称"]);
                    }
                }
            dr.Close();
            this.ComboBox_Subject_Manage.Items.Remove(GetSubjectName);
            if (this.ComboBox_Subject_Manage.Items.Count != 0)
            {
                this.ComboBox_Subject_Manage.SelectedIndex = index_Subject;
            }
            if (this.ComboBox_Object_Manage.Items.Count != 0)
            {
                this.ComboBox_Object_Manage.SelectedIndex = index_Object;
            }
            oleDB.Close();
        }

        /// <summary>
        /// 授权管理界面，主体名称改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_Subject_Manage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Refresh_CheckListBox();
        }
        // 授权管理界面，课题名称改变
        private void ComboBox_Object_Manage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Refresh_CheckListBox();
        }
        private void Refresh_CheckListBox()
        {
            this.CheckedListBox_Ability_Delete.Items.Clear();
            this.CheckedListBox_Ability_Deliver_Delete.Items.Clear();
            this.CheckedListBox_Ability_Add.Items.Clear();
            this.CheckedListBox_Ability_Deliver_Add.Items.Clear();

            string SubjectName = this.ComboBox_Subject_Manage.Text;
            string ObjectName = this.ComboBox_Object_Manage.Text;

            LinkedList<StoreAbility> Link_Ability = Get_Subject_Ability(SubjectName, ObjectName);//获取当前选择的主客体之间的关系
            while (Link_Ability.Count != 0)
            {
                this.CheckedListBox_Ability_Delete.Items.Add(Link_Ability.First.Value.Ability_Name + "(从" + Link_Ability.First.Value.Name_Deliver + "获得)");
                if (Link_Ability.First.Value.IsDeliver == true)//如果有控制权，则添加
                {
                    this.CheckedListBox_Ability_Deliver_Delete.Items.Add(Link_Ability.First.Value.Ability_Name + "(从" + Link_Ability.First.Value.Name_Deliver + "获得)");
                }
                Link_Ability.RemoveFirst();
            }

            if (Is_Subject_Creat_Object(GetSubjectName, ObjectName) == false)//当前的登录主体不是客体的创建者
            {
                if (Is_Subject_Creat_Object(this.ComboBox_Subject_Manage.Text, ObjectName) == true)//当前如果选择的主体创建了客体，则不能再添加任何权限
                {
                    return;
                }
                LinkedList<StoreAbility> Link_Ability_Login = Get_Subject_Ability(GetSubjectName, ObjectName);
                while (Link_Ability_Login.Count != 0)
                {
                    if (Is_Subject_Object_Ability_Deliver(GetSubjectName, ObjectName, Link_Ability_Login.First.Value.Ability_Name))//判断当前登录的账号是否具有对该客体的某种权限的传递权
                    {
                        //还需要判断当前选择的主体是否已经具有了该种权限，或者 具备了 该权限 但不是当前登录的主体给予的 
                        if (Is_Subject_Object_Ability(this.ComboBox_Subject_Manage.Text, ObjectName, Link_Ability_Login.First.Value.Ability_Name) == false)//没有该权限
                        {
                            this.CheckedListBox_Ability_Add.Items.Add(Link_Ability_Login.First.Value.Ability_Name);
                            this.CheckedListBox_Ability_Deliver_Add.Items.Add(Link_Ability_Login.First.Value.Ability_Name);
                        }
                        else//具备了权限，但不是当前登录的主体给的
                        {
                            if (Is_Subject_Object_Ability_SubjectDeliver(this.ComboBox_Subject_Manage.Text, ObjectName, Link_Ability_Login.First.Value.Ability_Name, GetSubjectName) == false)
                            {
                                this.CheckedListBox_Ability_Add.Items.Add(Link_Ability_Login.First.Value.Ability_Name);
                                this.CheckedListBox_Ability_Deliver_Add.Items.Add(Link_Ability_Login.First.Value.Ability_Name);
                            }
                            else
                                //还应该判断该主体得到了权限，但是没有授予权的情况
                                if (Is_Subject_Object_Ability_SubjectDeliver(this.ComboBox_Subject_Manage.Text, ObjectName, Link_Ability_Login.First.Value.Ability_Name, GetSubjectName, true) == false)
                                {
                                    this.CheckedListBox_Ability_Deliver_Add.Items.Add(Link_Ability_Login.First.Value.Ability_Name);
                                }
                        }
                    }

                    Link_Ability_Login.RemoveFirst();
                }
            }
            else//当前登录的主体是客体的创建者
            {
                if (string.Equals(this.ComboBox_Subject_Manage.Text, GetSubjectName) == false)//当前选择的主体不是是登录的主体
                {
                    foreach (var v in ABILITY)
                    {
                        if (this.CheckedListBox_Ability_Delete.Items.IndexOf(v + "(从" + this.GetSubjectName + "获得)") == -1 || Is_Subject_Object_Ability_SubjectDeliver(this.ComboBox_Subject_Manage.Text, ObjectName, v, GetSubjectName) == false)
                        {
                            this.CheckedListBox_Ability_Add.Items.Add(v);
                        }
                        if (this.CheckedListBox_Ability_Deliver_Delete.Items.IndexOf(v + "(从" + this.GetSubjectName + "获得)") == -1 || Is_Subject_Object_Ability_SubjectDeliver(this.ComboBox_Subject_Manage.Text, ObjectName, v, GetSubjectName) == false)
                        {
                            this.CheckedListBox_Ability_Deliver_Add.Items.Add(v);
                        }
                    }
                }
            }
        }

        //
        private bool Is_Subject_Object_Ability_SubjectDeliver(string subjectName, string objectName, string ability, string SubjectDeliver, bool Isdeliver)
        {
            bool b = false;

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [访问控制矩阵表]  where 主体名称= " + "'" + subjectName + "'" + "  and  客体名称= " + "'" + objectName
                + "'" + "  and  权限= " + "'" + ability + "'" + "  and  权限授予者= " + "'" + SubjectDeliver + "'" + "  and  授予权= " + "'" + Isdeliver.ToString() + "'";
            OleDbDataReader dr = cmd.ExecuteReader();

            b = dr.HasRows;

            dr.Close();
            oleDB.Close();

            return b;
        }

        /// <summary>
        /// 判断主体 是否是客体的创建者
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private bool Is_Subject_Creat_Object(string subjectName, string objectName)
        {
            bool b = false;

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [客体信息表]  where 客体名称= " + "'" + objectName + "'";
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    b = string.Equals(dr["创建者"], subjectName);
                }
            }

            dr.Close();
            oleDB.Close();
            return b;
        }
        /// <summary>
        /// 判断主体 是否是具有客体的某种权限的授予权
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private bool Is_Subject_Object_Ability_Deliver(string subjectName, string objectName, string ability)
        {
            bool b = false;

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [访问控制矩阵表]  where 主体名称= " + "'" + subjectName + "'" + "  and  客体名称= " + "'" + objectName
                + "'" + "  and  权限= " + "'" + ability + "'";
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    b = string.Equals(dr["授予权"].ToString().ToLower(), "true");
                }
            }

            dr.Close();
            oleDB.Close();
            return b;
        }
        /// <summary>
        /// 判断主体 是否是具有客体的某种权限
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private bool Is_Subject_Object_Ability(string subjectName, string objectName, string ability)
        {
            bool b = false;

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [访问控制矩阵表]  where 主体名称= " + "'" + subjectName + "'" + "  and  客体名称= " + "'" + objectName
                + "'" + "  and  权限= " + "'" + ability + "'";
            OleDbDataReader dr = cmd.ExecuteReader();

            b = dr.HasRows;

            dr.Close();
            oleDB.Close();
            return b;
        }
        /// <summary>
        /// 判断参数之间是否为访问控制的一条记录
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private bool Is_Subject_Object_Ability_SubjectDeliver(string subjectName, string objectName, string ability, string SubjectDeliver)
        {
            bool b = false;

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [访问控制矩阵表]  where 主体名称= " + "'" + subjectName + "'" + "  and  客体名称= " + "'" + objectName
                + "'" + "  and  权限= " + "'" + ability + "'" + "  and  权限授予者= " + "'" + SubjectDeliver + "'";
            OleDbDataReader dr = cmd.ExecuteReader();

            b = dr.HasRows;

            dr.Close();
            oleDB.Close();
            return b;
        }


        private LinkedList<StoreAbility> Get_Subject_Ability(string SubjectName, string ObjectName)
        {
            LinkedList<StoreAbility> Link = new LinkedList<StoreAbility>();

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand conn = new OleDbCommand();
            conn.Connection = oleDB;
            conn.CommandText = "select *  from  [访问控制矩阵表] where 客体名称=" + "'" + ObjectName.ToLower() + "'" + "and 主体名称=" + "'" + SubjectName.ToLower() + "'";//查找主客体对应权限
            OleDbDataReader dr = conn.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (string.Equals("true", dr["授予权"].ToString().ToLower()))
                    {
                        StoreAbility S = new StoreAbility(dr["主体名称"].ToString(), dr["客体名称"].ToString(), dr["权限"].ToString(), dr["权限授予者"].ToString(), true);
                        Link.AddFirst(S);
                    }
                    else
                    {
                        StoreAbility S = new StoreAbility(dr["主体名称"].ToString(), dr["客体名称"].ToString(), dr["权限"].ToString(), dr["权限授予者"].ToString(), false);
                        Link.AddFirst(S);
                    }
                }
            }

            dr.Close();
            oleDB.Close();

            return Link;
        }

        private static LinkedList<string> Get_Link(string SubjectName, string ObjectName, string ability)
        {
            LinkedList<string> Link = new LinkedList<string>();
            string returnString = Get_Subject_Object_Deliver(SubjectName, ObjectName, ability);
            if (returnString.Length != 0)
            {
                Link.AddFirst(returnString);

                LinkedList<string> R = Get_Link(returnString, ObjectName, ability);
                while (R.Count != 0)
                {
                    Link.AddFirst(R.Last.Value);
                    R.RemoveLast();
                }
            }
            return Link;
        }

        //增加权限
        private void Btn_Ability_Add_Click(object sender, EventArgs e)
        {
            if (this.CheckedListBox_Ability_Add.CheckedItems.Count == 0 && this.CheckedListBox_Ability_Deliver_Add.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "还未选中任何项，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //不允许只添加授予权，而无该权限
            for (int i = 0; i < CheckedListBox_Ability_Deliver_Add.CheckedItems.Count; i++)
            {
                string s = this.CheckedListBox_Ability_Deliver_Add.CheckedItems[i].ToString();

                if (this.CheckedListBox_Ability_Add.CheckedItems.IndexOf(this.CheckedListBox_Ability_Deliver_Add.CheckedItems[i].ToString()) == -1)
                {
                    bool b = false;
                    foreach (var v in this.CheckedListBox_Ability_Delete.Items)
                    {
                        string s1 = v.ToString().Substring(0, v.ToString().IndexOf('('));
                        int start = v.ToString().IndexOf('(');
                        int end = v.ToString().IndexOf(')');
                        string s2 = v.ToString().Substring(start + 2, end - start - 4);
                        if (string.Equals(s, s1) == true && string.Equals(s2, this.GetSubjectName))
                        {
                            b = true;
                            break;
                        }
                    }
                    if (b == false)
                    {
                        MessageBox.Show(this, "不允许主体只拥有授予权，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //如果权限已经被创建者发放了黑令牌
                if (Is_Subject_Object_Ability_Limit(this.ComboBox_Subject_Manage.Text, this.ComboBox_Object_Manage.Text, this.CheckedListBox_Ability_Deliver_Add.CheckedItems[i].ToString()) == true)
                {
                    MessageBox.Show(this, "主体 " + this.ComboBox_Subject_Manage.Text + "对于客体 " +  this.ComboBox_Object_Manage.Text
                        +" 的权限  " + this.CheckedListBox_Ability_Deliver_Add.CheckedItems[i].ToString()+"已经被客体的创建者发放了黑令牌，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            foreach (var v in this.CheckedListBox_Ability_Add.CheckedItems)
            {
                //如果权限已经被创建者发放了黑令牌
                if (Is_Subject_Object_Ability_Limit(this.ComboBox_Subject_Manage.Text, this.ComboBox_Object_Manage.Text, v.ToString()) == true)
                {
                    MessageBox.Show(this, "主体 " + this.ComboBox_Subject_Manage.Text + "对于客体 " +  this.ComboBox_Object_Manage.Text
                        + " 的权限  " +v.ToString() + "已经被客体的创建者发放了黑令牌，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                LinkedList<string> L = Get_Link(this.GetSubjectName, this.ComboBox_Object_Manage.Text, v.ToString());//返回当前主体得到客体的传递链
                if (L.Find(this.ComboBox_Subject_Manage.Text) != null)
                {
                    MessageBox.Show(this, "不允许循环授权，请重新选择( " + this.ComboBox_Subject_Manage.Text + " 曾将权限 " + v.ToString() + " 授予给 " + this.GetSubjectName + ")", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            foreach (var v in this.CheckedListBox_Ability_Add.CheckedItems)
            {
                Insert_Ability(this.ComboBox_Subject_Manage.Text, this.ComboBox_Object_Manage.Text, v.ToString(), this.GetSubjectName, false);
            }
            foreach (var v in this.CheckedListBox_Ability_Deliver_Add.CheckedItems)//插入授予权
            {
                Delete_Ability(this.ComboBox_Subject_Manage.Text, this.ComboBox_Object_Manage.Text, v.ToString(), this.GetSubjectName);
                Insert_Ability(this.ComboBox_Subject_Manage.Text, this.ComboBox_Object_Manage.Text, v.ToString(), this.GetSubjectName, true);
            }
            MessageBox.Show("添加授权成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Refresh__Abillity_Manage(this.ComboBox_Subject_Manage.SelectedIndex, this.ComboBox_Object_Manage.SelectedIndex);
        }
        /// <summary>
        /// 判断当前三 个参数是否为黑令牌表中的一条记录
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="objectName"></param>
        /// <param name="ability"></param>
        /// <param name="SubjectDeliver"></param>
        /// <returns></returns>
        private bool Is_Subject_Object_Ability_Limit(string subjectName, string objectName, string ability)
        {
            bool b = false;

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [黑令牌表]  where 主体名称= " + "'" + subjectName + "'" + "  and  客体名称= " + "'" + objectName
                + "'" + "  and  限制权限= " + "'" + ability + "'"  ;
            OleDbDataReader dr = cmd.ExecuteReader();

            b = dr.HasRows;

            dr.Close();
            oleDB.Close();
            return b;
        }

        //删除权限
        private void Btn_Ability_Delete_Click(object sender, EventArgs e)
        {
            if (this.CheckedListBox_Ability_Delete.CheckedItems.Count == 0 && this.CheckedListBox_Ability_Deliver_Delete.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "还未选中任何项，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //当前选中的主体是客体的创建者
            if (string.Equals(Get_Object_Creat_Subject(this.ComboBox_Object_Manage.Text), this.ComboBox_Subject_Manage.Text))
            {
                MessageBox.Show(this, "当前选中的主体是客体的创建者，无法删除权限，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var v in this.CheckedListBox_Ability_Delete.CheckedItems)
            {

                int start = v.ToString().IndexOf('(');
                int end = v.ToString().IndexOf(')');
                string s = v.ToString().Substring(start + 2, end - start - 4);
                if (string.Equals(s, this.GetSubjectName) == false)
                {
                    MessageBox.Show(this, "当前登陆的主体 " + GetSubjectName + " 不是主体 " + this.ComboBox_Subject_Manage.Text + " 对于客体 " + this.ComboBox_Object_Manage.Text + " " + v.ToString() +
                    " 的直接授权者，故不能回收其权限  ", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            foreach (var v in this.CheckedListBox_Ability_Deliver_Delete.CheckedItems)
            {
                int start = v.ToString().IndexOf('(');
                int end = v.ToString().IndexOf(')');
                string s = v.ToString().Substring(start + 2, end - start - 4);
                if (string.Equals(s, this.GetSubjectName) == false)
                {
                    MessageBox.Show(this, "当前登陆的主体 " + GetSubjectName + " 不是主体 " + this.ComboBox_Subject_Manage.Text + " 对于客体 " + this.ComboBox_Object_Manage.Text + " " + v.ToString() +
                   " 的直接授权者，故不能回收其授予权  ", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            foreach (var v in this.CheckedListBox_Ability_Deliver_Delete.CheckedItems)//当删除授予权时，应该将其授予出去的权限全部回收
            {
                string s = v.ToString().Substring(0, v.ToString().IndexOf('('));
                Restore_Ability(this.ComboBox_Subject_Manage.Text, this.ComboBox_Object_Manage.Text, s);//回收授出去的权限
                Updata_Ability(this.ComboBox_Subject_Manage.Text, this.ComboBox_Object_Manage.Text, s, false);

            }
            foreach (var v in this.CheckedListBox_Ability_Delete.CheckedItems)
            {
                string s = v.ToString().Substring(0, v.ToString().IndexOf('('));
                Restore_Ability(this.ComboBox_Subject_Manage.Text, this.ComboBox_Object_Manage.Text, s); //回收授出去的权限
                Delete_Ability(this.ComboBox_Subject_Manage.Text, this.ComboBox_Object_Manage.Text, s, this.GetSubjectName);
            }
            MessageBox.Show("删除授权成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Refresh__Abillity_Manage(this.ComboBox_Subject_Manage.SelectedIndex, 0);
        }

        /// <summary>
        /// 判断LoginSubjectName是否为SubjectName的某种权限的直接授予者
        /// </summary>
        /// <param name="LoginSubjectName"></param>
        /// <param name="objectName"></param>
        /// <param name="SubjectName"></param>
        /// <param name="ability"></param>
        /// <returns></returns>
        private bool Is_Subject_Subjects(string LoginSubjectName, string objectName, string SubjectName, string ability)//##add a "s" in the end
        {
            bool b = false;

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [访问控制矩阵表]  where 主体名称= " + "'" + SubjectName + "'" + "  and  客体名称= " + "'" + objectName
                + "'" + "  and  权限= " + "'" + ability + "'" + "  and  权限授予者= " + "'" + LoginSubjectName + "'";
            OleDbDataReader dr = cmd.ExecuteReader();
            b = dr.HasRows;
            dr.Close();
            oleDB.Close();
            return b;
        }
        /// <summary>
        ///  回收授权subjectName授予给其他主体的所有ability权限
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="objectName"></param>
        /// <param name="ability"></param>


        private void Restore_Ability(string subjectName, string objectName, string ability)
        {
            LinkedList<StoreAbility> S = Get_Subject_Ability(subjectName, objectName, ability);
            while (S.Count != 0)
            {
                Delete_Ability(S.First.Value.Subject_Name, objectName, ability, S.First.Value.Name_Deliver);
                S.RemoveFirst();
            }
        }
        /// <summary>
        /// 通过 subjectName,获得 关于客体objectName 权限 ability  的所有主体
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="objectName"></param>
        /// <param name="ability"></param>
        /// <returns></returns>
        private LinkedList<StoreAbility> Get_Subject_Ability(string subjectName, string objectName, string ability)
        {
            LinkedList<StoreAbility> S = Get_Subject_Deliver_Ability(subjectName, objectName, ability);//
            int i = 0;
            for (LinkedListNode<StoreAbility> T = S.First; i < S.Count; i++, T = T.Next)
            {

                LinkedList<StoreAbility> returnList = Get_Subject_Ability(T.Value.Subject_Name, objectName, ability);
                while (returnList.Count != 0)
                {
                    if (S.Find(returnList.First.Value) == null)
                    {
                        S.AddLast(returnList.First.Value);
                    }
                    returnList.RemoveFirst();
                }
            }
            return S;
        }

        /// <summary
        /// 获取所有以subject 为 客体权限授予者的的所有主体的名称
        /// </summary>
        /// <param name="SubjectName"></param>
        /// <param name="ObjectName"></param>
        /// <param name="ability"></param>
        /// <returns></returns>
        private LinkedList<StoreAbility> Get_Subject_Deliver_Ability(string SubjectName, string ObjectName, string ability)
        {
            LinkedList<StoreAbility> Link = new LinkedList<StoreAbility>();

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand conn = new OleDbCommand();
            conn.Connection = oleDB;
            conn.CommandText = "select *  from  [访问控制矩阵表] where 客体名称=" + "'" + ObjectName.ToLower() + "'" + "and 权限授予者=" + "'" + SubjectName.ToLower() + "'" + "and 权限=" + "'" + ability + "'";//查找主客体对应权限
            OleDbDataReader dr = conn.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    StoreAbility S = new StoreAbility(dr["主体名称"].ToString(), dr["客体名称"].ToString(), dr["权限"].ToString(), dr["权限授予者"].ToString(), true);
                    Link.AddFirst(S);
                }
            }

            dr.Close();
            oleDB.Close();

            return Link;
        }
        /// <summary>
        /// 向访问控制矩阵表中插入一条信息
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="objectName"></param>
        /// <param name="ability"></param>
        /// <param name="subjectDeliver"></param>
        /// <param name="isDeliver"></param>
        private void Insert_Ability(string subjectName, string objectName, string ability, string subjectDeliver, bool isDeliver)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "insert into  [访问控制矩阵表](主体名称,客体名称,权限,授予权,权限授予者) values('" + subjectName + "','" + objectName + "','" + ability + "','" +
                isDeliver.ToString() + "','" + subjectDeliver + "')";

            cmd.ExecuteNonQuery();

            oleDB.Close();
        }
        /// <summary>
        /// 更改访问控制矩阵中的授予权
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="objectName"></param>
        /// <param name="ability"></param>
        /// <param name="subjectDeliver"></param>
        /// <param name="isDeliver"></param>
        private void Updata_Ability(string subjectName, string objectName, string ability, bool isDeliver)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "update [访问控制矩阵表]   set  授予权= '" + isDeliver.ToString() + "'" + "where 主体名称='" +
            subjectName + "'" + "and 客体名称='" + objectName + "'" + "and 权限='" + ability + "'";

            cmd.ExecuteNonQuery();

            oleDB.Close();
        }
        /// <summary>
        /// 从访问控制矩阵中删除一个信息
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="objectName"></param>
        /// <param name="ability"></param>
        /// <param name="subjectDeliver"></param>
        /// <param name="isDeliver"></param>
        private void Delete_Ability(string subjectName, string objectName, string ability, string subjectDeliver)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "delete from  [访问控制矩阵表] where 主体名称=" + "'" + subjectName + "'" + "and 客体名称=" + "'" + objectName + "'" + "and 权限=" + "'" + ability + "'" + "and 权限授予者=" + "'" + subjectDeliver + "'";


            cmd.ExecuteNonQuery();

            oleDB.Close();
        }
        /// <summary>
        /// 从访问控制矩阵中删除一个信息
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="objectName"></param>
        /// <param name="ability"></param>
        /// <param name="subjectDeliver"></param>
        /// <param name="isDeliver"></param>
        private void Delete_Ability_From_Deliver(string subjectName, string objectName, string ability)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "delete from  [访问控制矩阵表] where 权限授予者=" + "'" + subjectName + "'" + "and 客体名称=" + "'" + objectName + "'" + "and 权限=" + "'" + ability + "'";

            cmd.ExecuteNonQuery();

            oleDB.Close();
        }

        /// <summary>
        /// 增加客体的选项卡的代码
        /// </summary>

        private void Refresh_Object_Add()
        {
            this.Txt_New_ObjectName.Text = string.Empty;
            this.Label_Warning_Object_Name.Visible = false;
        }

        private void Txt_New_ObjectName_TextChanged(object sender, EventArgs e)
        {
            if (IsObjectNameUsed(Txt_New_ObjectName.Text) == true)
            {
                this.Label_Warning_Object_Name.Visible = true;
                this.Label_Warning_Object_Name.Text = "客体名字已被占用";
            }
            else
            {
                this.Label_Warning_Object_Name.Visible = false;
            }
        }
        /// <summary
        /// 客体名称是否已经被占用
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private bool IsObjectNameUsed(string ObjectName)
        {
            Boolean isUsed = false;
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand conn = new OleDbCommand();
            conn.Connection = oleDB;
            conn.CommandText = "select *  from  [客体信息表] where 客体名称=" + "'" + ObjectName.ToLower() + "'";//查找客体名称是否已经被占用
            OleDbDataReader dr = conn.ExecuteReader();
            isUsed = dr.HasRows;

            dr.Close();
            oleDB.Close();
            return isUsed;
        }

        private void Btn_Creat_Click(object sender, EventArgs e)
        {
            if (IsObjectNameUsed(Txt_New_ObjectName.Text) == true)
            {
                MessageBox.Show(this, "客体名字已被占用,请重新输入", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Refresh_Object_Add();

                return;
            }
            else
            {
                Insert_Object_Info(GetSubjectName,Txt_New_ObjectName.Text.ToLower());
                Insert_Ability_table(GetSubjectName, Txt_New_ObjectName.Text, "ALL", true, GetSubjectName);//插入到访问控制矩阵中
                MessageBox.Show(this, "新建客体成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Refresh_Object_Add();
            }
        }



        private void Btn_Exit_Click(object sender, EventArgs e)
        {
            Refresh_Object_Add();
        }
        /// <summary>
        /// 增加新的信息到访问控制矩阵中
        /// </summary>   
        private static void Insert_Ability_table(string subjectName,string objectName,string ability, bool isCanDeliver,string Deliver )
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
           
            // 增加到主体信息表中
            cmd.CommandText = "insert into  [访问控制矩阵表](主体名称,客体名称,权限,授予权,权限授予者) values('"
                + subjectName.ToLower() + "','" + objectName.ToLower() + "','" + ability  +"','" + isCanDeliver.ToString() + "','" + Deliver.ToLower()+"')";
            cmd.ExecuteNonQuery();
            oleDB.Close();
        }
        /// <summary>
        /// 插入信息到访问控制矩阵中
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="objectName"></param>
        private static void Insert_Object_Info(string subjectName, string objectName)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;


            // 增加到主体信息表中
            cmd.CommandText = "insert into  [客体信息表](客体名称,创建时间,创建者) values('" + objectName.ToLower() + "','" + DateTime.Now.ToString() + "','" + subjectName.ToLower() + "')";
            cmd.ExecuteNonQuery();
            oleDB.Close();
        }

        /// <summary>
        /// 以下是删除客体选项卡的代码
        /// </summary>

        private void Refresh_Object_Delete()
        {
            this.Combox_DeleteObject.Items.Clear();

            //添加主体创建的客体的名称到下拉列表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [客体信息表] where 创建者=   " + "'" + this.GetSubjectName + "'";
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {
                while (dr.Read())
                {
                    this.Combox_DeleteObject.Items.Add(dr["客体名称"]);
                }
            }

            if (this.Combox_DeleteObject.Items.Count != 0)
            {
                this.Combox_DeleteObject.SelectedIndex = 0;
            }
            dr.Close();
            oleDB.Close();
        }

        private void DeleteObjectEnter_Click(object sender, EventArgs e)
        {
            Delete_Object(this.Combox_DeleteObject.Text);
            MessageBox.Show(this, "客体被删除，其他主体相应的权限已被撤销", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Refresh_Object_Delete();
        }
        /// <summary>
        ///删除客体
        /// </summary>
        /// <param name="objectName"></param>
        private void Delete_Object( string objectName )
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "delete from  [访问控制矩阵表] where  客体名称=" + "'" + objectName + "'"  ;
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from  [客体信息表] where  客体名称=" + "'" + objectName + "'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from  [黑令牌表] where  客体名称=" + "'" + objectName + "'";
            cmd.ExecuteNonQuery();
            oleDB.Close();
        }
        /// <summary>
        /// 以下是删除主体的选项卡代码
        /// </summary>
        private void Refresh_Subject_Delete()
        {
            this.Combox_DeleteSubject.Items.Clear();

            //添加主体创建的客体的名称到下拉列表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [主体信息表] ";
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {
                while (dr.Read())
                {
                    this.Combox_DeleteSubject.Items.Add(dr["主体名称"]);
                }
            }
            this.Combox_DeleteSubject.Items.Remove("admin");
            if (this.Combox_DeleteSubject.Items.Count != 0)
            {
                this.Combox_DeleteSubject.SelectedIndex = 0;
            }
            dr.Close();
            oleDB.Close();
        }


        //删除主体
        private void DeleteSubjectEnter_Click(object sender, EventArgs e)
        {
            LinkedList<StoreAbility> S = Get_Subject_Object(this.Combox_DeleteSubject.Text); //返回以this.Combox_DeleteSubject.Text为权限授予者的所有记录

            while (S.Count != 0)
            {
                Restore_Ability(S.First.Value.Subject_Name, S.First.Value.Object_Name, S.First.Value.Ability_Name);//回收权限
                Delete_Ability(S.First.Value.Subject_Name, S.First.Value.Object_Name, S.First.Value.Ability_Name,S.First.Value.Name_Deliver);
                S.RemoveFirst();
            }
            //删除主体信息表中的信息，和其他在访问控制信息表、客体信息表中的信息
            Delete_Subject(this.Combox_DeleteSubject.Text);

            MessageBox.Show(this, "主体被删除,其授予出去的权限已经被回收，创建的客体被删除", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Refresh_Subject_Delete();
        }
        /// <summary>
        /// 返回以subjectName为权限授予者的所有记录
        /// </summary>
        /// <param name="subjectName"></param>
        /// <returns></returns>
        private LinkedList<StoreAbility> Get_Subject_Object(string subjectName)
        {
            LinkedList<StoreAbility> Link = new LinkedList<StoreAbility>();

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand conn = new OleDbCommand();
            conn.Connection = oleDB;
            conn.CommandText = "select *  from  [访问控制矩阵表] where 权限授予者=" + "'" + subjectName.ToLower() + "'";//查找主客体对应权限
            OleDbDataReader dr = conn.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (string.Equals(dr["主体名称"], subjectName) == false)
                    {
                        StoreAbility S = new StoreAbility(dr["主体名称"].ToString(), dr["客体名称"].ToString(), dr["权限"].ToString(), dr["权限授予者"].ToString(), true);
                        Link.AddFirst(S);

                    }
                }
            }

            dr.Close();
            oleDB.Close();

            return Link;
        }
        /// <summary>
        /// 删除主体
        /// </summary>
        /// <param name="objectName"></param>
        private void Delete_Subject(string subjectName)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            
            LinkedList<string> Card = Get_Subject_Creat_Object(subjectName);//获取主体所创建的所有客体
            while (Card.Count != 0)
            {
                cmd.CommandText = "delete from  [黑令牌表] where  客体名称=" + "'" + Card.First.Value + "'";
                cmd.ExecuteNonQuery();
                Card.RemoveFirst();
             }

            cmd.CommandText = "delete from  [访问控制矩阵表] where  主体名称=" + "'" + subjectName.ToLower() + "'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from  [客体信息表] where  创建者=" + "'" + subjectName.ToLower() + "'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from  [主体信息表] where  主体名称=" + "'" + subjectName.ToLower() + "'";
            cmd.ExecuteNonQuery();

            oleDB.Close();
        }


        //以下是注册管理选项卡的代码
        /// <summary>
        ///更新 
        /// </summary>
        private void Refresh_SubjectRegister()
        {
            this.Combox_SubjectRegister.Items.Clear();
            this.RegisterTime.Text = "";

            //添加注册管理中的待拉用户到列表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [注册管理表] ";
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {
                while (dr.Read())
                {
                    this.Combox_SubjectRegister.Items.Add(dr["主体名称"]);//    增加到下拉框中            
                }
            }

            if (this.Combox_SubjectRegister.Items.Count != 0)
            {
                this.Combox_SubjectRegister.SelectedIndex = 0;
            }
            dr.Close();
            oleDB.Close();
        }

        private void Combox_SubjectRegister_SelectedIndexChanged(object sender, EventArgs e)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [注册管理表] where 主体名称=" + "'" + this.Combox_SubjectRegister.Text.ToLower() + "'";
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {
                if (dr.Read())
                {
                    this.RegisterTime.Text = dr["注册时间"].ToString();
                }
            }
            dr.Close();
            oleDB.Close();
        }

        private void btn_Register_Clear_Click(object sender, EventArgs e)
        {
            if (this.Combox_SubjectRegister.Text.Length == 0)
            {
                MessageBox.Show(this, "还未选中任何注册主体", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "delete from [注册管理表] where 主体名称=" + "'" + this.Combox_SubjectRegister.Text.ToLower() + "'";
            if (cmd.ExecuteNonQuery() != 0)
            {
                MessageBox.Show("已清除该条注册信息");
            }
            oleDB.Close();
            Refresh_SubjectRegister();
        }

        private void btn_Register_Sure_Click(object sender, EventArgs e)
        {
            if (this.Combox_SubjectRegister.Text.Length == 0)
            {
                MessageBox.Show(this, "还未选中任何主体", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Insert_Subject_Info(this.Combox_SubjectRegister.Text);
            MessageBox.Show("增加该用户成功！，按确定返回", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Refresh_SubjectRegister();
        }

        /// <summary>
        /// 增加主体信息到主体信息表中
        /// </summary>   
        private static void Insert_Subject_Info(string subjectName)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [注册管理表] where 主体名称=" + "'" + subjectName.ToLower() + "'";
            OleDbDataReader dr = cmd.ExecuteReader();

            string pwd = string.Empty;
            string time = string.Empty;
            if (dr.HasRows == true)
            {
                if (dr.Read())
                {
                    pwd = dr["密码"].ToString();
                    time = dr["注册时间"].ToString();
                }
            }
            dr.Close();
            //删除在注册管理表中的信息
            cmd.CommandText = "delete from [注册管理表] where 主体名称=" + "'" + subjectName.ToLower() + "'";
            cmd.ExecuteNonQuery();
            // 增加到主体信息表中
            cmd.CommandText = "insert into  [主体信息表](主体名称,密码,注册时间) values('" + subjectName.ToLower() + "','" + pwd + "','" + time + "')";
            cmd.ExecuteNonQuery();
            oleDB.Close();
        }


        /// <summary>
        /// 插入访问控制矩阵信息
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private bool InsertAbility(StoreAbility S)
        {
            bool isSuccess = false;

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            //其次插入到访问控制矩阵中
            //   cmd.CommandText = "insert into  [访问控制矩阵表](主体名称,客体名称,权限) values('" + subjectName + "','" + objectName + "','" + ability.ToString() + "')";

            if (cmd.ExecuteNonQuery() != 0)
            {
                isSuccess = true;
            }
            oleDB.Close();

            return isSuccess;
        }


        private void AbilityTable_Selected(object sender, TabControlEventArgs e)
        {
            switch (e.TabPageIndex)
            {
                case 0:
                    {
                        Refresh__AuthorityTable();
                        break;
                    }
                case 1:
                    {
                        Refresh_Subject_Object_Inquiry();
                        break;
                    }
                case 2:
                    {
                        Refresh__Abillity_Manage(0, 0);
                        break;
                    }
                case 3:
                    {
                        Refresh_Object_Add();
                        break;
                    }
                case 4:
                    {
                        Refresh_Object_Delete();
                        break;
                    }
                case 5:
                    {
                        Refresh_Limit_Card(0,0);
                        break;
                    }
                case 6:
                    {
                        Refresh_Subject_Delete();
                        break;
                    }
                case 7:
                    {
                        Refresh_SubjectRegister();
                        break;
                    }
                default: break;
            }
        }

        //private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    this.AbilityTable.SelectedIndex = 3;
        //}

        private void 重新登录NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubjectRegister register = new SubjectRegister();
            register.Text = "设置普通用户账号及密码";
            register.isAdmin = false;
            register.ShowDialog();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 主体SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubjectRegister register = new SubjectRegister();
            register.Text = "设置普通用户账号及密码";
            register.isAdmin = false;
            register.ShowDialog();
        }

        private void 客体OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AbilityTable.SelectedIndex = 3;
        }

        private void 注册管理RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AbilityTable.SelectedIndex = 7;
        }

        private void 权限回收SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AbilityTable.SelectedIndex = 2;
        }

        private void 授予ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AbilityTable.SelectedIndex = 2;
        }

        private void 授权标TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AbilityTable.SelectedIndex = 0;
        }

        private void 授权链LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AbilityTable.SelectedIndex = 1;
        }

        private void 主客查询SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AbilityTable.SelectedIndex = 1;
        }

        private void 主体SToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.AbilityTable.SelectedIndex = 6;
        }

        private void 客体OToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.AbilityTable.SelectedIndex = 4;
        }

        private void Refresh_Limit_Card(int index_Subject,int index_Object)
        {  
            this.DataGridView_LimitCard.Rows.Clear();
            this.ComboBox_LimtCard_Object.Items.Clear();
            this.ComboBox_LimtCard_Subject.Items.Clear();

            //所有用户到用户列表中(最后还需要移除自身)
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [主体信息表] ";
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {
                while (dr.Read())
                {
                    this.ComboBox_LimtCard_Subject.Items.Add(dr["主体名称"]);//    增加到下拉框中            
                }
            }

            this.ComboBox_LimtCard_Subject.Items.Remove(this.GetSubjectName);//移除自身
            if (this.ComboBox_LimtCard_Subject.Items.Count != 0)
            {
               this.ComboBox_LimtCard_Subject.SelectedIndex = index_Subject;
            }
            dr.Close();
  
            oleDB.Close();
           LinkedList<string> Object_Link= Get_Subject_Creat_Object(this.GetSubjectName);
           while (Object_Link.Count != 0)
           {
               this.ComboBox_LimtCard_Object.Items.Add(Object_Link.First.Value);
               LinkedList<string> Subject_Link=new LinkedList<string>();
               LinkedList<string> Ability_Link = Get_Subject_Object_LimitCard(Object_Link.First.Value, ref Subject_Link);
               while (Subject_Link.Count != 0)
               {
                   DataGridViewRow r = new DataGridViewRow();
                   r.CreateCells(this.DataGridView_LimitCard);
                   r.Cells[1].Value = Subject_Link.First.Value;
                   r.Cells[0].Value = Object_Link.First.Value;
                   r.Cells[2].Value = Ability_Link.First.Value;

                   this.DataGridView_LimitCard.Rows.Add(r);
                   Subject_Link.RemoveFirst();
                   Ability_Link.RemoveFirst();
                }
               
               Object_Link.RemoveFirst();
           }

           if (this.ComboBox_LimtCard_Object.Items.Count != 0)
           {
               this.ComboBox_LimtCard_Object.SelectedIndex = index_Object;
           }
        }
        /// <summary>
        /// 获取主体创建的所有客体的名
        /// </summary>
        /// <param name="subjectName"></param>
        /// <returns></returns>
        private LinkedList<string> Get_Subject_Creat_Object( string  subjectName)
        {
            LinkedList<string> L = new LinkedList<string>();

            //所有用户到用户列表中(最后还需要移除自身)
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [客体信息表] where 创建者 = "+"'"+subjectName.ToLower()+"'";
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {
                while (dr.Read())
                {
                    L.AddLast(dr["客体名称"].ToString());
                }
            }          
            dr.Close();

            oleDB.Close();
            return L;
        }
        /// <summary>
        /// 获取客体对应的限制权限及相应的主体
        /// </summary>
        /// <param name="subjectName"></param>
        /// <returns></returns>
        private LinkedList<string> Get_Subject_Object_LimitCard(string objectName,ref LinkedList<string> subject_Link )
        {
            LinkedList<string> L = new LinkedList<string>();

            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [黑令牌表] where  客体名称= " + "'" + objectName.ToLower() + "'";
            OleDbDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {
                while (dr.Read())
                {
                    L.AddLast(dr["限制权限"].ToString());
                    subject_Link.AddLast(dr["主体名称"].ToString());
                }
            }
            dr.Close();

            oleDB.Close();
            return L;
        }

        private void ComboBox_LimtCard_Subject_SelectedIndexChanged(object sender, EventArgs e)
        {
            Refresh_LimitCard();
        }

        private void ComboBox_LimtCard_Object_SelectedIndexChanged(object sender, EventArgs e)
        {
            Refresh_LimitCard();
        }
        private void Refresh_LimitCard()
        {
            this.CheckedListBox_LimitCard_Add.Items.Clear();
            this.CheckedListBox_LimitCard_Delete.Items.Clear();
       
            //所有用户到用户列表中(最后还需要移除自身)
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "select * from [黑令牌表] where  客体名称= " + "'" +this.ComboBox_LimtCard_Object.Text+ "'" + "and 主体名称= " + "'" + this.ComboBox_LimtCard_Subject.Text+ "'";
            OleDbDataReader dr = cmd.ExecuteReader();
           
            if (dr.HasRows == true)
            {
                while (dr.Read())
                {
                    this.CheckedListBox_LimitCard_Delete.Items.Add(dr["限制权限"]);   
                }
            }
            foreach(var  v in  ABILITY )
            {
                if (this.CheckedListBox_LimitCard_Delete.Items.IndexOf(v.ToString()) == -1)
                {
                    this.CheckedListBox_LimitCard_Add.Items.Add(v.ToString());
                }
            }
            dr.Close();

            oleDB.Close();
        }

        private void Btn_LimitCard_Delete_Click(object sender, EventArgs e)
        {
            if (this.CheckedListBox_LimitCard_Delete.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "还未选择任何权限，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (var v in this.CheckedListBox_LimitCard_Delete.CheckedItems)
            {
               Delete_Limit_Card(this.ComboBox_LimtCard_Subject.Text, this.ComboBox_LimtCard_Object.Text, v.ToString());
            }

            Refresh_Limit_Card(this.ComboBox_LimtCard_Subject.SelectedIndex, this.ComboBox_LimtCard_Object.SelectedIndex);
        }

        private void Btn_LimitCard_Add_Click(object sender, EventArgs e)
        {
            if (this.CheckedListBox_LimitCard_Add.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "还未选择任何权限，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (var v in this.CheckedListBox_LimitCard_Add.CheckedItems) //要发放的黑令牌中 主体已经有了该权限，则不能添加黑令牌
            {
                 if(Is_Subject_Object_Ability(this.ComboBox_LimtCard_Subject.Text ,this.ComboBox_LimtCard_Object.Text,v.ToString())==true)
                  {
                      MessageBox.Show(this, "主体已经拥有了对于客体的 "+ v.ToString()+ " 权限  ，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                      return;
                  }
            }
              
            foreach (var v in this.CheckedListBox_LimitCard_Add.CheckedItems)
            {
                Insert_Limit_Card(this.ComboBox_LimtCard_Subject.Text,this.ComboBox_LimtCard_Object.Text,v.ToString());
            }
            Refresh_Limit_Card(this.ComboBox_LimtCard_Subject.SelectedIndex, this.ComboBox_LimtCard_Object.SelectedIndex);
        }
       /// <summary>
       /// 黑令牌的添加
       /// </summary>
       /// <param name="subjectName"></param>
       /// <param name="objectName"></param>
       /// <param name="ability"></param>
  
       private void Insert_Limit_Card(string subjectName,string objectName,string ability)
       {
           OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
           oleDB.Open();

           OleDbCommand cmd = new OleDbCommand();
           cmd.Connection = oleDB;

           // 增加到主体信息表中
           cmd.CommandText = "insert into  [黑令牌表](主体名称,客体名称,限制权限) values('" + subjectName.ToLower() + "','" + objectName.ToLower()+ "','" +ability + "')";
           cmd.ExecuteNonQuery();
           oleDB.Close();
        }
        /// <summary>
        /// 黑令牌的删除
        /// </summary>
        /// <param name="subjectName"></param>
        /// <param name="objectName"></param>
        /// <param name="ability"></param>
        private void Delete_Limit_Card(string subjectName, string objectName, string ability)
        {
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();

            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = oleDB;
            cmd.CommandText = "delete from  [黑令牌表] where 主体名称=" + "'" + subjectName.ToLower() + "'" + "and 客体名称=" + "'" + objectName + "'" + "and 限制权限=" + "'" + ability + "'";


            cmd.ExecuteNonQuery();

            oleDB.Close();
        }

        private void DataGridView_AuthorityData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Label_Warning_Object_Name_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }
       
    }
    //存储权限的类
    class StoreAbility
    {
        public string Subject_Name = string.Empty;//主体名称
        public string Object_Name = string.Empty;//客体名称
        public string Ability_Name = string.Empty;//能力的名称
        public string Name_Deliver = string.Empty;//授权传递者
        public bool IsDeliver = false; //是否具有授予权
        /// <summary>
        /// 存储能力的类的初始化
        /// </summary>
        /// <param name="Subject_Name"></param>
        /// <param name="Object_Name"></param>
        /// <param name="Ability_Name"></param>
        /// <param name="Name_Deliver"></param>
        /// <param name="IsDeliver"></param>
        public StoreAbility(string Subject_Name, string Object_Name, string Ability_Name, string Name_Deliver, bool IsDeliver)
        {
            this.Object_Name = Object_Name;
            this.Subject_Name = Subject_Name;
            this.Ability_Name = Ability_Name;
            this.Name_Deliver = Name_Deliver ;
            this.IsDeliver = IsDeliver;
        }
    }
}
 


  