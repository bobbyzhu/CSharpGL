﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace CSharpGL.Demos
{
    public partial class Form03OrderDependentTransparency : Form
    {

        private IMouseHandler rotator;

        public Form03OrderDependentTransparency(Form02OrderIndependentTransparency form02)
        {
            InitializeComponent();

            this.form02 = form02;

            this.glCanvas1.OpenGLDraw += glCanvas1_OpenGLDraw;
            this.glCanvas1.MouseDown += glCanvas1_MouseDown;
            this.glCanvas1.MouseMove += glCanvas1_MouseMove;
            this.glCanvas1.MouseUp += glCanvas1_MouseUp;
            this.glCanvas1.MouseWheel += glCanvas1_MouseWheel;

            Application.Idle += Application_Idle;
            // 天蓝色背景
            OpenGL.ClearColor(0x87 / 255.0f, 0xce / 255.0f, 0xeb / 255.0f, 0xff / 255.0f);
        }

        void Application_Idle(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} - FPS: {1}", this.GetType().Name, this.glCanvas1.FPS.ToShortString());
        }

        private void glCanvas1_OpenGLDraw(object sender, PaintEventArgs e)
        {
            OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);

            this.scene.Render(RenderModes.Render, this.glCanvas1.ClientRectangle, this.glCanvas1.PointToClient(Control.MousePosition));
        }

        private Form02OrderIndependentTransparency form02;

        internal void glCanvas1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (sender == this.glCanvas1)
            {
                this.form02.glCanvas1_MouseWheel(sender, e);
            }
            else
            {
                this.rotator.canvas_MouseWheel(sender, e);
            }
        }

        private void glCanvas1_Resize(object sender, EventArgs e)
        {
            Camera camera = this.scene.Camera;
            if (camera != null)
            {
                camera.Resize(this.glCanvas1.Width, this.glCanvas1.Height);
            }
        }

    }
}
