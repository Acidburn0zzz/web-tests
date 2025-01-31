﻿//
// TestAuthentication.cs
//
// Author:
//       Martin Baulig <martin.baulig@xamarin.com>
//
// Copyright (c) 2014 Xamarin Inc. (http://www.xamarin.com)
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
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Xamarin.WebTests.Tests
{
	using Runners;
	using Handlers;
	using Framework;

	[TestFixture]
	public class TestAuthentication
	{
		HttpTestRunner runner;

		[TestFixtureSetUp]
		public void Start ()
		{
			runner = new HttpTestRunner ();
			runner.Start ();
		}

		[TestFixtureTearDown]
		public void Stop ()
		{
			runner.Stop ();
			runner = null;
		}

		public static IEnumerable<Handler> GetAllTests ()
		{
			return TestPost.GetAllTests ();
		}

		void Run (Handler handler)
		{
			runner.Run (handler);
		}

		[TestCaseSource ("GetAllTests")]
		public void TestBasicAuthentication (Handler handler)
		{
			Run (new AuthenticationHandler (AuthenticationType.Basic, handler));
		}

		[TestCaseSource ("GetAllTests")]
		public void TestNTLM (Handler handler)
		{
			Run (new AuthenticationHandler (AuthenticationType.NTLM, handler));
		}

		[Test]
		public void MustClearAuthOnRedirect ()
		{
			var target = new HelloWorldHandler ();
			var targetAuth = new AuthenticationHandler (AuthenticationType.ForceNone, target);

			var redirect = new RedirectHandler (targetAuth, HttpStatusCode.Redirect);
			var authHandler = new AuthenticationHandler (AuthenticationType.Basic, redirect);

			Run (authHandler);
		}
	}
}
