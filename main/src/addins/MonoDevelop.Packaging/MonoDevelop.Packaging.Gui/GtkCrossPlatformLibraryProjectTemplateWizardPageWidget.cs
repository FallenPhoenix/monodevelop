﻿//
// GtkCrossPlatformLibraryProjectTemplateWizardPageWidget.cs
//
// Author:
//       Matt Ward <matt.ward@xamarin.com>
//
// Copyright (c) 2016 Xamarin Inc. (http://xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Gtk;
using MonoDevelop.Packaging.Templating;

namespace MonoDevelop.Packaging.Gui
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class GtkCrossPlatformLibraryProjectTemplateWizardPageWidget : Gtk.Bin
	{
		CrossPlatformLibraryTemplateWizardPage wizardPage;

		public GtkCrossPlatformLibraryProjectTemplateWizardPageWidget ()
		{
			this.Build ();
		}

		internal GtkCrossPlatformLibraryProjectTemplateWizardPageWidget (CrossPlatformLibraryTemplateWizardPage wizardPage)
			: this ()
		{
			this.wizardPage = wizardPage;

			nameTextBox.TextInserted += NameTextInserted;
			nameTextBox.Changed += NameTextChanged;

			descriptionTextBox.Text = wizardPage.Description;
			descriptionTextBox.Changed += DescriptionTextChanged;

			nameTextBox.ActivatesDefault = true;
			descriptionTextBox.ActivatesDefault = true;

			nameTextBox.TruncateMultiline = true;
			descriptionTextBox.TruncateMultiline = true;

			androidCheckButton.Active = wizardPage.IsAndroidChecked;
			androidCheckButton.Toggled += AndroidCheckButtonToggled;

			iOSCheckButton.Active = wizardPage.IsIOSChecked;
			iOSCheckButton.Toggled += IOSCheckButtonToggled;

			portableClassLibraryRadioButton.Active = wizardPage.IsPortableClassLibrarySelected;
			portableClassLibraryRadioButton.Toggled += PortableClassLibraryRadioButtonToggled;

			sharedProjectRadioButton.Active = wizardPage.IsSharedProjectSelected;
			sharedProjectRadioButton.Toggled += SharedProjectRadioButtonToggled;
		}

		protected override void OnFocusGrabbed ()
		{
			nameTextBox.GrabFocus ();
		}

		void NameTextInserted (object o, TextInsertedArgs args)
		{
			if (args.Text.IndexOf ('\r') >= 0) {
				var textBox = (Entry)o;
				textBox.Text = textBox.Text.Replace ("\r", string.Empty);
			}
		}

		void NameTextChanged (object sender, EventArgs e)
		{
			wizardPage.LibraryName = nameTextBox.Text;
		}

		void DescriptionTextChanged (object sender, EventArgs e)
		{
			wizardPage.Description = descriptionTextBox.Text;
		}

		void AndroidCheckButtonToggled (object sender, EventArgs e)
		{
			wizardPage.IsAndroidChecked = androidCheckButton.Active;
		}

		void IOSCheckButtonToggled (object sender, EventArgs e)
		{
			wizardPage.IsIOSChecked = iOSCheckButton.Active;
		}

		void PortableClassLibraryRadioButtonToggled (object sender, EventArgs e)
		{
			wizardPage.IsPortableClassLibrarySelected = portableClassLibraryRadioButton.Active;
		}

		void SharedProjectRadioButtonToggled (object sender, EventArgs e)
		{
			wizardPage.IsSharedProjectSelected = sharedProjectRadioButton.Active;
		}
	}
}