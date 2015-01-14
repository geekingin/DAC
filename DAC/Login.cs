using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;//数据库通信标准
using System.Collections;
using ADOX;//应用程序层次界面，用oledb和数据库通信
using System.IO;//系统IO
 
namespace DAC
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private void login_Load(object sender, EventArgs e)
        {
            if (File.Exists("dac.accdb") == false || SubjectRegister.IsUserNameUsed("admin")==false)//若数据库文件不存在或则管理员账号不存在，则创建新的文件或者管理员帐号并退出
            {
                if (File.Exists("dac.accdb") == false)
                {
                    CreatDb();
                }
                MessageBox.Show(this,"第一次使用，按确定设置管理员账号及密码","设置管理员账号及密码",MessageBoxButtons.OK,MessageBoxIcon.Information);
               
                SubjectRegister register = new SubjectRegister();
                register.Text = "设置管理员账号及密码";
                register.getIsAdmin= true;
                register.ShowDialog();//显示
            }
            userInfoUpdata();//将数据库中用户信息添加到用户列表中
        }
     
        private void userInfoUpdata()
        {
            //将数据库中用户信息添加到用户列表中
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");//连接数据库并打开
            oleDB.Open();
           
            OleDbCommand conn = new OleDbCommand();
            conn.Connection = oleDB;
            conn.CommandText = "select *  from  [主体信息表]";//SQL命令

            OleDbDataReader dr = conn.ExecuteReader();//执行读操作
            if (dr.HasRows == true)
            {
                userCombox.Items.Clear();//清除当前的下拉窗口中的值
                while (dr.Read())
                {
                    userCombox.Items.Add(dr[0].ToString());//显示所有注册用户到下拉窗口
                }
            }
            if (userCombox.Items.Count != 0)
            {
                userCombox.SelectedIndex = 0;
            }

            dr.Close();
            oleDB.Close();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enter_Click(object sender, EventArgs e)
        {
            bool isLogin = false;

            if (SubjectRegister.IsUserNameUsed(userCombox.Text) == false)
            {
                MessageBox.Show(this,"数据库中找不到这样的账号","登录失败",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if (userCombox.Text.Length == 0 || pwd.Text.Length==0)
            {
                MessageBox.Show(this, "账户名称及密码不能为空", "登录失败", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
              }
            OleDbConnection oleDB = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb");
            oleDB.Open();
            OleDbCommand conn = new OleDbCommand();
            conn.Connection = oleDB;
            conn.CommandText = "select *  from  [主体信息表] where 主体名称="+"'"+userCombox.Text.ToLower()+"'";          
            OleDbDataReader dr = conn.ExecuteReader();
            if (dr.HasRows == true)
            {
                while (dr.Read())
                {
                    if (dr[1].ToString().CompareTo(pwd.Text.GetHashCode().ToString()) == 0)//找到相应记录，置登录标志为真
                    {
                        isLogin = true;
                    }
                }
            }
            dr.Close();
            oleDB.Close();
            if (isLogin == false)
            {
                MessageBox.Show(this, "密码错误", "登录失败", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show(this, "登录成功,按确定进入", "登录成功", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }


            //显示信息主界面
            this.Visible = false;   //隐藏登录框
            Dac m = new Dac();
            m.Text = userCombox.Text.ToLower();
            m.GetSubjectName = userCombox.Text.ToLower();
            m.ShowDialog();
            userInfoUpdata();
        
            this.Visible = true;
        }
       
        /// <summary>
        /// 退出t
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        /// <summary>
        /// 创建新的信息数据库
        /// </summary>
        private void CreatDb()
        {
            string con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=dac.accdb";//connectionString
            
            ADOX.Catalog catalog = new Catalog();//数据库目录
            catalog.Create(con);//创建新的数据库
         
            catalog.ActiveConnection();//激活连接
            ADODB.Connection connection = new ADODB.Connection();
            connection.Open(con, null, null, -1);//打开连接 (con name password -1)

            //新建主体信息表数据表
            ADOX.Table table = new Table();
            table.Name = "主体信息表";

            ADOX.Column subjectName = new Column();//主体名称
            ADOX.Column pwd = new Column();//主体登录密码
            ADOX.Column registerTime = new Column();//主体登录密码
            subjectName.Name = "主体名称";
            pwd.Name = "密码";
            registerTime.Name = "注册时间";

            table.Columns.Append(subjectName);//将列添加到表中
            table.Columns.Append(pwd);
            table.Columns.Append(registerTime);

            catalog.Tables.Append(table);//表加到目录中


            //新建客体信息数据表
            table = new Table();
            table.Name = "客体信息表";

            ADOX.Column objectName = new Column();//客体名称
            objectName.Name = "客体名称";

            ADOX.Column object_registerTime = new Column();//创建时间
            object_registerTime.Name = "创建时间";

            ADOX.Column object_subjectName = new Column();//创建时间
            object_subjectName.Name = "创建者";

            table.Columns.Append(objectName);            
            table.Columns.Append(object_registerTime);
            table.Columns.Append(object_subjectName.Name);

            catalog.Tables.Append(table);


            //新建访问控制矩阵信息数据表
            table = new Table();
            table.Name = "访问控制矩阵表";

            ADOX.Column subject_Name = new Column();//客体名称
            subject_Name.Name = "主体名称";

            ADOX.Column object_Name = new Column();//客体名称
            object_Name.Name = "客体名称";

            ADOX.Column ability = new Column();//权限
            ability.Name = "权限";
            ADOX.Column isCanBeDeliver = new Column();//权限
            isCanBeDeliver.Name = "授予权";
            ADOX.Column subject_Name_Deliver = new Column();//权限
            subject_Name_Deliver.Name = "权限授予者";

            table.Columns.Append(subject_Name);
            table.Columns.Append(object_Name);
            table.Columns.Append(ability);
            table.Columns.Append(isCanBeDeliver);
            table.Columns.Append(subject_Name_Deliver);

            catalog.Tables.Append(table);


            //新建注册管理表
            table = new Table();
            table.Name = "注册管理表";
            ADOX.Column subject_Name1 = new Column();//客体名称
            subject_Name1.Name = "主体名称";
            ADOX.Column time = new Column();//客体名称
            time.Name = "注册时间";
            ADOX.Column pwd1= new Column();//客体名称
            pwd1.Name = "密码";

            table.Columns.Append(subject_Name1);
            table.Columns.Append(time);
            table.Columns.Append(pwd1);
            catalog.Tables.Append(table);


            //新建黑令牌表
            table = new Table();
            table.Name = "黑令牌表";
            ADOX.Column subject_Name2 = new Column();//客体名称
            subject_Name2.Name = "主体名称";
            ADOX.Column objectName1 = new Column();//客体名称
            objectName1.Name = "客体名称";
            ADOX.Column ability1 = new Column();//客体名称
            ability1.Name = "限制权限";
            table.Columns.Append(subject_Name2);
            table.Columns.Append(objectName1);
            table.Columns.Append(ability1);
            catalog.Tables.Append(table);

            //关闭连接
            connection.Close();
        }
       
        private void userCombox_TextChanged(object sender, EventArgs e)
        {
            if (userCombox.Text.ToLower().CompareTo("admin") == 0)
            {
                roleText.Text = "管理员";
            }
            else
            {
                roleText.Text = "普通用户";
            }
        }

        private void btn_register_Click(object sender, EventArgs e)
        {
            if (SubjectRegister.IsUserNameUsed("admin") == false)//若数据库文件不存在或则管理员账号不存在，则创建新的文件或者管理员帐号并退出
            {

                MessageBox.Show(this, "第一次使用，按确定设置管理员账号及密码", "设置管理员账号及密码", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SubjectRegister register = new SubjectRegister();
                register.Text = "设置管理员账号及密码";
                register.getIsAdmin = true;
                register.ShowDialog();
            }
            else
            {
                SubjectRegister register = new SubjectRegister();
                register.Text = "设置普通用户账号及密码";
                register.isAdmin = false;
                register.ShowDialog();
            }

            userInfoUpdata();//将数据库中用户信息添加到用户列表中
            this.pwd.Text = string.Empty;
        }

        private void userCombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pwd.Text = "";
        }
    }
}
