﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace CSharpGL.SceneEditor
{
    public partial class FormMain : Form
    {

        public Scene scene;

        public FormMain()
        {
            InitializeComponent();

            this.Load += FormMain_Load;
        }


    }
}
