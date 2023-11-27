using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;

namespace WordPress_Post
{
    public partial class frm_main : Form
    {
        WordPress_Connect db=new WordPress_Connect();
        WordPress_post wp = new WordPress_post();
        MSG msg;

        public frm_main()
        {
            InitializeComponent();
            msg=new MSG(lst_msg);
        }

        private void frm_main_Load(object sender, EventArgs e)
        {
            msg.push("system Initialized");

        }

        private async void btn_read_Click(object sender, EventArgs e)
        {
            object obj = wp.Get_Read_obj();
            await  db.send(obj);
            wp.Fill_DataGrid(db.str_response, dg_display);
           
        }

        private async void btn_connect_Click(object sender, EventArgs e)
        {
            object obj = wp.Get_Connect_obj();
            await db.send(obj);
            if (db.str_response=="ok") msg.push("connected");
        }

        private async void btn_insert_Click(object sender, EventArgs e)
        {
            object obj = wp.Get_Insert_String(txt_user_id.Text, txt_sys.Text, txt_dia.Text, txt_pul.Text);
            await db.send(obj);
            obj = wp.Get_Read_obj();
            await db.send(obj);
            wp.Fill_DataGrid(db.str_response, dg_display);
        }

        private void dg_display_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            int i = e.RowIndex;
            txt_id.Text=dg_display[0, i].Value.ToString();
            txt_user_id.Text=  dg_display[1, i].Value.ToString();
            txt_sys.Text=  dg_display[3, i].Value.ToString();
            txt_dia.Text=  dg_display[4, i].Value.ToString();
            txt_pul.Text = dg_display[5, i].Value.ToString();
        }

        private async void btn_update_Click(object sender, EventArgs e)
        {
            object obj = wp.Get_Update_data(txt_id.Text,txt_user_id.Text, txt_sys.Text, txt_dia.Text, txt_pul.Text);
            await db.send(obj);
            obj = wp.Get_Read_obj();
            await db.send(obj);
            wp.Fill_DataGrid(db.str_response, dg_display);
        }

        private async void btn_delete_Click(object sender, EventArgs e)
        {
            object obj = wp.Get_Delete_row_obj(txt_id.Text);
            await db.send(obj);
            obj = wp.Get_Read_obj();
            await db.send(obj);
            wp.Fill_DataGrid(db.str_response, dg_display);
        }
    }
}
