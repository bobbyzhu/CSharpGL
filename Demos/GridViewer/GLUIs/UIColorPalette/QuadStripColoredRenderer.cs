﻿using CSharpGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridViewer
{
    /// <summary>
    ///  /|\ y
    ///   |
    ///   |
    ///   |
    ///   ---------------&gt; x
    /// (0, 0)
    /// 0    2    4    6    8    10
    /// --------------------------
    /// |    |    |    |    |    |
    /// |    |    |    |    |    |
    /// |    |    |    |    |    |
    /// |    |    |    |    |    |
    /// |    |    |    |    |    |
    /// |    |    |    |    |    |
    /// |    |    |    |    |    |
    /// --------------------------
    /// 1    3    5    7    9    11
    /// side length is 1.
    /// </summary>
    class QuadStripColoredRenderer : Renderer
    {

        private PolygonModeSwitch polygonModeSwitch = new PolygonModeSwitch(PolygonModes.Lines);
        private LineWidthSwitch lineWidthSwitch = new LineWidthSwitch(1);

        private PolygonOffsetSwitch offsetSwitch = new PolygonOffsetLineSwitch();
        private PropertyBufferPtr positionBufferPtr;
        private PropertyBufferPtr colorBufferPtr;
        private int quadCount;

        public static QuadStripColoredRenderer Create(QuadStripColoredModel model)
        {
            var shaderCodes = new ShaderCode[2];
            shaderCodes[0] = new ShaderCode(File.ReadAllText(@"shaders\QuadStripColor.vert"), ShaderType.VertexShader);
            shaderCodes[1] = new ShaderCode(File.ReadAllText(@"shaders\QuadStripColor.frag"), ShaderType.FragmentShader);
            var map = new PropertyNameMap();
            map.Add("in_Position", QuadStripColoredModel.position);
            map.Add("in_Color", QuadStripColoredModel.color);

            var renderer = new QuadStripColoredRenderer(model, shaderCodes, map);
            renderer.quadCount = model.quadCount;
            return renderer;
        }

        private QuadStripColoredRenderer(IBufferable bufferable, ShaderCode[] shaderCodes,
            PropertyNameMap propertyNameMap, params GLSwitch[] switches)
            : base(bufferable, shaderCodes, propertyNameMap, switches)
        {
        }

        protected override void DoInitialize()
        {
            base.DoInitialize();

            this.positionBufferPtr = this.bufferable.GetProperty(QuadStripColoredModel.position, null);
            this.colorBufferPtr = this.bufferable.GetProperty(QuadStripColoredModel.color, null);
        }

        protected override void DoRender(RenderEventArgs arg)
        {
            this.SetUniform("renderWireframe", false);
            base.DoRender(arg);

            //polygonModeSwitch.On();
            //lineWidthSwitch.On();
            //// offsetSwitch.On();
            //this.SetUniform("renderWireframe", true);
            //base.DoRender(arg);
            ////offsetSwitch.Off(); 
            //lineWidthSwitch.Off();
            //polygonModeSwitch.Off();
        }

        //public enum ColorType
        //{
        //    Color,
        //    Texture,
        //}

        //public void SetQuadCount(int quadCount)
        //{
        //    OpenGL.BindBuffer(BufferTarget.ArrayBuffer, this.positionBufferPtr.BufferId);
        //    IntPtr pointer = OpenGL.MapBuffer(BufferTarget.ArrayBuffer, MapBufferAccess.ReadWrite);
        //    unsafe
        //    {
        //        var array = (vec3*)pointer.ToPointer();
        //        for (int i = 0; i < (quadCount + 1); i++)
        //        {
        //            array[i * 2 + 0] = new vec3(-0.5f + (float)i / (float)(quadCount), 0.5f, 0);
        //            array[i * 2 + 1] = new vec3(-0.5f + (float)i / (float)(quadCount), -0.5f, 0);
        //        }
        //    }
        //    OpenGL.UnmapBuffer(BufferTarget.ArrayBuffer);
        //    OpenGL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        //    this.currentQuadCount = quadCount;
        //}

        //public void UpdateCodedColor(CodedColor[] codedColors)
        //{
        //    int quadCount = codedColors.Length - 1;

        //    {
        //        OpenGL.BindBuffer(BufferTarget.ArrayBuffer, this.positionBufferPtr.BufferId);
        //        IntPtr pointer = OpenGL.MapBuffer(BufferTarget.ArrayBuffer, MapBufferAccess.ReadWrite);
        //        unsafe
        //        {
        //            var array = (vec3*)pointer.ToPointer();
        //            for (int i = 0; i < (quadCount + 1); i++)
        //            {
        //                //array[i * 2 + 0] = new vec3(-0.5f + (float)i / (float)(quadCount), 0.5f, 0);
        //                //array[i * 2 + 1] = new vec3(-0.5f + (float)i / (float)(quadCount), -0.5f, 0);
        //                array[i * 2 + 0] = new vec3(-0.5f + codedColors[i].Coord, 0.5f, 0);
        //                array[i * 2 + 1] = new vec3(-0.5f + codedColors[i].Coord, -0.5f, 0);
        //            }
        //        }
        //        OpenGL.UnmapBuffer(BufferTarget.ArrayBuffer);
        //        OpenGL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        //    }

        //    if (this.colorType == ColorType.Texture)
        //    {
        //        OpenGL.BindBuffer(BufferTarget.ArrayBuffer, this.texCoordBufferPtr.BufferId);
        //        IntPtr pointer = OpenGL.MapBuffer(BufferTarget.ArrayBuffer, MapBufferAccess.ReadWrite);
        //        unsafe
        //        {
        //            var array = (float*)pointer.ToPointer();
        //            for (int i = 0; i < (quadCount + 1); i++)
        //            {
        //                array[i * 2 + 0] = codedColors[i].Coord;
        //                array[i * 2 + 1] = codedColors[i].Coord;
        //            }
        //        }
        //        OpenGL.UnmapBuffer(BufferTarget.ArrayBuffer);
        //        OpenGL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        //    }
        //    else if (this.colorType == ColorType.Color)
        //    {
        //        OpenGL.BindBuffer(BufferTarget.ArrayBuffer, this.colorBufferPtr.BufferId);
        //        IntPtr pointer = OpenGL.MapBuffer(BufferTarget.ArrayBuffer, MapBufferAccess.ReadWrite);
        //        unsafe
        //        {
        //            var array = (vec3*)pointer.ToPointer();
        //            for (int i = 0; i < (quadCount + 1); i++)
        //            {
        //                array[i * 2 + 0] = codedColors[i].DisplayColor;
        //                array[i * 2 + 1] = codedColors[i].DisplayColor;
        //            }
        //        }
        //        OpenGL.UnmapBuffer(BufferTarget.ArrayBuffer);
        //        OpenGL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        //    }

        //    {
        //        var pointer = this.indexBufferPtr as ZeroIndexBufferPtr;
        //        pointer.VertexCount = (quadCount + 1) * 2;
        //    }
        //}

        public void UpdateColorBar(System.Drawing.Bitmap bitmap)
        {
            OpenGL.BindBuffer(BufferTarget.ArrayBuffer, this.colorBufferPtr.BufferId);
            IntPtr pointer = OpenGL.MapBuffer(BufferTarget.ArrayBuffer, MapBufferAccess.ReadWrite);
            unsafe
            {
                var array = (vec3*)pointer.ToPointer();
                for (int i = 0; i < (quadCount + 1); i++)
                {
                    int x = bitmap.Width * i / quadCount;
                    if (x >= bitmap.Width) { x = 0; }
                    array[i * 2 + 0] = bitmap.GetPixel(x, 0).ToVec3();
                    array[i * 2 + 1] = array[i * 2 + 0];
                }
            }
            OpenGL.UnmapBuffer(BufferTarget.ArrayBuffer);
            OpenGL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }

}
