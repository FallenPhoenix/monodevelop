﻿//
// Workbench.cs
//
// Author:
//       Manish Sinha <manish.sinha@xamarin.com>
//
// Copyright (c) 2015 Xamarin Inc.
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
using MonoDevelop.Core;
using MonoDevelop.Components.AutoTest;
using System.Text.RegularExpressions;
using MonoDevelop.Ide.Commands;
using System.Linq;

namespace UserInterfaceTests
{
	public static class Workbench
	{
		static AutoTestClientSession Session {
			get { return TestService.Session; }
		}

		static readonly Regex buildRegex = new Regex (@"Build: (?<errors>\d*) error\D*, (?<warnings>\d*) warning\D*", RegexOptions.Compiled);

		public static string GetStatusMessage (int timeout = 20000)
		{
			if (Platform.IsMac) {
				Ide.WaitUntil (
					() => Session.GetGlobalValue<string> ("MonoDevelop.Ide.IdeApp.Workbench.RootWindow.StatusBar.text") != string.Empty,
					timeout
				);
				return (string)Session.GetGlobalValue ("MonoDevelop.Ide.IdeApp.Workbench.RootWindow.StatusBar.text");
			}

			Ide.WaitUntil (
				() => Session.GetGlobalValue<int> ("MonoDevelop.Ide.IdeApp.Workbench.RootWindow.StatusBar.messageQueue.Count") == 0,
				timeout
			);
			return (string) Session.GetGlobalValue ("MonoDevelop.Ide.IdeApp.Workbench.RootWindow.StatusBar.renderArg.CurrentText");
		}

		public static bool IsBuildSuccessful (int timeoutInSecs)
		{
			bool isBuildSuccessful = false;
			Ide.WaitUntil (() => {
				var actualStatusMessage = Workbench.GetStatusMessage ();
				if (actualStatusMessage == "Build successful.") {
					isBuildSuccessful = true;
					return true;
				}
				if (actualStatusMessage == "Build failed.") {
					isBuildSuccessful = false;
					return true;
				}
				var match = buildRegex.Match (actualStatusMessage);
				if (match != null && match.Success) {
					isBuildSuccessful = string.Equals (match.Groups ["errors"].ToString (), "0");
					return true;
				}
				return false;
			}, pollStep: 5 * 1000, timeout: timeoutInSecs * 1000);
			return isBuildSuccessful;
		}

		public static bool Run (int timeoutSeconds = 20, int pollStepSecs = 5)
		{
			Session.ExecuteCommand (ProjectCommands.Run);
			try {
				Ide.WaitUntil (
					() => !Session.Query (c => c.Marked ("MonoDevelop.MacIntegration.MainToolbar.RunButton").Property ("Icon", "Stop")).Any (),
					timeout: timeoutSeconds * 1000, pollStep: pollStepSecs * 1000);
				return false;
			} catch (TimeoutException) {
				return true;
			}
		}
	}
}
