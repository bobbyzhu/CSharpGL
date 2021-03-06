﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;

namespace CSharpGL
{
    /// <summary>
    /// 
    /// </summary>
    [Editor(typeof(PropertyGridEditor), typeof(UITypeEditor))]
    public abstract class GLSwitch
    {

        private bool inUse = false;

        /// <summary>
        /// You want to use this switch?
        /// </summary>
        public bool InUse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GLSwitch()
        {
            this.InUse = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void On()
        {
            if (this.InUse)
            {
                this.inUse = true;
                this.SwitchOn();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Off()
        {
            if (this.inUse)
            {
                this.inUse = false;
                this.SwitchOff();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void SwitchOn();

        /// <summary>
        /// 
        /// </summary>
        protected abstract void SwitchOff();
    }

}
