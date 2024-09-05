﻿using QuodLib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace QuodLib.WinForms.Objects.Puppeteers {
    public class PictureBoxButton : Puppeteer<PictureBox> {
        private bool _enabled;
        public bool Enabled {
            get => _enabled;
            set {
                if (_enabled == value)
                    return;

                _enabled = value;
                ButtonState = value ? ButtonState.Normal : ButtonState.Disabled;
                Puppet.Enabled = value;
            } 
        }

        protected Dictionary<ButtonState, Image> Images { get; init; }
        private ButtonState _buttonState;
        public ButtonState ButtonState {
            get => _buttonState;
            protected set {
                if (_buttonState == value)
                    return;

                _buttonState = value;
                _ = Images.TryGetValue(value, out Image? img);
                Puppet.Image = img ?? Images[ButtonState.Normal];
            }
        }

        public PictureBoxButton(PictureBox puppet, Image normal, Image hover, Image pressed, Image? disabled) : base(puppet) {
            Images = new() {
                { ButtonState.Normal, normal },
                { ButtonState.Hovered, hover },
                { ButtonState.Pressed, pressed },
            };

            if (disabled != null)
                Images.Add(ButtonState.Disabled, disabled);

            Enabled = puppet.Enabled;
        }

        protected override void AttachPuppet(PictureBox puppet) {
            base.AttachPuppet(puppet);

            puppet.MouseDown += Puppet_MouseDown;
            puppet.MouseEnter += Puppet_MouseEnter;
            puppet.MouseLeave += Puppet_MouseLeave;
            puppet.MouseUp += Puppet_MouseUp;
        }

        private void Puppet_MouseUp(object? sender, MouseEventArgs e) {
            if (!Enabled)
                return;

            ButtonState = ButtonState.Hovered;
        }

        private void Puppet_MouseLeave(object? sender, EventArgs e) {
            if (!Enabled)
                return;

            ButtonState = ButtonState.Normal;
        }

        private void Puppet_MouseEnter(object? sender, EventArgs e) {
            if (!Enabled)
                return;

            ButtonState = ButtonState.Hovered;
        }

        private void Puppet_MouseDown(object? sender, MouseEventArgs e) {
            if (!Enabled)
                return;

            ButtonState = ButtonState.Pressed;
        }
    }
}
